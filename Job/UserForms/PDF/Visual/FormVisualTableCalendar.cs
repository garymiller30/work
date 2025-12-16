using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives;
using JobSpace.Static.Pdf.Visual.BlocknoteSpiral;
using JobSpace.Static.Pdf.Visual.Commons.Screen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualTableCalendar : Form
    {
        IFileSystemInfoExt _file;
        List<IScreenPrimitive> _primitives;

        public FormVisualTableCalendar(IFileSystemInfoExt f)
        {
            InitializeComponent();
            
            _file = f;

            uc_VisualRectangleControl_top.DisableRows(1,2);
            uc_VisualRectangleControl_bottom.DisableRows(0,1);
        }

        private void FormVisualTableCalendar_Load(object sender, EventArgs e)
        {
            if (_file != null)
            {
                uc_PreviewBrowserFile1.Show(_file);

                uc_VisualRectangleControl_top.SetPdfPageInfo(uc_PreviewBrowserFile1.GetCurrentPageInfo());
                uc_VisualRectangleControl_bottom.SetPdfPageInfo(uc_PreviewBrowserFile1.GetCurrentPageInfo());

                uc_SelectSpiralControl1.OnSpiralChanged += (s, ee) => Draw();
                uc_VisualRectangleControl_top.OnRectPositionChanged += (s, ee) => Draw();
                uc_VisualRectangleControl_top.OnRectSizeChanged += (s, ee) => Draw();

                uc_VisualRectangleControl_bottom.OnRectEnabledChanged += (s, ee) => Draw();
                uc_VisualRectangleControl_bottom.OnRectPositionChanged += (s, ee) => Draw();
                uc_VisualRectangleControl_bottom.OnRectSizeChanged += (s, ee) => Draw();

                Draw();
            }
        }

        private void nud_osnova_ValueChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            _primitives = new List<IScreenPrimitive>();
            DrawRectangles();
            DrawSpiral();
            DrawOsnova();
            uc_PreviewBrowserFile1.SetPrimitives( _primitives);
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

            var y = ((float)pageInfo.Trimbox.hMM() - osnovaHeight)/2;

            using (Pen pen = new Pen(Color.Red, 0.5f))
            {
                _primitives.Add(new ScreenLine(pen,0,y,(float)pageInfo.Trimbox.width,y));

                y+= osnovaHeight;

                _primitives.Add(new ScreenLine(pen, 0, y, (float)pageInfo.Trimbox.width, y));
            }
        }
    }
}
