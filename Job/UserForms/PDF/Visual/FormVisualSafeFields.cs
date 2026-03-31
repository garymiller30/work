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

            if (nud_left.Value != 0)
            {
                _primitives.Add(new ScreenFillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(64, 255, 0, 0)),
                   x: 0,
                   y: 0,
                    (float)nud_left.Value,
                   (float)box.Trimbox.hMM()
                  ));
            }
            if (nud_right.Value != 0)
            {
                _primitives.Add(new ScreenFillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(64, 255, 0, 0)),
                   x: (float)(box.Trimbox.wMM() - (double)nud_right.Value),
                   y: 0,
                    (float)nud_right.Value,
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
    }
}
