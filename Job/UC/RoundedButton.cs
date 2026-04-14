using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JobSpace.UC
{
    public sealed partial class RoundedButton : Button
    {
        public RoundedButton()
        {
            InitializeComponent();
        }

        public RoundedButton(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        GraphicsPath GetRoundPath(RectangleF rect, int radius)
        {
            float m = 2.75F;
            float r2 = radius / 2f;
            GraphicsPath GraphPath = new GraphicsPath();

            GraphPath.AddArc(rect.X + m, rect.Y + m, radius, radius, 180, 90);
            GraphPath.AddLine(rect.X + r2 + m, rect.Y + m, rect.Width - r2 - m, rect.Y + m);
            GraphPath.AddArc(rect.X + rect.Width - radius - m, rect.Y + m, radius, radius, 270, 90);
            GraphPath.AddLine(rect.Width - m, rect.Y + r2, rect.Width - m, rect.Height - r2 - m);
            GraphPath.AddArc(rect.X + rect.Width - radius - m,
                rect.Y + rect.Height - radius - m, radius, radius, 0, 90);
            GraphPath.AddLine(rect.Width - r2 - m, rect.Height - m, rect.X + r2 - m, rect.Height - m);
            GraphPath.AddArc(rect.X + m, rect.Y + rect.Height - radius - m, radius, radius, 90, 90);
            GraphPath.AddLine(rect.X + m, rect.Height - r2 - m, rect.X + m, rect.Y + r2 + m);

            GraphPath.CloseFigure();
            return GraphPath;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int borderRadius = 10;
            float borderThickness = 1.75f;
            base.OnPaint(e);
            RectangleF rect = new RectangleF(0, 0, this.Width, this.Height);
            GraphicsPath graphPath = GetRoundPath(rect, borderRadius);

            this.Region = new Region(graphPath);
            using (Pen pen = new Pen(Color.SteelBlue, borderThickness))
            {
                pen.Alignment = PenAlignment.Inset;
                e.Graphics.DrawPath(pen, graphPath);
            }
        }
    }
}
