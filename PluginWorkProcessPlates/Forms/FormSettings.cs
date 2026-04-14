using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginWorkProcessPlates.Forms
{
    public partial class FormSettings : Form
    {
        private PlateSettings _settings;

        public FormSettings()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public FormSettings(PlateSettings settings): this()
        {

            _settings = settings;
            Bind();
        }

        private void Bind()
        {
            numericUpDownPriceForPlate.Value = _settings.PriceForPlate;
            listBoxFormats.Items.AddRange(_settings.Formats.ToArray());
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Unbind();
            Close();
        }

        private void Unbind()
        {
            _settings.PriceForPlate = numericUpDownPriceForPlate.Value;

            _settings.Formats = listBoxFormats.Items.Cast<Format>().ToList();

        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var delList = listBoxFormats.SelectedItems.Cast<Format>().ToList();

            foreach (var item in delList)
            {
                listBoxFormats.Items.Remove(item);
            }
        }
    }
}
