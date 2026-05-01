using JobSpace.Static;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public partial class FormSpotColorRenamer : KryptonForm
    {
        Interfaces.IFileSystemInfoExt _file;
        readonly Dictionary<string, string> _savedRenames;

        public Dictionary<string,string> ColorRenames { get; private set; } = new Dictionary<string, string>();

        public FormSpotColorRenamer(Interfaces.IFileSystemInfoExt file)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            _file = file;
            _savedRenames = LoadSavedRenames();
        }

        private void btn_rename_Click(object sender, EventArgs e)
        {
            List<ColorRename> renames = objectListView1.Objects.Cast<ColorRename>().Where(x => !string.IsNullOrWhiteSpace(x.Target)).ToList();

            if (renames.Count == 0)
            {
                MessageBox.Show("Будь ласка, введіть нові назви для принаймні одного кольору.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var rename in renames)
            {
                ColorRenames.Add(rename.Source, rename.Target);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormSpotColorRenamer_Shown(object sender, EventArgs e)
        {
            PdfUtils.GetColorspaces(_file);
            if (_file.UsedColors != null && _file.UsedColors.Count > 0)
            {
                // Потрібно видалити CMYK, Indexed та інші загальні кольори, залишивши лише Spot кольори
                var colors = _file.UsedColors.Where(c => !c.Equals("CMYK", StringComparison.OrdinalIgnoreCase) &&
                                              !c.Equals("Indexed", StringComparison.OrdinalIgnoreCase) &&
                                              !c.Equals("RGB", StringComparison.OrdinalIgnoreCase) &&
                                              !c.Equals("Lab", StringComparison.OrdinalIgnoreCase) &&
                                              !c.Equals("ICCBased", StringComparison.OrdinalIgnoreCase) &&
                                              !c.Equals("All", StringComparison.OrdinalIgnoreCase) &&
                                              !c.Equals("Pattern", StringComparison.OrdinalIgnoreCase) &&
                                              !c.Equals("None", StringComparison.OrdinalIgnoreCase));

                if (colors.Any())
                {
                    objectListView1.AddObjects(colors.Select(x=> new ColorRename() { Source = x, Target = GetSavedTarget(x)}).ToArray());
                    return; // Виходимо з конструктора, якщо знайдено Spot кольори
                }
            }

            MessageBox.Show("У цьому PDF не знайдено Spot кольорів для перейменування.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveCurrentRenames();
            base.OnFormClosing(e);
        }

        private string GetSavedTarget(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return null;
            }

            return _savedRenames.TryGetValue(source, out var target) ? target : null;
        }

        private void SaveCurrentRenames()
        {
            var renames = objectListView1.Objects?.Cast<ColorRename>()
                .Where(x => !string.IsNullOrWhiteSpace(x.Source))
                .ToList();

            if (renames == null || renames.Count == 0)
            {
                return;
            }

            foreach (var rename in renames)
            {
                if (string.IsNullOrWhiteSpace(rename.Target))
                {
                    _savedRenames.Remove(rename.Source);
                }
                else
                {
                    _savedRenames[rename.Source] = rename.Target;
                }
            }

            SaveSavedRenames(_savedRenames);
        }

        private static Dictionary<string, string> LoadSavedRenames()
        {
            try
            {
                var file = GetSavedRenamesFile();
                if (!File.Exists(file))
                {
                    return new Dictionary<string, string>();
                }

                var json = File.ReadAllText(file);
                return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        private static void SaveSavedRenames(Dictionary<string, string> renames)
        {
            try
            {
                var file = GetSavedRenamesFile();
                Directory.CreateDirectory(Path.GetDirectoryName(file));
                var json = JsonSerializer.Serialize(renames);
                File.WriteAllText(file, json);
            }
            catch
            {
            }
        }

        private static string GetSavedRenamesFile()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ActiveWorks",
                "spot_color_renames.json");
        }


        class ColorRename
        {
            public string Source { get; set; }
            public string Target { get; set; }
        }
    }
}
