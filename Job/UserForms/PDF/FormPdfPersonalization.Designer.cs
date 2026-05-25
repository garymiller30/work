using System.Drawing;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public sealed partial class FormPdfPersonalization
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox _basePdfTextBox;
        private TextBox _dataTextBox;
        private TextBox _outputTextBox;
        private ComboBox _exportModeComboBox;
        private TextBox _exportRowsTextBox;
        private NumericUpDown _previewRow;
        private CheckBox _autoPreviewCheckBox;
        private Timer _previewTimer;
        private Label _dataInfoLabel;
        private Label _sourceHintLabel;
        private DataGridView _layersGrid;
        private Panel _previewPanel;
        private PictureBox _previewBox;
        private ComboBox _zoomComboBox;
        private SplitContainer _rootSplitContainer;
        private TableLayoutPanel _leftLayout;
        private TableLayoutPanel _previewLayout;
        private FlowLayoutPanel _layerButtonsPanel;
        private FlowLayoutPanel _actionButtonsPanel;
        private FlowLayoutPanel _previewToolbarPanel;
        private Button _browseBasePdfButton;
        private Button _browseDataButton;
        private Button _browseOutputButton;
        private Button _addBaseLayerButton;
        private Button _addPdfLayerButton;
        private Button _addTextLayerButton;
        private Button _addCodeLayerButton;
        private Button _moveLayerUpButton;
        private Button _moveLayerDownButton;
        private Button _duplicateLayerButton;
        private Button _deleteLayerButton;
        private Button _okButton;
        private Button _cancelButton;
        private Button _previewButton;
        private Button _loadTemplateButton;
        private Button _saveTemplateButton;
        private Label _basePdfLabel;
        private Label _dataLabel;
        private Label _outputLabel;
        private Label _exportLabel;
        private Label _previewRowLabel;
        private Label _zoomLabel;
        private DataGridViewCheckBoxColumn colEnabled;
        private DataGridViewComboBoxColumn colType;
        private DataGridViewTextBoxColumn colSource;
        private DataGridViewTextBoxColumn colX;
        private DataGridViewTextBoxColumn colYmm;
        private DataGridViewComboBoxColumn colBaseAnchor;
        private DataGridViewComboBoxColumn colAnchor;
        private DataGridViewTextBoxColumn colRotation;
        private DataGridViewTextBoxColumn colScale;
        private DataGridViewComboBoxColumn colCodeType;
        private DataGridViewTextBoxColumn colTargetWidth;
        private DataGridViewTextBoxColumn colTargetHeight;
        private DataGridViewCheckBoxColumn colShowText;
        private DataGridViewComboBoxColumn colFont;
        private DataGridViewTextBoxColumn colFontSize;
        private DataGridViewTextBoxColumn colC;
        private DataGridViewTextBoxColumn colM;
        private DataGridViewTextBoxColumn colColorY;
        private DataGridViewTextBoxColumn colK;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _previewBox?.Image?.Dispose();
                _previewComposer?.Dispose();
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _previewTimer = new Timer(components);
            _rootSplitContainer = new SplitContainer();
            _leftLayout = new TableLayoutPanel();
            _basePdfLabel = new Label();
            _dataLabel = new Label();
            _outputLabel = new Label();
            _exportLabel = new Label();
            _previewRowLabel = new Label();
            _basePdfTextBox = new TextBox();
            _dataTextBox = new TextBox();
            _outputTextBox = new TextBox();
            _browseBasePdfButton = new Button();
            _browseDataButton = new Button();
            _browseOutputButton = new Button();
            _exportModeComboBox = new ComboBox();
            _exportRowsTextBox = new TextBox();
            _previewRow = new NumericUpDown();
            _autoPreviewCheckBox = new CheckBox();
            _dataInfoLabel = new Label();
            _sourceHintLabel = new Label();
            _layersGrid = new DataGridView();
            colEnabled = new DataGridViewCheckBoxColumn();
            colType = new DataGridViewComboBoxColumn();
            colSource = new DataGridViewTextBoxColumn();
            colX = new DataGridViewTextBoxColumn();
            colYmm = new DataGridViewTextBoxColumn();
            colBaseAnchor = new DataGridViewComboBoxColumn();
            colAnchor = new DataGridViewComboBoxColumn();
            colRotation = new DataGridViewTextBoxColumn();
            colScale = new DataGridViewTextBoxColumn();
            colCodeType = new DataGridViewComboBoxColumn();
            colTargetWidth = new DataGridViewTextBoxColumn();
            colTargetHeight = new DataGridViewTextBoxColumn();
            colShowText = new DataGridViewCheckBoxColumn();
            colFont = new DataGridViewComboBoxColumn();
            colFontSize = new DataGridViewTextBoxColumn();
            colC = new DataGridViewTextBoxColumn();
            colM = new DataGridViewTextBoxColumn();
            colColorY = new DataGridViewTextBoxColumn();
            colK = new DataGridViewTextBoxColumn();
            _layerButtonsPanel = new FlowLayoutPanel();
            _addBaseLayerButton = new Button();
            _addPdfLayerButton = new Button();
            _addTextLayerButton = new Button();
            _addCodeLayerButton = new Button();
            _moveLayerUpButton = new Button();
            _moveLayerDownButton = new Button();
            _duplicateLayerButton = new Button();
            _deleteLayerButton = new Button();
            _actionButtonsPanel = new FlowLayoutPanel();
            _okButton = new Button();
            _cancelButton = new Button();
            _previewButton = new Button();
            _loadTemplateButton = new Button();
            _saveTemplateButton = new Button();
            _previewLayout = new TableLayoutPanel();
            _previewToolbarPanel = new FlowLayoutPanel();
            _zoomLabel = new Label();
            _zoomComboBox = new ComboBox();
            _previewPanel = new Panel();
            _previewBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)_rootSplitContainer).BeginInit();
            _rootSplitContainer.Panel1.SuspendLayout();
            _rootSplitContainer.Panel2.SuspendLayout();
            _rootSplitContainer.SuspendLayout();
            _leftLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_previewRow).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_layersGrid).BeginInit();
            _layerButtonsPanel.SuspendLayout();
            _actionButtonsPanel.SuspendLayout();
            _previewLayout.SuspendLayout();
            _previewToolbarPanel.SuspendLayout();
            _previewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_previewBox).BeginInit();
            SuspendLayout();
            // 
            // _previewTimer
            // 
            _previewTimer.Interval = 650;
            _previewTimer.Tick += PreviewTimerTick;
            // 
            // _rootSplitContainer
            // 
            _rootSplitContainer.Dock = DockStyle.Fill;
            _rootSplitContainer.FixedPanel = FixedPanel.Panel1;
            _rootSplitContainer.Location = new Point(0, 0);
            _rootSplitContainer.Name = "_rootSplitContainer";
            // 
            // _rootSplitContainer.Panel1
            // 
            _rootSplitContainer.Panel1.Controls.Add(_leftLayout);
            // 
            // _rootSplitContainer.Panel2
            // 
            _rootSplitContainer.Panel2.Controls.Add(_previewLayout);
            _rootSplitContainer.Size = new Size(1280, 820);
            _rootSplitContainer.SplitterDistance = 545;
            _rootSplitContainer.TabIndex = 0;
            // 
            // _leftLayout
            // 
            _leftLayout.ColumnCount = 3;
            _leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105F));
            _leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            _leftLayout.Controls.Add(_basePdfLabel, 0, 0);
            _leftLayout.Controls.Add(_dataLabel, 0, 1);
            _leftLayout.Controls.Add(_outputLabel, 0, 2);
            _leftLayout.Controls.Add(_exportLabel, 0, 3);
            _leftLayout.Controls.Add(_previewRowLabel, 0, 4);
            _leftLayout.Controls.Add(_basePdfTextBox, 1, 0);
            _leftLayout.Controls.Add(_dataTextBox, 1, 1);
            _leftLayout.Controls.Add(_outputTextBox, 1, 2);
            _leftLayout.Controls.Add(_browseBasePdfButton, 2, 0);
            _leftLayout.Controls.Add(_browseDataButton, 2, 1);
            _leftLayout.Controls.Add(_browseOutputButton, 2, 2);
            _leftLayout.Controls.Add(_exportModeComboBox, 1, 3);
            _leftLayout.Controls.Add(_exportRowsTextBox, 2, 3);
            _leftLayout.Controls.Add(_previewRow, 1, 4);
            _leftLayout.Controls.Add(_autoPreviewCheckBox, 2, 4);
            _leftLayout.Controls.Add(_dataInfoLabel, 0, 5);
            _leftLayout.Controls.Add(_sourceHintLabel, 0, 6);
            _leftLayout.Controls.Add(_layersGrid, 0, 7);
            _leftLayout.Controls.Add(_layerButtonsPanel, 0, 8);
            _leftLayout.Controls.Add(_actionButtonsPanel, 0, 9);
            _leftLayout.Dock = DockStyle.Fill;
            _leftLayout.Location = new Point(0, 0);
            _leftLayout.Name = "_leftLayout";
            _leftLayout.Padding = new Padding(8);
            _leftLayout.RowCount = 10;
            _leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            _leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            _leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            _leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            _leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            _leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            _leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            _leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            _leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            _leftLayout.Size = new Size(545, 820);
            _leftLayout.TabIndex = 0;
            // 
            // _basePdfLabel
            // 
            _basePdfLabel.Dock = DockStyle.Fill;
            _basePdfLabel.Location = new Point(11, 8);
            _basePdfLabel.Name = "_basePdfLabel";
            _basePdfLabel.Size = new Size(99, 34);
            _basePdfLabel.TabIndex = 0;
            _basePdfLabel.Text = "Основа";
            _basePdfLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _dataLabel
            // 
            _dataLabel.Dock = DockStyle.Fill;
            _dataLabel.Location = new Point(11, 42);
            _dataLabel.Name = "_dataLabel";
            _dataLabel.Size = new Size(99, 34);
            _dataLabel.TabIndex = 1;
            _dataLabel.Text = "Дані";
            _dataLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _outputLabel
            // 
            _outputLabel.Dock = DockStyle.Fill;
            _outputLabel.Location = new Point(11, 76);
            _outputLabel.Name = "_outputLabel";
            _outputLabel.Size = new Size(99, 34);
            _outputLabel.TabIndex = 2;
            _outputLabel.Text = "Вивід";
            _outputLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _exportLabel
            // 
            _exportLabel.Dock = DockStyle.Fill;
            _exportLabel.Location = new Point(11, 110);
            _exportLabel.Name = "_exportLabel";
            _exportLabel.Size = new Size(99, 34);
            _exportLabel.TabIndex = 3;
            _exportLabel.Text = "Експорт";
            _exportLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _previewRowLabel
            // 
            _previewRowLabel.Dock = DockStyle.Fill;
            _previewRowLabel.Location = new Point(11, 144);
            _previewRowLabel.Name = "_previewRowLabel";
            _previewRowLabel.Size = new Size(99, 34);
            _previewRowLabel.TabIndex = 4;
            _previewRowLabel.Text = "Рядок";
            _previewRowLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _basePdfTextBox
            // 
            _basePdfTextBox.Dock = DockStyle.Fill;
            _basePdfTextBox.Location = new Point(116, 11);
            _basePdfTextBox.Name = "_basePdfTextBox";
            _basePdfTextBox.Size = new Size(328, 23);
            _basePdfTextBox.TabIndex = 5;
            _basePdfTextBox.TextChanged += BasePdfTextChanged;
            // 
            // _dataTextBox
            // 
            _dataTextBox.Dock = DockStyle.Fill;
            _dataTextBox.Location = new Point(116, 45);
            _dataTextBox.Name = "_dataTextBox";
            _dataTextBox.Size = new Size(328, 23);
            _dataTextBox.TabIndex = 6;
            _dataTextBox.TextChanged += DataTextChanged;
            // 
            // _outputTextBox
            // 
            _outputTextBox.Dock = DockStyle.Fill;
            _outputTextBox.Location = new Point(116, 79);
            _outputTextBox.Name = "_outputTextBox";
            _outputTextBox.Size = new Size(328, 23);
            _outputTextBox.TabIndex = 7;
            // 
            // _browseBasePdfButton
            // 
            _browseBasePdfButton.Dock = DockStyle.Fill;
            _browseBasePdfButton.Location = new Point(450, 11);
            _browseBasePdfButton.Name = "_browseBasePdfButton";
            _browseBasePdfButton.Size = new Size(84, 28);
            _browseBasePdfButton.TabIndex = 8;
            _browseBasePdfButton.Text = "...";
            _browseBasePdfButton.Click += SelectBasePdf;
            // 
            // _browseDataButton
            // 
            _browseDataButton.Dock = DockStyle.Fill;
            _browseDataButton.Location = new Point(450, 45);
            _browseDataButton.Name = "_browseDataButton";
            _browseDataButton.Size = new Size(84, 28);
            _browseDataButton.TabIndex = 9;
            _browseDataButton.Text = "...";
            _browseDataButton.Click += SelectDataFile;
            // 
            // _browseOutputButton
            // 
            _browseOutputButton.Dock = DockStyle.Fill;
            _browseOutputButton.Location = new Point(450, 79);
            _browseOutputButton.Name = "_browseOutputButton";
            _browseOutputButton.Size = new Size(84, 28);
            _browseOutputButton.TabIndex = 10;
            _browseOutputButton.Text = "...";
            _browseOutputButton.Click += SelectOutputFolder;
            // 
            // _exportModeComboBox
            // 
            _exportModeComboBox.Dock = DockStyle.Fill;
            _exportModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _exportModeComboBox.Items.AddRange(new object[] { "Все окремими файлами", "Все в одному файлі" });
            _exportModeComboBox.Location = new Point(116, 113);
            _exportModeComboBox.Name = "_exportModeComboBox";
            _exportModeComboBox.Size = new Size(328, 23);
            _exportModeComboBox.TabIndex = 11;
            // 
            // _exportRowsTextBox
            // 
            _exportRowsTextBox.Dock = DockStyle.Fill;
            _exportRowsTextBox.Location = new Point(450, 113);
            _exportRowsTextBox.Name = "_exportRowsTextBox";
            _exportRowsTextBox.Size = new Size(84, 23);
            _exportRowsTextBox.TabIndex = 12;
            _exportRowsTextBox.TextChanged += SchedulePreviewTextChanged;
            // 
            // _previewRow
            // 
            _previewRow.Dock = DockStyle.Left;
            _previewRow.Location = new Point(116, 147);
            _previewRow.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            _previewRow.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _previewRow.Name = "_previewRow";
            _previewRow.Size = new Size(80, 23);
            _previewRow.TabIndex = 13;
            _previewRow.Value = new decimal(new int[] { 1, 0, 0, 0 });
            _previewRow.ValueChanged += SchedulePreviewTextChanged;
            // 
            // _autoPreviewCheckBox
            // 
            _autoPreviewCheckBox.Dock = DockStyle.Fill;
            _autoPreviewCheckBox.Location = new Point(450, 147);
            _autoPreviewCheckBox.Name = "_autoPreviewCheckBox";
            _autoPreviewCheckBox.Size = new Size(84, 28);
            _autoPreviewCheckBox.TabIndex = 14;
            _autoPreviewCheckBox.Text = "автоматичне оновлення";
            _autoPreviewCheckBox.CheckedChanged += SchedulePreviewTextChanged;
            // 
            // _dataInfoLabel
            // 
            _leftLayout.SetColumnSpan(_dataInfoLabel, 3);
            _dataInfoLabel.Dock = DockStyle.Fill;
            _dataInfoLabel.Location = new Point(11, 178);
            _dataInfoLabel.Name = "_dataInfoLabel";
            _dataInfoLabel.Size = new Size(523, 24);
            _dataInfoLabel.TabIndex = 15;
            _dataInfoLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _sourceHintLabel
            // 
            _leftLayout.SetColumnSpan(_sourceHintLabel, 3);
            _sourceHintLabel.Dock = DockStyle.Fill;
            _sourceHintLabel.Location = new Point(11, 202);
            _sourceHintLabel.Name = "_sourceHintLabel";
            _sourceHintLabel.Size = new Size(523, 34);
            _sourceHintLabel.TabIndex = 16;
            _sourceHintLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _layersGrid
            // 
            _layersGrid.AllowUserToAddRows = false;
            _leftLayout.SetColumnSpan(_layersGrid, 3);
            _layersGrid.Dock = DockStyle.Fill;
            _layersGrid.Location = new Point(11, 239);
            _layersGrid.MultiSelect = false;
            _layersGrid.Name = "_layersGrid";
            _layersGrid.RowHeadersVisible = false;
            _layersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _layersGrid.Size = new Size(523, 486);
            _layersGrid.TabIndex = 17;
            _layersGrid.CellValueChanged += LayersGridCellValueChanged;
            _layersGrid.CurrentCellDirtyStateChanged += LayersGridCurrentCellDirtyStateChanged;
            _layersGrid.RowsRemoved += LayersGridRowsRemoved;
            // 
            // colEnabled
            // 
            colEnabled.HeaderText = "✓";
            colEnabled.Name = "Enabled";
            colEnabled.Width = 28;
            // 
            // colType
            // 
            colType.HeaderText = "Шар";
            colType.Items.AddRange(new object[] { "Основа PDF", "PDF", "Текст", "Код" });
            colType.Name = "Type";
            colType.Width = 88;
            // 
            // colSource
            // 
            colSource.HeaderText = "Файл/колонка/текст";
            colSource.Name = "Source";
            colSource.Width = 150;
            // 
            // colX
            // 
            colX.HeaderText = "X мм";
            colX.Name = "X";
            colX.Width = 58;
            // 
            // colYmm
            // 
            colYmm.HeaderText = "Y мм";
            colYmm.Name = "Ymm";
            colYmm.Width = 58;
            // 
            // colBaseAnchor
            // 
            colBaseAnchor.HeaderText = "Від основи";
            colBaseAnchor.Items.AddRange(new object[] { "лівий нижній", "лівий центр", "лівий верхній", "центр нижній", "центр", "центр верхній", "правий нижній", "правий центр", "правий верхній" });
            colBaseAnchor.Name = "BaseAnchor";
            colBaseAnchor.Width = 112;
            // 
            // colAnchor
            // 
            colAnchor.HeaderText = "Прив'язка";
            colAnchor.Items.AddRange(new object[] { "лівий нижній", "лівий центр", "лівий верхній", "центр нижній", "центр", "центр верхній", "правий нижній", "правий центр", "правий верхній" });
            colAnchor.Name = "Anchor";
            colAnchor.Width = 112;
            // 
            // colRotation
            // 
            colRotation.HeaderText = "°";
            colRotation.Name = "Rotation";
            colRotation.Width = 44;
            // 
            // colScale
            // 
            colScale.HeaderText = "%";
            colScale.Name = "Scale";
            colScale.Width = 46;
            // 
            // colCodeType
            // 
            colCodeType.HeaderText = "Тип коду";
            colCodeType.Items.AddRange(new object[] { "Code-128", "EAN-13", "EAN-8", "QR" });
            colCodeType.Name = "CodeType";
            colCodeType.Width = 82;
            // 
            // colTargetWidth
            // 
            colTargetWidth.HeaderText = "Ш код мм";
            colTargetWidth.Name = "TargetWidth";
            colTargetWidth.Width = 70;
            // 
            // colTargetHeight
            // 
            colTargetHeight.HeaderText = "В код мм";
            colTargetHeight.Name = "TargetHeight";
            colTargetHeight.Width = 70;
            // 
            // colShowText
            // 
            colShowText.HeaderText = "текст";
            colShowText.Name = "ShowText";
            colShowText.Width = 46;
            // 
            // colFont
            // 
            colFont.HeaderText = "Шрифт";
            colFont.Name = "Font";
            colFont.Width = 120;
            // 
            // colFontSize
            // 
            colFontSize.HeaderText = "pt";
            colFontSize.Name = "FontSize";
            colFontSize.Width = 44;
            // 
            // colC
            // 
            colC.HeaderText = "C";
            colC.Name = "C";
            colC.Width = 38;
            // 
            // colM
            // 
            colM.HeaderText = "M";
            colM.Name = "M";
            colM.Width = 38;
            // 
            // colColorY
            // 
            colColorY.HeaderText = "Y";
            colColorY.Name = "ColorY";
            colColorY.Width = 38;
            // 
            // colK
            // 
            colK.HeaderText = "K";
            colK.Name = "K";
            colK.Width = 38;
            _layersGrid.Columns.AddRange(new DataGridViewColumn[] { colEnabled, colType, colSource, colX, colYmm, colBaseAnchor, colAnchor, colRotation, colScale, colCodeType, colTargetWidth, colTargetHeight, colShowText, colFont, colFontSize, colC, colM, colColorY, colK });
            // 
            // _layerButtonsPanel
            // 
            _leftLayout.SetColumnSpan(_layerButtonsPanel, 3);
            _layerButtonsPanel.Controls.Add(_addBaseLayerButton);
            _layerButtonsPanel.Controls.Add(_addPdfLayerButton);
            _layerButtonsPanel.Controls.Add(_addTextLayerButton);
            _layerButtonsPanel.Controls.Add(_addCodeLayerButton);
            _layerButtonsPanel.Controls.Add(_moveLayerUpButton);
            _layerButtonsPanel.Controls.Add(_moveLayerDownButton);
            _layerButtonsPanel.Controls.Add(_duplicateLayerButton);
            _layerButtonsPanel.Controls.Add(_deleteLayerButton);
            _layerButtonsPanel.Dock = DockStyle.Fill;
            _layerButtonsPanel.Location = new Point(11, 731);
            _layerButtonsPanel.Name = "_layerButtonsPanel";
            _layerButtonsPanel.Size = new Size(523, 36);
            _layerButtonsPanel.TabIndex = 18;
            // 
            // _addBaseLayerButton
            // 
            _addBaseLayerButton.AutoSize = true;
            _addBaseLayerButton.Location = new Point(3, 3);
            _addBaseLayerButton.Name = "_addBaseLayerButton";
            _addBaseLayerButton.Size = new Size(75, 25);
            _addBaseLayerButton.TabIndex = 0;
            _addBaseLayerButton.Text = "+ основа";
            _addBaseLayerButton.Click += AddBaseLayerClick;
            // 
            // _addPdfLayerButton
            // 
            _addPdfLayerButton.AutoSize = true;
            _addPdfLayerButton.Location = new Point(84, 3);
            _addPdfLayerButton.Name = "_addPdfLayerButton";
            _addPdfLayerButton.Size = new Size(75, 25);
            _addPdfLayerButton.TabIndex = 1;
            _addPdfLayerButton.Text = "+ PDF";
            _addPdfLayerButton.Click += AddPdfLayerClick;
            // 
            // _addTextLayerButton
            // 
            _addTextLayerButton.AutoSize = true;
            _addTextLayerButton.Location = new Point(165, 3);
            _addTextLayerButton.Name = "_addTextLayerButton";
            _addTextLayerButton.Size = new Size(75, 25);
            _addTextLayerButton.TabIndex = 2;
            _addTextLayerButton.Text = "+ текст";
            _addTextLayerButton.Click += AddTextLayerClick;
            // 
            // _addCodeLayerButton
            // 
            _addCodeLayerButton.AutoSize = true;
            _addCodeLayerButton.Location = new Point(246, 3);
            _addCodeLayerButton.Name = "_addCodeLayerButton";
            _addCodeLayerButton.Size = new Size(75, 25);
            _addCodeLayerButton.TabIndex = 3;
            _addCodeLayerButton.Text = "+ код";
            _addCodeLayerButton.Click += AddCodeLayerClick;
            // 
            // _moveLayerUpButton
            // 
            _moveLayerUpButton.AutoSize = true;
            _moveLayerUpButton.Location = new Point(327, 3);
            _moveLayerUpButton.Name = "_moveLayerUpButton";
            _moveLayerUpButton.Size = new Size(75, 25);
            _moveLayerUpButton.TabIndex = 4;
            _moveLayerUpButton.Text = "вгору";
            _moveLayerUpButton.Click += MoveLayerUpClick;
            // 
            // _moveLayerDownButton
            // 
            _moveLayerDownButton.AutoSize = true;
            _moveLayerDownButton.Location = new Point(408, 3);
            _moveLayerDownButton.Name = "_moveLayerDownButton";
            _moveLayerDownButton.Size = new Size(75, 25);
            _moveLayerDownButton.TabIndex = 5;
            _moveLayerDownButton.Text = "вниз";
            _moveLayerDownButton.Click += MoveLayerDownClick;
            // 
            // _duplicateLayerButton
            // 
            _duplicateLayerButton.AutoSize = true;
            _duplicateLayerButton.Location = new Point(3, 34);
            _duplicateLayerButton.Name = "_duplicateLayerButton";
            _duplicateLayerButton.Size = new Size(77, 25);
            _duplicateLayerButton.TabIndex = 6;
            _duplicateLayerButton.Text = "дублювати";
            _duplicateLayerButton.Click += DuplicateLayerClick;
            // 
            // _deleteLayerButton
            // 
            _deleteLayerButton.AutoSize = true;
            _deleteLayerButton.Location = new Point(86, 34);
            _deleteLayerButton.Name = "_deleteLayerButton";
            _deleteLayerButton.Size = new Size(75, 25);
            _deleteLayerButton.TabIndex = 7;
            _deleteLayerButton.Text = "видалити";
            _deleteLayerButton.Click += DeleteLayerClick;
            // 
            // _actionButtonsPanel
            // 
            _leftLayout.SetColumnSpan(_actionButtonsPanel, 3);
            _actionButtonsPanel.Controls.Add(_okButton);
            _actionButtonsPanel.Controls.Add(_cancelButton);
            _actionButtonsPanel.Controls.Add(_previewButton);
            _actionButtonsPanel.Controls.Add(_loadTemplateButton);
            _actionButtonsPanel.Controls.Add(_saveTemplateButton);
            _actionButtonsPanel.Dock = DockStyle.Fill;
            _actionButtonsPanel.FlowDirection = FlowDirection.RightToLeft;
            _actionButtonsPanel.Location = new Point(11, 773);
            _actionButtonsPanel.Name = "_actionButtonsPanel";
            _actionButtonsPanel.Size = new Size(523, 36);
            _actionButtonsPanel.TabIndex = 19;
            // 
            // _okButton
            // 
            _okButton.AutoSize = true;
            _okButton.Location = new Point(445, 3);
            _okButton.Name = "_okButton";
            _okButton.Size = new Size(75, 25);
            _okButton.TabIndex = 0;
            _okButton.Text = "OK";
            _okButton.Click += OkClick;
            // 
            // _cancelButton
            // 
            _cancelButton.AutoSize = true;
            _cancelButton.Location = new Point(364, 3);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Size = new Size(75, 25);
            _cancelButton.TabIndex = 1;
            _cancelButton.Text = "Скасувати";
            _cancelButton.Click += CancelClick;
            // 
            // _previewButton
            // 
            _previewButton.AutoSize = true;
            _previewButton.Location = new Point(283, 3);
            _previewButton.Name = "_previewButton";
            _previewButton.Size = new Size(75, 25);
            _previewButton.TabIndex = 2;
            _previewButton.Text = "Preview";
            _previewButton.Click += PreviewClick;
            // 
            // _loadTemplateButton
            // 
            _loadTemplateButton.AutoSize = true;
            _loadTemplateButton.Location = new Point(141, 3);
            _loadTemplateButton.Name = "_loadTemplateButton";
            _loadTemplateButton.Size = new Size(136, 25);
            _loadTemplateButton.TabIndex = 3;
            _loadTemplateButton.Text = "Завантажити шаблон";
            _loadTemplateButton.Click += LoadTemplateClick;
            // 
            // _saveTemplateButton
            // 
            _saveTemplateButton.AutoSize = true;
            _saveTemplateButton.Location = new Point(20, 3);
            _saveTemplateButton.Name = "_saveTemplateButton";
            _saveTemplateButton.Size = new Size(115, 25);
            _saveTemplateButton.TabIndex = 4;
            _saveTemplateButton.Text = "Зберегти шаблон";
            _saveTemplateButton.Click += SaveTemplateClick;
            // 
            // _previewLayout
            // 
            _previewLayout.ColumnCount = 1;
            _previewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _previewLayout.Controls.Add(_previewToolbarPanel, 0, 0);
            _previewLayout.Controls.Add(_previewPanel, 0, 1);
            _previewLayout.Dock = DockStyle.Fill;
            _previewLayout.Location = new Point(0, 0);
            _previewLayout.Name = "_previewLayout";
            _previewLayout.RowCount = 2;
            _previewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            _previewLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _previewLayout.Size = new Size(731, 820);
            _previewLayout.TabIndex = 0;
            // 
            // _previewToolbarPanel
            // 
            _previewToolbarPanel.Controls.Add(_zoomLabel);
            _previewToolbarPanel.Controls.Add(_zoomComboBox);
            _previewToolbarPanel.Dock = DockStyle.Fill;
            _previewToolbarPanel.Location = new Point(3, 3);
            _previewToolbarPanel.Name = "_previewToolbarPanel";
            _previewToolbarPanel.Padding = new Padding(4, 3, 4, 3);
            _previewToolbarPanel.Size = new Size(725, 28);
            _previewToolbarPanel.TabIndex = 0;
            // 
            // _zoomLabel
            // 
            _zoomLabel.AutoSize = true;
            _zoomLabel.Location = new Point(7, 3);
            _zoomLabel.Name = "_zoomLabel";
            _zoomLabel.Padding = new Padding(0, 5, 4, 0);
            _zoomLabel.Size = new Size(43, 20);
            _zoomLabel.TabIndex = 0;
            _zoomLabel.Text = "Zoom";
            // 
            // _zoomComboBox
            // 
            _zoomComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _zoomComboBox.Items.AddRange(new object[] { "Fit", "50%", "75%", "100%", "150%", "200%", "300%" });
            _zoomComboBox.Location = new Point(56, 6);
            _zoomComboBox.Name = "_zoomComboBox";
            _zoomComboBox.Size = new Size(90, 23);
            _zoomComboBox.TabIndex = 1;
            _zoomComboBox.SelectedIndexChanged += ZoomSelectedIndexChanged;
            // 
            // _previewPanel
            // 
            _previewPanel.AutoScroll = true;
            _previewPanel.BackColor = Color.White;
            _previewPanel.Controls.Add(_previewBox);
            _previewPanel.Dock = DockStyle.Fill;
            _previewPanel.Location = new Point(3, 37);
            _previewPanel.Name = "_previewPanel";
            _previewPanel.Size = new Size(725, 780);
            _previewPanel.TabIndex = 1;
            _previewPanel.TabStop = true;
            _previewPanel.MouseEnter += PreviewPanelMouseEnter;
            _previewPanel.MouseWheel += PreviewMouseWheel;
            // 
            // _previewBox
            // 
            _previewBox.BackColor = Color.White;
            _previewBox.Dock = DockStyle.Fill;
            _previewBox.Location = new Point(0, 0);
            _previewBox.Name = "_previewBox";
            _previewBox.Size = new Size(725, 780);
            _previewBox.SizeMode = PictureBoxSizeMode.Zoom;
            _previewBox.TabIndex = 0;
            _previewBox.TabStop = false;
            _previewBox.MouseDown += PreviewMouseDown;
            _previewBox.MouseEnter += PreviewPanelMouseEnter;
            _previewBox.MouseMove += PreviewMouseMove;
            _previewBox.MouseUp += PreviewMouseUp;
            _previewBox.MouseWheel += PreviewMouseWheel;
            // 
            // FormPdfPersonalization
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 820);
            Controls.Add(_rootSplitContainer);
            KeyPreview = true;
            MinimizeBox = false;
            Name = "FormPdfPersonalization";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Персоналізація PDF";
            _rootSplitContainer.Panel1.ResumeLayout(false);
            _rootSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_rootSplitContainer).EndInit();
            _rootSplitContainer.ResumeLayout(false);
            _leftLayout.ResumeLayout(false);
            _leftLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_previewRow).EndInit();
            ((System.ComponentModel.ISupportInitialize)_layersGrid).EndInit();
            _layerButtonsPanel.ResumeLayout(false);
            _layerButtonsPanel.PerformLayout();
            _actionButtonsPanel.ResumeLayout(false);
            _actionButtonsPanel.PerformLayout();
            _previewLayout.ResumeLayout(false);
            _previewToolbarPanel.ResumeLayout(false);
            _previewToolbarPanel.PerformLayout();
            _previewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_previewBox).EndInit();
            ResumeLayout(false);
        }
    }
}
