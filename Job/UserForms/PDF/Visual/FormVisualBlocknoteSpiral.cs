using Interfaces;
using Interfaces.Licensing;
using JobSpace.Licensing;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.BlocknoteSpiral;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualBlocknoteSpiral : Form
    {
        public SpiralSettings SpiralSettings { get; set; } = new SpiralSettings();
        FileInfo _fsi;

        List<IScreenPrimitive> _primitives = new List<IScreenPrimitive>();

        public FormVisualBlocknoteSpiral()
        {
            InitializeComponent();
            SetDefaults();
            cb_files.DisplayMember = "Name";

            uc_PreviewBrowserFile1.SetFunc_GetScreenPrimitives(GetPrimitives);
            uc_SelectSpiralControl1.OnSpiralChanged += OnSpiralChanged;

            uc_VisualRectangleControl1.OnRectEnabledChanged += (s, ee) => Redraw();
            uc_VisualRectangleControl1.OnRectPositionChanged += (s, ee) => Redraw();
            uc_VisualRectangleControl1.OnRectSizeChanged += (s, ee) => Redraw();

            uc_SelectFileAndPageControl1.FileSelectionChanged += (s, e) => Redraw();
            DialogResult = DialogResult.Cancel;
        }

        private List<IScreenPrimitive> GetPrimitives(int pageNo)
        {
            _primitives.Clear();

            DrawSpirales(pageNo);

            return _primitives;

        }

        void Redraw() => uc_PreviewBrowserFile1.Redraw();

        private void OnSpiralChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        public FormVisualBlocknoteSpiral(IFileSystemInfoExt fsi) : this()
        {
            _fsi = new FileInfo(fsi.FileInfo.FullName);
            cb_files.Items.Add(_fsi);
            cb_files.SelectedIndex = 0;

            AddBlockFiles(fsi);

            
        }

        private void AddBlockFiles(IFileSystemInfoExt fsi)
        {
            string parentDir = Path.GetDirectoryName(fsi.FileInfo.FullName);

            var files = Directory.GetFiles(parentDir, "*.pdf")
                .Where(f => !f.Equals(fsi.FileInfo.FullName))
                .Select(f => new FileInfo(f))
                .ToList();

            if (files.Count > 0)
            {
                uc_SelectFileAndPageControl1.AddFiles(files.Select(f => f.FullName).ToList());
                
            }

        }

        public FormVisualBlocknoteSpiral(List<IFileSystemInfoExt> fsis) : this()
        {
            foreach (var fsi in fsis)
            {
                FileInfo fi = new FileInfo(fsi.FileInfo.FullName);
                cb_files.Items.Add(fi);
            }
            if (cb_files.Items.Count > 0)
            {
                cb_files.SelectedIndex = 0;
                AddBlockFiles(fsis[0]);
            }
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


        }
        [RequiresFeature(LicenseFeature.ExportPdf)]
        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (!LicenseUiGate.RequireFor(this, GetType(), nameof(btn_ok_Click)))
            {
                return;
            }

            SpiralSettings.SpiralPlace = (SpiralPlaceEnum)cb_place.SelectedIndex;
            SpiralSettings.SpiralFile = uc_SelectSpiralControl1.GetSelectedSpiralFilePath();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DrawRectangle()
        {
            if (!uc_VisualRectangleControl1.RectEnabled) return;
            _primitives.Add(new ScreenFillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(64, 255, 0, 0)),
                    (float)uc_VisualRectangleControl1.X,
                    (float)uc_VisualRectangleControl1.Y,
                    (float)uc_VisualRectangleControl1.W,
                    (float)uc_VisualRectangleControl1.H));
        }

        private void DrawSpiral(int pageNo)
        {

            var spiralPreview = uc_SelectSpiralControl1.GetSpiralBitmap();
            if (spiralPreview == null) return;

            PdfPageInfo pageInfo = uc_PreviewBrowserFile1.GetPageInfo(pageNo);
            if (pageInfo == null) return;

            var spiralPdfInfo = uc_SelectSpiralControl1.GetSpiralPdfInfo();

            SpiralPlaceEnum place = (SpiralPlaceEnum)cb_place.SelectedIndex;

            _primitives.AddRange(Static.Pdf.Visual.Commons.Screen.DrawSpiral.Draw(spiralPreview, spiralPdfInfo, place, pageInfo, pageNo-1));
        }

        void DrawSpirales(int pageNo)
        {
            DrawFile();
            DrawSpiral(pageNo);
            DrawRectangle();
        }

        private void DrawFile()
        {
            if (uc_SelectFileAndPageControl1.IsFileSelected)
            {
                var selectedFilePath = uc_SelectFileAndPageControl1.SelectedFilePath;
                var selectedPageNo = uc_SelectFileAndPageControl1.SelectedPageNo;
                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    var blockPrimitives = new ScreenPdf(selectedFilePath, selectedPageNo) { X = -1, Y = 0 };
                    _primitives.AddRange(blockPrimitives);
                }
            }
        }

        private void cb_place_SelectedIndexChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        private void cb_files_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cb_files.SelectedItem is FileInfo fsi)
            {
                _fsi = fsi;
                uc_PreviewBrowserFile1.Show(fsi.ToFileSystemInfoExt());
                Redraw();
            }
        }
    }
}
