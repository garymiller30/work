using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualSafeFields : Form
    {
        IFileSystemInfoExt _file;
        public FormVisualSafeFields(IFileSystemInfoExt file)
        {
            InitializeComponent();
            _file = file;

            uc_FilePreviewControl1.SetFunc_GetScreenPrimitives(GetPrimitives);
        }

        private List<IScreenPrimitive> GetPrimitives(int pageNo)
        {
            var _primitives = new List<IScreenPrimitive>();
            PdfPageInfo box = uc_FilePreviewControl1.GetCurrentPageInfo();

            if (nud_top.Value != 0)
            {
                _primitives.Add(new ScreenFillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(64, 255, 0, 0)),
                   x: 0,
                   y: 0,
                   (float)box.Trimbox.wMM(),
                   (float)nud_top.Value));
            }
            if (nud_bottom.Value != 0)
            {
                _primitives.Add(new ScreenFillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(64, 255, 0, 0)),
                   x: 0,
                   y: (float)(box.Trimbox.hMM() - (double)nud_bottom.Value),
                   (float)box.Trimbox.wMM(),
                   (float)nud_bottom.Value));
            }

            var left = nud_left.Value;
            var right = nud_right.Value;

            if (cb_spread.Checked && pageNo % 2 == 0)
            {
                (left, right) = (right, left);
            }


            if (left != 0)
            {
                float w = (float)left;
                float h = (float)box.Trimbox.hMM();

                //using (var bmp = new Bitmap((int)w, (int)h))
                //{
                //    using (var g = Graphics.FromImage(bmp))
                //    {
                //        RectangleF rect = new RectangleF(0, 0, w, h);
                //        using (LinearGradientBrush brush = new LinearGradientBrush(
                //            rect,
                //            Color.FromArgb(60, 0, 0, 0), // Solid Blue
                //            Color.FromArgb(0, 0, 0, 0),   // Transparent Blue
                //            LinearGradientMode.Horizontal))
                //        {
                //            g.FillRectangle(brush, rect);
                //        }

                //    }
                //    _primitives.Add(new ScreenImage(new Bitmap(bmp), 0, 0, w, h));
                //}

                _primitives.Add(new ScreenFillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(64, 255, 0, 0)),
                   x: 0,y: 0,w,h));
            }
            if (right != 0)
            {
                _primitives.Add(new ScreenFillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(64, 255, 0, 0)),
                   x: (float)(box.Trimbox.wMM() - (double)right),
                   y: 0,
                    (float)right,
                   (float)box.Trimbox.hMM()
                  ));
            }

            return _primitives;
        }

        private void nud_ValueChanged(object sender, EventArgs e)
        {
            uc_FilePreviewControl1.Redraw();
        }

        private void FormVisualSafeFields_Shown(object sender, EventArgs e)
        {
            if (_file != null)
            {
                uc_FilePreviewControl1.Show(_file);
            }
        }

        private void cb_spread_CheckedChanged(object sender, EventArgs e)
        {
            uc_FilePreviewControl1.Redraw();
        }
    }
}
