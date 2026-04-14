using BrightIdeasSoftware;
using JobSpace.Profiles;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ActiveWorks
{
    public sealed class JobRenderer : AbstractRenderer
    {

        public Profile UserProfile;

        public override bool RenderItem(DrawListViewItemEventArgs e, Graphics g, Rectangle itemBounds, object rowObject)
        {
            //// If we're in any other view than Tile, return false to say that we haven't done
            //// the rendereing and the default process should do it's stuff
            if (!(e.Item.ListView is ObjectListView olv) || olv.View != View.Tile)
                return false;

            //// Use buffered graphics to kill flickers
            var buffered = BufferedGraphicsManager.Current.Allocate(g, itemBounds);
            g = buffered.Graphics;
            g.Clear(olv.BackColor);
            g.SmoothingMode = ObjectListView.SmoothingMode;
            g.TextRenderingHint = ObjectListView.TextRenderingHint;

            if (e.Item.Selected)
            {
                BorderPen = Pens.Blue;
                HeaderBackBrush = new SolidBrush(olv.SelectedBackColorOrDefault);
            }
            else
            {
                BorderPen = new Pen(Color.FromArgb(0x33, 0x33, 0x33));
                HeaderBackBrush = new SolidBrush(Color.FromArgb(0x33, 0x33, 0x33));
            }
            DrawBusinessCard(g, itemBounds, rowObject, olv, (OLVListItem)e.Item);

            //// Finally render the buffered graphics
            buffered.Render();
            buffered.Dispose();

            //// Return true to say that we've handled the drawing
            return true;
        }

        internal Pen BorderPen = new Pen(Color.FromArgb(0x33, 0x33, 0x33));
        internal Brush TextBrush = new SolidBrush(Color.FromArgb(0x22, 0x22, 0x22));
        internal Brush HeaderTextBrush = Brushes.AliceBlue;
        internal Brush HeaderBackBrush = new SolidBrush(Color.FromArgb(0x33, 0x33, 0x33));
        internal Brush BackBrush = Brushes.LemonChiffon;

        public void DrawBusinessCard(Graphics g, Rectangle itemBounds, object rowObject, ObjectListView olv, OLVListItem item)
        {
            const int spacing = 8;

            //// Allow a border around the card
            itemBounds.Inflate(-2, -2);

            //// Draw card background
            const int rounding = 20;
            GraphicsPath path = GetRoundedRect(itemBounds, rounding);
            g.FillPath(BackBrush, path);
            g.DrawPath(BorderPen, path);

            g.Clip = new Region(itemBounds);

            if (rowObject is JobSpace.Job job)
            {

                //// Now draw the text portion
                RectangleF textBoxRect = itemBounds;
                textBoxRect.X += spacing;
                textBoxRect.Width = itemBounds.Right - textBoxRect.X - spacing;

                var fmt = new StringFormat(StringFormatFlags.NoWrap)
                {
                    Trimming = StringTrimming.EllipsisCharacter,
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Near
                };
                String txt = job.Customer;

                using (Font font = new Font("Tahoma", 11))
                {
                    //// Measure the height of the title
                    SizeF size = g.MeasureString(txt, font, (int)textBoxRect.Width, fmt);
                    //// Draw the title
                    RectangleF r3 = textBoxRect;
                    r3.Height = size.Height;
                    path = GetRoundedRect(r3, 15);
                    g.FillPath(HeaderBackBrush, path);
                    g.DrawString(txt, font, HeaderTextBrush, textBoxRect, fmt);
                    textBoxRect.Y += size.Height + spacing;
                }

                //// Draw the other bits of information
                using (Font font = new Font("Tahoma", 8))
                {
                    SizeF size = g.MeasureString("Wj", font, itemBounds.Width, fmt);
                    textBoxRect.Height = size.Height;
                    fmt.Alignment = StringAlignment.Near;
                    for (int i = 0; i < olv.Columns.Count; i++)
                    {
                        OLVColumn column = olv.GetColumn(i);
                        if (column.IsTileViewColumn)
                        {
                            txt = job.Description;
                            ////txt = column.GetStringValue(rowObject);
                            g.DrawString(txt, font, TextBrush, textBoxRect, fmt);
                            textBoxRect.Y += size.Height;
                        }
                    }
                }
                //// Draw the other bits of information
                using (Font font = new Font("Tahoma", 8))
                {
                    SizeF size = g.MeasureString("Wj", font, itemBounds.Width, fmt);
                    textBoxRect.Height = size.Height;
                    fmt.Alignment = StringAlignment.Near;
                    for (int i = 0; i < olv.Columns.Count; i++)
                    {
                        OLVColumn column = olv.GetColumn(i);
                        if (column.IsTileViewColumn)
                        {
                            //txt = UserProfile.Forms.GetPlannedFormForJob(job);
                            ////txt = column.GetStringValue(rowObject);
                            g.DrawString(txt, font, TextBrush, textBoxRect, fmt);
                            textBoxRect.Y += size.Height;
                        }
                    }
                }
            }
        }

        private GraphicsPath GetRoundedRect(RectangleF rect, float diameter)
        {
            GraphicsPath path = new GraphicsPath();

            RectangleF arc = new RectangleF(rect.X, rect.Y, diameter, diameter);
            path.AddArc(arc, 180, 90);
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
