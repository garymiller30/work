using JobSpace.Static;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public partial class FormSpotColorRenamer : KryptonForm
    {
        Interfaces.IFileSystemInfoExt _file;

        public Dictionary<string,string> ColorRenames { get; private set; } = new Dictionary<string, string>();

        public FormSpotColorRenamer(Interfaces.IFileSystemInfoExt file)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            _file = file;
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
                    objectListView1.AddObjects(colors.Select(x=> new ColorRename() { Source = x}).ToArray());
                    return; // Виходимо з конструктора, якщо знайдено Spot кольори
                }
            }

            MessageBox.Show("У цьому PDF не знайдено Spot кольорів для перейменування.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }


        class ColorRename
        {
            public string Source { get; set; }
            public string Target { get; set; }
        }
    }
}
