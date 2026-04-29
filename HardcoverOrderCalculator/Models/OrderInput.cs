using System.ComponentModel.DataAnnotations;

namespace HardcoverOrderCalculator.Models;

public sealed class OrderInput
{
    [Range(1, 100000)]
    public int Quantity { get; set; } = 280;

    [Range(0, 10000)]
    public int BooksPerPack { get; set; } = 20;

    [Range(0, 500)]
    public decimal MarkupPercent { get; set; } = 120;

    public PartInput Block { get; set; } = new()
    {
        Name = "Блок",
        WidthMm = 148,
        HeightMm = 210,
        Pages = 192,
        SheetFormat = "A2",
        PaperName = "Офсет 90 г/м2, пухл. 0.118",
        Grammage = 90,
        Bulk = 0.118m,
        GrainAllowanceMm = 3,
        LaminationSides = 0,
        Headband = true
    };

    public CoverInput Cover { get; set; } = new();

    public EndpaperInput Endpaper { get; set; } = new();

    public BoardInput Board { get; set; } = new();

    public PackagingInput Packaging { get; set; } = new();
}

public sealed class PartInput
{
    public string Name { get; set; } = "";

    [Range(1, 5000)]
    public decimal WidthMm { get; set; }

    [Range(1, 5000)]
    public decimal HeightMm { get; set; }

    [Range(1, 5000)]
    public int Pages { get; set; }

    public string SheetFormat { get; set; } = "A2";

    public string PaperName { get; set; } = "";

    [Range(1, 2000)]
    public decimal Grammage { get; set; }

    [Range(0.001, 20)]
    public decimal Bulk { get; set; }

    [Range(0, 100)]
    public decimal GrainAllowanceMm { get; set; } = 3;

    [Range(0, 2)]
    public int LaminationSides { get; set; }

    public bool Headband { get; set; }
}

public sealed class CoverInput
{
    public string SheetFormat { get; set; } = "B2";

    public string PaperName { get; set; } = "Офсет 150 г/м2, пухл. 0.175";

    public decimal Grammage { get; set; } = 150;

    public decimal Bulk { get; set; } = 0.175m;

    public decimal FlapMm { get; set; } = 20;

    public int LaminationSides { get; set; } = 1;

    public bool Ribbon { get; set; } = true;
}

public sealed class EndpaperInput
{
    public string SheetFormat { get; set; } = "B1";

    public string PaperName { get; set; } = "Офсет 150 г/м2, пухл. 0.175";

    public decimal Grammage { get; set; } = 150;

    public decimal Bulk { get; set; } = 0.175m;

    public int Pages { get; set; } = 8;

    public bool PrintEndpaper { get; set; }
}

public sealed class BoardInput
{
    public string SheetFormat { get; set; } = "B1";

    public decimal ThicknessMm { get; set; } = 2;

    public decimal DensityGramPerSquareMeter { get; set; } = 886m;

    public decimal BoardOffsetMm { get; set; } = 3;

    public decimal GapMm { get; set; } = 8;

    public decimal BlockToSpineMm { get; set; } = 4;

    public decimal CreaseThicknessMm { get; set; } = 4;
}

public sealed class PackagingInput
{
    public decimal PackLengthMm { get; set; } = 333;

    public decimal PackWidthMm { get; set; } = 270;

    public decimal PackHeightMm { get; set; } = 200;
}
