using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Personalization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public sealed class FormPdfPersonalization : Form
    {
        private static readonly Dictionary<string, PersonalizationLayerType> LayerTypes =
            new Dictionary<string, PersonalizationLayerType>
            {
                { "Основа PDF", PersonalizationLayerType.BasePdf },
                { "PDF", PersonalizationLayerType.Pdf },
                { "Текст", PersonalizationLayerType.Text },
                { "Код", PersonalizationLayerType.Code }
            };

        private static readonly Dictionary<string, PersonalizationCodeType> CodeTypes =
            new Dictionary<string, PersonalizationCodeType>
            {
                { "Code-128", PersonalizationCodeType.Code128 },
                { "EAN-13", PersonalizationCodeType.Ean13 },
                { "EAN-8", PersonalizationCodeType.Ean8 },
                { "QR", PersonalizationCodeType.Qr }
            };

        private static readonly Dictionary<string, PersonalizationExportMode> ExportModes =
            new Dictionary<string, PersonalizationExportMode>
            {
                { "Все окремими файлами", PersonalizationExportMode.SeparateFiles },
                { "Все в одному файлі", PersonalizationExportMode.SingleFile }
            };

        private static readonly Dictionary<string, PersonalizationAnchorPoint> Anchors =
            new Dictionary<string, PersonalizationAnchorPoint>
            {
                { "лівий нижній", PersonalizationAnchorPoint.BottomLeft },
                { "лівий центр", PersonalizationAnchorPoint.LeftCenter },
                { "лівий верхній", PersonalizationAnchorPoint.TopLeft },
                { "центр нижній", PersonalizationAnchorPoint.BottomCenter },
                { "центр", PersonalizationAnchorPoint.Center },
                { "центр верхній", PersonalizationAnchorPoint.TopCenter },
                { "правий нижній", PersonalizationAnchorPoint.BottomRight },
                { "правий центр", PersonalizationAnchorPoint.RightCenter },
                { "правий верхній", PersonalizationAnchorPoint.TopRight }
            };

        private readonly TextBox _basePdfTextBox = new TextBox();
        private readonly TextBox _dataTextBox = new TextBox();
        private readonly TextBox _outputTextBox = new TextBox();
        private readonly ComboBox _exportModeComboBox = new ComboBox();
        private readonly TextBox _exportRowsTextBox = new TextBox();
        private readonly NumericUpDown _previewRow = new NumericUpDown();
        private readonly CheckBox _autoPreviewCheckBox = new CheckBox();
        private readonly Timer _previewTimer = new Timer();
        private readonly Label _dataInfoLabel = new Label();
        private readonly Label _sourceHintLabel = new Label();
        private readonly DataGridView _layersGrid = new DataGridView();
        private readonly Panel _previewPanel = new Panel();
        private readonly PictureBox _previewBox = new PictureBox();
        private readonly ComboBox _zoomComboBox = new ComboBox();
        private readonly string[] _fontNames;
        private bool _loadingTemplate;
        private bool _renderingPreview;
        private bool _draggingPreviewLayer;
        private Point _dragStartPoint;
        private double _dragStartXmm;
        private double _dragStartYmm;

        public PdfPersonalizationSettings Settings { get; private set; }

        public FormPdfPersonalization(string basePdfPath)
        {
            _fontNames = new InstalledFontCollection()
                .Families
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToArray();

            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(basePdfPath))
            {
                _basePdfTextBox.Text = basePdfPath;
                _outputTextBox.Text = Path.GetDirectoryName(basePdfPath);
                AddLayerRow(PersonalizationLayerType.BasePdf, basePdfPath);
            }
            else
            {
                AddLayerRow(PersonalizationLayerType.BasePdf, string.Empty);
            }
        }

        private void InitializeComponent()
        {
            Text = "Персоналізація PDF";
            Width = 1280;
            Height = 820;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            KeyPreview = true;

            var root = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 545,
                FixedPanel = FixedPanel.Panel1
            };

            Controls.Add(root);

            var left = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 10,
                Padding = new Padding(8),
                AutoSize = false
            };

            left.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105));
            left.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            left.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            root.Panel1.Controls.Add(left);

            AddFilePicker(left, 0, "Основа", _basePdfTextBox, "PDF (*.pdf)|*.pdf", SelectBasePdf);
            AddFilePicker(left, 1, "Дані", _dataTextBox, "TSV/CSV (*.csv;*.tsv;*.txt)|*.csv;*.tsv;*.txt|Усі файли (*.*)|*.*", SelectDataFile);
            AddFolderPicker(left, 2, "Вивід", _outputTextBox);

            left.Controls.Add(new Label { Text = "Експорт", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 3);
            _exportModeComboBox.Dock = DockStyle.Fill;
            _exportModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _exportModeComboBox.Items.AddRange(ExportModes.Keys.Cast<object>().ToArray());
            if (_exportModeComboBox.Items.Count > 0)
                _exportModeComboBox.SelectedIndex = 0;
            left.Controls.Add(_exportModeComboBox, 1, 3);
            _exportRowsTextBox.Dock = DockStyle.Fill;
            _exportRowsTextBox.TextChanged += (s, e) => SchedulePreviewUpdate();
            left.Controls.Add(_exportRowsTextBox, 2, 3);

            var previewLabel = new Label { Text = "Рядок", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            left.Controls.Add(previewLabel, 0, 4);
            _previewRow.Minimum = 1;
            _previewRow.Maximum = 1;
            _previewRow.Dock = DockStyle.Left;
            _previewRow.Width = 80;
            _previewRow.ValueChanged += (s, e) => SchedulePreviewUpdate();
            left.Controls.Add(_previewRow, 1, 4);
            _autoPreviewCheckBox.Text = "автоматичне оновлення";
            _autoPreviewCheckBox.Dock = DockStyle.Fill;
            _autoPreviewCheckBox.CheckedChanged += (s, e) => SchedulePreviewUpdate();
            left.Controls.Add(_autoPreviewCheckBox, 2, 4);

            _dataInfoLabel.Dock = DockStyle.Fill;
            _dataInfoLabel.TextAlign = ContentAlignment.MiddleLeft;
            left.SetColumnSpan(_dataInfoLabel, 3);
            left.Controls.Add(_dataInfoLabel, 0, 5);

            _sourceHintLabel.Dock = DockStyle.Fill;
            _sourceHintLabel.TextAlign = ContentAlignment.MiddleLeft;
            left.SetColumnSpan(_sourceHintLabel, 3);
            left.Controls.Add(_sourceHintLabel, 0, 6);

            ConfigureGrid();
            left.SetColumnSpan(_layersGrid, 3);
            left.Controls.Add(_layersGrid, 0, 7);

            var layerButtons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
            layerButtons.Controls.Add(MakeButton("+ основа", (s, e) => AddLayerRow(PersonalizationLayerType.BasePdf, _basePdfTextBox.Text)));
            layerButtons.Controls.Add(MakeButton("+ PDF", (s, e) => AddLayerRow(PersonalizationLayerType.Pdf, GetFirstColumn())));
            layerButtons.Controls.Add(MakeButton("+ текст", (s, e) => AddLayerRow(PersonalizationLayerType.Text, GetFirstColumn())));
            layerButtons.Controls.Add(MakeButton("+ код", (s, e) => AddLayerRow(PersonalizationLayerType.Code, GetFirstColumn())));
            layerButtons.Controls.Add(MakeButton("вгору", (s, e) => MoveSelectedRow(-1)));
            layerButtons.Controls.Add(MakeButton("вниз", (s, e) => MoveSelectedRow(1)));
            layerButtons.Controls.Add(MakeButton("дублювати", (s, e) => DuplicateSelectedRow()));
            layerButtons.Controls.Add(MakeButton("видалити", (s, e) => DeleteSelectedRow()));
            left.SetColumnSpan(layerButtons, 3);
            left.Controls.Add(layerButtons, 0, 8);

            var actionButtons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            actionButtons.Controls.Add(MakeButton("OK", OkClick));
            actionButtons.Controls.Add(MakeButton("Скасувати", (s, e) => Close()));
            actionButtons.Controls.Add(MakeButton("Preview", PreviewClick));
            actionButtons.Controls.Add(MakeButton("Завантажити шаблон", LoadTemplateClick));
            actionButtons.Controls.Add(MakeButton("Зберегти шаблон", SaveTemplateClick));
            left.SetColumnSpan(actionButtons, 3);
            left.Controls.Add(actionButtons, 0, 9);

            var previewLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            previewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            previewLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.Panel2.Controls.Add(previewLayout);

            var previewToolbar = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(4, 3, 4, 3)
            };
            previewToolbar.Controls.Add(new Label { Text = "Zoom", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 5, 4, 0) });
            _zoomComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _zoomComboBox.Width = 90;
            _zoomComboBox.Items.AddRange(new object[] { "Fit", "50%", "75%", "100%", "150%", "200%", "300%" });
            _zoomComboBox.SelectedIndex = 0;
            _zoomComboBox.SelectedIndexChanged += (s, e) => TryUpdatePreview(false);
            previewToolbar.Controls.Add(_zoomComboBox);
            previewLayout.Controls.Add(previewToolbar, 0, 0);

            _previewPanel.Dock = DockStyle.Fill;
            _previewPanel.AutoScroll = true;
            _previewPanel.BackColor = Color.White;
            _previewPanel.TabStop = true;
            _previewPanel.MouseWheel += PreviewMouseWheel;
            _previewPanel.MouseEnter += (s, e) => _previewPanel.Focus();
            previewLayout.Controls.Add(_previewPanel, 0, 1);

            _previewBox.Dock = DockStyle.Fill;
            _previewBox.BackColor = Color.White;
            _previewBox.SizeMode = PictureBoxSizeMode.Zoom;
            _previewBox.MouseDown += PreviewMouseDown;
            _previewBox.MouseMove += PreviewMouseMove;
            _previewBox.MouseUp += PreviewMouseUp;
            _previewBox.MouseWheel += PreviewMouseWheel;
            _previewBox.MouseEnter += (s, e) => _previewPanel.Focus();
            _previewPanel.Controls.Add(_previewBox);

            _basePdfTextBox.TextChanged += (s, e) => SchedulePreviewUpdate();
            _dataTextBox.TextChanged += (s, e) =>
            {
                UpdateDataInfo();
                SchedulePreviewUpdate();
            };
            _layersGrid.CellValueChanged += (s, e) => SchedulePreviewUpdate();
            _layersGrid.RowsRemoved += (s, e) => SchedulePreviewUpdate();
            _layersGrid.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (_layersGrid.IsCurrentCellDirty)
                    _layersGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            _previewTimer.Interval = 650;
            _previewTimer.Tick += (s, e) =>
            {
                _previewTimer.Stop();
                if (_autoPreviewCheckBox.Checked)
                    TryUpdatePreview(false);
            };

            UpdateDataInfo();
        }

        private static Button MakeButton(string text, EventHandler handler)
        {
            var button = new Button
            {
                Text = text,
                AutoSize = true,
                Height = 30,
                Margin = new Padding(3)
            };
            button.Click += handler;
            return button;
        }

        private void AddFilePicker(TableLayoutPanel panel, int row, string labelText, TextBox textBox, string filter, EventHandler browse)
        {
            panel.Controls.Add(new Label { Text = labelText, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
            textBox.Dock = DockStyle.Fill;
            panel.Controls.Add(textBox, 1, row);
            panel.Controls.Add(MakeButton("...", browse), 2, row);
        }

        private void AddFolderPicker(TableLayoutPanel panel, int row, string labelText, TextBox textBox)
        {
            panel.Controls.Add(new Label { Text = labelText, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
            textBox.Dock = DockStyle.Fill;
            panel.Controls.Add(textBox, 1, row);
            panel.Controls.Add(MakeButton("...", (s, e) =>
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    dialog.SelectedPath = Directory.Exists(textBox.Text) ? textBox.Text : string.Empty;
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                        textBox.Text = dialog.SelectedPath;
                }
            }), 2, row);
        }

        private void ConfigureGrid()
        {
            _layersGrid.Dock = DockStyle.Fill;
            _layersGrid.AllowUserToAddRows = false;
            _layersGrid.AllowUserToDeleteRows = true;
            _layersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            _layersGrid.RowHeadersVisible = false;
            _layersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _layersGrid.MultiSelect = false;

            _layersGrid.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Enabled", HeaderText = "✓", Width = 28 });
            _layersGrid.Columns.Add(new DataGridViewComboBoxColumn { Name = "Type", HeaderText = "Шар", Width = 88, DataSource = LayerTypes.Keys.ToList() });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Source", HeaderText = "Файл/колонка/текст", Width = 150 });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "X", HeaderText = "X мм", Width = 58 });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ymm", HeaderText = "Y мм", Width = 58 });
            _layersGrid.Columns.Add(new DataGridViewComboBoxColumn { Name = "BaseAnchor", HeaderText = "Від основи", Width = 112, DataSource = Anchors.Keys.ToList() });
            _layersGrid.Columns.Add(new DataGridViewComboBoxColumn { Name = "Anchor", HeaderText = "Прив'язка", Width = 112, DataSource = Anchors.Keys.ToList() });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Rotation", HeaderText = "°", Width = 44 });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Scale", HeaderText = "%", Width = 46 });
            _layersGrid.Columns.Add(new DataGridViewComboBoxColumn { Name = "CodeType", HeaderText = "Тип коду", Width = 82, DataSource = CodeTypes.Keys.ToList() });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "TargetWidth", HeaderText = "Ш код мм", Width = 70 });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "TargetHeight", HeaderText = "В код мм", Width = 70 });
            _layersGrid.Columns.Add(new DataGridViewCheckBoxColumn { Name = "ShowText", HeaderText = "текст", Width = 46 });
            _layersGrid.Columns.Add(new DataGridViewComboBoxColumn { Name = "Font", HeaderText = "Шрифт", Width = 120, DataSource = _fontNames });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "FontSize", HeaderText = "pt", Width = 44 });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "C", HeaderText = "C", Width = 38 });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "M", HeaderText = "M", Width = 38 });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColorY", HeaderText = "Y", Width = 38 });
            _layersGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "K", HeaderText = "K", Width = 38 });
        }

        private void SelectBasePdf(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog { Filter = "PDF (*.pdf)|*.pdf" })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                _basePdfTextBox.Text = dialog.FileName;
                if (string.IsNullOrWhiteSpace(_outputTextBox.Text))
                    _outputTextBox.Text = Path.GetDirectoryName(dialog.FileName);

                if (_layersGrid.Rows.Count == 0)
                    AddLayerRow(PersonalizationLayerType.BasePdf, dialog.FileName);
                else if (LayerTypes[Convert.ToString(_layersGrid.Rows[0].Cells["Type"].Value)] == PersonalizationLayerType.BasePdf)
                    _layersGrid.Rows[0].Cells["Source"].Value = dialog.FileName;

                SchedulePreviewUpdate();
            }
        }

        private void SelectDataFile(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog { Filter = "TSV/CSV (*.csv;*.tsv;*.txt)|*.csv;*.tsv;*.txt|Усі файли (*.*)|*.*" })
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _dataTextBox.Text = dialog.FileName;
                    UpdateDataInfo();
                    SchedulePreviewUpdate();
                }
            }
        }

        private void AddLayerRow(PersonalizationLayerType type, string source)
        {
            int row = _layersGrid.Rows.Add();
            var gridRow = _layersGrid.Rows[row];
            gridRow.Cells["Enabled"].Value = true;
            gridRow.Cells["Type"].Value = LayerTypes.First(x => x.Value == type).Key;
            gridRow.Cells["Source"].Value = source;
            gridRow.Cells["X"].Value = "0";
            gridRow.Cells["Ymm"].Value = "0";
            gridRow.Cells["BaseAnchor"].Value = "лівий нижній";
            gridRow.Cells["Anchor"].Value = "лівий нижній";
            gridRow.Cells["Rotation"].Value = "0";
            gridRow.Cells["Scale"].Value = "100";
            gridRow.Cells["CodeType"].Value = "Code-128";
            gridRow.Cells["TargetWidth"].Value = type == PersonalizationLayerType.Code ? "40" : "0";
            gridRow.Cells["TargetHeight"].Value = type == PersonalizationLayerType.Code ? "15" : "0";
            gridRow.Cells["ShowText"].Value = true;
            gridRow.Cells["Font"].Value = _fontNames.Contains("Arial") ? "Arial" : _fontNames.FirstOrDefault();
            gridRow.Cells["FontSize"].Value = "12";
            gridRow.Cells["C"].Value = "0";
            gridRow.Cells["M"].Value = "0";
            gridRow.Cells["ColorY"].Value = "0";
            gridRow.Cells["K"].Value = "100";
            SchedulePreviewUpdate();
        }

        private void MoveSelectedRow(int delta)
        {
            if (_layersGrid.CurrentRow == null)
                return;

            int index = _layersGrid.CurrentRow.Index;
            int target = index + delta;
            if (target < 0 || target >= _layersGrid.Rows.Count)
                return;

            DataGridViewRow row = _layersGrid.Rows[index];
            _layersGrid.Rows.RemoveAt(index);
            _layersGrid.Rows.Insert(target, row);
            _layersGrid.ClearSelection();
            _layersGrid.Rows[target].Selected = true;
            _layersGrid.CurrentCell = _layersGrid.Rows[target].Cells[0];
            SchedulePreviewUpdate();
        }

        private void DeleteSelectedRow()
        {
            if (_layersGrid.CurrentRow != null)
            {
                _layersGrid.Rows.RemoveAt(_layersGrid.CurrentRow.Index);
                SchedulePreviewUpdate();
            }
        }

        private void DuplicateSelectedRow()
        {
            if (_layersGrid.CurrentRow == null)
                return;

            int sourceIndex = _layersGrid.CurrentRow.Index;
            int targetIndex = sourceIndex + 1;
            int newIndex = _layersGrid.Rows.Add();
            DataGridViewRow source = _layersGrid.Rows[sourceIndex];
            DataGridViewRow clone = _layersGrid.Rows[newIndex];

            foreach (DataGridViewCell sourceCell in source.Cells)
            {
                clone.Cells[sourceCell.ColumnIndex].Value = sourceCell.Value;
            }

            if (targetIndex < _layersGrid.Rows.Count - 1)
            {
                _layersGrid.Rows.RemoveAt(newIndex);
                _layersGrid.Rows.Insert(targetIndex, clone);
            }

            _layersGrid.ClearSelection();
            _layersGrid.Rows[targetIndex].Selected = true;
            _layersGrid.CurrentCell = _layersGrid.Rows[targetIndex].Cells[0];
            SchedulePreviewUpdate();
        }

        private void PreviewClick(object sender, EventArgs e)
        {
            try
            {
                TryUpdatePreview(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Preview", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OkClick(object sender, EventArgs e)
        {
            try
            {
                Settings = ReadSettings(true);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Персоналізація PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private PdfPersonalizationSettings ReadSettings(bool requireOutput)
        {
            if (!File.Exists(_basePdfTextBox.Text))
                throw new InvalidOperationException("Виберіть PDF-основу.");
            if (!File.Exists(_dataTextBox.Text))
                throw new InvalidOperationException("Виберіть файл даних з табуляцією як роздільником.");
            if (requireOutput && string.IsNullOrWhiteSpace(_outputTextBox.Text))
                throw new InvalidOperationException("Виберіть папку для результатів.");

            var settings = new PdfPersonalizationSettings
            {
                BasePdfPath = _basePdfTextBox.Text,
                DataFilePath = _dataTextBox.Text,
                OutputFolder = _outputTextBox.Text
                ,
                ExportMode = ExportModes.TryGetValue(Convert.ToString(_exportModeComboBox.SelectedItem) ?? string.Empty, out PersonalizationExportMode exportMode)
                    ? exportMode
                    : PersonalizationExportMode.SeparateFiles,
                ExportRows = _exportRowsTextBox.Text
            };

            foreach (DataGridViewRow row in _layersGrid.Rows)
            {
                string typeText = Convert.ToString(row.Cells["Type"].Value);
                string codeTypeText = Convert.ToString(row.Cells["CodeType"].Value);
                string baseAnchorText = Convert.ToString(row.Cells["BaseAnchor"].Value);
                string anchorText = Convert.ToString(row.Cells["Anchor"].Value);

                settings.Layers.Add(new PdfPersonalizationLayer
                {
                    Enabled = Convert.ToBoolean(row.Cells["Enabled"].Value ?? true),
                    Type = LayerTypes.TryGetValue(typeText ?? string.Empty, out PersonalizationLayerType type) ? type : PersonalizationLayerType.Text,
                    Source = Convert.ToString(row.Cells["Source"].Value),
                    Xmm = PdfPersonalizationData.ParseDouble(row.Cells["X"].Value, 0),
                    Ymm = PdfPersonalizationData.ParseDouble(row.Cells["Ymm"].Value, 0),
                    BaseAnchor = Anchors.TryGetValue(baseAnchorText ?? string.Empty, out PersonalizationAnchorPoint baseAnchor) ? baseAnchor : PersonalizationAnchorPoint.BottomLeft,
                    Anchor = Anchors.TryGetValue(anchorText ?? string.Empty, out PersonalizationAnchorPoint anchor) ? anchor : PersonalizationAnchorPoint.BottomLeft,
                    Rotation = PdfPersonalizationData.ParseDouble(row.Cells["Rotation"].Value, 0),
                    ScalePercent = PdfPersonalizationData.ParseDouble(row.Cells["Scale"].Value, 100),
                    CodeType = CodeTypes.TryGetValue(codeTypeText ?? string.Empty, out PersonalizationCodeType codeType) ? codeType : PersonalizationCodeType.Code128,
                    TargetWidthMm = PdfPersonalizationData.ParseDouble(row.Cells["TargetWidth"].Value, 0),
                    TargetHeightMm = PdfPersonalizationData.ParseDouble(row.Cells["TargetHeight"].Value, 0),
                    ShowHumanReadableText = Convert.ToBoolean(row.Cells["ShowText"].Value ?? true),
                    FontName = Convert.ToString(row.Cells["Font"].Value) ?? "Arial",
                    FontSize = PdfPersonalizationData.ParseDouble(row.Cells["FontSize"].Value, 12),
                    C = PdfPersonalizationData.ParseDouble(row.Cells["C"].Value, 0),
                    M = PdfPersonalizationData.ParseDouble(row.Cells["M"].Value, 0),
                    Y = PdfPersonalizationData.ParseDouble(row.Cells["ColorY"].Value, 0),
                    K = PdfPersonalizationData.ParseDouble(row.Cells["K"].Value, 100)
                });
            }

            if (settings.Layers.Count == 0)
                throw new InvalidOperationException("Додайте хоча б один шар.");

            return settings;
        }

        private void TryUpdatePreview(bool showErrors)
        {
            string tempFile = null;

            try
            {
                if (_renderingPreview)
                    return;

                _renderingPreview = true;
                Settings = ReadSettings(false);
                tempFile = Path.Combine(Path.GetTempPath(), $"pdf_personalization_preview_{Guid.NewGuid():N}.pdf");
                new PdfPersonalizationRenderer().RenderPreview(Settings, (int)_previewRow.Value - 1, tempFile);

                using (var bitmap = PdfHelper.RenderByTrimBox(tempFile, 0, GetPreviewRenderDpi()))
                {
                    Image old = _previewBox.Image;
                    _previewBox.Image = new Bitmap(bitmap);
                    ApplyPreviewZoom();
                    old?.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (showErrors)
                    MessageBox.Show(this, ex.Message, "Preview", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(tempFile))
                    TryDelete(tempFile);

                _renderingPreview = false;
            }
        }

        private void SchedulePreviewUpdate()
        {
            if (_loadingTemplate || !_autoPreviewCheckBox.Checked)
                return;

            _previewTimer.Stop();
            _previewTimer.Start();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (IsPreviewMoveKey(keyData) && MoveSelectedLayerByKey(keyData))
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private static bool IsPreviewMoveKey(Keys keyData)
        {
            Keys key = keyData & Keys.KeyCode;
            return key == Keys.Left || key == Keys.Right || key == Keys.Up || key == Keys.Down;
        }

        private bool MoveSelectedLayerByKey(Keys keyData)
        {
            if (!PreviewHasFocus() || _layersGrid.CurrentRow == null)
                return false;

            double step = 1.0;
            if ((keyData & Keys.Shift) == Keys.Shift)
                step = 10.0;
            else if ((keyData & Keys.Control) == Keys.Control)
                step = 0.1;

            Keys key = keyData & Keys.KeyCode;
            double dx = 0;
            double dy = 0;

            if (key == Keys.Left)
                dx = -step;
            else if (key == Keys.Right)
                dx = step;
            else if (key == Keys.Up)
                dy = step;
            else if (key == Keys.Down)
                dy = -step;
            else
                return false;

            MoveSelectedLayer(dx, dy, true);
            return true;
        }

        private bool PreviewHasFocus()
        {
            return _previewBox.Focused || _previewPanel.Focused || _previewPanel.ContainsFocus;
        }

        private void PreviewMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _layersGrid.CurrentRow == null)
                return;

            _draggingPreviewLayer = true;
            _dragStartPoint = e.Location;
            _dragStartXmm = GetCellDouble(_layersGrid.CurrentRow, "X", 0);
            _dragStartYmm = GetCellDouble(_layersGrid.CurrentRow, "Ymm", 0);
            _previewBox.Capture = true;
            _previewPanel.Focus();
        }

        private void PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_draggingPreviewLayer || _layersGrid.CurrentRow == null || _previewBox.Image == null)
                return;

            double pixelsPerImagePixel = GetPreviewScreenScale();
            if (pixelsPerImagePixel <= 0)
                return;

            double imageDx = (e.X - _dragStartPoint.X) / pixelsPerImagePixel;
            double imageDy = (e.Y - _dragStartPoint.Y) / pixelsPerImagePixel;
            double dpiX = _previewBox.Image.HorizontalResolution > 0 ? _previewBox.Image.HorizontalResolution : 110;
            double dpiY = _previewBox.Image.VerticalResolution > 0 ? _previewBox.Image.VerticalResolution : 110;
            double dxMm = imageDx * 25.4 / dpiX;
            double dyMm = -imageDy * 25.4 / dpiY;

            SetSelectedLayerPosition(_dragStartXmm + dxMm, _dragStartYmm + dyMm, false);
            SchedulePreviewUpdate();
        }

        private void PreviewMouseUp(object sender, MouseEventArgs e)
        {
            if (!_draggingPreviewLayer)
                return;

            _draggingPreviewLayer = false;
            _previewBox.Capture = false;
            TryUpdatePreview(false);
        }

        private void PreviewMouseWheel(object sender, MouseEventArgs e)
        {
            if (_previewBox.Image == null)
                return;

            int[] zooms = { 50, 75, 100, 150, 200, 300 };
            int current = GetCurrentZoomPercent();
            int index = Array.IndexOf(zooms, current);
            if (index < 0)
                index = 2;

            index += e.Delta > 0 ? 1 : -1;
            index = Math.Max(0, Math.Min(zooms.Length - 1, index));
            _zoomComboBox.SelectedItem = $"{zooms[index]}%";
        }

        private void ApplyPreviewZoom()
        {
            if (_previewBox.Image == null)
                return;

            string selected = Convert.ToString(_zoomComboBox.SelectedItem);
            if (string.Equals(selected, "Fit", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(selected))
            {
                _previewBox.Dock = DockStyle.Fill;
                _previewBox.SizeMode = PictureBoxSizeMode.Zoom;
                return;
            }

            int zoom = GetCurrentZoomPercent();
            _previewBox.Dock = DockStyle.None;
            _previewBox.SizeMode = PictureBoxSizeMode.StretchImage;
            _previewBox.Size = new Size(
                Math.Max(1, (int)Math.Round(_previewBox.Image.Width * zoom / 100.0)),
                Math.Max(1, (int)Math.Round(_previewBox.Image.Height * zoom / 100.0)));
            _previewBox.Location = new Point(0, 0);
        }

        private int GetCurrentZoomPercent()
        {
            string selected = Convert.ToString(_zoomComboBox.SelectedItem) ?? "100%";
            selected = selected.Trim().TrimEnd('%');
            return int.TryParse(selected, out int zoom) ? zoom : 100;
        }

        private int GetPreviewRenderDpi()
        {
            string selected = Convert.ToString(_zoomComboBox.SelectedItem);
            if (string.Equals(selected, "Fit", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(selected))
                return 110;

            int zoom = GetCurrentZoomPercent();
            return Math.Max(110, Math.Min(330, (int)Math.Round(110 * zoom / 100.0)));
        }

        private double GetPreviewScreenScale()
        {
            if (_previewBox.Image == null)
                return 0;

            if (_previewBox.SizeMode == PictureBoxSizeMode.StretchImage)
                return _previewBox.Width / (double)_previewBox.Image.Width;

            double scaleX = _previewBox.ClientSize.Width / (double)_previewBox.Image.Width;
            double scaleY = _previewBox.ClientSize.Height / (double)_previewBox.Image.Height;
            return Math.Min(scaleX, scaleY);
        }

        private void MoveSelectedLayer(double dxMm, double dyMm, bool refreshPreview)
        {
            if (_layersGrid.CurrentRow == null)
                return;

            double x = GetCellDouble(_layersGrid.CurrentRow, "X", 0) + dxMm;
            double y = GetCellDouble(_layersGrid.CurrentRow, "Ymm", 0) + dyMm;
            SetSelectedLayerPosition(x, y, refreshPreview);
        }

        private void SetSelectedLayerPosition(double xMm, double yMm, bool refreshPreview)
        {
            if (_layersGrid.CurrentRow == null)
                return;

            _layersGrid.CurrentRow.Cells["X"].Value = xMm.ToString("0.###", CultureInfo.InvariantCulture);
            _layersGrid.CurrentRow.Cells["Ymm"].Value = yMm.ToString("0.###", CultureInfo.InvariantCulture);

            if (refreshPreview)
                TryUpdatePreview(false);
            else
                SchedulePreviewUpdate();
        }

        private static double GetCellDouble(DataGridViewRow row, string columnName, double fallback)
        {
            return PdfPersonalizationData.ParseDouble(row.Cells[columnName].Value, fallback);
        }

        private void SaveTemplateClick(object sender, EventArgs e)
        {
            try
            {
                var settings = ReadSettings(false);
                using (var dialog = new SaveFileDialog
                {
                    Filter = "PDF personalization template (*.json)|*.json|Усі файли (*.*)|*.*",
                    FileName = "pdf-personalization-template.json"
                })
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK)
                        return;

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    File.WriteAllText(dialog.FileName, JsonSerializer.Serialize(settings, options));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Зберегти шаблон", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadTemplateClick(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Filter = "PDF personalization template (*.json)|*.json|Усі файли (*.*)|*.*"
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    var settings = JsonSerializer.Deserialize<PdfPersonalizationSettings>(File.ReadAllText(dialog.FileName));
                    if (settings == null)
                        return;

                    ApplyTemplate(settings);
                    SchedulePreviewUpdate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Завантажити шаблон", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ApplyTemplate(PdfPersonalizationSettings settings)
        {
            _loadingTemplate = true;
            try
            {
                _basePdfTextBox.Text = settings.BasePdfPath ?? string.Empty;
                _dataTextBox.Text = settings.DataFilePath ?? string.Empty;
                _outputTextBox.Text = settings.OutputFolder ?? string.Empty;
                _exportModeComboBox.SelectedItem = ExportModes.FirstOrDefault(x => x.Value == settings.ExportMode).Key ?? "Все окремими файлами";
                _exportRowsTextBox.Text = settings.ExportRows ?? string.Empty;
                _layersGrid.Rows.Clear();

                foreach (var layer in settings.Layers ?? Enumerable.Empty<PdfPersonalizationLayer>())
                    AddLayerRow(layer);

                UpdateDataInfo();
            }
            finally
            {
                _loadingTemplate = false;
            }
        }

        private void AddLayerRow(PdfPersonalizationLayer layer)
        {
            AddLayerRow(layer.Type, layer.Source);
            var row = _layersGrid.Rows[_layersGrid.Rows.Count - 1];
            row.Cells["Enabled"].Value = layer.Enabled;
            row.Cells["X"].Value = layer.Xmm.ToString(CultureInfo.InvariantCulture);
            row.Cells["Ymm"].Value = layer.Ymm.ToString(CultureInfo.InvariantCulture);
            row.Cells["BaseAnchor"].Value = Anchors.FirstOrDefault(x => x.Value == layer.BaseAnchor).Key ?? "лівий нижній";
            row.Cells["Anchor"].Value = Anchors.FirstOrDefault(x => x.Value == layer.Anchor).Key ?? "лівий нижній";
            row.Cells["Rotation"].Value = layer.Rotation.ToString(CultureInfo.InvariantCulture);
            row.Cells["Scale"].Value = layer.ScalePercent.ToString(CultureInfo.InvariantCulture);
            row.Cells["CodeType"].Value = CodeTypes.FirstOrDefault(x => x.Value == layer.CodeType).Key ?? "Code-128";
            row.Cells["TargetWidth"].Value = layer.TargetWidthMm.ToString(CultureInfo.InvariantCulture);
            row.Cells["TargetHeight"].Value = layer.TargetHeightMm.ToString(CultureInfo.InvariantCulture);
            row.Cells["ShowText"].Value = layer.ShowHumanReadableText;
            row.Cells["Font"].Value = _fontNames.Contains(layer.FontName) ? layer.FontName : (_fontNames.FirstOrDefault() ?? "Arial");
            row.Cells["FontSize"].Value = layer.FontSize.ToString(CultureInfo.InvariantCulture);
            row.Cells["C"].Value = layer.C.ToString(CultureInfo.InvariantCulture);
            row.Cells["M"].Value = layer.M.ToString(CultureInfo.InvariantCulture);
            row.Cells["ColorY"].Value = layer.Y.ToString(CultureInfo.InvariantCulture);
            row.Cells["K"].Value = layer.K.ToString(CultureInfo.InvariantCulture);
        }

        private void UpdateDataInfo()
        {
            _dataInfoLabel.Text = "CSV/TSV: роздільник табуляція. Діапазон експорту: порожньо = всі, приклад 1-5,8,10.";
            _sourceHintLabel.Text = "У полі джерела вкажіть назву колонки або стале значення. Для шару PDF значення трактується як шлях до файлу.";

            if (!File.Exists(_dataTextBox.Text))
                return;

            try
            {
                PdfPersonalizationData data = PdfPersonalizationData.Load(_dataTextBox.Text);
                _previewRow.Maximum = Math.Max(1, data.Rows.Count);
                _dataInfoLabel.Text = $"Рядків: {data.Rows.Count}; колонки: {string.Join(", ", data.Headers)}";
            }
            catch (Exception ex)
            {
                _dataInfoLabel.Text = ex.Message;
            }
        }

        private string GetFirstColumn()
        {
            if (!File.Exists(_dataTextBox.Text))
                return string.Empty;

            return PdfPersonalizationData.Load(_dataTextBox.Text).Columns.FirstOrDefault() ?? string.Empty;
        }

        private static void TryDelete(string file)
        {
            try
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            catch
            {
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _previewBox.Image?.Dispose();

            base.Dispose(disposing);
        }
    }
}
