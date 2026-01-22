using Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models.ScreenPrimitives
{
    public class ScreenText : IScreenPrimitive
    {
        public string FontName { get; set; } = "Arial";
        public float FontSize { get; set; } = 6f;
        public FontStyle FontStyle { get; set; } = FontStyle.Bold;
        public System.Drawing.Color FontColor { get; set; } = System.Drawing.Color.Green;
        public string Text { get; set; } = string.Empty;
        public PointF Location { get; set; }

        public void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = new GraphicsPath())
            {

                using (Font font = new Font(FontName, FontSize, FontStyle))
                {
                    using (SolidBrush brush = new SolidBrush(FontColor))
                    {
                        using (Pen stroke = new Pen(System.Drawing.Color.White, 1))
                        {
                            stroke.LineJoin = LineJoin.Round;
                            path.AddString(
                                Text,
                                font.FontFamily,
                                (int)font.Style,
                                g.DpiY * font.Size / 72f,   // коректний розмір
                                Location,
                                new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });

                            g.DrawPath(stroke, path);  // контур
                            g.FillPath(brush, path);   // заливка
                        }
                    }
                }
            }
        }
    }
}