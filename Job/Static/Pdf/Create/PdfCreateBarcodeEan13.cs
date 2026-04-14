using Interfaces.FileBrowser;
using Interfaces.Plugins;
using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using JobSpace.UserForms.PDF;
using iText.Kernel.Geom;
using System.IO;

namespace JobSpace.Static.Pdf.Create
{
    [PdfTool("Створити","штрих-код EAN-13",Icon = "create_barcode_ean13",Order =10)]
    public class PdfCreateBarcodeEan13 : IPdfTool
    {
        string _code;

        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormCreateBarcodeEan13())
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _code = form.Barcode;
                    return true;
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            string filename = System.IO.Path.Combine(context.FileManager.Settings.CurFolder,$"{_code}.pdf");

            PdfWriter writer = new PdfWriter(filename);
            PdfDocument pdf = new PdfDocument(writer);


            BarcodeEAN barcode = new BarcodeEAN(pdf);
            barcode.SetCode(_code);
            barcode.SetCodeType(BarcodeEAN.EAN13);
            DeviceCmyk black = new DeviceCmyk(0f, 0f, 0f, 1f);

            var form = barcode.CreateFormXObject(pdf);

            float width = form.GetWidth();
            float height = form.GetHeight();

            var page = pdf.AddNewPage(new PageSize(width, height));

            PdfCanvas canvas = new PdfCanvas(page);
            canvas.AddXObjectAt(form, 0, 0);

            pdf.Close();
        }
    }
}
