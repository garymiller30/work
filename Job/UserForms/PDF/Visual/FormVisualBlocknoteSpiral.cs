using JobSpace.Static.Pdf.Visual.BlocknoteSpiral;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualBlocknoteSpiral : Form
    {
        public SpiralSettings SpiralSettings { get;set; } = new SpiralSettings();
        public FormVisualBlocknoteSpiral()
        {
            InitializeComponent();
            SetDefaults();
            DialogResult = DialogResult.Cancel;
        }

        private void SetDefaults()
        {
            // використовуємо DescriptionAttribute для відображення назв елементів enum у комбобоксі

            var placeValues = Enum.GetValues(typeof(SpiralPlaceEnum)).Cast<SpiralPlaceEnum>();
            foreach (var val in placeValues)
            {
                var memInfo = typeof(SpiralPlaceEnum).GetMember(val.ToString());
                var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                string description = descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : val.ToString();
                cb_place.Items.Add(description);
            }
            cb_place.SelectedIndex = 0;

            string spiralFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db", "spirals");

            var files = Directory.GetFiles(spiralFolderPath, "*.pdf");

            cb_files.DisplayMember = "Name";

            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);

                cb_files.Items.Add(fi);
            }
            if (cb_files.Items.Count > 0)
            {
                cb_files.SelectedIndex = 0;
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            SpiralSettings.SpiralPlace = (SpiralPlaceEnum)cb_place.SelectedIndex;
            SpiralSettings.SpiralFile = (cb_files.SelectedItem as FileInfo).FullName;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
