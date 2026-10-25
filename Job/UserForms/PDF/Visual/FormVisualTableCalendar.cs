using Interfaces;
using Interfaces.Licensing;
using JobSpace.Licensing;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives;
using JobSpace.Static.Pdf.Visual.BlocknoteSpiral;
using JobSpace.Static.Pdf.Visual.Commons.Screen;
using JobSpace.Static.Pdf.Visual.TableCalendar;
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
    public partial class FormVisualTableCalendar : Form
    {
        IFileSystemInfoExt _file;
        List <IScreenPrimitive> _primitives = new List<IScreenPrimitive>();

        decimal _x_top_coef = 0;
        decimal _y_top_coef = 0;

        decimal _x_bottom_coef = 0.5m;
        decimal _y_bottom_coef = 1m;


        public FormVisualTableCalendar(IFileSystemInfoExt f)
        {
            InitializeComponent();

            _file = f;

            LoadBlockFiles();

            uc_VisualRectangleControl_top.DisableRows(1, 2);
            uc_VisualRectangleControl_bottom.DisableRows(0, 1);

            uc_SelectFileAndPageControl1.FileSelectionChanged += (s, e) => Redraw();
            uc_SelectFileAndPageControl1.PositionChanged += (s, e) =>
            {
                _x_top_coef = e.Item1;
                _y_top_coef = e.Item2;
                Redraw();
            };
            uc_SelectFileAndPageControl2.FileSelectionChanged += (s, e) => Redraw();
            uc_SelectFileAndPageControl2.PositionChanged += (s, e) =>
            {
                _x_bottom_coef = e.Item1;
                _y_bottom_coef = e.Item2;
                Redraw();
            };
            uc_PreviewBrowserFile1.SetFunc_GetScreenPrimitives(GetPrimitives);
        }
        private void LoadBlockFiles()
        {
            string parentDir = Path.GetDirectoryName(_file.FileInfo.FullName);
            // Load block files from the parent directory
            var blockFiles = Directory.GetFiles(parentDir, "*.pdf")
                .Where(f => !f.Equals(_file.FileInfo.FullName))
                .Select(f => new FileInfo(f))
                .ToList();

            if (blockFiles.Count > 0)
            {
                var files = blockFiles.Select(f => f.FullName).ToList();
                uc_SelectFileAndPageControl1.AddFiles(files);
                uc_SelectFileAndPageControl2.AddFiles(files);
            }
        }

        private List<IScreenPrimitive> GetPrimitives(int pageNo)
        {
            _primitives.Clear();

            Draw(pageNo);
            return _primitives;
        }

        void Redraw() => uc_PreviewBrowserFile1.Redraw();

        private void FormVisualTableCalendar_Load(object sender, EventArgs e)
        {
            if (_file != null)
            {
                uc_PreviewBrowserFile1.Show(_file);

                var boxes = PdfHelper.GetPageInfo(_file.FileInfo.FullName);
                if (boxes != null)
                {
                    uc_VisualRectangleControl_top.SetPdfPageInfo(boxes);
                    uc_VisualRectangleControl_bottom.SetPdfPageInfo(boxes);
                }

                uc_SelectSpiralControl1.OnSpiralChanged += (s, ee) => Redraw();

                uc_VisualRectangleControl_top.OnRectEnabledChanged += (s, ee) => Redraw();
                uc_VisualRectangleControl_top.OnRectPositionChanged += (s, ee) => Redraw();
                uc_VisualRectangleControl_top.OnRectSizeChanged += (s, ee) => Redraw();

                uc_VisualRectangleControl_bottom.OnRectEnabledChanged += (s, ee) => Redraw();
                uc_VisualRectangleControl_bottom.OnRectPositionChanged += (s, ee) => Redraw();
                uc_VisualRectangleControl_bottom.OnRectSizeChanged += (s, ee) => Redraw();

                Redraw();
            }
        }

        private void nud_osnova_ValueChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        [RequiresFeature(LicenseFeature.ThreeDPreview)]
        private void btn_3d_Click(object sender, EventArgs e)
        {
            if (!LicenseUiGate.RequireFor(this, GetType(), nameof(btn_3d_Click)))
                return;

            try
            {
                PdfPageInfo pageInfo = uc_PreviewBrowserFile1.GetCurrentPageInfo();
                if (pageInfo == null)
                {
                    MessageBox.Show("Не вдалося отримати поточну сторінку PDF.", "3D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                double pageHeight = pageInfo.Trimbox.hMM();
                double baseWidth = (double)nud_osnova.Value;
                if (baseWidth <= 0)
                {
                    MessageBox.Show("Ширина основи має бути більшою за нуль.", "3D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Cursor = Cursors.WaitCursor;
                using (Bitmap preview = Create3dPreviewTexture())
                {
                    TableCalendar3DHtmlExporter.ExportAndOpen(
                        _file.FileInfo.FullName,
                        preview,
                        pageInfo.Trimbox.wMM(),
                        pageHeight,
                        baseWidth);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "3D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // The texture intentionally contains the printable preview layers (PDF, selected blocks and spiral),
        // but not the semi-transparent collision rectangles and red construction guides.
        private Bitmap Create3dPreviewTexture()
        {
            int pageIndex = uc_PreviewBrowserFile1.GetCurrentPageIdx();
            PdfPageInfo pageInfo = uc_PreviewBrowserFile1.GetCurrentPageInfo();
            Bitmap texture = PdfHelper.RenderByTrimBox(_file.FileInfo.FullName, pageIndex, 140);

            using (Graphics graphics = Graphics.FromImage(texture))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.ScaleTransform(texture.Width / (float)pageInfo.Trimbox.wMM(), texture.Height / (float)pageInfo.Trimbox.hMM());
                DrawFileBlock();
                DrawSpiral();
                foreach (IScreenPrimitive primitive in _primitives)
                {
                    primitive.Draw(graphics);
                    (primitive as IDisposable)?.Dispose();
                }
                _primitives.Clear();
            }

            return texture;
        }

        private void Draw(int pageNo)
        {
            DrawFileBlock();
            DrawRectangles();
            DrawSpiral();
            DrawOsnova();
        }

        private void DrawFileBlock()
        {
            if (uc_SelectFileAndPageControl1.IsFileSelected)
            {
                PdfPageInfo pageInfo = uc_PreviewBrowserFile1.GetCurrentPageInfo();
                if (pageInfo == null) return;

                var w = pageInfo.Trimbox.wMM();
                var h = pageInfo.Trimbox.hMM();

                var selectedFilePath = uc_SelectFileAndPageControl1.SelectedFilePath;
                var selectedPageNo = uc_SelectFileAndPageControl1.SelectedPageNo;

                var blockPageInfo = PdfHelper.GetPageInfo(selectedFilePath, selectedPageNo - 1);

                var x = (w - blockPageInfo.Trimbox.wMM()) * (double)_x_top_coef;
                var y = (h - blockPageInfo.Trimbox.hMM()) * (double)_y_top_coef;

                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    var blockPrimitives = new ScreenPdf(selectedFilePath, selectedPageNo) { X = (float)x, Y = (float)y };

                    _primitives.AddRange(blockPrimitives);
                }
            }

            if (uc_SelectFileAndPageControl2.IsFileSelected)
            {
                PdfPageInfo pageInfo = uc_PreviewBrowserFile1.GetCurrentPageInfo();
                if (pageInfo == null) return;

                var w = pageInfo.Trimbox.wMM();
                var h = pageInfo.Trimbox.hMM();

                var selectedFilePath = uc_SelectFileAndPageControl2.SelectedFilePath;
                var selectedPageNo = uc_SelectFileAndPageControl2.SelectedPageNo;

                var pageblockInfo = PdfHelper.GetPageInfo(selectedFilePath, selectedPageNo-1);
                if (pageblockInfo == null) return;

                

                var x = (w - pageblockInfo.Trimbox.wMM()) * (double)_x_bottom_coef;
                var y = (h - pageblockInfo.Trimbox.hMM()) * (double)_y_bottom_coef;

                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    var blockPrimitives = new ScreenPdf(selectedFilePath, selectedPageNo) { X = (float)x, Y = (float)y, Angle = 180 };
                    _primitives.AddRange(blockPrimitives);
                }
            }
        }

        private void DrawRectangles()
        {
            if (uc_VisualRectangleControl_top.RectEnabled)
            {
                _primitives.Add(new ScreenFillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(64, 255, 0, 0)),
                    (float)uc_VisualRectangleControl_top.X,
                    (float)uc_VisualRectangleControl_top.Y,
                    (float)uc_VisualRectangleControl_top.W,
                    (float)uc_VisualRectangleControl_top.H));
            }

            if (uc_VisualRectangleControl_bottom.RectEnabled)
            {
                _primitives.Add(new ScreenFillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(64, 255, 0, 0)),
                    (float)uc_VisualRectangleControl_bottom.X,
                    (float)uc_VisualRectangleControl_bottom.Y,
                    (float)uc_VisualRectangleControl_bottom.W,
                    (float)uc_VisualRectangleControl_bottom.H));
            }

        }

        private void DrawSpiral()
        {
            var spiralPreview = uc_SelectSpiralControl1.GetSpiralBitmap();
            if (spiralPreview == null) return;

            PdfPageInfo pageInfo = uc_PreviewBrowserFile1.GetCurrentPageInfo();
            if (pageInfo == null) return;

            var spiralPdfInfo = uc_SelectSpiralControl1.GetSpiralPdfInfo();

            SpiralPlaceEnum place = SpiralPlaceEnum.top_bottom;
            int curPageIdx = uc_PreviewBrowserFile1.GetCurrentPageIdx();

            _primitives.AddRange(Static.Pdf.Visual.Commons.Screen.DrawSpiral.Draw(spiralPreview, spiralPdfInfo, place, pageInfo, curPageIdx));
        }

        private void DrawOsnova()
        {
            PdfPageInfo pageInfo = uc_PreviewBrowserFile1.GetCurrentPageInfo();
            if (pageInfo == null) return;
            float osnovaHeight = (float)nud_osnova.Value;

            var y = ((float)pageInfo.Trimbox.hMM() - osnovaHeight) / 2;

            using (Pen pen = new Pen(Color.Red, 0.5f))
            {
                _primitives.Add(new ScreenLine(pen, 0, y, (float)pageInfo.Trimbox.width, y));

                y += osnovaHeight;

                _primitives.Add(new ScreenLine(pen, 0, y, (float)pageInfo.Trimbox.width, y));
            }
        }
    }
}
