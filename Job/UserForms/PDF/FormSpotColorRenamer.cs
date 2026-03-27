using JobSpace.Static;
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
    public partial class FormSpotColorRenamer : Form
    {
        Interfaces.IFileSystemInfoExt _file;
        public string SelectedColor { get => cb_names.Text; }
        public string NewColorName { get => tb_newName.Text.Trim(); }


        public FormSpotColorRenamer(Interfaces.IFileSystemInfoExt file)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            _file = file;



        }

        private void btn_rename_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cb_names.Text) || string.IsNullOrWhiteSpace(tb_newName.Text))
            {
                MessageBox.Show("Будь ласка, заповніть обидва поля для перейменування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
                                              !c.Equals("Pattern", StringComparison.OrdinalIgnoreCase));

                if (colors.Any())
                {
                    cb_names.Items.AddRange(colors.ToArray());
                    cb_names.SelectedIndex = 0;
                    return; // Виходимо з конструктора, якщо знайдено Spot кольори
                }
            }

            MessageBox.Show("У цьому PDF не знайдено Spot кольорів для перейменування.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
