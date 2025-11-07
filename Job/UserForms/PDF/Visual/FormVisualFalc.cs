using Amazon.Runtime.Internal.Util;
using Interfaces;
using Interfaces.PdfUtils;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using MongoDB.Bson.IO;
using PDFiumSharp;
using PDFiumSharp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualFalc : Form
    {
        Control[] _parts;
        NumericUpDown[] _deltas;
        Label[] _labels;
        IFileSystemInfoExt _fsi;
        bool _isShowing = false;
        private Pen _redLinePen = new Pen(Color.Red, 2);
        decimal[] partsDelta;

        public FormVisualFalc(IFileSystemInfoExt fsi)
        {
            _fsi = fsi;
            InitializeComponent();

            nud_width.Value = _fsi.Format.Width;
            _parts = new Control[] { gb_p1, gb_p2, gb_p3, gb_p4, gb_p5, gb_p6, gb_p7, gb_p8, gb_p9, gb_p10 };
            _deltas = new NumericUpDown[] {numericUpDown1, numericUpDown2, numericUpDown3, numericUpDown4, numericUpDown5,
                numericUpDown6, numericUpDown7, numericUpDown8, numericUpDown9, numericUpDown10 };
            _labels = new Label[] { label1, label2, label3, label4, label5,
                label6, label7, label8, label9, label10 };

            cb_cnt_falc.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            for (int i = 1; i < _parts.Length; i++)
            {
                _parts[i].Enabled = (i <= cb_cnt_falc.SelectedIndex + 1);
            }
            Recalc();
        }

        private void Recalc()
        {
            int cntPart = cb_cnt_falc.SelectedIndex + 2;
            decimal width = nud_width.Value;

            partsDelta = new decimal[cntPart];
            decimal delta = 0;
            for (int i = 0; i < cntPart; i++)
            {
                delta += _deltas[i].Value;
                partsDelta[i] = delta;
            }

            delta = partsDelta.Sum();
            decimal partWidth = (width + delta) / cntPart;
            for (int i = 0; i < cntPart; i++)
            {
                partsDelta[i] = Math.Round(partWidth - partsDelta[i], 2);
                _labels[i].Text = partsDelta[i].ToString();
            }
            if (_isShowing)
                pb_preview.Invalidate();

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Recalc();
            
        }

        private void FormVisualFalc_Load(object sender, EventArgs e)
        {
            Image pageImage = RenderTrimBox(_fsi, 0, 150);
            pb_preview.Width = pageImage.Width;
            pb_preview.Height = pageImage.Height;
            pb_preview.Image = pageImage;

            pb_preview.Paint += Pb_preview_Paint;
            //pb_preview.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void Pb_preview_Paint(object sender, PaintEventArgs e)
        {
            if (partsDelta == null) return;
            if (_isShowing == false) return;
            if (e.Graphics == null) return;
            Graphics g = e.Graphics;

            float x=0;
                for (int i = 0; i < partsDelta.Length-1; i++)
                {
                    x += (float)((double)partsDelta[i] * PdfHelper.mn * (150.0f / 72.0f));
                   g.DrawLine(_redLinePen, x, 0, x, pb_preview.Height);
                }
        }

        public Bitmap RenderTrimBox(IFileSystemInfoExt fsi, int pageIndex, int dpi = 150)
        {
            var boxes = PdfHelper.GetPagesInfo(fsi.FileInfo.FullName);


            using (var document = new PdfDocument(fsi.FileInfo.FullName))
            {
                using (var page = document.Pages[pageIndex])
                {
                    double scale = dpi / 72.0;
                    int pagePxW = (int)Math.Ceiling(page.Width * scale);
                    int pagePxH = (int)Math.Ceiling(page.Height * scale);

                    // Create PDFiumBitmap for rendering
                    using (var pdfiumBmp = new PDFiumBitmap(pagePxW, pagePxH, true))
                    {
                        page.Render(
                            pdfiumBmp,
                            (0, 0, pagePxW, pagePxH),
                            PDFiumSharp.Enums.PageOrientations.Normal,
                            PDFiumSharp.Enums.RenderingFlags.Annotations | PDFiumSharp.Enums.RenderingFlags.LcdText
                        );

                        // Convert PDFiumBitmap to System.Drawing.Bitmap
                        using (var bmpStream = pdfiumBmp.AsBmpStream(dpi, dpi))
                        {
                            using (var fullBmp = new Bitmap(bmpStream))
                            {
                                double trimLeft = boxes[pageIndex].Trimbox.left,
                                        trimTop = boxes[pageIndex].Trimbox.top,
                                        trimWidth = boxes[pageIndex].Trimbox.width,
                                        trimHeight = boxes[pageIndex].Trimbox.height;

                                int trimX = (int)Math.Round(trimLeft * scale);
                                int trimYTop = (int)Math.Round(trimTop * scale);
                                int trimW = (int)Math.Round(trimWidth * scale);
                                int trimH = (int)Math.Round(trimHeight * scale);

                                // Convert PDF bottom-left origin to bitmap top-left origin correctly:
                                int trimY = pagePxH - trimYTop - trimH;

                                // Clamp to image bounds
                                var requested = new Rectangle(trimX, trimY, trimW, trimH);
                                var imageRect = new Rectangle(0, 0, pagePxW, pagePxH);
                                var crop = Rectangle.Intersect(requested, imageRect);

                                // Validate crop before cloning
                                if (crop.Width <= 0 || crop.Height <= 0)
                                {
                                    // fallback: return a safe small empty bitmap or the whole image
                                    // choose behavior appropriate for your app. Example: return the full image:
                                    return new Bitmap(fullBmp);
                                }

                                var preview = fullBmp.Clone(crop, fullBmp.PixelFormat);

                                return preview;
                            }
                        }
                    }
                }
            }
        }

        private void pb_preview_Paint(object sender, PaintEventArgs e)
        {
            //// зависає із-за цього форма

            
            //// відмалювати вертикальні лінії згинів
            
        }

        private void FormVisualFalc_Shown(object sender, EventArgs e)
        {
            _isShowing = true;
        }
    }
}
