using JobSpace.Static.Pdf.Common;
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

namespace JobSpace.UC.PDF.Visual
{
    public partial class Uc_SelectSpiralControl : UserControl
    {
        Bitmap _spiralPreview;
        PdfPageInfo _spiralBox;

        public EventHandler OnSpiralChanged = delegate{ };

        public Uc_SelectSpiralControl()
        {
            InitializeComponent();
            
        }

        public string GetSelectedSpiralFilePath()
        {
            if (cb_spiral_files.SelectedItem is FileInfo fi)
            {
                return fi.FullName;
            }
            return null;
        }

        public PdfPageInfo GetSpiralPdfInfo()
        {
            return _spiralBox;
        }

        public Bitmap GetSpiralBitmap()
        {
            return _spiralPreview;
        }

        private void SetDefaults()
        {

            string spiralFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db", "spirals");

            if (Directory.Exists(spiralFolderPath))
            {
                var files = Directory.GetFiles(spiralFolderPath, "*.pdf");

                cb_spiral_files.DisplayMember = "Name";

                foreach (var file in files)
                {
                    FileInfo fi = new FileInfo(file);

                    cb_spiral_files.Items.Add(fi);
                }
                if (cb_spiral_files.Items.Count > 0)
                {
                    cb_spiral_files.SelectedIndex = 0;
                }
            }
            else
            {
                cb_spiral_files.Enabled = false;
            }
        }

        private void GetSpiralPreview()
        {
            if (cb_spiral_files.SelectedItem is FileInfo fi)
            {
                _spiralBox = PdfHelper.GetPageInfo(fi.FullName);
                if (_spiralPreview != null) _spiralPreview.Dispose();
                _spiralPreview = PdfHelper.RenderByTrimBox(fi, 0);
            }
        }

        private void cb_spiral_files_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSpiralPreview();
            OnSpiralChanged(this, EventArgs.Empty);
        }

        private void Uc_SelectSpiralControl_Load(object sender, EventArgs e)
        {
            SetDefaults();
        }

        ~Uc_SelectSpiralControl() { 
            if (_spiralPreview != null) _spiralPreview.Dispose();
        }
    }
}
