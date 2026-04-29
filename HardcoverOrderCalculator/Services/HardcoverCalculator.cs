using HardcoverOrderCalculator.Models;

namespace HardcoverOrderCalculator.Services;

public sealed class HardcoverCalculator
{
    private const decimal StackCutHeightMm = 90m;
    private const decimal PurGlueFactor = 0.00065m;
    private const decimal WaterGlueAreaDivisor = 145000m;

    public CalculationResult Calculate(OrderInput input, CalculatorData data)
    {
        var result = new CalculationResult();

        var block = CalculateTextBlock(input, data, result);
        var endpaper = CalculateEndpaper(input, data, result, block);
        var board = CalculateBoard(input, data, result, block, endpaper);
        var cover = CalculateCover(input, data, result, block, board);
        var book = CalculateFinishedBook(input, result, block, endpaper, board, cover);
        var materials = CalculateMaterials(input, data, result, block, endpaper, board, cover, book);
        var packaging = CalculatePackaging(input, result, book);

        CalculateWork(input, data, result, block, endpaper, board, cover, materials, packaging);

        result.MaterialLines = BuildMaterialLines(input, data, materials, endpaper, board, cover);
        result.Summary = BuildSummary(input, result, book, packaging);

        return result;
    }

    private static PartCalc CalculateTextBlock(OrderInput input, CalculatorData data, CalculationResult result)
    {
        var part = input.Block;
        var sheet = data.Formats[part.SheetFormat];
        var layout = CalculateLayout(sheet, part.WidthMm, part.HeightMm, part.GrainAllowanceMm, part.Pages / 2m);
        var thickness = part.Pages / 2m * part.Bulk;
        var weight = AreaWeight(part.WidthMm, part.HeightMm, part.Grammage, part.Pages / 2m);
        var fullSheetsPerBook = part.Pages / 2m / layout.ItemsPerSheet;
        var roundedSheetsPerBook = RoundExcel(fullSheetsPerBook);
        var totalSheets = roundedSheetsPerBook * input.Quantity;
        var extraSheets = roundedSheetsPerBook >= fullSheetsPerBook ? 0 : Math.Ceiling(input.Quantity / 4m);
        var totalCutOperations = CalculateCuts(totalSheets, part.Bulk, layout.ItemsPerSheet, 0);
        var laminationMeters = CalculateLaminationMeters(part.SheetFormat, totalSheets + extraSheets, part.LaminationSides);

        result.Sections.Add(new CalculationSection
        {
            Title = "1. Блок",
            Description = "Внутрішній блок рахується від формату книги, кількості сторінок, пухлості й розкладки на друкарському аркуші.",
            Steps =
            {
                Step("Товщина блоку", "сторінки / 2 * пухлість", Mm(thickness)),
                Step("Вага блоку", "ширина_м * висота_м * граматура * (сторінки / 2)", Gram(weight)),
                Step("Розкладка по прямій орієнтації", "floor(ширина_аркуша / (ширина+припуск)) * floor(висота_аркуша / (висота+припуск))", $"{layout.DirectItems} шт."),
                Step("Розкладка з поворотом", "floor(висота_аркуша / (ширина+припуск)) * floor(ширина_аркуша / (висота+припуск))", $"{layout.RotatedItems} шт."),
                Step("Обрана розкладка", "max(пряма, поворот)", $"{layout.ItemsPerSheet} шт. на аркуші"),
                Step("Друкарські аркуші на книгу", "(сторінки / 2) / виробів_на_аркуші", Number(fullSheetsPerBook)),
                Step("Листи на тираж", "округлені_листи_на_книгу * тираж + добір", $"{totalSheets + extraSheets:n0} листів"),
                Step("Різи блоку", "закладки_по_90мм * листи * різи_на_закладку", $"{totalCutOperations:n0} різів"),
                Step("Ламінація блоку", "якщо сторони > 0: листи * ширина_рулону_м * сторони + 8м", Meter(laminationMeters))
            }
        });

        return new PartCalc(part.SheetFormat, part.WidthMm, part.HeightMm, part.Pages, part.Grammage, part.Bulk, thickness, weight, layout, totalSheets, extraSheets, totalCutOperations, laminationMeters);
    }

