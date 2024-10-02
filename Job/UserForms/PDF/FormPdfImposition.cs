using BrightIdeasSoftware;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF;
using JobSpace.Static.Pdf.Imposition.Drawers.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.Impos;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding;
using JobSpace.UserForms.PDF.ImposItems;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.UserForms.PDF
{
    public partial class FormPdfImposition : KryptonForm
    {
        string _curJobFolder;
        private IEnumerable<string> _files;

        List<PdfFile> _pdfFiles = new List<PdfFile>();

        ImposToolsParameters _parameters = new ImposToolsParameters();
        ControlBindParameters _controlBindParameters = new ControlBindParameters();
        int printId = 1;
        ProductPart _productPart;

        public FormPdfImposition()
        {
            InitializeComponent();
            InitBindParameters();
            InitImposTools();

            addTemplateSheetControl1.OnSheetSelected += OnTemplateSheetSelected;
            printSheetsControl1.OnPrintSheetsChanged += OnTemplateSheetSelected;
            printSheetsControl1.OnPrintSheetDeleted += OnPrintSheetDeleted;
            addTemplateSheetControl1.OnSheetAddToPrint += OnAddSheetToPrintEvent;
            addTemplateSheetControl1.OnSheetAddManyToPrint += OnSheetAddManyToPrintEvent;
            PrintSheet.ResetId();
        }

        private void OnPrintSheetDeleted(object sender, PrintSheet e)
        {
            runListControl1.ReassignPrintSheets(printSheetsControl1.GetSheets());
        }

        private void OnSheetAddManyToPrintEvent(object sender, TemplateSheet e)
        {
            
            // Отримати сторінки, що не задіяні
            int cnt = runListControl1.GetUnassignedPagesCount();
            int maxId = e.TemplatePageContainer.GetMaxIdx();
            int sheetCnt = cnt / maxId;

            for (int i = 0; i<sheetCnt; i++)
            {
                AddPrintSheet(e);
            }
        }

        void AddPrintSheet(TemplateSheet e)
        {
            PrintSheet sheet = PrintSheet.ConvertTemplateSheetToPrintSheet(e);
            runListControl1.AssignPrintSheet(sheet);
            printSheetsControl1.AddSheet(sheet);
        }

        private void OnAddSheetToPrintEvent(object sender, TemplateSheet e)
        {
            AddPrintSheet(e);

        }

        private void OnTemplateSheetSelected(object sender, TemplateSheet e)
        {
            previewControl1.SetSheet(e);
        }

        private void InitBindParameters()
        {
            _controlBindParameters.PdfFiles = _pdfFiles;
            
            pdfFileListControl1.SetControlBindParameters(_controlBindParameters);

        }

        private void InitImposTools()
        {
            previewControl1.InitBindParameters(_parameters);
        }

     

        public FormPdfImposition(IEnumerable<string> files, string curFolder) : this()
        {
            _files = files;
            _curJobFolder = curFolder;
            int id = 1;
            foreach (var file in _files)
            {
                var pdfFile = new PdfFile(file) { Id = id++ };
                _pdfFiles.Add(pdfFile);
                runListControl1.AddPages(ImposRunList.CreatePagesFromFile(pdfFile));
            }

            pdfFileListControl1.AddFiles(_pdfFiles);

            _controlBindParameters.MasterPage.W = _pdfFiles[0].Pages[0].Trim.W;
            _controlBindParameters.MasterPage.H = _pdfFiles[0].Pages[0].Trim.H;
            _controlBindParameters.MasterPage.Bleeds = (_pdfFiles[0].Pages[0].Media.W - _pdfFiles[0].Pages[0].Trim.W)/2;
            _controlBindParameters.MasterPage.SetMarginsLikeBleed();

            addTemplateSheetControl1.SetControlBindParameters(_controlBindParameters);
            printSheetsControl1.SetControlBindParameters(_controlBindParameters);

            //LoadImposFromFile();

        }

        private void LoadImposFromFile()
        {
            string folderPath = Path.Combine(_curJobFolder, ".impos");
            if (Directory.Exists(folderPath))
            {
                string filePath = Path.Combine(folderPath, "imposition.json");

                if (File.Exists(filePath))
                {
                    var str = File.ReadAllText(filePath);
                    _productPart = JsonSerializer.Deserialize<ProductPart>(str);

                    RedrawProductPart();

                }
            }
        }

        void RedrawProductPart()
        {
            previewControl1.RedrawSheet();
        }

        private void btn_SaveToPdf_Click(object sender, EventArgs e)
        {
            //_productPart.Proof.Enable = cb_DrawProofColor.Checked;
            SaveToPdf();
        }

        private void SaveToPdf()
        {
            _productPart = new ProductPart();
            _productPart.TemplateSheets = addTemplateSheetControl1.GetSheets();
            _productPart.PrintSheets = printSheetsControl1.GetSheets();


            _productPart.RunList.RunPages = runListControl1.GetRunPages();
            _productPart.PdfFiles = _pdfFiles;

            var drawer = new PdfDrawer(_files.ToList()[0] + ".impos.pdf");
            drawer.Draw(_productPart);
            MessageBox.Show("Виконано!");

        }
        private void FormPdfImposition_FormClosing(object sender, FormClosingEventArgs e)
        {

            //if (_productPart != null)
            //{
            //    string pathImpos = Path.Combine(_curJobFolder, ".impos");
            //    Directory.CreateDirectory(pathImpos);

            //    string fileImpos = Path.Combine(pathImpos, "imposition.json");

            //    _productPart.Save(fileImpos);
            //}
        }

    }
}
