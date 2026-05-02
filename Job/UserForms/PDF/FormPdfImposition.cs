using JobSpace.Profiles;
using JobSpace.Static.Pdf.Imposition;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.View;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Processes;
using Krypton.Toolkit;
using Ookii.Dialogs.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public partial class FormPdfImposition : KryptonForm
    {
        private const string ImpositionFolderName = ".impos";
        private const string ImpositionFileName = "imposition.json";

        private readonly GlobalImposParameters _imposParam;
        private PdfDrawer _drawer;

        public FormPdfImposition(Profile profile)
        {
            InitializeComponent();

            _imposParam = new GlobalImposParameters
            {
                Profile = profile
            };

            InitBindParameters();
            InitImposTools();
            SubscribeSheetEvents();
            BindExportControls();

            LoadExportParameters();
            LoadCustomOutputFolders();
        }

        private void SubscribeSheetEvents()
        {
            addTemplateSheetControl1.OnSheetSelected += OnTemplateSheetSelected;
            printSheetsControl1.OnPrintSheetsChanged += OnTemplateSheetSelected;
            printSheetsControl1.OnPrintSheetDeleted += OnPrintSheetDeleted;
            printSheetsControl1.JustReassignPages += NeedCheckRunListPages;

            addTemplateSheetControl1.OnSheetAddToPrint += OnAddSheetToPrintEvent;
            addTemplateSheetControl1.OnSheetAddManyToPrint += OnSheetAddManyToPrintEvent;
            PrintSheet.ResetId();
        }

        private void BindExportControls()
        {
            cb_CustomOutputPath.DataBindings.Add("Enabled", cb_useCustomOutputFolder, "Checked");
            btn_selectCustomFolder.DataBindings.Add("Enabled", cb_useCustomOutputFolder, "Checked");
            tb_useTemplate.DataBindings.Add("Enabled", cb_useTemplate, "Checked");
        }

        private void LoadCustomOutputFolders()
        {
            cb_CustomOutputPath.Items.AddRange(_imposParam.Profile.ImposService.LoadCustomsPath().ToArray());
        }

        private void LoadExportParameters()
        {
            ExportParameters exportParameters = _imposParam.Profile.ImposService.LoadExportParameters();
            ApplyExportParametersToUi(exportParameters);
        }

        private void ApplyExportParametersToUi(ExportParameters exportParameters)
        {
            cb_savePrintSheetInOrder.Checked = exportParameters.SavePrintSheetToOrderFolder;
            cb_useTemplate.Checked = exportParameters.UseTemplate;
            tb_useTemplate.Text = exportParameters.TemplateString;
            cb_useCustomOutputFolder.Checked = exportParameters.UseCustomOutputFolder;
            cb_CustomOutputPath.Text = exportParameters.CustomOutputFolder;
        }

        private ExportParameters ReadExportParametersFromUi()
        {
            var exportParameters = new ExportParameters
            {
                SavePrintSheetToOrderFolder = cb_savePrintSheetInOrder.Checked,
                OutputFolder = _imposParam.ImposInput.JobFolder,
                UseTemplate = cb_useTemplate.Checked,
                TemplateString = tb_useTemplate.Text,
                UseCustomOutputFolder = cb_useCustomOutputFolder.Checked,
                CustomOutputFolder = cb_CustomOutputPath.Text
            };

            if (_imposParam.ImposInput.Files.Count > 0)
            {
                string firstFile = _imposParam.ImposInput.Files[0];
                exportParameters.OutputFileName = Path.Combine(
                    Path.GetDirectoryName(firstFile),
                    $"{Path.GetFileNameWithoutExtension(firstFile)}_impos{Path.GetExtension(firstFile)}");
            }

            return exportParameters;
        }

        private void OnPrintSheetDeleted(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Alt)
            {
                NeedCheckRunListPages(this, null);
            }
            else
            {
                _controlBindParameters_NeedRearangePages(this, null);
            }
        }

        private void OnClickCenterH(object sender, EventArgs e)
        {
            ProcessMoveSubject.CenterHor(_imposParam.ControlsBind.Sheet);
            imposBindingControl1.FixBackPageSizePosition(_imposParam.ControlsBind.Sheet.TemplatePageContainer);
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void OnClickCenterV(object sender, EventArgs e)
        {
            ProcessMoveSubject.CenterVer(_imposParam.ControlsBind.Sheet);
            imposBindingControl1.FixBackPageSizePosition(_imposParam.ControlsBind.Sheet.TemplatePageContainer);
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void OnSheetAddManyToPrintEvent(object sender, TemplateSheet e)
        {
            // Отримати сторінки, що не задіяні
            int cnt = runListControl1.GetUnassignedPagesCount();
            int maxId = e.TemplatePageContainer.GetMaxIdx();
            if (maxId == 0) return;

            int sheetCnt = cnt / maxId;

            PrintSheet sheet = PrintSheet.ConvertTemplateSheetToPrintSheet(e);
            printSheetsControl1.AddSheets(sheet, sheetCnt);
            _controlBindParameters_NeedRearangePages(this, null);
        }

        void AddPrintSheet(TemplateSheet e)
        {
            PrintSheet sheet = PrintSheet.ConvertTemplateSheetToPrintSheet(e);
           
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
            _imposParam.ControlsBind.SetSheet(e);
        }

        private void InitBindParameters()
        {
            _imposParam.ControlsBind.NeedRearangePages += _controlBindParameters_NeedRearangePages;
            _imposParam.ControlsBind.NeedCheckRunListPages += NeedCheckRunListPages;
            imposBindingControl1.SetControlBindParameters(_imposParam);
            pdfFileListControl1.SetControlBindParameters(_imposParam);
            runListControl1.SetControlBindParameters(_imposParam);
            marksControl1.SetControlBindParameters(_imposParam);
            previewControl1.SetControlBindParameters(_imposParam);
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
            var _tool_param = _imposParam.ImposTools;

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
            _tool_param.OnPageGroupDistributeVer += OnPageGroupDistributeVer;
            _tool_param.OnPageGroupDelete += OnPageGroupDelete;
            
        }

        private void OnPageGroupDistributeVer(object sender, List<PageGroup> e)
        {
            PageGroupsService.DistributeVer(_imposParam.ControlsBind.Sheet, _imposParam.ImposTools.IgnoreSheetFields, e);
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void OnPageGroupDelete(object sender, List<PageGroup> e)
        {
            PageGroupsService.DeleteGroups(_imposParam.ControlsBind.Sheet, e);
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void OnPageGroupDistributeHor(object sender, List<PageGroup> e)
        {
            PageGroupsService.DistributeHor(_imposParam.ControlsBind.Sheet, _imposParam.ImposTools.IgnoreSheetFields, e);
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void OnAddPageToGroup(object sender, TemplatePage e)
        {
            e.Group = _imposParam.ImposTools.CurGroup;
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void OnFlipRowAngle(object sender, TemplatePage e)
        {
            _imposParam.ControlsBind.Sheet.TemplatePageContainer.FlipPagesAngle(e, _imposParam.ControlsBind.Sheet.SheetPlaceType);
            ProcessFixBleeds.Front(_imposParam.ControlsBind.Sheet.TemplatePageContainer);
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void OnFlipAngle(object sender, TemplatePage e)
        {
            e.FlipAngle(_imposParam.ControlsBind.Sheet.SheetPlaceType);
            ProcessFixBleeds.Front(_imposParam.ControlsBind.Sheet.TemplatePageContainer);
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void OnSwitchWH(object sender, EventArgs e)
        {
            UpdateSelectedPage(page => page.SwitchWH(_imposParam.ControlsBind.Sheet.SheetPlaceType));
        }

        private void OnRotateRight(object sender, EventArgs e)
        {
            UpdateSelectedPage(page => ProcessRotatePage.Right(_imposParam.ControlsBind.Sheet, page));
        }

        private void OnRotateLeft(object sender, EventArgs e)
        {
            UpdateSelectedPage(page => ProcessRotatePage.Left(_imposParam.ControlsBind.Sheet, page));
        }

        private void UpdateSelectedPage(Action<TemplatePage> update)
        {
            var selectedPage = _imposParam.ControlsBind.SelectedPreviewPage;
            if (selectedPage == null) return;

            update(selectedPage);
            _imposParam.ControlsBind.UpdateSheet();
            _imposParam.ControlsBind.SelectedPreviewPage = selectedPage;
        }

        private void OnMoveDownClick(object sender, double e)
        {
            ProcessMoveSubject.Down(_imposParam.ControlsBind.Sheet, e);
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void OnMoveUpClick(object sender, double e)
        {
            ProcessMoveSubject.Up(_imposParam.ControlsBind.Sheet, e);
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void OnMoveRightClick(object sender, double e)
        {
            ProcessMoveSubject.Right(_imposParam.ControlsBind.Sheet, e);
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void OnMoveLeftClick(object sender, double e)
        {
            ProcessMoveSubject.Left(_imposParam.ControlsBind.Sheet, e);
            _imposParam.ControlsBind.UpdateSheet();
        }

        public FormPdfImposition(ImposInputParam param) : this((Profile)param.UserProfile)
        {
            _imposParam.ImposInput = param;

            if (param.Job != null)
            {
                _imposParam.TextVariables.SetValue(ValueList.OrderNo, param.Job.Number);
                _imposParam.TextVariables.SetValue(ValueList.Customer, param.Job.Customer);
                _imposParam.TextVariables.SetValue(ValueList.OrderDesc, param.Job.Description);
            }

            int id = 1;
            foreach (var file in _imposParam.ImposInput.Files)
            {
                var pdfFile = new PdfFile(file) { Id = id++ };
                _imposParam.ProductPart.PdfFiles.Add(pdfFile);
                runListControl1.AddPages(ImposRunList.CreatePagesFromFile(pdfFile));
            }

            pdfFileListControl1.AddFiles(_imposParam.ProductPart.PdfFiles);

            // слідкуємо за змінами майстер сторінки
            masterPageSelectControl1.OnMasterPageChanged += OnMasterPageChanged;
            masterPageSelectControl1.OnMasterPageAdded += OnMasterPageAdded;
            masterPageSelectControl1.SetFormats(_imposParam.ProductPart.PdfFiles);

            addTemplateSheetControl1.SetControlBindParameters(_imposParam);
            printSheetsControl1.SetControlBindParameters(_imposParam);

            LoadImposFromFile();

        }

        private void OnMasterPageAdded(object sender, PageFormatView e)
        {
            // додаємо сторінку на поточний лист
            //TODO: додати сторінку на лист
            if (_imposParam.ControlsBind.Sheet == null) return;

            _imposParam.ControlsBind.Sheet.TemplatePageContainer.AddPage(e.ToTemplatePage());
            _imposParam.ControlsBind.UpdatePreview();
        }

        private void OnMasterPageChanged(object sender, PageFormatView e)
        {
            _imposParam.ControlsBind.MasterPage = e.ToTemplatePage();
        }

        private void LoadImposFromFile()
        {
            if (!TryGetImpositionFilePath(out string filePath) || !File.Exists(filePath))
            {
                return;
            }

            try
            {
                var str = File.ReadAllText(filePath);
                var imposition = JsonSerializer.Deserialize<SavedImposition>(str);
                if (imposition == null) return;

                ApplyLoadedImposition(imposition);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося завантажити спуск полос:\n{ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ApplyLoadedImposition(SavedImposition imposition)
        {
            _imposParam.ProductPart.RunList = imposition.RunList ?? new ImposRunList();
            _imposParam.ProductPart.TemplateSheets = imposition.TemplateSheets ?? new List<TemplateSheet>();
            _imposParam.ProductPart.PrintSheets = imposition.PrintSheets ?? new List<PrintSheet>();
            _imposParam.ProductPart.UsedColors = imposition.UsedColors ?? new ImposColors();
            _imposParam.ProductPart.Proof = imposition.Proof ?? new ProofParameters();

            if (imposition.ExportParameters != null)
            {
                _imposParam.ProductPart.ExportParameters = imposition.ExportParameters;
                ApplyExportParametersToUi(imposition.ExportParameters);
            }

            cb_UseProofColor.Checked = _imposParam.ProductPart.Proof.Enable;

            addTemplateSheetControl1.SetSheets(_imposParam.ProductPart.TemplateSheets);
            printSheetsControl1.SetSheets(_imposParam.ProductPart.PrintSheets);
            runListControl1.SetRunPages(_imposParam.ProductPart.RunList.RunPages);
            imposColorsControl1.SetUsedColors(_imposParam.ProductPart.UsedColors);

            ResetSheetIdsAfterLoad();
            NeedCheckRunListPages(this, EventArgs.Empty);

            if (_imposParam.ProductPart.PrintSheets.Count > 0)
            {
                _imposParam.ControlsBind.SetSheet(_imposParam.ProductPart.PrintSheets[0]);
            }

            RedrawProductPart();
        }

        private void ResetSheetIdsAfterLoad()
        {
            var sheetIds = _imposParam.ProductPart.TemplateSheets.Select(x => x.Id)
                .Concat(_imposParam.ProductPart.PrintSheets.Select(x => x.Id))
                .ToList();

            if (sheetIds.Count > 0)
            {
                TemplateSheet.SheetId = Math.Max(TemplateSheet.SheetId, sheetIds.Max() + 1);
            }

            if (_imposParam.ProductPart.PrintSheets.Count > 0)
            {
                PrintSheet.printId = Math.Max(PrintSheet.printId, _imposParam.ProductPart.PrintSheets.Max(x => x.Id) + 1);
            }
        }

        private void SaveImposToFile()
        {
            if (!TryGetImpositionFilePath(out string filePath)) return;

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                var imposition = CreateSavedImposition();
                var str = JsonSerializer.Serialize(imposition);
                File.WriteAllText(filePath, str);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося зберегти спуск полос:\n{ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private SavedImposition CreateSavedImposition()
        {
            BuildProductPartFromUi();

            return new SavedImposition
            {
                RunList = _imposParam.ProductPart.RunList,
                TemplateSheets = _imposParam.ProductPart.TemplateSheets,
                PrintSheets = _imposParam.ProductPart.PrintSheets,
                UsedColors = _imposParam.ProductPart.UsedColors,
                Proof = _imposParam.ProductPart.Proof,
                ExportParameters = _imposParam.ProductPart.ExportParameters
            };
        }

        private bool TryGetImpositionFilePath(out string filePath)
        {
            filePath = null;

            if (string.IsNullOrWhiteSpace(_imposParam.ImposInput.JobFolder))
            {
                return false;
            }

            filePath = Path.Combine(_imposParam.ImposInput.JobFolder, ImpositionFolderName, ImpositionFileName);
            return true;
        }

        private void RedrawProductPart()
        {
            previewControl1.RedrawSheet();
        }

        private async void btn_SaveToPdf_Click(object sender, EventArgs e)
        {
            if (printSheetsControl1.GetSheets().Count == 0)
            {
                MessageBox.Show("Нема листів для друку");
            }
            else
            {
                await SaveToPdfAsync();
            }

        }

        private async Task SaveToPdfAsync()
        {
            BuildProductPartFromUi();
            SaveImposToFile();

            var pdfDrawer = new PdfDrawer(_imposParam);
            _drawer = pdfDrawer;

            try
            {
                pdfDrawer.StartEvent += startEvent;
                pdfDrawer.ProcessingEvent += processingEvent;
                pdfDrawer.FinishEvent += finishEvent;

                // якщо не вибрано листи, то друкуємо всі
                pdfDrawer.CustomSheets = printSheetsControl1.GetSheetsIdxForPrint();
                await Task.Run(() => pdfDrawer.Draw(_imposParam.ProductPart)).ConfigureAwait(true);

                if (!pdfDrawer.IsCancelled)
                {
                    if (MessageBox.Show("Відкрити?", "Виконано!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        Process.Start(_imposParam.ProductPart.ExportParameters.OutputFilePath);
                    }
                }
            }
            finally
            {
                pdfDrawer.StartEvent -= startEvent;
                pdfDrawer.ProcessingEvent -= processingEvent;
                pdfDrawer.FinishEvent -= finishEvent;

                if (ReferenceEquals(_drawer, pdfDrawer))
                {
                    _drawer = null;
                }
            }
        }

        private void BuildProductPartFromUi()
        {
            _imposParam.ProductPart.Proof.Enable = cb_UseProofColor.Checked;
            _imposParam.ProductPart.TemplateSheets = addTemplateSheetControl1.GetSheets();
            _imposParam.ProductPart.PrintSheets = printSheetsControl1.GetSheets();
            _imposParam.ProductPart.RunList.RunPages = runListControl1.GetRunPages();
            _imposParam.ProductPart.UsedColors = imposColorsControl1.GetUsedColors();
            _imposParam.ProductPart.ExportParameters = ReadExportParametersFromUi();
        }

        private void btn_cancel_export_Click(object sender, EventArgs e)
        {
            if (_drawer != null)
            {
                var result = MessageBox.Show("Скасувати експорт?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _drawer.Cancel();
                }
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
            progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Maximum = e; }));
        }

        private void FormPdfImposition_FormClosing(object sender, FormClosingEventArgs e)
        {
            BuildProductPartFromUi();
            _imposParam.Profile.ImposService.SaveExportParameters(_imposParam.ProductPart.ExportParameters);
            SaveImposToFile();
        }

        private void FormPdfImposition_Shown(object sender, EventArgs e)
        {
            _imposParam.ControlsBind.PropertyChanged += _controlBindParameters_PropertyChanged;
        }

        private void _controlBindParameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPreviewPage")
            {
                pg_Parameters2.SelectedObject = _imposParam.ControlsBind.SelectedPreviewPage;
            }
        }

        private void pg_Parameters_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            imposBindingControl1.FixBackPageSizePosition(_imposParam.ControlsBind.SelectedPreviewPage);
            _imposParam.ControlsBind.UpdateSheet();
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

                    _imposParam.Profile.ImposService.SaveCustomsPath(paths.ToList());
                }
            }
        }

        private sealed class SavedImposition
        {
            public ImposRunList RunList { get; set; } = new ImposRunList();
            public List<TemplateSheet> TemplateSheets { get; set; } = new List<TemplateSheet>();
            public List<PrintSheet> PrintSheets { get; set; } = new List<PrintSheet>();
            public ImposColors UsedColors { get; set; } = new ImposColors();
            public ProofParameters Proof { get; set; } = new ProofParameters();
            public ExportParameters ExportParameters { get; set; } = new ExportParameters();
        }

    }
}