    private static EndpaperCalc CalculateEndpaper(OrderInput input, CalculatorData data, CalculationResult result, PartCalc block)
    {
        var endpaper = input.Endpaper;
        var width = input.Block.WidthMm * 2m;
        var height = input.Block.HeightMm;
        var sheet = data.Formats[endpaper.SheetFormat];
        var layout = CalculateLayout(sheet, width, height, 4m, endpaper.Pages / 4m);
        var thickness = endpaper.Pages / 2m * endpaper.Bulk;
        var weight = AreaWeight(width, height, endpaper.Grammage, endpaper.Pages / 2m);
        var sheets = Math.Ceiling(input.Quantity / layout.ItemsPerSheet * 2m);
        var stripWidth = Math.Ceiling(8m + 2m + block.ThicknessMm + thickness + 2m + 8m);
        var stripHeight = height + 16m;
        var stripSheets = Math.Ceiling(input.Quantity / 105m);
        var cuts = CalculateCuts(sheets, endpaper.Bulk, layout.ItemsPerSheet, 76);
        var laminationMeters = CalculateLaminationMeters(endpaper.SheetFormat, sheets, endpaper.PrintEndpaper ? 1 : 0);

        result.Sections.Add(new CalculationSection
        {
            Title = "2. Форзац",
            Description = "Форзац має подвійний розмір від ширини блоку. Окремо враховується смуга корінця форзаца, обкладинки й розставів.",
            Steps =
            {
                Step("Формат форзаца", "ширина блоку * 2; висота блоку", $"{Number(width)} x {Number(height)} мм"),
                Step("Товщина форзаца", "сторінки / 2 * пухлість", Mm(thickness)),
                Step("Вага форзаца", "ширина_м * висота_м * граматура * (сторінки / 2)", Gram(weight)),
                Step("Виробів на аркуші", "краща з двох орієнтацій розкладки", $"{layout.ItemsPerSheet} шт."),
                Step("Аркуші форзаца", "ceil(тираж / виробів_на_аркуші * 2)", $"{sheets:n0} листів"),
                Step("Смуга корінця форзаца", "ceil(8 + 2 + товщина_блоку + товщина_форзаца + 2 + 8)", $"{Number(stripWidth)} x {Number(stripHeight)} мм"),
                Step("Листи книжкового матеріалу", "ceil(тираж / 105)", $"{stripSheets:n0} листів"),
                Step("Різи форзаца", "різи основного форзаца + різи смуги", $"{cuts:n0} різів")
            }
        });

        return new EndpaperCalc(width, height, thickness, weight, layout, sheets, stripWidth, stripHeight, stripSheets, cuts, laminationMeters);
    }

    private static BoardCalc CalculateBoard(OrderInput input, CalculatorData data, CalculationResult result, PartCalc block, EndpaperCalc endpaper)
    {
        var board = input.Board;
        var boardWidth = input.Block.WidthMm + board.BoardOffsetMm - (board.GapMm - board.BlockToSpineMm);
        var boardHeight = input.Block.HeightMm + board.BoardOffsetMm * 2m;
        var spineWidth = Math.Round(block.ThicknessMm + endpaper.ThicknessMm + board.ThicknessMm * 2m, 1);
        var sheet = data.Formats[board.SheetFormat];

        var sideLayout = CalculateLayout(sheet, boardWidth, boardHeight, 4m, 2m);
        var sideSheets = Math.Ceiling(input.Quantity * (2m / sideLayout.ItemsPerSheet));
        var sideWeight = AreaWeight(boardWidth, boardHeight, board.DensityGramPerSquareMeter, 2m);
        var sideCuts = (sideLayout.Horizontal + sideLayout.Vertical) * 2m * sideSheets;

        var spineLayout = CalculateLayout(sheet, spineWidth, boardHeight, 4m, 1m);
        var spineSheets = Math.Ceiling(input.Quantity * (1m / spineLayout.ItemsPerSheet));
        var spineWeight = AreaWeight(spineWidth, boardHeight, board.DensityGramPerSquareMeter, 1m);
        var spineCuts = (spineLayout.Horizontal + spineLayout.Vertical) * 2m * spineSheets;

        result.Sections.Add(new CalculationSection
        {
            Title = "3. Картон і корінець",
            Description = "Кришка твердої палітурки складається з двох картонних сторін і окремого корінця.",
            Steps =
            {
                Step("Картонна сторона", "ширина блоку + відступ обкладинки - (розстав - відступ блоку від корінця)", $"{Number(boardWidth)} x {Number(boardHeight)} мм"),
                Step("Товщина корінця", "товщина блоку + товщина форзаца + 2 * товщина картону", Mm(spineWidth)),
                Step("Картон на тираж", "ceil(тираж * 2 / виробів_на_аркуші)", $"{sideSheets:n0} листів"),
                Step("Корінець на тираж", "ceil(тираж / виробів_на_аркуші)", $"{spineSheets:n0} листів"),
                Step("Вага картону на книгу", "сторони + корінець за площею й щільністю", Gram(sideWeight + spineWeight)),
                Step("Різи картону", "різи сторін + різи корінця", $"{sideCuts + spineCuts:n0} різів")
            }
        });

        return new BoardCalc(boardWidth, boardHeight, spineWidth, sideSheets, spineSheets, sideWeight, spineWeight, sideCuts + spineCuts);
    }

