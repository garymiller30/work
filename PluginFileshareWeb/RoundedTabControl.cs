using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class RoundedTabControl : TabControl
{
    private List<Rectangle> tabRects = new List<Rectangle>();
    private int hoverIndex = -1;

    public RoundedTabControl()
    {
        SetStyle(ControlStyles.UserPaint, true);
        DoubleBuffered = true;

        Appearance = TabAppearance.FlatButtons;
        ItemSize = new Size(0, 1);
        SizeMode = TabSizeMode.Fixed;
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        const int WM_PAINT = 0x000F;

        if (m.Msg == WM_PAINT)
        {
            using (Graphics g = CreateGraphics())
            {
                DrawTabs(g);
            }
        }
    }

    private void DrawTabs(Graphics g)
    {
        tabRects.Clear();

        int x = 4;
        int y = 2;
        int height = 28;

        for (int i = 0; i < TabPages.Count; i++)
        {
            var page = TabPages[i];

            Size textSize = TextRenderer.MeasureText(page.Text, Font);
            int width = textSize.Width + 40;

            Rectangle rect = new Rectangle(x, y, width, height);
            tabRects.Add(rect);

            bool selected = SelectedIndex == i;

            Color back =
                selected ? Color.White :
                (hoverIndex == i ? Color.FromArgb(230, 230, 230) : Color.FromArgb(210, 210, 210));

            using (Brush b = new SolidBrush(back))
                g.FillRectangle(b, rect);

            g.DrawRectangle(Pens.Gray, rect);

            Rectangle textRect = new Rectangle(
                rect.Left + 10,
                rect.Top,
                rect.Width - 20,
                rect.Height);

            TextRenderer.DrawText(
                g,
                page.Text,
                Font,
                textRect,
                Color.Black,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            x += width + 4;
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        int newHover = -1;

        for (int i = 0; i < tabRects.Count; i++)
        {
            if (tabRects[i].Contains(e.Location))
            {
                newHover = i;
                break;
            }
        }

        if (hoverIndex != newHover)
        {
            hoverIndex = newHover;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        hoverIndex = -1;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);

        for (int i = 0; i < tabRects.Count; i++)
        {
            if (tabRects[i].Contains(e.Location))
            {
                SelectedIndex = i;
                Invalidate();
                break;
            }
        }
    }
}