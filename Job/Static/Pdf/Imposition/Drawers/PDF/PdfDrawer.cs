using Interfaces.Pdf.Imposition;
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
using System.Linq;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF
{
    public class PdfDrawer
    {

        public EventHandler<int> StartEvent { get; set; } = delegate { };
        public EventHandler<int> ProcessingEvent { get; set; } = delegate { };
        public EventHandler FinishEvent { get; set; } = delegate { };

        public int[] CustomSheets { get; set; } = null;

        public bool IsCancelled { get; set; } = false;

        GlobalImposParameters _imposParam;

        public PdfDrawer(GlobalImposParameters imposParam)
        {
            _imposParam = imposParam;
        }

        public void Draw(ProductPart impos)
        {
            PDFlib p = new PDFlib();

            try
            {
                impos.ExportParameters.CreateOutputFileName(_imposParam.TextVariables);
                var targetFile = impos.ExportParameters.OutputFilePath;

                int[] range;

                if (CustomSheets == null)
                {
                    range = Enumerable.Range(0, impos.PrintSheets.Count).ToArray();
                }
                else
                {
                    range = CustomSheets;
                }


                p.begin_document(targetFile, "optimize=true");

                StartEvent(this, range.Length);

                _imposParam.PdfDrawParameters.LayerPrint = p.define_layer("Print", "");
                _imposParam.PdfDrawParameters.LayerProof = p.define_layer("Proof", "");

                foreach (var i in range)
                {

                    if (IsCancelled)
                        break;

                    int pos = Array.IndexOf(range, i) + 1;
                    // 
                    ProcessingEvent(this, pos);

                    var sheet = impos.PrintSheets[i];

                    _imposParam.TextVariables.SetValue(ValueList.SheetIdx, i + 1);
                    _imposParam.TextVariables.SetValue(ValueList.SheetFormat, $"{sheet.W}x{sheet.H}");
                    _imposParam.TextVariables.SetValue(ValueList.SheetDesc, sheet.Description);
                    _imposParam.TextVariables.SetValue(ValueList.CurDate, DateTime.Now.ToString());
                    _imposParam.TextVariables.SetValue(ValueList.SheetCount, sheet.Count);

                    //CropMarksService.FixCropMarks(sheet, _imposParam);

                    switch (sheet.SheetPlaceType)
                    {
                        case TemplateSheetPlaceType.SingleSide:
                            _imposParam.TextVariables.SetValue(ValueList.SheetSide, "Без звороту");
                            DrawSheet.Front(p, impos, sheet,_imposParam);
                            break;

                        case TemplateSheetPlaceType.Sheetwise:

                            _imposParam.TextVariables.SetValue(ValueList.SheetSide, "Лице");
                            DrawSheet.Front(p, impos, sheet,_imposParam);
                            _imposParam.TextVariables.SetValue(ValueList.SheetSide, "Зворот");
                            DrawSheet.Back(p, impos, sheet,_imposParam);
                            break;

                        case TemplateSheetPlaceType.WorkAndTurn:
                            _imposParam.TextVariables.SetValue(ValueList.SheetSide, "Свій зворот");
                            DrawSheet.WorkAndTurn(p, impos, sheet, _imposParam);
                            break;

                        case TemplateSheetPlaceType.WorkAndTumble:
                            _imposParam.TextVariables.SetValue(ValueList.SheetSide, "Клапан-хвіст");
                            DrawSheet.WorkAndTumble(p, impos, sheet, _imposParam);
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
                    _imposParam.Profile.ImposService.SavePrintSheets(impos.PrintSheets, orderFile);
                }
            }
        }

        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}