    private static CoverCalc CalculateCover(OrderInput input, CalculatorData data, CalculationResult result, PartCalc block, BoardCalc board)
    {
        var cover = input.Cover;
        var width = cover.FlapMm + board.BoardWidthMm + input.Board.GapMm + board.SpineWidthMm + input.Board.GapMm + board.BoardWidthMm + cover.FlapMm;
        var height = input.Board.BoardOffsetMm + input.Board.ThicknessMm + board.BoardHeightMm + input.Board.ThicknessMm + input.Board.BoardOffsetMm;
        var sheet = data.Formats[cover.SheetFormat];
        var layout = CalculateLayout(sheet, width, height, 4m, 1m);
        var sheets = Math.Ceiling(input.Quantity / layout.ItemsPerSheet);
        var weight = AreaWeight(width, height, cover.Grammage, 1m);
        var cuts = CalculateCuts(sheets, cover.Bulk, layout.ItemsPerSheet, 0);
        var laminationMeters = CalculateLaminationMeters(cover.SheetFormat, sheets, cover.LaminationSides);

        result.Sections.Add(new CalculationSection
        {
            Title = "4. Обкладинка",
            Description = "Розмір покривного матеріалу збирається з клапанів, картонних сторін, розставів і корінця.",
            Steps =
            {
                Step("Ширина обкладинки", "клапан + картон + розстав + корінець + розстав + картон + клапан", Mm(width)),
                Step("Висота обкладинки", "відступ + картон + відступ", Mm(height)),
                Step("Вага обкладинки", "ширина_м * висота_м * граматура", Gram(weight)),
                Step("Виробів на аркуші", "краща орієнтація розкладки", $"{layout.ItemsPerSheet} шт."),
                Step("Листи обкладинки", "ceil(тираж / виробів_на_аркуші)", $"{sheets:n0} листів"),
                Step("Ламінація обкладинки", "листи * ширина_рулону_м * сторони + 8м", Meter(laminationMeters)),
                Step("Різи обкладинки", "закладки_по_90мм * листи * різи_на_закладку", $"{cuts:n0} різів")
            }
        });

        return new CoverCalc(width, height, weight, layout, sheets, cuts, laminationMeters);
    }

    private static BookCalc CalculateFinishedBook(OrderInput input, CalculationResult result, PartCalc block, EndpaperCalc endpaper, BoardCalc board, CoverCalc cover)
    {
        var width = input.Block.WidthMm + 7m;
        var height = input.Block.HeightMm + input.Board.BoardOffsetMm * 2m;
        var thickness = board.SpineWidthMm;
        var weight = Math.Ceiling(block.WeightGram + endpaper.WeightGram + board.SideWeightGram + board.SpineWeightGram + cover.WeightGram);
        var packWidth = Math.Ceiling((width + Math.Ceiling(thickness)) / 10m) * 10m;
        var packHeight = Math.Ceiling((height + Math.Ceiling(thickness)) / 10m) * 10m;

        result.Sections.Add(new CalculationSection
        {
            Title = "5. Готова книга",
            Description = "Після складання деталей визначається чистий формат, товщина, вага й мінімальний розмір індивідуального пакування.",
            Steps =
            {
                Step("Формат книги", "ширина блоку + 7мм; висота блоку + 2 * відступ картону", $"{Number(width)} x {Number(height)} мм"),
                Step("Товщина книги", "товщина корінця", Mm(thickness)),
                Step("Вага книги", "ceil(блок + форзац + обкладинка + картон + корінець)", Gram(weight)),
                Step("Індивідуальна упаковка", "округлення формату книги з товщиною до десятків", $"{Number(packWidth)} x {Number(packHeight)} мм")
            }
        });

        return new BookCalc(width, height, thickness, weight, packWidth, packHeight);
    }

