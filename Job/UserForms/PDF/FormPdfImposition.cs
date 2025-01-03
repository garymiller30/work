﻿using BrightIdeasSoftware;
using JobSpace.Static.Pdf.Imposition.Drawers;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF;
using JobSpace.Static.Pdf.Imposition.Drawers.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.View;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.Impos;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Processes;
using JobSpace.UserForms.PDF.ImposItems;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        ImposToolsParameters _tool_param = new ImposToolsParameters();
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

       

        private void OnPrintSheetDeleted(object sender, EventArgs e)
        {
            _controlBindParameters_NeedRearangePages(this,null);
        }

        private void OnClickCenterH(object sender, EventArgs e)
        {
            ProcessCenterH.Center(_controlBindParameters.Sheet);
            _controlBindParameters.UpdateSheet();
        }

        private void OnClickCenterV(object sender, EventArgs e)
        {
            ProcessCenterV.Center(_controlBindParameters.Sheet);
            _controlBindParameters.UpdateSheet();
        }

        private void OnSheetAddManyToPrintEvent(object sender, TemplateSheet e)
        {
            // Отримати сторінки, що не задіяні
            int cnt = runListControl1.GetUnassignedPagesCount();
            int maxId = e.TemplatePageContainer.GetMaxIdx();
            int sheetCnt = cnt / maxId;

            PrintSheet sheet = PrintSheet.ConvertTemplateSheetToPrintSheet(e);
            printSheetsControl1.AddSheets(sheet,sheetCnt);
            _controlBindParameters_NeedRearangePages(this, null);
        }

        void AddPrintSheet(TemplateSheet e)
        {

            PrintSheet sheet = PrintSheet.ConvertTemplateSheetToPrintSheet(e);
            //runListControl1.AssignPrintSheet(sheet);
            printSheetsControl1.AddSheet(sheet);
            _controlBindParameters_NeedRearangePages(this,null);
        }

        private void OnAddSheetToPrintEvent(object sender, TemplateSheet e)
        {
            AddPrintSheet(e);
        }

        private void OnTemplateSheetSelected(object sender, TemplateSheet e)
        {
            _controlBindParameters.Sheet = e;
            
        }

        private void InitBindParameters()
        {
            _controlBindParameters.PdfFiles = _pdfFiles;
            _controlBindParameters.NeedRearangePages += _controlBindParameters_NeedRearangePages;
            _controlBindParameters.NeedCheckRunListPages += NeedCheckRunListPages;
            imposBindingControl1.SetControlBindParameters(_controlBindParameters);
            pdfFileListControl1.SetControlBindParameters(_controlBindParameters);
            runListControl1.SetControlBindParameters(_controlBindParameters);
            marksControl1.SetControlBindParameters(_controlBindParameters);
            previewControl1.SetControlBindParameters(_controlBindParameters);
        }

        private void NeedCheckRunListPages(object sender, EventArgs e)
        {
            imposBindingControl1.CheckRunListPages(printSheetsControl1.GetSheets(), runListControl1.GetRunPages());
            runListControl1.UpdateRunList();
        }

        private void _controlBindParameters_NeedRearangePages(object sender, EventArgs e)
        {
            imposBindingControl1.RearangePages(printSheetsControl1.GetSheets(), runListControl1.GetRunPages());
            runListControl1.UpdateRunList();
        }

        private void InitImposTools()
        {
            _tool_param.OnClickCenterV += OnClickCenterV;
            _tool_param.OnClickCenterH += OnClickCenterH;
            previewControl1.InitBindParameters(_tool_param);
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

            // слідкуємо за змінами майстер сторінки
            masterPageSelectControl1.OnMasterPageChanged += OnMasterPageChanged;
            masterPageSelectControl1.OnMasterPageAdded += OnMasterPageAdded;
            masterPageSelectControl1.SetFormats(_pdfFiles);

            addTemplateSheetControl1.SetControlBindParameters(_controlBindParameters);
            printSheetsControl1.SetControlBindParameters(_controlBindParameters);

            //LoadImposFromFile();

        }

        private void OnMasterPageAdded(object sender, PageFormatView e)
        {
            // додаємо сторінку на поточний лист
            //TODO: додати сторінку на лист
            if (_controlBindParameters.Sheet == null) return;

            _controlBindParameters.Sheet.TemplatePageContainer.AddPage(e.ToTemplatePage());
            _controlBindParameters.UpdatePreview();
        }

        private void OnMasterPageChanged(object sender, PageFormatView e)
        {

            TemplatePage page = new TemplatePage
            {
                W = (double)e.Width,
                H= (double)e.Height,
            };

            page.Bleeds.SetDefault((double)e.Bleed);
            page.SetMarginsLikeBleed();

            _controlBindParameters.MasterPage = page;
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
                    imposColorsControl1.SetUsedColors(_productPart.UsedColors);
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
            if (printSheetsControl1.GetSheets().Count == 0)
            {
                MessageBox.Show("Нема листів для друку");
            }
            else
            {
                SaveToPdf();
            }
            
        }

        private async void SaveToPdf()
        {
            _productPart = new ProductPart();
            _productPart.Proof.Enable = cb_UseProofColor.Checked;
            _productPart.TemplateSheets = addTemplateSheetControl1.GetSheets();
            _productPart.PrintSheets = printSheetsControl1.GetSheets();

            _productPart.RunList.RunPages = runListControl1.GetRunPages();
            _productPart.PdfFiles = _pdfFiles;
            _productPart.UsedColors = imposColorsControl1.GetUsedColors();

            string imposFile = _files.ToList()[0] + ".impos.pdf";

            DrawerStatic.CurProductPart = _productPart;

            PdfDrawer drawer = new PdfDrawer(imposFile);

            drawer.StartEvent+= startEvent;
            drawer.ProcessingEvent+= processingEvent;
            drawer.FinishEvent+= finishEvent;

            await Task.Run(()=>  drawer.Draw(_productPart)).ConfigureAwait(true);
            if (MessageBox.Show("Відкрити?","Виконано!",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)== DialogResult.OK)
            {
                Process.Start(imposFile);
            }

        }

        private void finishEvent(object sender, EventArgs e)
        {
            progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Value = 0; }));
           
        }

        private void processingEvent(object sender, int e)
        {
            progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Value = e; }));
        }

        private void startEvent(object sender, int e)
        {
            progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Maximum = e; } ));
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

        private void FormPdfImposition_Shown(object sender, EventArgs e)
        {
            _controlBindParameters.PropertyChanged += _controlBindParameters_PropertyChanged;
        }

        private void _controlBindParameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPreviewPage")
            {
                pg_Parameters.SelectedObject = _controlBindParameters.SelectedPreviewPage;
            }
        }

        private void pg_Parameters_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

            _controlBindParameters.UpdateSheet();
        }
    }
}
