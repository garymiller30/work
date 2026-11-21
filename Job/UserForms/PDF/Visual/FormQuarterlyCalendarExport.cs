using JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormQuarterlyCalendarExport : Form
    {
        public CalendarExportSettings Settings { get; } = new CalendarExportSettings();
        public string SelectedSavePath { get; private set; } = string.Empty;

        private ComboBox cb_format;
        private ComboBox cb_dpi;
        private CheckBox chk_transparent;
        private TextBox txt_path;
        private Button btn_browse;
        private Button btn_export;
        private Button btn_cancel;

        public FormQuarterlyCalendarExport(CalendarConfiguration config, string? defaultDir = null)
        {
            InitializeCustomComponents();

            Settings = config.ExportSettings ?? new CalendarExportSettings();

            cb_format.SelectedItem = Settings.Format.ToUpperInvariant();
            if (cb_format.SelectedIndex < 0) cb_format.SelectedIndex = 0;

            cb_dpi.SelectedItem = $"{Settings.Dpi} DPI";
            if (cb_dpi.SelectedIndex < 0) cb_dpi.SelectedItem = "300 DPI";

            chk_transparent.Checked = Settings.TransparentBackground;
            chk_transparent.Enabled = cb_format.SelectedItem?.ToString() == "PNG";

            string dir = !string.IsNullOrWhiteSpace(defaultDir) && Directory.Exists(defaultDir)
                ? defaultDir
                : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string defaultFileName = $"Calendar_{DateTime.Now:yyyyMMdd_HHmm}.png";
            txt_path.Text = Path.Combine(dir, defaultFileName);
        }

        private void InitializeCustomComponents()
        {
            Text = "Експорт візуалізації календаря";
            Size = new Size(500, 270);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;

            var lblFormat = new Label { Text = "Формат зображення:", Location = new Point(20, 22), AutoSize = true };
            cb_format = new ComboBox
            {
                Location = new Point(170, 18),
                Width = 280,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cb_format.Items.AddRange(new object[] { "PNG", "JPEG" });
            cb_format.SelectedIndexChanged += (s, e) =>
            {
                bool isPng = cb_format.SelectedItem?.ToString() == "PNG";
                chk_transparent.Enabled = isPng;
                if (!isPng) chk_transparent.Checked = false;

                string curPath = txt_path.Text;
                if (!string.IsNullOrWhiteSpace(curPath))
                {
                    string ext = isPng ? ".png" : ".jpg";
                    txt_path.Text = Path.ChangeExtension(curPath, ext);
                }
            };

            var lblDpi = new Label { Text = "Роздільна здатність:", Location = new Point(20, 62), AutoSize = true };
            cb_dpi = new ComboBox
            {
                Location = new Point(170, 58),
                Width = 280,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cb_dpi.Items.AddRange(new object[] { "150 DPI", "300 DPI", "600 DPI" });

            chk_transparent = new CheckBox
            {
                Text = "Прозорий фон (тільки для формату PNG)",
                Location = new Point(170, 98),
                AutoSize = true
            };

            var lblPath = new Label { Text = "Місце збереження:", Location = new Point(20, 138), AutoSize = true };
            txt_path = new TextBox
            {
                Location = new Point(170, 135),
                Width = 200
            };
            btn_browse = new Button
            {
                Text = "Огляд...",
                Location = new Point(375, 133),
                Width = 75,
                Height = 26
            };
            btn_browse.Click += Btn_browse_Click;

            btn_export = new Button
            {
                Text = "Експортувати",
                Location = new Point(265, 185),
                Width = 105,
                Height = 30,
                DialogResult = DialogResult.OK
            };
            btn_export.Click += Btn_export_Click;

            btn_cancel = new Button
            {
                Text = "Скасувати",
                Location = new Point(380, 185),
                Width = 85,
                Height = 30,
                DialogResult = DialogResult.Cancel
            };

            Controls.AddRange(new Control[]
            {
                lblFormat, cb_format,
                lblDpi, cb_dpi,
                chk_transparent,
                lblPath, txt_path, btn_browse,
                btn_export, btn_cancel
            });

            AcceptButton = btn_export;
            CancelButton = btn_cancel;
        }

        private void Btn_browse_Click(object? sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Title = "Зберегти візуалізацію календаря",
                FileName = Path.GetFileName(txt_path.Text)
            };

            bool isPng = cb_format.SelectedItem?.ToString() == "PNG";
            if (isPng)
            {
                sfd.Filter = "PNG зображення (*.png)|*.png";
                sfd.DefaultExt = "png";
            }
            else
            {
                sfd.Filter = "JPEG зображення (*.jpg;*.jpeg)|*.jpg;*.jpeg";
                sfd.DefaultExt = "jpg";
            }

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                txt_path.Text = sfd.FileName;
            }
        }

        private void Btn_export_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_path.Text))
            {
                MessageBox.Show(this, "Вкажіть шлях для збереження файлу.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            Settings.Format = cb_format.SelectedItem?.ToString() ?? "PNG";
            string dpiStr = cb_dpi.SelectedItem?.ToString() ?? "300";
            Settings.Dpi = dpiStr.Contains("150") ? 150 : (dpiStr.Contains("600") ? 600 : 300);
            Settings.TransparentBackground = chk_transparent.Checked;
            Settings.LastExportFilePath = txt_path.Text;
            SelectedSavePath = txt_path.Text;
        }
    }
}