    private static MaterialCalc CalculateMaterials(OrderInput input, CalculatorData data, CalculationResult result, PartCalc block, EndpaperCalc endpaper, BoardCalc board, CoverCalc cover, BookCalc book)
    {
        var purPerBook = (input.Block.HeightMm + 6m) * block.ThicknessMm * PurGlueFactor
                         + (input.Block.HeightMm + 6m) * 7m * PurGlueFactor;
        var purStarts = purPerBook * 15m * Math.Ceiling(input.Quantity / 800m) + purPerBook * Math.Ceiling(input.Quantity / 100m);
        var purTotalGram = purPerBook * input.Quantity + purStarts;
        var purWashGram = 60m * Math.Ceiling(input.Quantity / 800m);
        var endpaperGlueKg = ((input.Block.HeightMm / 10m) * 0.7m * 2m) * input.Quantity / WaterGlueAreaDivisor;
        var coverGlueKg = (cover.WidthMm / 10m) * (cover.HeightMm / 10m) * input.Quantity / WaterGlueAreaDivisor;
        var blockInsertGlueKg = (input.Block.WidthMm / 10m) * (input.Block.HeightMm / 10m) * 2m * input.Quantity / WaterGlueAreaDivisor;
        var waterGlueKg = endpaperGlueKg + coverGlueKg + blockInsertGlueKg;
        var headbandMeters = input.Block.Headband ? Math.Ceiling(block.ThicknessMm) * 2m * input.Quantity / 1000m : 0;
        var ribbonOneMeter = (decimal)Math.Sqrt(Math.Pow((double)(input.Block.WidthMm / 1000m), 2) + Math.Pow((double)(input.Block.HeightMm / 1000m), 2)) + 0.03m;
        var ribbonMeters = input.Cover.Ribbon ? ribbonOneMeter * input.Quantity : 0;
        var totalBookWeightKg = book.WeightGram / 1000m * input.Quantity;

        result.Sections.Add(new CalculationSection
        {
            Title = "6. Витрати матеріалів",
            Description = "Окремо рахуються клеї, змивка, каптал, ляссе, картон, плівка й вага всього тиражу.",
            Steps =
            {
                Step("ПУР на книгу", "(висота+6) * товщина блоку * 0.00065 + (висота+6) * 7 * 0.00065", Gram(purPerBook)),
                Step("ПУР старт/стоп", "ПУР_на_книгу * 15 * ceil(тираж/800) + ПУР_на_книгу * ceil(тираж/100)", Gram(purStarts)),
                Step("ПУР всього", "ПУР_на_книгу * тираж + старт/стоп", $"{Number(purTotalGram / 1000m)} кг"),
                Step("Змивка ПУР", "60 г * ceil(тираж / 800)", $"{Number(purWashGram / 1000m)} кг"),
                Step("Клей Акванс", "клей форзац-обкладинка + клей кришка + клей блоковставка", $"{Number(waterGlueKg)} кг"),
                Step("Каптал", "ceil(товщина блоку) * 2 * тираж / 1000", Meter(headbandMeters)),
                Step("Ляссе", "(діагональ блоку + 0.03м) * тираж", Meter(ribbonMeters)),
                Step("Вага тиражу", "вага книги * тираж / 1000", $"{Number(totalBookWeightKg)} кг")
            }
        });

        return new MaterialCalc(purTotalGram / 1000m, purWashGram / 1000m, waterGlueKg, headbandMeters, ribbonMeters, totalBookWeightKg);
    }

