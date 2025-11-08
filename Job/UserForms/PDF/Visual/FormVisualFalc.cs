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
        float _zoomFactor = 1.0f;
        Control[] _parts;
        NumericUpDown[] _deltas;
        Label[] _labels;
        IFileSystemInfoExt _fsi;
        bool _isShowing = false;
        private Pen _redLinePen = new Pen(Color.Red, 1);
        private Pen _whiteLinePen = new Pen(Color.White, 2);
        decimal[] partsDelta;
        Image document_prev;
        List<PdfPageInfo> boxes;

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
            pb_preview.Paint += Pb_preview_Paint;
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

            boxes = PdfHelper.GetPagesInfo(_fsi.FileInfo.FullName);
            nud_page_no.Maximum = boxes.Count;
            label_total_pages.Text = $"/ {boxes.Count}";
            GeneratePageImage(0);

        }

        private void GeneratePageImage(int pageIdx)
        {
            if (document_prev != null)
            {
                document_prev.Dispose();
            }
            Image pageImage = RenderTrimBox(_fsi, pageIdx, 150);
            document_prev = pageImage;
        }

        private void Pb_preview_Paint(object sender, PaintEventArgs e)
        {
            if (partsDelta == null) return;
            if (_isShowing == false) return;
            if (e.Graphics == null) return;
            if (document_prev == null) return;

            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            e.Graphics.ScaleTransform(_zoomFactor, _zoomFactor);
            e.Graphics.DrawImage(document_prev, 0, 0, (float)_fsi.Format.Width, (float)_fsi.Format.Height);
            float x = 0;

            if (cb_mirrored_parts.Checked)
            {
                if (nud_page_no.Value % 2 == 0)
                {
                    for (int i = 0; i < partsDelta.Length - 1; i++)
                    {
                        x += (float)((double)partsDelta[i]);
                        g.DrawLine(_whiteLinePen, x, 0, x, pb_preview.Height);
                        g.DrawLine(_redLinePen, x, 0, x, pb_preview.Height);
                    }
                }
                else

                    for (int i = partsDelta.Length - 1; i > 0; i--)
                    {
                        x += (float)((double)partsDelta[i]);
                        g.DrawLine(_whiteLinePen, x, 0, x, pb_preview.Height);
                        g.DrawLine(_redLinePen, x, 0, x, pb_preview.Height);
                    }

            }
            else
            {
                for (int i = partsDelta.Length - 1; i > 0; i--)
                {
                    x += (float)((double)partsDelta[i]);
                    g.DrawLine(_whiteLinePen, x, 0, x, pb_preview.Height);
                    g.DrawLine(_redLinePen, x, 0, x, pb_preview.Height);
                }

            }


        }

        public Bitmap RenderTrimBox(IFileSystemInfoExt fsi, int pageIndex, int dpi = 150)
        {


            // Open FileStream and use PDFiumSharp stream constructor to avoid loading whole file into memory
            using (var fs = System.IO.File.Open(fsi.FileInfo.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                // PdfDocument(Stream, FPDF_FILEREAD, Int32, string) requires the file-length as Int32.
                if (fs.Length > int.MaxValue)
                    throw new NotSupportedException("PDF too large to open via PDFiumSharp stream constructor (file length > Int32.MaxValue).");

                int length = (int)fs.Length;
                var fr = CreateFileReadStruct(fs);

                using (var document = new PdfDocument(fs, fr, length, null))
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
        }

        // Helper: build FPDF_FILEREAD that reads from the provided FileStream on-demand.
        // Uses reflection to wire up the internal delegate/fields of PDFiumSharp.Types.FPDF_FILEREAD.
        // If PDFiumSharp changes internal layout this routine tries to find sensible fields by type/name.
        private PDFiumSharp.Types.FPDF_FILEREAD CreateFileReadStruct(System.IO.FileStream fs)
        {
            // Delegate signature: bool Handler(IntPtr fileAccess, int position, IntPtr buffer, int size)
            PDFiumSharp.Types.FileReadBlockHandler handler = (IntPtr fileAccess, int position, IntPtr buffer, int size) =>
            {
                try
                {
                    if (size <= 0) return true;
                    var temp = new byte[size];
                    lock (fs) // ensure concurrent calls are serialized
                    {
                        if (fs.Position != position) fs.Position = position;
                        int read = 0;
                        while (read < size)
                        {
                            int r = fs.Read(temp, read, size - read);
                            if (r <= 0) break;
                            read += r;
                        }
                        if (read > 0)
                            System.Runtime.InteropServices.Marshal.Copy(temp, 0, buffer, read);
                        return read == size;
                    }
                }
                catch
                {
                    return false;
                }
            };

            var fr = new PDFiumSharp.Types.FPDF_FILEREAD((int)fs.Length, handler);
            var t = typeof(PDFiumSharp.Types.FPDF_FILEREAD);
            var fields = t.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

            // try to find the delegate field and assign it
            var delField = fields.FirstOrDefault(f => f.FieldType == typeof(PDFiumSharp.Types.FileReadBlockHandler));
            if (delField != null)
                delField.SetValue(fr, handler);
            else
            {
                // best-effort: try fields with likely names
                var p = fields.FirstOrDefault(f => f.Name.ToLower().Contains("getblock") || f.Name.ToLower().Contains("read"));
                if (p != null && p.FieldType.IsAssignableFrom(typeof(PDFiumSharp.Types.FileReadBlockHandler)))
                    p.SetValue(fr, handler);
            }

            // try to set file length field if present
            var lenField = fields.FirstOrDefault(f => f.FieldType == typeof(long) || f.FieldType == typeof(int)
                                                 || f.Name.ToLower().Contains("filelen") || f.Name.ToLower().Contains("filesize"));
            if (lenField != null)
            {
                if (lenField.FieldType == typeof(long))
                    lenField.SetValue(fr, fs.Length);
                else if (lenField.FieldType == typeof(int))
                    lenField.SetValue(fr, (int)fs.Length);
            }

            return fr;
        }

        private void FormVisualFalc_Shown(object sender, EventArgs e)
        {
            _isShowing = true;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            _zoomFactor = trackBar1.Value / 100.0f;
            pb_preview.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pb_preview.Invalidate();
        }

        private void nud_page_no_ValueChanged(object sender, EventArgs e)
        {
            GeneratePageImage((int)(nud_page_no.Value - 1));
            pb_preview.Invalidate();
        }
    }
}
