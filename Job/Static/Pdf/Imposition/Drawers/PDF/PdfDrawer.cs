using JobSpace.Profiles;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Models;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Sheet;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF
{
    public class PdfDrawer
    {

        public EventHandler<int> StartEvent { get; set; } = delegate { };
        public EventHandler<int> ProcessingEvent { get; set; } = delegate { };
        public EventHandler FinishEvent { get; set; } = delegate { };

        Profile _profile;

        public PdfDrawer(Profile profile)
        {
            _profile= profile;
        }

        public void Draw(ProductPart impos)
        {
            PDFlib p = new PDFlib();

            try
            {
                impos.ExportParameters.CreateOutputFileName();
                var targetFile = impos.ExportParameters.OutputFilePath;

                p.begin_document(targetFile, "");

                StartEvent(this, impos.PrintSheets.Count);

                for (int i = 0; i < impos.PrintSheets.Count; i++)
                {

                    ProcessingEvent(this, i + 1);

                    var sheet = impos.PrintSheets[i];

                    TextVariablesService.SetValue(ValueList.SheetIdx, i + 1);
                    TextVariablesService.SetValue(ValueList.SheetFormat, $"{sheet.W}x{sheet.H}");
                    TextVariablesService.SetValue(ValueList.SheetDesc, sheet.Description);
                    TextVariablesService.SetValue(ValueList.CurDate, DateTime.Now.ToString());
                    TextVariablesService.SetValue(ValueList.SheetCount, sheet.Count);

                    CropMarksService.FixCropMarks(sheet);

                    switch (sheet.SheetPlaceType)
                    {
                        case TemplateSheetPlaceType.SingleSide:
                            TextVariablesService.SetValue(ValueList.SheetSide, "Без звороту");
                            DrawSheet.Front(p, impos, sheet);
                            break;

                        case TemplateSheetPlaceType.Sheetwise:

                            TextVariablesService.SetValue(ValueList.SheetSide, "Лице");
                            DrawSheet.Front(p, impos, sheet);
                            TextVariablesService.SetValue(ValueList.SheetSide, "Зворот");
                            DrawSheet.Back(p, impos, sheet);
                            break;

                        case TemplateSheetPlaceType.WorkAndTurn:
                            TextVariablesService.SetValue(ValueList.SheetSide, "Свій зворот");
                            DrawSheet.WorkAndTurn(p, impos, sheet);
                            break;

                        case TemplateSheetPlaceType.WorkAndTumble:
                            TextVariablesService.SetValue(ValueList.SheetSide, "Клапан-хвіст");
                            DrawSheet.WorkAndTurn(p, impos, sheet);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                p.end_document("");
            }
            catch (PDFlibException)
            {
            }
            finally
            {
                p?.Dispose();

                FinishEvent(this, null);

                if (impos.ExportParameters.SavePrintSheetToOrderFolder)
                {
                    var orderFolder = impos.ExportParameters.OutputFolder;
                    var orderFileName = Path.GetFileNameWithoutExtension(impos.ExportParameters.OutputFilePath);

                    var orderFile = Path.Combine(orderFolder, Path.GetFileNameWithoutExtension(orderFileName) + ".json");
                    _profile.ImposService.SavePrintSheets(impos.PrintSheets, orderFile);
                }
            }
        }
    }
}