    private static PackagingCalc CalculatePackaging(OrderInput input, CalculationResult result, BookCalc book)
    {
        var directFlat = Math.Floor(input.Packaging.PackLengthMm / book.WidthMm) * Math.Floor(input.Packaging.PackWidthMm / book.HeightMm);
        var rotatedFlat = Math.Floor(input.Packaging.PackWidthMm / book.WidthMm) * Math.Floor(input.Packaging.PackLengthMm / book.HeightMm);
        var flatCapacity = Math.Max(directFlat, rotatedFlat);
        var heightCapacity = Math.Floor(Math.Min(input.Quantity, input.Packaging.PackHeightMm / book.ThicknessMm));
        var autoBooksPerPack = (int)Math.Min(input.Quantity, flatCapacity * heightCapacity);
        var booksPerPack = input.BooksPerPack == 0 ? Math.Max(autoBooksPerPack, 1) : input.BooksPerPack;
        var packs = (int)Math.Ceiling(input.Quantity / (decimal)booksPerPack);
        var packWeightKg = book.WeightGram * booksPerPack / 1000m;

        result.Sections.Add(new CalculationSection
        {
            Title = "7. Пакування",
            Description = "Калькулятор порівнює дві орієнтації книги в пачці й обмежує кількість висотою пачки.",
            Steps =
            {
                Step("Книг по площині", "max(пряма орієнтація, поворот)", $"{flatCapacity:n0} шт."),
                Step("Книг по висоті", "floor(висота пачки / товщина книги)", $"{heightCapacity:n0} шт."),
                Step("Книг у пачці", "якщо поле = 0, то площина * висота; інакше ручне значення", $"{booksPerPack:n0} шт."),
                Step("Кількість пачок", "ceil(тираж / книг_у_пачці)", $"{packs:n0} пачок"),
                Step("Вага пачки", "вага книги * книг_у_пачці / 1000", $"{Number(packWeightKg)} кг")
            }
        });

        return new PackagingCalc(booksPerPack, packs, packWeightKg);
    }

