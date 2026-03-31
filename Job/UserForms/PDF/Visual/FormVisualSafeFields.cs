using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives;
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
                (left,right) = (right, left);
            }


            if (left != 0)
            {
                _primitives.Add(new ScreenFillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(64, 255, 0, 0)),
                   x: 0,
                   y: 0,
                    (float)left,
                   (float)box.Trimbox.hMM()
                  ));
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
