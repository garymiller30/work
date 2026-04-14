using System;
using System.Windows.Forms;

namespace SpeedDevelopingPlate
{
    public partial class FormEditSpeed : Form
    {
        private readonly PlateFormat _plateFormat;

        public FormEditSpeed(object plateFormat)
        {
            InitializeComponent();

            _plateFormat = plateFormat as PlateFormat;

            var ps = _plateFormat?.GetLastPlateSpeed();
            if (ps != null)
            {
                numericUpDown1.Value = ps.Temperature;
                numericUpDown2.Value = ps.Speed;
                textBoxManufacture.Text = _plateFormat.Manufacturer;
                textBoxPlateType.Text = _plateFormat.PlateType;
                textBoxPlateFormat.Text = _plateFormat.Format;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _plateFormat.Manufacturer = textBoxManufacture.Text;
            _plateFormat.PlateType = textBoxPlateType.Text;
            _plateFormat.Format = textBoxPlateFormat.Text;

            PlateFormatsManager.ChangePlateSpeed(_plateFormat, numericUpDown1.Value, (int)numericUpDown2.Value);

            Close();

        }
    }
}
