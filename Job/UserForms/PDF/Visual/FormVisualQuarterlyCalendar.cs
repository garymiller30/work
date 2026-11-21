using Interfaces;
using Interfaces.Licensing;
using JobSpace.Licensing;
using JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models;
using JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    /// <summary>
    /// Конструктор візуалізації квартального календаря (п. 1 ТЗ).
    /// Дозволяє самостійно збирати конструкцію календаря з окремих PDF-компонентів,
    /// налаштовувати взаємне розташування, параметри кріплень та експортувати зображення.
    /// </summary>
    public partial class FormVisualQuarterlyCalendar : Form
    {
        private CalendarConfiguration _config;
        private string? _currentConfigFilePath;
        private string? _workingDirectory;

        // Поточний вибраний компонент для редагування
        private CalendarComponent? _selectedComponent;

        // Параметри інтерактивного перегляду (Viewport)
        private float _zoomFactor = 1.0f;
        private PointF _panOffsetPx = new PointF(40, 40);
        private bool _isPanning = false;
        private Point _lastMousePos;

        public FormVisualQuarterlyCalendar()
        {
            InitializeComponent();
            EnableDoubleBuffering(panel_canvas_viewport);

            _config = new CalendarConfiguration(3); // 3 модулі за замовчуванням
            _workingDirectory = Directory.GetCurrentDirectory();

            panel_canvas_viewport.Paint += Panel_canvas_viewport_Paint;
            panel_canvas_viewport.MouseDown += Panel_canvas_viewport_MouseDown;
            panel_canvas_viewport.MouseMove += Panel_canvas_viewport_MouseMove;
            panel_canvas_viewport.MouseUp += Panel_canvas_viewport_MouseUp;
            panel_canvas_viewport.MouseWheel += Panel_canvas_viewport_MouseWheel;
            panel_canvas_viewport.Resize += (s, e) => panel_canvas_viewport.Invalidate();

            BuildStructureTree();
            SelectComponent(_config.Header);
            UpdateDimensionsStatus();
            CenterView();
        }

        public FormVisualQuarterlyCalendar(IFileSystemInfoExt fsi) : this()
        {
            if (fsi?.FileInfo != null)
            {
                _workingDirectory = Path.GetDirectoryName(fsi.FileInfo.FullName);
                // Автоматично призначаємо перший файл як шапку календаря
                AssignPdfToComponent(_config.Header, fsi.FileInfo.FullName, 0);
            }
        }

        public FormVisualQuarterlyCalendar(List<IFileSystemInfoExt> fsis) : this()
        {
            if (fsis != null && fsis.Count > 0)
            {
                if (fsis[0].FileInfo != null)
                {
                    _workingDirectory = Path.GetDirectoryName(fsis[0].FileInfo.FullName);
                }

                // Якщо передано кілька файлів — заповнюємо шапку, потім підкладки та блоки
                var components = _config.GetAllComponents().ToList();
                for (int i = 0; i < Math.Min(fsis.Count, components.Count); i++)
                {
                    if (fsis[i].FileInfo != null)
                    {
                        AssignPdfToComponent(components[i], fsis[i].FileInfo.FullName, 0);
                    }
                }
            }
        }

        private static void EnableDoubleBuffering(Control c)
        {
            var prop = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            prop?.SetValue(c, true, null);
        }

        #region Tree & Structure Management

        /// <summary>
        /// Побудова ієрархічного дерева конструкції календаря (п. 9.1 ТЗ).
        /// </summary>
        private void BuildStructureTree()
        {
            tv_structure.BeginUpdate();
            tv_structure.Nodes.Clear();

            // 1. Шапка календаря
            var headerNode = new TreeNode(_config.Header.Name)
            {
                Tag = _config.Header
            };
            tv_structure.Nodes.Add(headerNode);

            // 2. Модулі (1..4)
            for (int i = 0; i < _config.Modules.Count; i++)
            {
                var mod = _config.Modules[i];
                var modNode = new TreeNode($"Модуль {mod.ModuleIndex}")
                {
                    Tag = mod
                };

                var backingNode = new TreeNode(mod.Backing.Name) { Tag = mod.Backing };
                var blockNode = new TreeNode(mod.CalendarBlock.Name) { Tag = mod.CalendarBlock };

                modNode.Nodes.Add(backingNode);
                modNode.Nodes.Add(blockNode);
                modNode.Expand();

                tv_structure.Nodes.Add(modNode);
            }

            headerNode.Expand();
            tv_structure.EndUpdate();

            btn_add_module.Enabled = _config.CanAddModule;
            btn_remove_module.Enabled = _config.CanRemoveModule;
            tsmi_add_module.Enabled = _config.CanAddModule;
            tsmi_remove_module.Enabled = _config.CanRemoveModule;
        }

        private void tv_structure_AfterSelect(object? sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is CalendarComponent comp)
            {
                SelectComponent(comp);
            }
            else if (e.Node?.Tag is CalendarModule mod)
            {
                // При виборі кореня модуля показуємо його підкладку за замовчуванням
                SelectComponent(mod.Backing);
            }
        }

        private void SelectComponent(CalendarComponent? comp)
        {
            _selectedComponent = comp;
            tssl_selected.Text = $"Вибрано: {comp?.Name ?? "(нічого)"}";
            PopulatePropertiesPanel();
            panel_canvas_viewport.Invalidate();
        }

        private void btn_add_module_Click(object? sender, EventArgs e)
        {
            if (!_config.CanAddModule)
            {
                MessageBox.Show(this, "Календар може містити максимум 4 конструктивні модулі.", "Обмеження", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var newMod = _config.AddModule();
            if (newMod != null)
            {
                CalendarLayoutEngine.AutoLayoutTopToBottom(_config);
                BuildStructureTree();
                SelectComponent(newMod.Backing);
                UpdateDimensionsStatus();
                panel_canvas_viewport.Invalidate();
            }
        }

        private void btn_remove_module_Click(object? sender, EventArgs e)
        {
            if (!_config.CanRemoveModule)
            {
                MessageBox.Show(this, "Календар повинен містити щонайменше 1 конструктивний модуль.", "Обмеження", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_config.RemoveLastModule())
            {
                CalendarLayoutEngine.AutoLayoutTopToBottom(_config);
                BuildStructureTree();
                SelectComponent(_config.Header);
                UpdateDimensionsStatus();
                panel_canvas_viewport.Invalidate();
            }
        }

        #endregion

        #region Properties Panel

        /// <summary>
        /// Динамічне заповнення панелі властивостей відповідно до вибраного компонента (п. 9.2 ТЗ).
        /// </summary>
        private void PopulatePropertiesPanel()
        {
            panel_props_container.SuspendLayout();
            panel_props_container.Controls.Clear();

            if (_selectedComponent == null)
            {
                var lblEmpty = new Label
                {
                    Text = "Виберіть елемент у дереві конструкції або на полотні.",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Gray
                };
                panel_props_container.Controls.Add(lblEmpty);
                panel_props_container.ResumeLayout();
                return;
            }

            var comp = _selectedComponent;
            int y = 10;

            // Назва
            var lblTitle = new Label
            {
                Text = comp.Name,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(10, y),
                AutoSize = true
            };
            panel_props_container.Controls.Add(lblTitle);
            y += 26;

            // Файл PDF
            var lblFile = new Label { Text = "PDF-файл:", Location = new Point(10, y), AutoSize = true };
            y += 18;
            var txtFile = new TextBox
            {
                Text = comp.FilePath ?? "",
                Location = new Point(10, y),
                Width = 240,
                ReadOnly = true
            };
            var btnBrowse = new Button
            {
                Text = "...",
                Location = new Point(255, y - 1),
                Width = 40,
                Height = 24
            };
            btnBrowse.Click += (s, e) => ChoosePdfForComponent(comp);
            panel_props_container.Controls.AddRange(new Control[] { lblFile, txtFile, btnBrowse });
            y += 30;

            // Сторінка PDF
            var lblPage = new Label { Text = "Сторінка PDF:", Location = new Point(10, y), AutoSize = true };
            var nudPage = new NumericUpDown
            {
                Location = new Point(110, y - 2),
                Width = 70,
                Minimum = 1,
                Maximum = 9999,
                Value = Math.Max(1, comp.PageIndex + 1)
            };
            nudPage.ValueChanged += (s, e) =>
            {
                int newIdx = (int)nudPage.Value - 1;
                if (newIdx != comp.PageIndex && comp.HasFile)
                {
                    AssignPdfToComponent(comp, comp.FilePath!, newIdx);
                }
            };
            panel_props_container.Controls.AddRange(new Control[] { lblPage, nudPage });
            y += 30;

            // Розміри TrimBox
            string trimStatus = comp.WidthMm > 0
                ? $"{comp.WidthMm:0.0} × {comp.HeightMm:0.0} мм" + (comp.HasExplicitTrimBox ? " (TrimBox OK)" : " (MediaBox fallback!)")
                : "(не задано)";
            var lblDim = new Label
            {
                Text = $"Фізичні розміри:\n{trimStatus}",
                Location = new Point(10, y),
                AutoSize = true,
                ForeColor = comp.HasExplicitTrimBox ? Color.DarkGreen : (comp.WidthMm > 0 ? Color.DarkOrange : Color.Gray)
            };
            panel_props_container.Controls.Add(lblDim);
            y += 38;

            // Позиціонування: X та Y
            var lblCoords = new Label
            {
                Text = comp is CalendarHeader ? "Позиція (Верх календаря):" : (comp.Name.Contains("блок", StringComparison.OrdinalIgnoreCase) ? "Позиція (Top Center підкладки):" : "Позиція (Вертикальний стек):"),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(10, y),
                AutoSize = true
            };
            y += 22;

            var lblX = new Label { Text = "X:", Location = new Point(10, y + 2), AutoSize = true };
            var nudX = new NumericUpDown
            {
                Location = new Point(35, y),
                Width = 85,
                Minimum = -1000,
                Maximum = 2000,
                DecimalPlaces = 1,
                Increment = 1m,
                Value = (decimal)comp.OffsetXMm,
                ReadOnly = true,
                Enabled = false
            };

            var lblY = new Label { Text = "Y:", Location = new Point(135, y + 2), AutoSize = true };
            var nudY = new NumericUpDown
            {
                Location = new Point(155, y),
                Width = 85,
                Minimum = -1000,
                Maximum = 5000,
                DecimalPlaces = 1,
                Increment = 1m,
                Value = (decimal)comp.OffsetYMm,
                ReadOnly = true,
                Enabled = false
            };

            panel_props_container.Controls.AddRange(new Control[] { lblCoords, lblX, nudX, lblY, nudY });
            y += 34;

            // Специфічні властивості шапки: Кріплення (Люверс / Пружина)
            if (comp is CalendarHeader header)
            {
                y += 10;
                var gbMount = new GroupBox
                {
                    Text = "Кріплення шапки",
                    Location = new Point(10, y),
                    Width = 320,
                    Height = 160
                };

                var rbSpring = new RadioButton
                {
                    Text = "Пружина зверху та знизу",
                    Location = new Point(15, 22),
                    AutoSize = true,
                    Checked = header.MountType == HeaderMountType.SpringTopAndBottom
                };

                var rbEyelet = new RadioButton
                {
                    Text = "Люверс зверху та пружина знизу",
                    Location = new Point(15, 48),
                    AutoSize = true,
                    Checked = header.MountType == HeaderMountType.EyeletTopSpringBottom
                };

                var pnlEyeletProps = new Panel
                {
                    Location = new Point(15, 78),
                    Width = 290,
                    Height = 90,
                    Visible = rbEyelet.Checked
                };

                var lblEyeletD = new Label { Text = "Діаметр люверса:", Location = new Point(0, 5), AutoSize = true };
                var nudEyeletD = new NumericUpDown
                {
                    Location = new Point(160, 2),
                    Width = 65,
                    Minimum = 3.0m,
                    Maximum = 5.0m,
                    DecimalPlaces = 1,
                    Increment = 0.5m,
                    Value = (decimal)header.Eyelet.DiameterMm
                };
                nudEyeletD.ValueChanged += (s, e) =>
                {
                    header.Eyelet.DiameterMm = (double)nudEyeletD.Value;
                    panel_canvas_viewport.Invalidate();
                };

                var lblEyeletMargin = new Label
                {
                    Text = "Відступ від краю шапки:",
                    Location = new Point(0, 35),
                    AutoSize = true
                };
                var nudEyeletMargin = new NumericUpDown
                {
                    Location = new Point(160, 32),
                    Width = 65,
                    Minimum = 1.0m,
                    Maximum = 50.0m,
                    DecimalPlaces = 1,
                    Increment = 0.5m,
                    Value = (decimal)header.Eyelet.TopMarginMm
                };
                nudEyeletMargin.ValueChanged += (s, e) =>
                {
                    header.Eyelet.TopMarginMm = (double)nudEyeletMargin.Value;
                    panel_canvas_viewport.Invalidate();
                };

                var lblEyeletNote = new Label
                {
                    Text = "до верхнього краю люверса (мм)",
                    Location = new Point(0, 60),
                    AutoSize = true,
                    ForeColor = Color.DimGray,
                    Font = new Font("Segoe UI", 8f)
                };

                pnlEyeletProps.Controls.AddRange(new Control[] { lblEyeletD, nudEyeletD, lblEyeletMargin, nudEyeletMargin, lblEyeletNote });

                rbSpring.CheckedChanged += (s, e) =>
                {
                    if (rbSpring.Checked)
                    {
                        header.MountType = HeaderMountType.SpringTopAndBottom;
                        pnlEyeletProps.Visible = false;
                        panel_canvas_viewport.Invalidate();
                    }
                };

                rbEyelet.CheckedChanged += (s, e) =>
                {
                    if (rbEyelet.Checked)
                    {
                        header.MountType = HeaderMountType.EyeletTopSpringBottom;
                        pnlEyeletProps.Visible = true;
                        panel_canvas_viewport.Invalidate();
                    }
                };

                gbMount.Controls.AddRange(new Control[] { rbSpring, rbEyelet, pnlEyeletProps });
                panel_props_container.Controls.Add(gbMount);
                y += 170;
            }

            panel_props_container.ResumeLayout();
        }

        private void ChoosePdfForComponent(CalendarComponent component)
        {
            using var ofd = new OpenFileDialog
            {
                Title = $"Виберіть PDF для «{component.Name}»",
                Filter = "PDF файли (*.pdf)|*.pdf|Всі файли (*.*)|*.*",
                InitialDirectory = _workingDirectory ?? Directory.GetCurrentDirectory()
            };

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                _workingDirectory = Path.GetDirectoryName(ofd.FileName);
                AssignPdfToComponent(component, ofd.FileName, 0);
            }
        }

        private void AssignPdfToComponent(CalendarComponent component, string filePath, int pageIndex)
        {
            component.FilePath = filePath;
            component.PageIndex = pageIndex;

            // Перевіряємо сторінку та TrimBox
            var trimInfo = CalendarPdfService.GetPageTrimBoxInfo(filePath, pageIndex);
            if (!trimInfo.Success)
            {
                MessageBox.Show(this, $"Не вдалося прочитати сторінку PDF: {trimInfo.ErrorMessage}", "Помилка PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            component.WidthMm = trimInfo.TrimWidthMm;
            component.HeightMm = trimInfo.TrimHeightMm;
            component.HasExplicitTrimBox = trimInfo.HasTrimBox;

            // Згідно з п. 6.1 ТЗ: якщо явний TrimBox відсутній — сповіщаємо користувача!
            if (!trimInfo.HasTrimBox)
            {
                string msg = $"У файлі «{Path.GetFileName(filePath)}» (стор. {pageIndex + 1}) відсутній явний TrimBox!\n\n" +
                             $"Виявлено розміри MediaBox: {trimInfo.MediaWidthMm:0.0} × {trimInfo.MediaHeightMm:0.0} мм.\n" +
                             "Використати розміри MediaBox для візуалізації?";
                var dr = MessageBox.Show(this, msg, "Відсутній TrimBox", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    component.FilePath = null;
                    component.WidthMm = 0;
                    component.HeightMm = 0;
                    component.ClearPreview();
                    PopulatePropertiesPanel();
                    panel_canvas_viewport.Invalidate();
                    return;
                }
            }

            // Рендеримо прев'ю
            CalendarPdfService.RenderComponentPreview(component, 150);

            // Застосовуємо суворе вертикальне компонування: блоки TOP CENTER, модулі зверху вниз
            CalendarLayoutEngine.ApplyStrictVerticalLayout(_config);
            UpdateDimensionsStatus();
            PopulatePropertiesPanel();
            panel_canvas_viewport.Invalidate();
        }

        #endregion

        #region Interactive Viewport & Canvas (Mouse, Zoom, Pan)

        private void Panel_canvas_viewport_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(panel_canvas_viewport.BackColor);

            // Зберігаємо вихідний стан графіки
            var state = g.Save();

            // Встановлюємо екранну трансформацію (Pan + Zoom)
            g.TranslateTransform(_panOffsetPx.X, _panOffsetPx.Y);

            // Переводимо міліметри полотна у екранні пікселі за заданого масштабу (Dpi: 96)
            float mmToPxScale = (panel_canvas_viewport.DeviceDpi / 25.4f) * _zoomFactor;
            g.ScaleTransform(mmToPxScale, mmToPxScale);

            // Відмальовуємо всю сцену календаря
            CalendarRenderService.RenderCalendarScene(g, _config, _selectedComponent, isScreenPreview: true);

            g.Restore(state);
        }

        private void Panel_canvas_viewport_MouseDown(object? sender, MouseEventArgs e)
        {
            panel_canvas_viewport.Focus();
            _lastMousePos = e.Location;

            var mouseMm = CalendarLayoutEngine.ScreenToMm(e.Location, _zoomFactor, _panOffsetPx, panel_canvas_viewport.DeviceDpi);

            if (e.Button == MouseButtons.Left)
            {
                // Перевіряємо клік по компоненту для його виділення у властивостях
                var hit = CalendarLayoutEngine.HitTest(_config, mouseMm);
                if (hit != null)
                {
                    SelectComponent(hit);
                }

                // Вмикаємо панорамування при перетягуванні мишею
                _isPanning = true;
                panel_canvas_viewport.Cursor = Cursors.Hand;
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Панорамування правою кнопкою миші
                _isPanning = true;
                panel_canvas_viewport.Cursor = Cursors.Hand;
            }
        }

        private void Panel_canvas_viewport_MouseMove(object? sender, MouseEventArgs e)
        {
            var mouseMm = CalendarLayoutEngine.ScreenToMm(e.Location, _zoomFactor, _panOffsetPx, panel_canvas_viewport.DeviceDpi);
            tssl_mouse_coords.Text = $"X: {mouseMm.X:0.0} мм, Y: {mouseMm.Y:0.0} мм";

            if (_isPanning)
            {
                // Панорамування полотна
                int dx = e.Location.X - _lastMousePos.X;
                int dy = e.Location.Y - _lastMousePos.Y;
                _panOffsetPx = new PointF(_panOffsetPx.X + dx, _panOffsetPx.Y + dy);
                _lastMousePos = e.Location;
                panel_canvas_viewport.Invalidate();
            }
            else
            {
                // Підсвітка курсора при наведенні на компонент
                var hit = CalendarLayoutEngine.HitTest(_config, mouseMm);
                panel_canvas_viewport.Cursor = hit != null ? Cursors.Hand : Cursors.Default;
            }
        }

        private void Panel_canvas_viewport_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                _isPanning = false;
                panel_canvas_viewport.Cursor = Cursors.Default;
            }
        }

        private void Panel_canvas_viewport_MouseWheel(object? sender, MouseEventArgs e)
        {
            float oldZoom = _zoomFactor;

            if (e.Delta > 0)
                _zoomFactor *= 1.15f;
            else
                _zoomFactor /= 1.15f;

            _zoomFactor = Math.Clamp(_zoomFactor, 0.05f, 5.0f);

            // Зум відносно поточної позиції миші
            float zoomRatio = _zoomFactor / oldZoom;
            _panOffsetPx = new PointF(
                e.X - (e.X - _panOffsetPx.X) * zoomRatio,
                e.Y - (e.Y - _panOffsetPx.Y) * zoomRatio
            );

            UpdateZoomStatus();
            panel_canvas_viewport.Invalidate();
        }

        private void btn_fit_Click(object? sender, EventArgs e)
        {
            CenterView();
        }

        private void btn_zoom_100_Click(object? sender, EventArgs e)
        {
            _zoomFactor = 1.0f;
            UpdateZoomStatus();
            panel_canvas_viewport.Invalidate();
        }

        private void btn_zoom_in_Click(object? sender, EventArgs e)
        {
            _zoomFactor = Math.Min(5.0f, _zoomFactor * 1.25f);
            UpdateZoomStatus();
            panel_canvas_viewport.Invalidate();
        }

        private void btn_zoom_out_Click(object? sender, EventArgs e)
        {
            _zoomFactor = Math.Max(0.05f, _zoomFactor / 1.25f);
            UpdateZoomStatus();
            panel_canvas_viewport.Invalidate();
        }

        private void CenterView()
        {
            _config.RecalculateBounds();

            float totalWidthMm = (float)(_config.TotalWidthMm > 0 ? _config.TotalWidthMm : 297.0);
            float totalHeightMm = (float)(_config.TotalHeightMm > 0 ? _config.TotalHeightMm : 840.0);

            float dpi = panel_canvas_viewport.DeviceDpi;
            float totalWidthPx = totalWidthMm * dpi / 25.4f;
            float totalHeightPx = totalHeightMm * dpi / 25.4f;

            int availW = Math.Max(100, panel_canvas_viewport.ClientSize.Width - 60);
            int availH = Math.Max(100, panel_canvas_viewport.ClientSize.Height - 60);

            float scaleX = availW / totalWidthPx;
            float scaleY = availH / totalHeightPx;

            _zoomFactor = Math.Clamp(Math.Min(scaleX, scaleY), 0.05f, 3.0f);

            float scenePxW = totalWidthPx * _zoomFactor;
            float scenePxH = totalHeightPx * _zoomFactor;

            _panOffsetPx = new PointF(
                Math.Max(20, (panel_canvas_viewport.ClientSize.Width - scenePxW) / 2f),
                Math.Max(20, (panel_canvas_viewport.ClientSize.Height - scenePxH) / 2f)
            );

            UpdateZoomStatus();
            panel_canvas_viewport.Invalidate();
        }

        private void UpdateZoomStatus()
        {
            tssl_zoom.Text = $"Масштаб: {(int)(_zoomFactor * 100)}%";
        }

        private void UpdateDimensionsStatus()
        {
            tssl_dimensions.Text = $"Габарити виробу: {_config.TotalWidthMm:0.0} × {_config.TotalHeightMm:0.0} мм";
        }

        #endregion

        #region Actions: Layout, Save, Open, Export

        private void tsmi_autolayout_Click(object? sender, EventArgs e)
        {
            CalendarLayoutEngine.AutoLayoutTopToBottom(_config);
            UpdateDimensionsStatus();
            PopulatePropertiesPanel();
            CenterView();
        }

        private void tsmi_new_Click(object? sender, EventArgs e)
        {
            var dr = MessageBox.Show(this, "Створити новий квартальний календар? Незбережені зміни буде втрачено.", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                _config = new CalendarConfiguration(3);
                _currentConfigFilePath = null;
                BuildStructureTree();
                SelectComponent(_config.Header);
                CenterView();
            }
        }

        private void tsmi_save_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_currentConfigFilePath))
            {
                tsmi_save_as_Click(sender, e);
            }
            else
            {
                SaveConfigToFile(_currentConfigFilePath);
            }
        }

        private void tsmi_save_as_Click(object? sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Title = "Зберегти конфігурацію квартального календаря",
                Filter = "Конфігурація календаря (*.json)|*.json",
                DefaultExt = "json",
                InitialDirectory = _workingDirectory ?? Directory.GetCurrentDirectory()
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                SaveConfigToFile(sfd.FileName);
            }
        }

        private void SaveConfigToFile(string path)
        {
            try
            {
                _config.RecalculateBounds();
                _config.SaveToFile(path);
                _currentConfigFilePath = path;
                MessageBox.Show(this, "Конфігурацію календаря успішно збережено!", "Збереження", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Помилка збереження конфігурації: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsmi_open_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Відкрити конфігурацію квартального календаря",
                Filter = "Конфігурація календаря (*.json)|*.json|Всі файли (*.*)|*.*",
                InitialDirectory = _workingDirectory ?? Directory.GetCurrentDirectory()
            };

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var loadedConfig = CalendarConfiguration.LoadFromFile(ofd.FileName);
                    if (loadedConfig == null)
                    {
                        MessageBox.Show(this, "Не вдалося завантажити конфігурацію з вибраного файлу.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _config = loadedConfig;
                    _currentConfigFilePath = ofd.FileName;
                    _workingDirectory = Path.GetDirectoryName(ofd.FileName);

                    // Відновлюємо розміри та прев'ю PDF-компонентів із перевіркою існування файлів (п. 10 ТЗ)
                    var missingFiles = new List<string>();
                    foreach (var comp in _config.GetAllComponents())
                    {
                        if (!string.IsNullOrWhiteSpace(comp.FilePath))
                        {
                            if (File.Exists(comp.FilePath))
                            {
                                CalendarPdfService.UpdateComponentDimensions(comp);
                                CalendarPdfService.RenderComponentPreview(comp, 150);
                            }
                            else
                            {
                                missingFiles.Add($"{comp.Name}: {comp.FilePath}");
                            }
                        }
                    }

                    if (missingFiles.Count > 0)
                    {
                        string msg = "Деякі PDF-файли за збереженими шляхами не знайдено:\n\n" +
                                     string.Join("\n", missingFiles.Take(5)) +
                                     (missingFiles.Count > 5 ? $"\n...та ще {missingFiles.Count - 5} файлів." : "") +
                                     "\n\nВи зможете повторно вибрати їх у панелі властивостей.";
                        MessageBox.Show(this, msg, "Відсутні файли PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Перераховуємо суворе вертикальне компонування для завантаженої сцени
                    CalendarLayoutEngine.ApplyStrictVerticalLayout(_config);
                    BuildStructureTree();
                    SelectComponent(_config.Header);
                    UpdateDimensionsStatus();
                    CenterView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Помилка відкриття файлу конфігурації: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [RequiresFeature(LicenseFeature.ExportPdf)]
        private void btn_export_Click(object? sender, EventArgs e)
        {
            if (!LicenseUiGate.RequireFor(this, GetType(), nameof(btn_export_Click)))
                return;

            using var exportDlg = new FormQuarterlyCalendarExport(_config, _workingDirectory);
            if (exportDlg.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    CalendarExportService.SaveExportImage(
                        _config,
                        exportDlg.SelectedSavePath,
                        exportDlg.Settings.Format,
                        exportDlg.Settings.Dpi,
                        exportDlg.Settings.TransparentBackground
                    );

                    MessageBox.Show(this, $"Готову візуалізацію календаря успішно експортовано:\n{exportDlg.SelectedSavePath}", "Експорт завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Помилка під час експорту: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void tsmi_exit_Click(object? sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