    private static void CalculateWork(OrderInput input, CalculatorData data, CalculationResult result, PartCalc block, EndpaperCalc endpaper, BoardCalc board, CoverCalc cover, MaterialCalc materials, PackagingCalc packaging)
    {
        var bookFormat = ResolveBookFormat(input.Block.WidthMm, input.Block.HeightMm);
        AddWork(result, data, "Блок", "Ламинация А2/В2", input.Block.SheetFormat, block.TotalSheets * input.Block.LaminationSides, 0, block.LaminationMeters > 0);
        AddWork(result, data, "Блок", "Порезка", input.Block.SheetFormat, block.Cuts, 0, true);
        AddWork(result, data, "Блок", "Листоподборка авто. (до 10л)", bookFormat, block.TotalSheets, 0.01m * block.TotalSheets, true);
        AddWork(result, data, "Блок", "Комплектовка", bookFormat, Math.Ceiling(block.TotalSheets / 4m), 0.01m * block.TotalSheets, true);
        AddWork(result, data, "Блок", "Пур биндер (до 20мм)", bookFormat, input.Quantity, materials.PurGlueKg * data.MaterialPrices["PurGlueKg"], true, customWorkCost: 1.35m * input.Quantity + 100m);
        AddWork(result, data, "Блок", "Зачистка до 25мм", bookFormat, input.Quantity, 0, true, customWorkCost: 0.5m * input.Quantity + 100m);
        AddWork(result, data, "Блок", "приклейка каптал", bookFormat, input.Quantity, materials.HeadbandMeters * data.MaterialPrices["HeadbandMeter"], input.Block.Headband, customWorkCost: input.Quantity);
        AddWork(result, data, "Блок", "Ляссэ", bookFormat, input.Quantity, materials.RibbonMeters * data.MaterialPrices["RibbonMeter"], input.Cover.Ribbon, customWorkCost: 0.5m * input.Quantity);

        AddWork(result, data, "Обкладинка", "Ламинация А2/В2", input.Cover.SheetFormat, cover.TotalSheets * input.Cover.LaminationSides, cover.LaminationMeters * data.MaterialPrices["FilmMeter"], cover.LaminationMeters > 0);
        AddWork(result, data, "Обкладинка", "Порезка", input.Cover.SheetFormat, cover.Cuts, 0, true);
        AddWork(result, data, "Обкладинка", "Порезка карт., пласт.", input.Board.SheetFormat, input.Quantity, (board.BoardSheets + board.SpineSheets) * data.MaterialPrices["BoardSheet"], true, customWorkCost: 2000m);
        AddWork(result, data, "Обкладинка", $"Составление крышки без форзаца {bookFormat}", bookFormat, input.Quantity, materials.CoverGlueKg * data.MaterialPrices["WaterGlueKg"], true, customWorkCost: RateByFormat(bookFormat, 5, 6, 7, 8) * input.Quantity);
        AddWork(result, data, "Обкладинка", $"Блоковставка {bookFormat}", bookFormat, input.Quantity, materials.BlockInsertGlueKg * data.MaterialPrices["WaterGlueKg"], true, customWorkCost: RateByFormat(bookFormat, 3, 4, 5, 6) * input.Quantity);
        AddWork(result, data, "Обкладинка", "штриховка", bookFormat, input.Quantity, 0, true, customWorkCost: input.Quantity);
        AddWork(result, data, "Обкладинка", "Упаковка в пачки", "", packaging.Packs, 0, true);

        AddWork(result, data, "Форзац", "Порезка", input.Endpaper.SheetFormat, endpaper.Cuts, input.Endpaper.PrintEndpaper ? endpaper.Sheets * data.MaterialPrices["EndpaperSheet"] : 0, true);
        AddWork(result, data, "Форзац", "Биговка авт.", "A3", input.Quantity * 2m, 0.5m * input.Quantity * 2m, input.Endpaper.PrintEndpaper);
        AddWork(result, data, "Форзац", "Фальцовка (ручная)", "A3", input.Quantity * 2m, 0, true, customWorkCost: 0.25m * input.Quantity * 2m);
        AddWork(result, data, "Форзац", "Форзац обл.", bookFormat, input.Quantity, materials.EndpaperGlueKg * data.MaterialPrices["WaterGlueKg"] + endpaper.StripSheets * data.MaterialPrices["BookClothSheet"], true, customWorkCost: input.Quantity);

        result.Sections.Add(new CalculationSection
        {
            Title = "8. Роботи",
            Description = "Для кожної операції береться норматив із JSON. Час = приладка + час на одиницю * кількість + підготовка на зміни.",
            Steps =
            {
                Step("Норматив операції", "рядок шукається за назвою операції у calculator-data.json", $"{result.WorkLines.Count(x => x.Enabled)} активних операцій"),
                Step("Час операції", "якщо кількість=0: 0; інакше hoursPerUnit * кількість + setup + підготовка * ceil(кількість / unitsPerShift)", $"{Number(result.WorkLines.Sum(x => x.Hours))} год"),
                Step("Вартість робіт", "ставка/ручна ціна * кількість або hours * hourRate", Money(result.WorkLines.Sum(x => x.WorkCost))),
                Step("Матеріали в операціях", "плівка, клей, картон, каптал, ляссе, форзац", Money(result.WorkLines.Sum(x => x.MaterialCost)))
            }
        });
    }

    private static List<MaterialLine> BuildMaterialLines(OrderInput input, CalculatorData data, MaterialCalc materials, EndpaperCalc endpaper, BoardCalc board, CoverCalc cover)
    {
        return new List<MaterialLine>
        {
            Material("Картон палітурний", board.BoardSheets + board.SpineSheets, "листів", data.MaterialPrices["BoardSheet"]),
            Material("Плівка обкладинки", cover.LaminationMeters, "м", data.MaterialPrices["FilmMeter"]),
            Material("ПУР клей", materials.PurGlueKg, "кг", data.MaterialPrices["PurGlueKg"]),
            Material("Змивка ПУР", materials.PurWashKg, "кг", data.MaterialPrices["PurGlueKg"]),
            Material("Клей Акванс", materials.WaterGlueKg, "кг", data.MaterialPrices["WaterGlueKg"]),
            Material("Каптал", materials.HeadbandMeters, "м", data.MaterialPrices["HeadbandMeter"]),
            Material("Ляссе", materials.RibbonMeters, "м", data.MaterialPrices["RibbonMeter"]),
            Material("Форзац", input.Endpaper.PrintEndpaper ? 0 : endpaper.Sheets, "листів", data.MaterialPrices["EndpaperSheet"]),
            Material("Книжковий матеріал", endpaper.StripSheets, "листів", data.MaterialPrices["BookClothSheet"])
        };
    }

