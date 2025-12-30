using Interfaces;
using iText.Kernel.Pdf;
using iTextSharp.text;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives;
using PDFiumSharp;
using PDFlib_dotnet;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.Static.Pdf.Common
{
    public static class PdfHelper
    {
        public static double mn = 2.83465;


        public static Boxes GetBoxes(PDFlib p, int doc, int pageIdx)
        {
            Boxes boxes = new Boxes();

            var trims = new double[] { 0, 0, 0, 0 };
            var media = new double[] { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                media[i] = p.pcos_get_number(doc, $"pages[{pageIdx}]/MediaBox[{i}]");
            }

            string trimtype = p.pcos_get_string(doc, $"type:pages[{pageIdx}]/TrimBox");

            if (string.Equals(trimtype, "array", System.StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < 4; i++)
                {
                    trims[i] = p.pcos_get_number(doc, $"pages[{pageIdx}]/TrimBox[{i}]");
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    trims[i] = media[i];
                }
            }

            boxes.Media.left = media[0];
            boxes.Media.bottom = media[1];
            boxes.Media.width = media[2] - media[0];
            boxes.Media.height = media[3] - media[1];

            boxes.Trim.left = trims[0] - media[0];
            boxes.Trim.bottom = trims[1] - media[1];
            boxes.Trim.width = trims[2] - trims[0];
            boxes.Trim.height = trims[3] - trims[1];
            boxes.Trim.top = media[3] - trims[3];
            boxes.Trim.right = media[2] - trims[2];

            return boxes;
        }


        public static Box GetTrimbox(PDFlib p, int doc, int page)
        {
            var trims = new double[] { 0, 0, 0, 0 };
            var media = new double[] { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                media[i] = p.pcos_get_number(doc, $"pages[{page}]/MediaBox[{i}]");
            }

            string trimtype = p.pcos_get_string(doc, $"type:pages[{page}]/TrimBox");

            if (string.Equals(trimtype, "array", System.StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < 4; i++)
                {
                    trims[i] = p.pcos_get_number(doc, $"pages[{page}]/TrimBox[{i}]");
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    trims[i] = media[i];
                }
            }

            return new Box()
            {
                left = trims[0] - media[0],
                bottom = trims[1] - media[1],
                width = trims[2] - trims[0],
                height = trims[3] - trims[1],
                top = media[3] - trims[3],
                right = media[2] - trims[2],
            };
        }


        public static List<PdfPageInfo> GetPagesInfo(string filePath)
        {
            List<PdfPageInfo> list = new List<PdfPageInfo>();

            PDFlib p = null;

            try
            {
                p = new PDFlib();
                p.begin_document("", "");
                int indoc = p.open_pdi_document(filePath, "");
                int pageCnt = (int)p.pcos_get_number(indoc, "length:pages");

                for (int i = 0; i < pageCnt; i++)
                {
                    var info = new PdfPageInfo();

                    int page = p.open_pdi_page(indoc, i + 1, "");

                    string rotated = p.pcos_get_string(indoc, $"type:pages[{i}]/Rotate");

                    if (string.Equals(rotated, "number", System.StringComparison.OrdinalIgnoreCase))
                    {
                        info.Rotate = p.pcos_get_number(indoc, $"pages[{i}]/Rotate");
                    }

                    Boxes boxes = GetBoxes(p, indoc, page);
                    info.Mediabox = boxes.Media;
                    info.Trimbox = boxes.Trim;

                    list.Add(info);

                    p.close_pdi_page(page);

                }

                p.close_pdi_document(indoc);

            }
            catch (PDFlibException e)
            {

                LogException(e, "GetPagesInfo");
            }
            finally
            {
                p?.Dispose();
            }

            return list;
        }

        public static PdfPageInfo GetPageInfo(string path,int pageIdx = 0)
        {
            PdfPageInfo pdfPageInfo = new PdfPageInfo();
            PDFlib p = null;

            try
            {
                p = new PDFlib();
                p.begin_document("", "");
                int indoc = p.open_pdi_document(path, "");
                int pageCnt = (int)p.pcos_get_number(indoc, "length:pages");

                int i = pageIdx;
                var info = new PdfPageInfo();

                int page = p.open_pdi_page(indoc, i + 1, "");

                string rotated = p.pcos_get_string(indoc, $"type:pages[{i}]/Rotate");

                if (string.Equals(rotated, "number", System.StringComparison.OrdinalIgnoreCase))
                {
                    info.Rotate = p.pcos_get_number(indoc, $"pages[{i}]/Rotate");
                }

                Boxes boxes = GetBoxes(p, indoc, page);
                info.Mediabox = boxes.Media;
                info.Trimbox = boxes.Trim;

                pdfPageInfo = info;

                p.close_pdi_page(page);

                p.close_pdi_document(indoc);

            }
            catch (PDFlibException e)
            {

                LogException(e, "GetPagesInfo");
            }
            finally
            {
                p?.Dispose();

            }
            return pdfPageInfo;
        }

        public static void SetFillStroke(PDFlib p,ColorPalette palette, PrimitiveAbstract primitive)
        {
            var t = (primitive.Tint / 100);

            var fill = palette.GetBaseColorById(primitive.FillId);
            var stroke = palette.GetBaseColorById(primitive.StrokeId);

            if (fill != null)
            {
                if (fill.IsSpot)
                {
                    SetColor(p,"fill",fill,1);
                    int spot = p.makespotcolor(fill.Name);
                    p.setcolor("fill", "spot", spot, t, 0.0, 0.0);
                }
                else
                {
                    SetColor(p,"fill",fill,t);
                }
            }

            if (stroke != null)
            {
                if (stroke.IsSpot)
                {
                    SetColor(p,"stroke",stroke,1);
                    int spot = p.makespotcolor(stroke.Name);
                    p.setcolor("stroke", "spot", spot, t, 0.0, 0.0);
                }
                else
                {
                    SetColor(p,"stroke",stroke,t);
                }
            }
        }

        public static void CloseFillStroke(PDFlib p, PrimitiveAbstract primitive)
        {
            bool fill = primitive.FillId != null;
            bool stroke = primitive.StrokeId != null;

            if (fill && stroke)
            {
                p.fill_stroke();
            }
            else if (stroke)
            {
                p.stroke();
            }
            else
            {
                p.fill();
            }

        }

        static void SetColor(PDFlib p, string fill_stroke, MarkColor color, double tint)
        {
            p.setcolor(fill_stroke, "cmyk",
                                color.C * tint / 100,
                                color.M * tint / 100,
                                color.Y * tint / 100,
                                color.K * tint / 100);
        }




        public static void LogException(PDFlibException e, string title)
        {
            Logger.Log.Error(null, title, $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
        }

        public static Bitmap RenderByTrimBox(string file, int pageIndex, int dpi = 150)
        {
            FileInfo fsi = new FileInfo(file);
            return RenderByTrimBox(fsi, pageIndex, dpi);
        }
        public static Bitmap RenderByTrimBox(FileInfo fsi, int pageIndex, int dpi = 150)
        {
            var box = GetPageInfo(fsi.FullName,pageIndex);

            // Open FileStream and use PDFiumSharp stream constructor to avoid loading whole file into memory
            using (var fs = System.IO.File.Open(fsi.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                // PdfDocument(Stream, FPDF_FILEREAD, Int32, string) requires the file-length as Int32.
                if (fs.Length > int.MaxValue)
                    throw new NotSupportedException("PDF too large to open via PDFiumSharp stream constructor (file length > Int32.MaxValue).");

                int length = (int)fs.Length;
                var fr = CreateFileReadStruct(fs);

                using (var document = new PDFiumSharp.PdfDocument(fs, fr, length, null))
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
                                    double trimLeft = box.Trimbox.left,
                                            trimTop = box.Trimbox.top,
                                            trimWidth = box.Trimbox.width,
                                            trimHeight = box.Trimbox.height;

                                    int trimX = (int)Math.Round(trimLeft * scale);
                                    int trimYTop = (int)Math.Round(trimTop * scale);
                                    int trimW = (int)Math.Round(trimWidth * scale);
                                    int trimH = (int)Math.Round(trimHeight * scale);

                                    // Convert PDF bottom-left origin to bitmap top-left origin correctly:
                                    int trimY = pagePxH - trimYTop - trimH;

                                    // Clamp to image bounds
                                    var requested = new System.Drawing.Rectangle(trimX, trimY, trimW, trimH);
                                    var imageRect = new System.Drawing.Rectangle(0, 0, pagePxW, pagePxH);
                                    var crop = System.Drawing.Rectangle.Intersect(requested, imageRect);

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
        private static PDFiumSharp.Types.FPDF_FILEREAD CreateFileReadStruct(System.IO.FileStream fs)
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

        public static int GetPageCount(string fullName)
        {
            // отримати кількість сторінок в pdf файлі
            int pageCount = 0;
            PDFlib p = null;
            try
            {
                p = new PDFlib();
                p.begin_document("", "");
                int indoc = p.open_pdi_document(fullName, "");
                pageCount = (int)p.pcos_get_number(indoc, "length:pages");
                p.close_pdi_document(indoc);
            }
            catch (PDFlibException e)
            {
                LogException(e, "GetPageCount");
            }
            finally
            {
                p?.Dispose();
            }
            return pageCount;
        }

        public static void GetPdfCreatorApp(IFileSystemInfoExt file)
        {
            #region [Use iText]
            try
            {
                using (var pdfReader = new PdfReader(file.FileInfo.FullName))
                {
                    using (var pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfReader))
                    {
                        PdfDocumentInfo documentInfo = pdfDocument.GetDocumentInfo();

                        // Get the standard metadata properties
                        //string author = documentInfo.GetAuthor();
                        //string title = documentInfo.GetTitle();
                        file.CreatorApp = documentInfo.GetCreator();
                        //string producer = documentInfo.GetProducer();
                    }
                }
            }
            catch (IOException ex)
            {
                
            }
            catch (Exception ex)
            {
                
            }
            #endregion



            #region [Use PDFLIB (evaluation mode)]

            PDFlib p = null;
            try
            {
                p = new PDFlib();
                p.set_option("errorpolicy=return");
                int doc = p.open_pdi_document(file.FileInfo.FullName, "infomode=true");
                string objType = p.pcos_get_string(doc, "type:/Info/Creator");


                if (p.begin_document("hello.pdf", "") == -1)
                {
                    Console.WriteLine("Error: {0}\n", p.get_errmsg());

                }
                if (objType == "string")
                {
                    file.CreatorApp = p.pcos_get_string(doc, "/Info/Creator");
                }
                else
                {
                    file.CreatorApp = string.Empty;
                }
                p.close_pdi_document(doc);
            }
            catch (PDFlibException ex)
            {
                LogException(ex, "GetPdfCreatorApp");
            }
            finally
            {
                p?.Dispose();
            }
            #endregion
        }
    }
}
