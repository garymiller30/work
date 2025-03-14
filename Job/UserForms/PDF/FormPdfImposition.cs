using BrightIdeasSoftware;
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
using Ookii.Dialogs.WinForms;
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
        ControlBindParameters _parameters = new ControlBindParameters();
        ProductPart _productPart;

        public FormPdfImposition()
        {
            InitializeComponent();
            InitBindParameters();
            InitImposTools();

            addTemplateSheetControl1.OnSheetSelected += OnTemplateSheetSelected;
            printSheetsControl1.OnPrintSheetsChanged += OnTemplateSheetSelected;
            printSheetsControl1.OnPrintSheetDeleted += OnPrintSheetDeleted;
            printSheetsControl1.JustReassignPages += NeedCheckRunListPages;


            addTemplateSheetControl1.OnSheetAddToPrint += OnAddSheetToPrintEvent;
            addTemplateSheetControl1.OnSheetAddManyToPrint += OnSheetAddManyToPrintEvent;
            PrintSheet.ResetId();

            cb_CustomOutputPath.DataBindings.Add("Enabled", cb_useCustomOutputFolder, "Checked");
            btn_selectCustomFolder.DataBindings.Add("Enabled", cb_useCustomOutputFolder, "Checked");
            tb_useTemplate.DataBindings.Add("Enabled", cb_useTemplate, "Checked");

            cb_CustomOutputPath.Items.AddRange(SaveLoadService.LoadCustomsPath().ToArray());
        }

       

        private void OnPrintSheetDeleted(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Alt)
            {
                NeedCheckRunListPages(this,null);
            }
            else
            {
                _controlBindParameters_NeedRearangePages(this, null);
            }
        }

        private void OnClickCenterH(object sender, EventArgs e)
        {
            ProcessMoveSubject.CenterHor(_parameters.Sheet);
            imposBindingControl1.FixBackPageSizePosition(_parameters.Sheet.TemplatePageContainer);
            _parameters.UpdateSheet();
        }

        private void OnClickCenterV(object sender, EventArgs e)
        {
            ProcessMoveSubject.CenterVer(_parameters.Sheet);
            imposBindingControl1.FixBackPageSizePosition(_parameters.Sheet.TemplatePageContainer);
            _parameters.UpdateSheet();
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

            if (ModifierKeys != Keys.Alt)
            {
                _controlBindParameters_NeedRearangePages(this, null);
            }
        }

        private void OnAddSheetToPrintEvent(object sender, TemplateSheet e)
        {
            AddPrintSheet(e);
        }

        private void OnTemplateSheetSelected(object sender, TemplateSheet e)
        {
            _parameters.SetSheet(e);
        }

        private void InitBindParameters()
        {
            _parameters.PdfFiles = _pdfFiles;
            _parameters.NeedRearangePages += _controlBindParameters_NeedRearangePages;
            _parameters.NeedCheckRunListPages += NeedCheckRunListPages;
            imposBindingControl1.SetControlBindParameters(_parameters);
            pdfFileListControl1.SetControlBindParameters(_parameters);
            runListControl1.SetControlBindParameters(_parameters);
            marksControl1.SetControlBindParameters(_parameters);
            previewControl1.SetControlBindParameters(_parameters);
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
            _tool_param.OnMoveLeftClick += OnMoveLeftClick;
            _tool_param.OnMoveRightClick += OnMoveRightClick;
            _tool_param.OnMoveUpClick += OnMoveUpClick;
            _tool_param.OnMoveDownClick += OnMoveDownClick;
            _tool_param.OnRotateLeft += OnRotateLeft;
            _tool_param.OnRotateRight += OnRotateRight;
            _tool_param.OnSwitchWH += OnSwitchWH;
            _tool_param.OnFlipAngle += OnFlipAngle;
            _tool_param.OnFlipRowAngle += OnFlipRowAngle;
            _tool_param.OnAddPageToGroup += OnAddPageToGroup;
            _tool_param.OnPageGroupDistributeHor += OnPageGroupDistributeHor;
            previewControl1.InitBindParameters(_tool_param);
        }

        private void OnPageGroupDistributeHor(object sender, List<PageGroup> e)
        {
            PageGroupsService.DistributeHor(_parameters.Sheet,e);
            _parameters.UpdateSheet();
        }

        private void OnAddPageToGroup(object sender, TemplatePage e)
        {
            e.Group = _tool_param.CurGroup;
            _parameters.UpdateSheet();
        }

        private void OnFlipRowAngle(object sender, TemplatePage e)
        {
            _parameters.Sheet.TemplatePageContainer.FlipPagesAngle(e,_parameters.Sheet.SheetPlaceType);
            ProcessFixBleeds.Front(_parameters.Sheet.TemplatePageContainer);
            _parameters.UpdateSheet();
        }

        private void OnFlipAngle(object sender, TemplatePage e)
        {
             e.FlipAngle(_parameters.Sheet.SheetPlaceType);
            ProcessFixBleeds.Front(_parameters.Sheet.TemplatePageContainer);
            _parameters.UpdateSheet();
        }

        private void OnSwitchWH(object sender, EventArgs e)
        {
            if (_parameters.SelectedPreviewPage != null)
            {
                var sel_page = _parameters.SelectedPreviewPage;
                sel_page.SwitchWH();
                _parameters.UpdateSheet();
                _parameters.SelectedPreviewPage = sel_page;
            }
        }

        private void OnRotateRight(object sender, EventArgs e)
        {
            if (_parameters.SelectedPreviewPage != null)
            {
                var sel_page = _parameters.SelectedPreviewPage;
                ProcessRotatePage.Right(_parameters.Sheet, sel_page);
                _parameters.UpdateSheet();
                _parameters.SelectedPreviewPage = sel_page;
            }
        }

        private void OnRotateLeft(object sender, EventArgs e)
        {
           if (_parameters.SelectedPreviewPage != null)
            {
                var sel_page = _parameters.SelectedPreviewPage;
                ProcessRotatePage.Left(_parameters.Sheet, sel_page);
                _parameters.UpdateSheet();
                _parameters.SelectedPreviewPage = sel_page;
            }
        }

        private void OnMoveDownClick(object sender, double e)
        {
            ProcessMoveSubject.Down(_parameters.Sheet, e);
            _parameters.UpdateSheet();
        }

        private void OnMoveUpClick(object sender, double e)
        {
            ProcessMoveSubject.Up(_parameters.Sheet, e);
            _parameters.UpdateSheet();
        }

        private void OnMoveRightClick(object sender, double e)
        {
            ProcessMoveSubject.Right(_parameters.Sheet, e);
            _parameters.UpdateSheet();
        }

        private void OnMoveLeftClick(object sender, double e)
        {
            ProcessMoveSubject.Left(_parameters.Sheet, e);
            _parameters.UpdateSheet();
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

            addTemplateSheetControl1.SetControlBindParameters(_parameters);
            printSheetsControl1.SetControlBindParameters(_parameters);

            //LoadImposFromFile();

        }

        private void OnMasterPageAdded(object sender, PageFormatView e)
        {
            // додаємо сторінку на поточний лист
            //TODO: додати сторінку на лист
            if (_parameters.Sheet == null) return;

            _parameters.Sheet.TemplatePageContainer.AddPage(e.ToTemplatePage());
            _parameters.UpdatePreview();
        }

        private void OnMasterPageChanged(object sender, PageFormatView e)
        {
            _parameters.MasterPage = e.ToTemplatePage();
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
            
            _productPart.ExportParameters.SavePrintSheetToOrderFolder = cb_savePrintSheetInOrder.Checked;
            _productPart.ExportParameters.OutputFolder = _curJobFolder;
            _productPart.ExportParameters.UseTemplate = cb_useTemplate.Checked;
            _productPart.ExportParameters.TemplateString = tb_useTemplate.Text;
            _productPart.ExportParameters.UseCustomOutputFolder = cb_useCustomOutputFolder.Checked;
            _productPart.ExportParameters.CustomOutputFolder = cb_CustomOutputPath.Text;

            _productPart.ExportParameters.OutputFileName = _files.ToList()[0] + ".impos.pdf";

            DrawerStatic.CurProductPart = _productPart;

            PdfDrawer drawer = new PdfDrawer();

            drawer.StartEvent+= startEvent;
            drawer.ProcessingEvent+= processingEvent;
            drawer.FinishEvent+= finishEvent;

            await Task.Run(()=>  drawer.Draw(_productPart)).ConfigureAwait(true);
            if (MessageBox.Show("Відкрити?","Виконано!",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)== DialogResult.OK)
            {
                Process.Start(_productPart.ExportParameters.OutputFilePath);
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
            _parameters.PropertyChanged += _controlBindParameters_PropertyChanged;
        }

        private void _controlBindParameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPreviewPage")
            {
                pg_Parameters2.SelectedObject = _parameters.SelectedPreviewPage;
            }
        }

        private void pg_Parameters_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            imposBindingControl1.FixBackPageSizePosition(_parameters.SelectedPreviewPage);
            _parameters.UpdateSheet();
        }

        private void btn_selectCustomFolder_Click(object sender, EventArgs e)
        {
            using (var form = new VistaFolderBrowserDialog())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    HashSet<string> paths = new HashSet<string>();
                    paths.Add(form.SelectedPath);
                    foreach (var item in cb_CustomOutputPath.Items)
                    {
                        paths.Add(item.ToString());
                    }
                    cb_CustomOutputPath.Items.Clear();

                    foreach (var item in paths)
                    {
                        cb_CustomOutputPath.Items.Add(item);
                    }

                    SaveLoadService.SaveCustomsPath(paths.ToList());
                }
            }
        }
    }
}