    private static SummaryResult BuildSummary(OrderInput input, CalculationResult result, BookCalc book, PackagingCalc packaging)
    {
        var totalHours = Math.Round(result.WorkLines.Where(x => x.Enabled).Sum(x => x.Hours), 2);
        var materialCost = Math.Round(result.WorkLines.Where(x => x.Enabled).Sum(x => x.MaterialCost), 2);
        var workCost = Math.Round(result.WorkLines.Where(x => x.Enabled).Sum(x => x.WorkCost), 2);
        var totalCost = materialCost + workCost;
        var priceWithMarkup = Math.Round(totalCost * (1m + input.MarkupPercent / 100m), 2);

        return new SummaryResult
        {
            TotalHours = totalHours,
            MaterialCost = materialCost,
            WorkCost = workCost,
            TotalCost = totalCost,
            CostPerBook = Math.Round(totalCost / input.Quantity, 2),
            PriceWithMarkup = priceWithMarkup,
            PricePerBookWithMarkup = Math.Round(priceWithMarkup / input.Quantity, 2),
            FinishedWidthMm = book.WidthMm,
            FinishedHeightMm = book.HeightMm,
            FinishedThicknessMm = book.ThicknessMm,
            BookWeightGram = book.WeightGram,
            BooksPerPack = packaging.BooksPerPack,
            Packs = packaging.Packs
        };
    }

    private static LayoutCalc CalculateLayout(SheetFormat sheet, decimal widthMm, decimal heightMm, decimal allowanceMm, decimal leavesPerBook)
    {
        var width = widthMm + allowanceMm;
        var height = heightMm + allowanceMm;
        var directHorizontal = Math.Floor(sheet.WidthMm / width);
        var directVertical = Math.Floor(sheet.HeightMm / height);
        var rotatedHorizontal = Math.Floor(sheet.HeightMm / width);
        var rotatedVertical = Math.Floor(sheet.WidthMm / height);
        var direct = directHorizontal * directVertical;
        var rotated = rotatedHorizontal * rotatedVertical;
        var items = Math.Max(direct, rotated);

        return new LayoutCalc(directHorizontal, directVertical, rotatedHorizontal, rotatedVertical, direct, rotated, Math.Max(items, 1), leavesPerBook);
    }

    private static decimal CalculateCuts(decimal sheets, decimal thicknessMm, decimal itemsPerSheet, decimal additionalCuts)
    {
        var sheetsInStack = Math.Max(Math.Floor(StackCutHeightMm / Math.Max(thicknessMm, 0.001m)), 1m);
        var stacks = Math.Ceiling(sheets / sheetsInStack);
        var cutsPerStack = Math.Max((itemsPerSheet + 2m) * 2m, 1m);
        return stacks * cutsPerStack + additionalCuts;
    }

    private static decimal CalculateLaminationMeters(string format, decimal sheets, int sides)
    {
        if (sides <= 0)
        {
            return 0;
        }

        var rollWidth = format switch
        {
            "A1" => 0.9m,
            "B2" => 0.7m,
            "A2" => 0.64m,
            "A3" => 0.32m,
            _ => 0.64m
        };

        return sheets * rollWidth * sides + 8m;
    }

    private static void AddWork(CalculationResult result, CalculatorData data, string group, string operation, string format, decimal quantity, decimal materialCost, bool enabled, decimal? customWorkCost = null)
    {
        var norm = data.OperationNorms.FirstOrDefault(x => string.Equals(x.Name, operation, StringComparison.OrdinalIgnoreCase))
                   ?? data.OperationNorms.FirstOrDefault(x => operation.StartsWith(x.Name, StringComparison.OrdinalIgnoreCase));

        if (norm is null)
        {
            norm = new OperationNorm { Name = operation, SetupHours = 0.25m, HoursPerUnit = 0.005m, UnitsPerShift = 1000m, HourRate = 75m, EnergyPerHour = 0.5m };
        }

        var hours = enabled && quantity > 0
            ? norm.HoursPerUnit * quantity + norm.SetupHours + Math.Max(norm.SetupHours, 0) * Math.Ceiling(quantity / Math.Max(norm.UnitsPerShift, 1))
            : 0;
        var workCost = enabled
            ? customWorkCost ?? Math.Round(hours * norm.HourRate, 2)
            : 0;

        result.WorkLines.Add(new WorkLine
        {
            Group = group,
            Operation = operation,
            Format = format,
            Quantity = quantity,
            Hours = Math.Round(hours, 2),
            MaterialCost = Math.Round(enabled ? materialCost : 0, 2),
            WorkCost = Math.Round(workCost, 2),
            Enabled = enabled
        });
    }

