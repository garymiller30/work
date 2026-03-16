using Interfaces.Plugins;
using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using JobSpace.UserForms.PDF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Geom;
using Interfaces.FileBrowser;

namespace JobSpace.Static.Pdf.Create
{
    [PdfTool("Створити", "штрих-код Code128", Icon = "create_barcode_code128",Order =10)]
    public class PdfCreateBarcodeCode128 : IPdfTool
    {

        string _code;

        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormCreateBarcodeCode128())
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
            string filename = System.IO.Path.Combine(context.FileManager.Settings.CurFolder, $"{_code}.pdf");

            PdfWriter writer = new PdfWriter(filename);
            PdfDocument pdf = new PdfDocument(writer);
            

            Barcode128 barcode = new Barcode128(pdf);
            barcode.SetX(0.8f);   // ширина модуля
            barcode.SetBarHeight(40f);
            barcode.SetCode(_code);
            DeviceCmyk black = new DeviceCmyk(0f, 0f, 0f, 1f);

            var form = barcode.CreateFormXObject(pdf);

            float width = form.GetWidth();
            float height = form.GetHeight();

            var page = pdf.AddNewPage(new PageSize(width, height));
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.AddXObjectAt(form, 0, 0);

            //// генеруємо штрихкод
            //barcode.PlaceBarcode(canvas, black, black);

            pdf.Close();
        }
    }
}
