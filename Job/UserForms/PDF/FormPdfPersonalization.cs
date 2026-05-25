using JobSpace.Static.Pdf.Personalization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public sealed partial class FormPdfPersonalization : Form
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

        private readonly string[] _fontNames;
        private bool _loadingTemplate;
        private bool _draggingPreviewLayer;
        private bool _dragMovedPreviewLayer;
        private Point _dragStartPoint;
        private double _dragStartXmm;
        private double _dragStartYmm;
        private bool _previewIsActive;
        private bool _previewRenderRunning;
        private bool _previewRenderPending;
        private bool _previewRenderPendingShowErrors;
        private int _previewRequestVersion;
        private Timer _interactivePreviewTimer;
        private readonly PdfPersonalizationPreviewComposer _previewComposer = new PdfPersonalizationPreviewComposer();
        private PdfPersonalizationData _cachedPreviewData;
        private string _cachedPreviewDataPath;
        private DateTime _cachedPreviewDataWriteTimeUtc;

        public PdfPersonalizationSettings Settings { get; private set; }

        public FormPdfPersonalization()
            : this(null)
        {
        }

        public FormPdfPersonalization(string basePdfPath)
        {
            _fontNames = new InstalledFontCollection()
                .Families
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToArray();

            InitializeComponent();
            _interactivePreviewTimer = new Timer(components) { Interval = 180 };
            _interactivePreviewTimer.Tick += InteractivePreviewTimerTick;
            colFont.Items.AddRange(_fontNames.Cast<object>().ToArray());
            UpdateDataInfo();

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

        private void SelectOutputFolder(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = Directory.Exists(_outputTextBox.Text) ? _outputTextBox.Text : string.Empty;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    _outputTextBox.Text = dialog.SelectedPath;
            }
        }

        private void AddBaseLayerClick(object sender, EventArgs e) => AddLayerRow(PersonalizationLayerType.BasePdf, _basePdfTextBox.Text);

        private void AddPdfLayerClick(object sender, EventArgs e) => AddLayerRow(PersonalizationLayerType.Pdf, GetFirstColumn());

        private void AddTextLayerClick(object sender, EventArgs e) => AddLayerRow(PersonalizationLayerType.Text, GetFirstColumn());

        private void AddCodeLayerClick(object sender, EventArgs e) => AddLayerRow(PersonalizationLayerType.Code, GetFirstColumn());

        private void MoveLayerUpClick(object sender, EventArgs e) => MoveSelectedRow(-1);

        private void MoveLayerDownClick(object sender, EventArgs e) => MoveSelectedRow(1);

        private void DuplicateLayerClick(object sender, EventArgs e) => DuplicateSelectedRow();

        private void DeleteLayerClick(object sender, EventArgs e) => DeleteSelectedRow();

        private void CancelClick(object sender, EventArgs e) => Close();

        private void BasePdfTextChanged(object sender, EventArgs e) => SchedulePreviewUpdate();

        private void DataTextChanged(object sender, EventArgs e)
        {
            ClearPreviewDataCache();
            UpdateDataInfo();
            SchedulePreviewUpdate();
        }

        private void SchedulePreviewTextChanged(object sender, EventArgs e) => SchedulePreviewUpdate();

        private void PreviewTimerTick(object sender, EventArgs e)
        {
            _previewTimer.Stop();
            if (_autoPreviewCheckBox.Checked)
                TryUpdatePreview(false);
        }

        private void InteractivePreviewTimerTick(object sender, EventArgs e)
        {
            _interactivePreviewTimer.Stop();
            TryUpdatePreview(false);
        }

        private void LayersGridCellValueChanged(object sender, DataGridViewCellEventArgs e) => SchedulePreviewUpdate();

        private void LayersGridRowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) => SchedulePreviewUpdate();

        private void LayersGridCurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (_layersGrid.IsCurrentCellDirty)
                _layersGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void ZoomSelectedIndexChanged(object sender, EventArgs e) => TryUpdatePreview(false);

        private void PreviewPanelMouseEnter(object sender, EventArgs e)
        {
            _previewIsActive = true;
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

        private async void TryUpdatePreview(bool showErrors)
        {
            int version = ++_previewRequestVersion;
            if (_previewRenderRunning)
            {
                _previewRenderPending = true;
                _previewRenderPendingShowErrors |= showErrors;
                return;
            }

            await RenderPreviewAsync(version, showErrors);
        }

        private async Task RenderPreviewAsync(int version, bool showErrors)
        {
            Point previewScroll = GetPreviewScrollPosition();

            try
            {
                _previewRenderRunning = true;
                Settings = ReadSettings(false);
                PdfPersonalizationData data = GetPreviewData();
                PdfPersonalizationSettings settings = Settings;
                int rowIndex = (int)_previewRow.Value - 1;
                int dpi = GetPreviewRenderDpi();

                Bitmap rendered = await Task.Run(() =>
                {
                    return _previewComposer.Compose(settings, data, rowIndex, dpi);
                });

                if (IsDisposed || version != _previewRequestVersion)
                {
                    rendered.Dispose();
                    return;
                }

                try
                {
                    Image old = _previewBox.Image;
                    _previewBox.Image = rendered;
                    ApplyPreviewZoom();
                    RestorePreviewScrollPosition(previewScroll);
                    old?.Dispose();
                }
                catch
                {
                    rendered.Dispose();
                    throw;
                }
            }
            catch (Exception ex)
            {
                if (showErrors && !IsDisposed && version == _previewRequestVersion)
                    MessageBox.Show(this, ex.Message, "Preview", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                _previewRenderRunning = false;
                if (_previewRenderPending && !IsDisposed)
                {
                    bool pendingShowErrors = _previewRenderPendingShowErrors;
                    _previewRenderPending = false;
                    _previewRenderPendingShowErrors = false;
                    TryUpdatePreview(pendingShowErrors);
                }
            }
        }

        private void SchedulePreviewUpdate()
        {
            if (_loadingTemplate || !_autoPreviewCheckBox.Checked)
                return;

            _previewTimer.Stop();
            _previewTimer.Start();
        }

        private void ScheduleInteractivePreviewUpdate()
        {
            if (_loadingTemplate)
                return;

            _interactivePreviewTimer.Stop();
            _interactivePreviewTimer.Start();
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
            return _previewIsActive || _previewBox.Focused || _previewPanel.Focused || _previewPanel.ContainsFocus;
        }

        private void PreviewMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _layersGrid.CurrentRow == null)
                return;

            _previewIsActive = true;
            _draggingPreviewLayer = true;
            _dragMovedPreviewLayer = false;
            _dragStartPoint = e.Location;
            _dragStartXmm = GetCellDouble(_layersGrid.CurrentRow, "X", 0);
            _dragStartYmm = GetCellDouble(_layersGrid.CurrentRow, "Ymm", 0);
            _previewBox.Capture = true;
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
            if (Math.Abs(imageDx) < 1 && Math.Abs(imageDy) < 1)
                return;

            double dpiX = _previewBox.Image.HorizontalResolution > 0 ? _previewBox.Image.HorizontalResolution : 110;
            double dpiY = _previewBox.Image.VerticalResolution > 0 ? _previewBox.Image.VerticalResolution : 110;
            double dxMm = imageDx * 25.4 / dpiX;
            double dyMm = -imageDy * 25.4 / dpiY;

            _dragMovedPreviewLayer = true;
            SetSelectedLayerPosition(_dragStartXmm + dxMm, _dragStartYmm + dyMm, false);
            SchedulePreviewUpdate();
        }

        private void PreviewMouseUp(object sender, MouseEventArgs e)
        {
            if (!_draggingPreviewLayer)
                return;

            _draggingPreviewLayer = false;
            _previewBox.Capture = false;
            if (_dragMovedPreviewLayer)
                ScheduleInteractivePreviewUpdate();
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
                _previewPanel.AutoScrollMinSize = Size.Empty;
                _previewBox.Dock = DockStyle.Fill;
                _previewBox.SizeMode = PictureBoxSizeMode.Zoom;
                return;
            }

            Point scroll = GetPreviewScrollPosition();
            int zoom = GetCurrentZoomPercent();
            _previewBox.Dock = DockStyle.None;
            _previewBox.SizeMode = PictureBoxSizeMode.StretchImage;
            _previewBox.Size = new Size(
                Math.Max(1, (int)Math.Round(_previewBox.Image.Width * zoom / 100.0)),
                Math.Max(1, (int)Math.Round(_previewBox.Image.Height * zoom / 100.0)));
            _previewPanel.AutoScrollMinSize = _previewBox.Size;
            if (scroll == Point.Empty)
                _previewBox.Location = Point.Empty;
        }

        private Point GetPreviewScrollPosition()
        {
            Point position = _previewPanel.AutoScrollPosition;
            return new Point(Math.Abs(position.X), Math.Abs(position.Y));
        }

        private void RestorePreviewScrollPosition(Point position)
        {
            if (position == Point.Empty)
                return;

            _previewPanel.AutoScrollPosition = position;
            BeginInvoke((Action)(() =>
            {
                if (!_previewPanel.IsDisposed)
                    _previewPanel.AutoScrollPosition = position;
            }));
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
                ScheduleInteractivePreviewUpdate();
            else
                SchedulePreviewUpdate();
        }

        private PdfPersonalizationData GetPreviewData()
        {
            string path = _dataTextBox.Text;
            DateTime writeTimeUtc = File.Exists(path) ? File.GetLastWriteTimeUtc(path) : DateTime.MinValue;

            if (_cachedPreviewData != null &&
                string.Equals(_cachedPreviewDataPath, path, StringComparison.OrdinalIgnoreCase) &&
                _cachedPreviewDataWriteTimeUtc == writeTimeUtc)
            {
                return _cachedPreviewData;
            }

            _cachedPreviewData = PdfPersonalizationData.Load(path);
            _cachedPreviewDataPath = path;
            _cachedPreviewDataWriteTimeUtc = writeTimeUtc;
            return _cachedPreviewData;
        }

        private void ClearPreviewDataCache()
        {
            _cachedPreviewData = null;
            _cachedPreviewDataPath = null;
            _cachedPreviewDataWriteTimeUtc = DateTime.MinValue;
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

    }
}