    private static decimal RateByFormat(string format, decimal a6, decimal a5, decimal a4, decimal a3)
    {
        return format switch
        {
            "A6" => a6,
            "A5" => a5,
            "A4" => a4,
            _ => a3
        };
    }

    private static string ResolveBookFormat(decimal widthMm, decimal heightMm)
    {
        var area = widthMm * heightMm;
        if (area < 17050m) return "A6";
        if (area < 32250m) return "A5";
        if (area < 64500m) return "A4";
        return "A3";
    }

    private static decimal AreaWeight(decimal widthMm, decimal heightMm, decimal grammage, decimal leaves)
    {
        return widthMm / 1000m * (heightMm / 1000m) * grammage * leaves;
    }

    private static decimal RoundExcel(decimal value)
    {
        return Math.Round(value, 0, MidpointRounding.AwayFromZero);
    }

    private static MaterialLine Material(string name, decimal quantity, string unit, decimal unitPrice)
    {
        return new MaterialLine
        {
            Name = name,
            Quantity = Math.Round(quantity, 3),
            Unit = unit,
            UnitPrice = unitPrice,
            Total = Math.Round(quantity * unitPrice, 2)
        };
    }

    private static CalculationStep Step(string label, string formula, string value)
    {
        return new CalculationStep { Label = label, Formula = formula, Value = value };
    }

    private static string Number(decimal value)
    {
        return value.ToString("0.##");
    }

    private static string Money(decimal value)
    {
        return $"{value:n2} грн";
    }

    private static string Mm(decimal value)
    {
        return $"{Number(value)} мм";
    }

    private static string Gram(decimal value)
    {
        return $"{Number(value)} г";
    }

    private static string Meter(decimal value)
    {
        return $"{Number(value)} м";
    }

    private sealed record LayoutCalc(decimal DirectHorizontal, decimal DirectVertical, decimal RotatedHorizontal, decimal RotatedVertical, decimal DirectItems, decimal RotatedItems, decimal ItemsPerSheet, decimal LeavesPerBook)
    {
        public decimal Horizontal => DirectItems >= RotatedItems ? DirectHorizontal : RotatedHorizontal;

        public decimal Vertical => DirectItems >= RotatedItems ? DirectVertical : RotatedVertical;
    }

    private sealed record PartCalc(string Format, decimal WidthMm, decimal HeightMm, int Pages, decimal Grammage, decimal Bulk, decimal ThicknessMm, decimal WeightGram, LayoutCalc Layout, decimal TotalSheets, decimal ExtraSheets, decimal Cuts, decimal LaminationMeters);

    private sealed record EndpaperCalc(decimal WidthMm, decimal HeightMm, decimal ThicknessMm, decimal WeightGram, LayoutCalc Layout, decimal Sheets, decimal StripWidthMm, decimal StripHeightMm, decimal StripSheets, decimal Cuts, decimal LaminationMeters);

    private sealed record BoardCalc(decimal BoardWidthMm, decimal BoardHeightMm, decimal SpineWidthMm, decimal BoardSheets, decimal SpineSheets, decimal SideWeightGram, decimal SpineWeightGram, decimal Cuts);

    private sealed record CoverCalc(decimal WidthMm, decimal HeightMm, decimal WeightGram, LayoutCalc Layout, decimal TotalSheets, decimal Cuts, decimal LaminationMeters);

    private sealed record BookCalc(decimal WidthMm, decimal HeightMm, decimal ThicknessMm, decimal WeightGram, decimal PackWidthMm, decimal PackHeightMm);

    private sealed record MaterialCalc(decimal PurGlueKg, decimal PurWashKg, decimal WaterGlueKg, decimal HeadbandMeters, decimal RibbonMeters, decimal TotalBookWeightKg)
    {
        public decimal EndpaperGlueKg => WaterGlueKg * 0.02m;

        public decimal CoverGlueKg => WaterGlueKg * 0.6m;

        public decimal BlockInsertGlueKg => WaterGlueKg * 0.38m;
    }

    private sealed record PackagingCalc(int BooksPerPack, int Packs, decimal PackWeightKg);
}
