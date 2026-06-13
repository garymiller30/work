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
        private const string PDF_OPTIMIZE_PARAM = "optimize=true";

        public event EventHandler<int>? StartEvent;
        public event EventHandler<int>? ProcessingEvent;
        public event EventHandler? FinishEvent;

        public int[]? CustomSheets { get; set; } = null;

        public bool IsCancelled { get; set; } = false;

        private readonly GlobalImposParameters _imposParam;

        public PdfDrawer(GlobalImposParameters imposParam)
        {
            _imposParam = imposParam;
        }

        public void Draw(ProductPart impos)
        {
            if (impos == null)
                throw new ArgumentNullException(nameof(impos));

            using PDFlib p = new PDFlib();

            try
            {
                impos.ExportParameters.CreateOutputFileName(_imposParam.TextVariables);
                var targetFile = impos.ExportParameters.OutputFilePath;

                int[] range = GetSheetIndices(impos);

                p.begin_document(targetFile, PDF_OPTIMIZE_PARAM);

                StartEvent?.Invoke(this, range.Length);

                InitializeLayers(p);

                for (int idx = 0; idx < range.Length; idx++)
                {

                    if (IsCancelled) break;

                    int sheetIndex = range[idx];

                    ProcessingEvent?.Invoke(this, idx + 1);

                    var sheet = impos.PrintSheets[sheetIndex];

                    _imposParam.TextVariables.SetValue(ValueList.SheetIdx, sheetIndex + 1);
                    _imposParam.TextVariables.SetValue(ValueList.SheetFormat, $"{sheet.W}x{sheet.H}");
                    _imposParam.TextVariables.SetValue(ValueList.SheetDesc, sheet.Description);
                    _imposParam.TextVariables.SetValue(ValueList.CurDate, DateTime.Now.ToString());
                    _imposParam.TextVariables.SetValue(ValueList.SheetCount, sheet.Count);

                    switch (sheet.SheetPlaceType)
                    {
                        case TemplateSheetPlaceType.SingleSide:
                            DrawSingleSide(p, impos, sheet);

                            break;

                        case TemplateSheetPlaceType.Sheetwise:
                            DrawSheetwise(p, impos, sheet);

                            break;

                        case TemplateSheetPlaceType.WorkAndTurn:
                            DrawWorkAndTurn(p, impos, sheet);

                            break;

                        case TemplateSheetPlaceType.WorkAndTumble:
                            DrawWorkAndTumble(p, impos, sheet);

                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                p.end_document("");
            }
            catch (PDFlibException ex)
            {
                Logger.Log.Error(null, nameof(PdfDrawer), ex.Message);
            }
            finally
            {
                FinishEvent?.Invoke(this, EventArgs.Empty);
                SavePrintSheetsIfNeeded(impos);
            }
        }

        private void DrawWorkAndTumble(PDFlib p, ProductPart impos, PrintSheet sheet)
        {
            _imposParam.TextVariables.SetValue(ValueList.SheetSide, "Клапан-хвіст");
            DrawSheet.WorkAndTumble(p, impos, sheet, _imposParam);
        }

        private void DrawWorkAndTurn(PDFlib p, ProductPart impos, PrintSheet sheet)
        {
            _imposParam.TextVariables.SetValue(ValueList.SheetSide, "Свій зворот");
            DrawSheet.WorkAndTurn(p, impos, sheet, _imposParam);
        }

        private void DrawSheetwise(PDFlib p, ProductPart impos, PrintSheet sheet)
        {
            _imposParam.TextVariables.SetValue(ValueList.SheetSide, "Лице");
            DrawSheet.Front(p, impos, sheet, _imposParam);
            _imposParam.TextVariables.SetValue(ValueList.SheetSide, "Зворот");
            DrawSheet.Back(p, impos, sheet, _imposParam);
        }

        private void DrawSingleSide(PDFlib p, ProductPart impos, PrintSheet sheet)
        {
            _imposParam.TextVariables.SetValue(ValueList.SheetSide, "Без звороту");
            DrawSheet.Front(p, impos, sheet, _imposParam);
        }

        private void InitializeLayers(PDFlib p)
        {
            _imposParam.PdfDrawParameters.LayerPrint = p.define_layer(Constants.PRINT_STRING, "");
            _imposParam.PdfDrawParameters.LayerProof = p.define_layer(Constants.PROOF_STRING, "");
        }

        private int[] GetSheetIndices(ProductPart impos) => CustomSheets ?? Enumerable.Range(0, impos.PrintSheets.Count).ToArray();
        private void SavePrintSheetsIfNeeded(ProductPart impos)
        {
            if (!impos.ExportParameters.SavePrintSheetToOrderFolder) return;

            var orderFolder = impos.ExportParameters.OutputFolder;
            var orderFileName = Path.GetFileNameWithoutExtension(impos.ExportParameters.OutputFilePath);

            var orderFile = Path.Combine(orderFolder, Path.GetFileNameWithoutExtension(orderFileName) + ".json");
            _imposParam.Profile.ImposService.SavePrintSheets(impos.PrintSheets, orderFile);

        }
        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}
