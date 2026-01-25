using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.SoftCover;
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
    public partial class FormVisualSoftCover : Form
    {
        IFileSystemInfoExt _fileInfo;
        public FormVisualSoftCover(IFileSystemInfoExt f)
        {
            InitializeComponent();
            _fileInfo = f;
        }

        private void FormVisualSoftCover_Shown(object sender, EventArgs e)
        {
            if (_fileInfo != null)
            {
                uc_PreviewBrowserFile1.Show(_fileInfo);

                ShowTotalCoverSize();
            }
        }

        private void ShowTotalCoverSize()
        {
            nud_total_width.Value = nud_root.Value + nud_left_klapan.Value + nud_right_klapan.Value + (nud_width.Value + nud_bleed.Value) * 2;
            nud_total_height.Value = nud_height.Value + (nud_bleed.Value * 2);
            DrawSchema();
        }

        private void DrawSchema()
        {
            float bleed = (float)nud_bleed.Value;
            float width = (float)nud_width.Value;
            float height = (float)nud_height.Value;
            float left_klapan = (float)nud_left_klapan.Value;
            float right_klapan = (float)nud_right_klapan.Value;
            float root = (float)nud_root.Value;
            float totalW = (float)nud_total_width.Value;
            float totalH = (float)nud_total_height.Value;

            var pdfPageInfo = uc_PreviewBrowserFile1.GetCurrentPageInfo();

            float x = ((float)pdfPageInfo.Trimbox.wMM() - totalW) / 2;
            float y = ((float)pdfPageInfo.Trimbox.hMM() - totalH) / 2;

            List<IScreenPrimitive> primitives = new List<IScreenPrimitive>();

            using (Pen pen = new Pen(Color.Red, 0.5f))
            {
                x += bleed;
                y += bleed;

                // обрізний розмір
                primitives.Add(new ScreenRectangle(pen, x, y, left_klapan + right_klapan + width * 2 + root, height));

                if (left_klapan > 0)
                {
                    x += left_klapan;
                    // лівий клапан
                    primitives.Add(new ScreenLine(pen, x, y, x, y + height));

                }
                x += width;
                // ліва сторінка
                primitives.Add(new ScreenLine(pen, x, y, x, y + height));

                x += root;
                //корінець
                primitives.Add(new ScreenLine(pen, x, y, x, y + height));


                x += width;

                // права сторінка
                primitives.Add(new ScreenLine(pen, x, y, x, y + height));

                if (right_klapan > 0)
                {
                    x += right_klapan;
                    // правий клапан
                    primitives.Add(new ScreenLine(pen, x, y, x, y + height));

                }

            }

            uc_PreviewBrowserFile1.SetPrimitives(primitives);
        }

        private void nud_ValueChanged(object sender, EventArgs e)
        {
            ShowTotalCoverSize();
        }

        private void btn_create_schema_Click(object sender, EventArgs e)
        {
            new SoftCover(new SoftCoverParams
            {
                Bleed = (double)nud_bleed.Value,
                Width = (double)nud_width.Value,
                Height = (double)nud_height.Value,
                LeftKlapan = (double)nud_left_klapan.Value,
                RightKlapan = (double)nud_right_klapan.Value,
                Root = (double)nud_root.Value,
                FolderOutput = Path.GetDirectoryName(_fileInfo.FullName),
                
            }).Run();
        }

        private void btn_apply_schema_Click(object sender, EventArgs e)
        {
            new SoftCover(new SoftCoverParams
            {
                Bleed = (double)nud_bleed.Value,
                Width = (double)nud_width.Value,
                Height = (double)nud_height.Value,
                LeftKlapan = (double)nud_left_klapan.Value,
                RightKlapan = (double)nud_right_klapan.Value,
                Root = (double)nud_root.Value,
                FolderOutput = Path.GetDirectoryName(_fileInfo.FullName),

            }).Run(_fileInfo.FullName);
        }
    }
}
