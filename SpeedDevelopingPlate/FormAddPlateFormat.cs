using System;
using System.Windows.Forms;

namespace SpeedDevelopingPlate
{
    public partial class FormAddPlateFormat : Form
    {
        public FormAddPlateFormat()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlateFormatsManager.Add(textBoxManufacturer.Text,textBoxFormat.Text);
        }
    }
}
