using ExtensionMethods;
using JobSpace.CustomForms;
using JobSpace.Profiles;
using JobSpace.Static.Pdf.Imposition;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.AutoImpos;
using JobSpace.Static.Pdf.Imposition.Models.View;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Processes;
using Krypton.Toolkit;
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

                if (!IsLoadedImpositionCompatible(imposition))
                {
                    MessageBox.Show(
                        "Збережений спуск полос створений для іншого набору PDF-файлів і не буде завантажений.",
                        "Спуск полос",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

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
                PdfFiles = CreateSavedPdfFileInfo(),
                RunList = _imposParam.ProductPart.RunList,
                TemplateSheets = _imposParam.ProductPart.TemplateSheets,
                PrintSheets = _imposParam.ProductPart.PrintSheets,
                UsedColors = _imposParam.ProductPart.UsedColors,
                Proof = _imposParam.ProductPart.Proof,
                ExportParameters = _imposParam.ProductPart.ExportParameters
            };
        }

        private List<SavedPdfFileInfo> CreateSavedPdfFileInfo()
        {
            return _imposParam.ProductPart.PdfFiles
                .Select(file => new SavedPdfFileInfo
                {
                    Id = file.Id,
                    FileName = file.FileName,
                    PageCount = file.Pages?.Length ?? 0,
                    Count = file.Count,
                    PageWidth = GetFirstPageWidth(file),
                    PageHeight = GetFirstPageHeight(file)
                })
                .ToList();
        }

        private bool IsLoadedImpositionCompatible(SavedImposition imposition)
        {
            if (imposition.PdfFiles != null && imposition.PdfFiles.Count > 0)
            {
                return IsSavedPdfFileInfoCompatible(imposition.PdfFiles);
            }

            return IsRunListCompatible(imposition.RunList);
        }

        private bool IsSavedPdfFileInfoCompatible(List<SavedPdfFileInfo> savedFiles)
        {
            var currentFiles = CreateSavedPdfFileInfo();

            if (savedFiles.Count != currentFiles.Count)
            {
                return false;
            }

            for (int i = 0; i < savedFiles.Count; i++)
            {
                var saved = savedFiles[i];
                var current = currentFiles[i];

                if (saved.Id != current.Id ||
                    saved.PageCount != current.PageCount ||
                    saved.Count != current.Count ||
                    !IsSavedPdfFormatCompatible(saved, current))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsSavedPdfFormatCompatible(SavedPdfFileInfo saved, SavedPdfFileInfo current)
        {
            if (saved.PageWidth <= 0 || saved.PageHeight <= 0 || current.PageWidth <= 0 || current.PageHeight <= 0)
            {
                return true;
            }

            var savedFormat = NormalizeFormat(saved.PageWidth, saved.PageHeight);
            var currentFormat = NormalizeFormat(current.PageWidth, current.PageHeight);

            return Math.Abs(savedFormat.width - currentFormat.width) <= 1 &&
                   Math.Abs(savedFormat.height - currentFormat.height) <= 1;
        }

        private static decimal GetFirstPageWidth(PdfFile file)
        {
            var page = file?.Pages?.FirstOrDefault();
            return page == null ? 0 : (decimal)page.Trim.W;
        }

        private static decimal GetFirstPageHeight(PdfFile file)
        {
            var page = file?.Pages?.FirstOrDefault();
            return page == null ? 0 : (decimal)page.Trim.H;
        }

        private bool IsRunListCompatible(ImposRunList runList)
        {
            if (runList?.RunPages == null)
            {
                return true;
            }

            foreach (var runPage in runList.RunPages)
            {
                if (runPage.FileId == 0 && runPage.PageIdx == 0)
                {
                    continue;
                }

                var file = _imposParam.ProductPart.PdfFiles.FirstOrDefault(x => x.Id == runPage.FileId);
                if (file?.Pages == null || runPage.PageIdx <= 0 || runPage.PageIdx > file.Pages.Length)
                {
                    return false;
                }
            }

            return true;
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

        public void ApplyAutoImposTemplate(AutoImposMatch match)
        {
            if (match == null || match.PrintSheets == null || match.PrintSheets.Count == 0)
                return;

            _imposParam.ProductPart.PrintSheets = match.PrintSheets;
            printSheetsControl1.SetSheets(match.PrintSheets);
            NeedCheckRunListPages(this, EventArgs.Empty);

            if (match.PrintSheets.Count > 0)
            {
                _imposParam.ControlsBind.SetSheet(match.PrintSheets[0]);
            }

            RedrawProductPart();
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

        private void btn_SaveAsAutoImpos_Click(object sender, EventArgs e)
        {
            BuildProductPartFromUi();

            if (_imposParam.ProductPart.PrintSheets.Count == 0)
            {
                MessageBox.Show("Нема листів для збереження", "Автоспуск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_imposParam.ProductPart.PdfFiles.Count == 0 || _imposParam.ProductPart.PdfFiles[0].Pages == null || _imposParam.ProductPart.PdfFiles[0].Pages.Length == 0)
            {
                MessageBox.Show("Нема PDF-сторінок для визначення формату", "Автоспуск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string defaultName = CreateDefaultAutoImposRuleName();
            using (var form = new FormEnterText(defaultName))
            {
                form.Text = "Назва автоспуску";
                if (form.ShowDialog(this) != DialogResult.OK || string.IsNullOrWhiteSpace(form.SelectedText))
                    return;

                SaveAutoImposRule(form.SelectedText.Trim());
            }
        }

        private void SaveAutoImposRule(string ruleName)
        {
            var profile = _imposParam.Profile;
            var service = new AutoImposService(profile);
            string templateFileName = CreateAutoImposTemplateFileName(ruleName);
            string templatePath = Path.Combine(profile.ImposService.PrintSheetsPath, templateFileName);

            if (File.Exists(templatePath))
            {
                var result = MessageBox.Show(
                    $"Шаблон \"{templateFileName}\" вже існує. Перезаписати?",
                    "Автоспуск",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;
            }

            profile.ImposService.SavePrintSheets(_imposParam.ProductPart.PrintSheets, templatePath);

            var rule = CreateAutoImposRule(ruleName, templateFileName);
            var rules = service.LoadRules();
            var existing = FindSimilarAutoImposRule(rules, rule);

            if (existing != null)
            {
                rules.Remove(existing);
            }

            rules.Add(rule);
            service.SaveRules(rules);

            MessageBox.Show("Автоспуск збережено", "Автоспуск", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private AutoImposRule CreateAutoImposRule(string ruleName, string templateFileName)
        {
            var firstPage = _imposParam.ProductPart.PdfFiles[0].Pages[0];
            var format = NormalizeFormat((decimal)firstPage.Trim.W, (decimal)firstPage.Trim.H);
            int pageCount = _imposParam.ProductPart.PdfFiles.Sum(file => file.Pages?.Length ?? 0);

            return new AutoImposRule
            {
                Name = ruleName,
                Customer = _imposParam.ImposInput.Job?.Customer,
                CategoryId = _imposParam.ImposInput.Job?.CategoryId?.ToString(),
                PageWidth = format.width,
                PageHeight = format.height,
                ExactPageCount = pageCount > 0 ? pageCount : (int?)null,
                PrintSheetTemplateFile = templateFileName,
                Priority = 100,
                OpenEditorBeforeExport = true
            };
        }

        private static AutoImposRule FindSimilarAutoImposRule(List<AutoImposRule> rules, AutoImposRule rule)
        {
            return rules.FirstOrDefault(x =>
                string.Equals(x.Customer, rule.Customer, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(x.CategoryId, rule.CategoryId, StringComparison.InvariantCultureIgnoreCase) &&
                x.PageWidth == rule.PageWidth &&
                x.PageHeight == rule.PageHeight &&
                x.ExactPageCount == rule.ExactPageCount);
        }

        private string CreateDefaultAutoImposRuleName()
        {
            var firstPage = _imposParam.ProductPart.PdfFiles.FirstOrDefault()?.Pages?.FirstOrDefault();
            string format = firstPage == null ? "PDF" : $"{firstPage.Trim.W:0.#}x{firstPage.Trim.H:0.#}";
            int pageCount = _imposParam.ProductPart.PdfFiles.Sum(file => file.Pages?.Length ?? 0);
            string customer = _imposParam.ImposInput.Job?.Customer;

            return string.IsNullOrWhiteSpace(customer)
                ? $"{format}_{pageCount}p"
                : $"{customer}_{format}_{pageCount}p";
        }

        private static string CreateAutoImposTemplateFileName(string ruleName)
        {
            string fileName = ruleName.Transliteration();
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(invalidChar, '_');
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = $"auto_impos_{DateTime.Now:yyyyMMdd_HHmmss}";
            }

            if (!fileName.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
            {
                fileName += ".json";
            }

            return fileName;
        }

        private static (decimal width, decimal height) NormalizeFormat(decimal width, decimal height)
        {
            width = Math.Round(width, 1);
            height = Math.Round(height, 1);
            return width <= height ? (width, height) : (height, width);
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
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = _imposParam.ProductPart.ExportParameters.OutputFilePath,
                            UseShellExecute = true
                        };

                        Process.Start(startInfo);
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
            using (var form = new FolderBrowserDialog())
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
            public List<SavedPdfFileInfo> PdfFiles { get; set; } = new List<SavedPdfFileInfo>();
            public ImposRunList RunList { get; set; } = new ImposRunList();
            public List<TemplateSheet> TemplateSheets { get; set; } = new List<TemplateSheet>();
            public List<PrintSheet> PrintSheets { get; set; } = new List<PrintSheet>();
            public ImposColors UsedColors { get; set; } = new ImposColors();
            public ProofParameters Proof { get; set; } = new ProofParameters();
            public ExportParameters ExportParameters { get; set; } = new ExportParameters();
        }

        private sealed class SavedPdfFileInfo
        {
            public int Id { get; set; }
            public string FileName { get; set; }
            public int PageCount { get; set; }
            public int Count { get; set; }
            public decimal PageWidth { get; set; }
            public decimal PageHeight { get; set; }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
