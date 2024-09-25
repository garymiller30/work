using Job.Static.Pdf.Imposition.Drawers.PDF.Marks.Text;
using Job.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job.Static.Pdf.Imposition.Drawers.Services.Screen
{
    public static class DrawSingleSideService
    {
        

        public static Bitmap Draw(ProductPart productPart)
        {
            var sheet = productPart.Sheet;
            var templateContainer = sheet.TemplatePageContainer;

            Bitmap bitmap = new Bitmap((int)sheet.W + 1, ((int)sheet.H + 1));

            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;

            // draw sheet
            Pen pen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(0, 0, (int)sheet.W, (int)sheet.H);

            g.DrawRectangle(pen, rect);
            pen.Dispose();

            // draw safe field
            DrawSheetSafeField(g,sheet);
            
            // draw pages
            foreach (var page in templateContainer.TemplatePages)
            {
                DrawPage(g, page,(int)sheet.H);
            }

            g.Dispose();

            return bitmap;

        }

        private static void DrawSheetSafeField(Graphics g,TemplateSheet sheet)
        {
            if (sheet.SafeFields.Left !=0)
            // left
            g.FillRectangle(Brushes.LightPink,new Rectangle
            {
                X=0, Y=0,
                Width = (int)sheet.SafeFields.Left,
                Height = (int)sheet.H,
            });

            if (sheet.SafeFields.Bottom != 0)
                // bottom
                g.FillRectangle(Brushes.LightPink,new Rectangle
            {
                X=0,Y=0,
                Width = (int)sheet.W,
                Height = (int)sheet.SafeFields.Bottom,
            });

            if (sheet.SafeFields.Top != 0)
                // top
                g.FillRectangle(Brushes.LightPink, new Rectangle
            {
                X = 0,
                Y = (int)(sheet.H - sheet.SafeFields.Top),
                Width = (int)sheet.W,
                Height = (int)sheet.SafeFields.Top,
            });

            if (sheet.SafeFields.Right != 0)
                //right
                g.FillRectangle(Brushes.LightPink, new Rectangle
            {
                X = (int)(sheet.W - sheet.SafeFields.Right),
                Y = 0,
                Width = (int)sheet.SafeFields.Right,
                Height = (int)sheet.H,
            });
        }

        static void DrawPage(Graphics g, TemplatePage page, int sH)
        {
            int dist = 5;
            int height = 7;

            int x = (int)page.GetPageDrawX();
            int y = sH - (int)page.GetPageDrawY() - (int)page.GetPageDrawH();
            int w = (int)page.GetPageDrawW();
            int h = (int)page.GetPageDrawH();

            Brush brush = new SolidBrush(Color.AliceBlue);
            Pen pen = new Pen(Color.Black);
          

             
            var rect = new Rectangle
            {
                X = x,
                Y = y,
                Width = w,
                Height = h
            };

            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            brush.Dispose();
            pen.Dispose();

            brush = new SolidBrush(Color.Gray);

            int sx = 0;
            int sy = 0;
            int sw = 0;
            int sh = 0;

            switch (page.Angle)
            {
                case 0:
                    sx = x + dist;
                    sy = y + dist;
                    sw = (int)page.GetPageDrawW() - dist * 2;
                    sh = height;
                    break;
                case 90:
                    sx = x + dist;
                    sy = y + dist;
                    sw = height;
                    sh = (int)page.GetPageDrawH() - dist * 2;
                    break;
                case 180:
                    sx = x + dist;
                    sy = y + (int)page.GetPageDrawH() - dist - height;
                    sw = (int)page.GetPageDrawW() - dist * 2;
                    sh = height;

                    break;
                case 270:

                    sx = x + (int)page.GetPageDrawW() - dist - height;
                    sy = y + dist;
                    sw = height;
                    sh = (int)page.GetPageDrawH() - dist * 2;
                    break;

                default:
                    break;
            }
            g.FillRectangle(brush, new Rectangle(sx, sy, sw, sh));
            brush.Dispose();

            DrawText(g, page,sH);

        }

        private static void DrawText(Graphics g, TemplatePage page,int sH)
        {
            var drawFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var x = (float)(page.GetPageDrawX() + page.GetPageDrawW() / 2);
            var y = sH - page.GetPageDrawH() - page.GetPageDrawY() + page.GetPageDrawH() / 2;
            var state = g.Save();
            g.TranslateTransform(x, (float)y);

            if (page.Angle == 90)
            {
                g.RotateTransform(270);
            }
            else if (page.Angle == 270)
            {
                g.RotateTransform(90);
            }
            else if (page.Angle == 180)
            {
                g.RotateTransform(180);
            }

            var path = new GraphicsPath();
            Font font = new Font("Arial", 12);
            FontFamily family = font.FontFamily;
            Pen pen = new Pen(Color.Black);
            
            path.AddString($"{page.FrontIdx}•{page.BackIdx}", family, 0, 12, new PointF { X = 0, Y = 0 }, drawFormat);
            g.DrawPath(pen, path);
            family.Dispose();            
            pen.Dispose();
            font.Dispose();
            g.Restore(state);
        }
    }
}

