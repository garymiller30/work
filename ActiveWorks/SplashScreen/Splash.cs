using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace SplashScreen
{
    public static class Splash
    {
        private static readonly object SyncRoot = new object();
        private static SplashForm _form;
        private static Thread _thread;
        private static string _status = string.Empty;

        public static void ShowSplashScreen()
        {
            lock (SyncRoot)
            {
                if (_form != null && !_form.IsDisposed)
                {
                    return;
                }
            }

            using (var ready = new ManualResetEventSlim(false))
            {
                var thread = new Thread(() =>
                {
                    Application.EnableVisualStyles();

                    var form = new SplashForm();
                    lock (SyncRoot)
                    {
                        if (_form != null && !_form.IsDisposed)
                        {
                            ready.Set();
                            return;
                        }

                        _form = form;
                    }

                    form.Load += (sender, args) => ready.Set();
                    Application.Run(form);

                    lock (SyncRoot)
                    {
                        if (ReferenceEquals(_form, form))
                        {
                            _form = null;
                        }
                    }
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true;
                thread.Name = "ActiveWorks splash screen";

                lock (SyncRoot)
                {
                    if (_form != null && !_form.IsDisposed)
                    {
                        return;
                    }

                    _thread = thread;
                }

                thread.Start();
                ready.Wait();
            }
        }

        public static void SetImage(Image image)
        {
            Invoke(form => form.SetImage(image));
        }

        public static void SetVersion(string version, Color color, int x, int y)
        {
            Invoke(form => form.SetVersion(version, color, x, y));
        }

        public static void SetHeader(string header)
        {
            SetHeader(header, ContentAlignment.TopLeft);
        }

        public static void SetHeader(string header, ContentAlignment alignment)
        {
            Invoke(form => form.SetHeader(header, alignment));
        }

        public static void SetStatus(string status)
        {
            _status = status ?? string.Empty;
            Invoke(form => form.SetStatus(_status));
        }

        public static string GetStatus()
        {
            return _status;
        }

        public static void CloseForm()
        {
            SplashForm form;
            lock (SyncRoot)
            {
                form = _form;
            }

            if (form == null || form.IsDisposed)
            {
                return;
            }

            try
            {
                if (form.InvokeRequired)
                {
                    form.BeginInvoke(new Action(form.Close));
                }
                else
                {
                    form.Close();
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (InvalidOperationException)
            {
            }
        }

        private static void Invoke(Action<SplashForm> action)
        {
            SplashForm form;
            lock (SyncRoot)
            {
                form = _form;
            }

            if (form == null || form.IsDisposed)
            {
                return;
            }

            try
            {
                if (form.InvokeRequired)
                {
                    form.BeginInvoke(new Action(() => action(form)));
                }
                else
                {
                    action(form);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (InvalidOperationException)
            {
            }
        }

        private sealed class SplashForm : Form
        {
            private readonly System.Windows.Forms.Timer _animationTimer = new System.Windows.Forms.Timer();
            private readonly Font _headerFont = new Font("Segoe UI", 13f, FontStyle.Bold);
            private readonly Font _statusFont = new Font("Segoe UI", 10f, FontStyle.Regular);
            private readonly Font _versionFont = new Font("Segoe UI Semibold", 9f, FontStyle.Regular);
            private Image _image;
            private string _version = string.Empty;
            private string _header = string.Empty;
            private string _status = string.Empty;
            private Color _versionColor = Color.FromArgb(255, 232, 150);
            private ContentAlignment _headerAlignment = ContentAlignment.TopLeft;
            private int _animationOffset;

            public SplashForm()
            {
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.CenterScreen;
                ShowInTaskbar = false;
                TopMost = true;
                DoubleBuffered = true;
                BackColor = Color.Black;
                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw,
                    true);

                _animationTimer.Interval = 35;
                _animationTimer.Tick += (sender, args) =>
                {
                    _animationOffset = (_animationOffset + 7) % Math.Max(1, ClientSize.Width);
                    Invalidate();
                };
                _animationTimer.Start();
            }

            public void SetImage(Image image)
            {
                if (image == null)
                {
                    return;
                }

                _image = image;
                ClientSize = image.Size;
                CenterToScreen();
                ApplyRoundedRegion();
                Invalidate();
            }

            public void SetVersion(string version, Color color, int x, int y)
            {
                _version = version ?? string.Empty;
                _versionColor = color;
                Invalidate();
            }

            public void SetHeader(string header, ContentAlignment alignment)
            {
                _header = header ?? string.Empty;
                _headerAlignment = alignment;
                Invalidate();
            }

            public void SetStatus(string status)
            {
                _status = status ?? string.Empty;
                Invalidate();
            }

            protected override void OnClientSizeChanged(EventArgs e)
            {
                base.OnClientSizeChanged(e);
                ApplyRoundedRegion();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                var graphics = e.Graphics;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                DrawBackground(graphics);
                DrawChrome(graphics);
                DrawHeader(graphics);
                DrawFooter(graphics);
                DrawProgress(graphics);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _animationTimer.Dispose();
                    _headerFont.Dispose();
                    _statusFont.Dispose();
                    _versionFont.Dispose();
                }

                base.Dispose(disposing);
            }

            private void DrawBackground(Graphics graphics)
            {
                var bounds = ClientRectangle;
                if (_image != null)
                {
                    graphics.DrawImage(_image, bounds);
                }
                else
                {
                    using (var brush = new LinearGradientBrush(bounds, Color.FromArgb(28, 30, 38), Color.FromArgb(116, 8, 32), 25f))
                    {
                        graphics.FillRectangle(brush, bounds);
                    }
                }

                using (var brush = new LinearGradientBrush(bounds, Color.FromArgb(15, Color.Black), Color.FromArgb(150, Color.Black), LinearGradientMode.Vertical))
                {
                    graphics.FillRectangle(brush, bounds);
                }

                var footer = new Rectangle(0, Math.Max(0, Height - 96), Width, 96);
                using (var brush = new LinearGradientBrush(footer, Color.FromArgb(0, Color.Black), Color.FromArgb(215, Color.Black), LinearGradientMode.Vertical))
                {
                    graphics.FillRectangle(brush, footer);
                }
            }

            private void DrawChrome(Graphics graphics)
            {
                var border = new Rectangle(0, 0, Width - 1, Height - 1);
                using (var path = CreateRoundedRectangle(border, 18))
                using (var pen = new Pen(Color.FromArgb(90, Color.White), 1f))
                {
                    graphics.DrawPath(pen, path);
                }
            }

            private void DrawHeader(Graphics graphics)
            {
                if (string.IsNullOrWhiteSpace(_header))
                {
                    return;
                }

                const int margin = 24;
                var size = TextRenderer.MeasureText(_header, _headerFont);
                var x = ResolveAlignedX(_headerAlignment, size.Width, margin);
                var y = margin;
                var pill = new Rectangle(x - 12, y - 7, size.Width + 24, size.Height + 12);

                using (var path = CreateRoundedRectangle(pill, 10))
                using (var brush = new SolidBrush(Color.FromArgb(125, 12, 14, 20)))
                {
                    graphics.FillPath(brush, path);
                }

                TextRenderer.DrawText(
                    graphics,
                    _header,
                    _headerFont,
                    new Point(x, y),
                    Color.White,
                    TextFormatFlags.NoPrefix);
            }

            private void DrawFooter(Graphics graphics)
            {
                const int margin = 24;
                var statusText = string.IsNullOrWhiteSpace(_status) ? "Завантаження..." : _status;
                var statusBounds = new Rectangle(margin, Height - 58, Math.Max(0, Width - margin * 2 - 115), 24);

                TextRenderer.DrawText(
                    graphics,
                    statusText,
                    _statusFont,
                    statusBounds,
                    Color.FromArgb(232, 236, 242),
                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter);

                if (!string.IsNullOrWhiteSpace(_version))
                {
                    var versionBounds = new Rectangle(Width - margin - 110, Height - 58, 110, 24);
                    TextRenderer.DrawText(
                        graphics,
                        "v" + _version,
                        _versionFont,
                        versionBounds,
                        _versionColor,
                        TextFormatFlags.Right | TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter);
                }
            }

            private void DrawProgress(Graphics graphics)
            {
                const int margin = 24;
                var track = new Rectangle(margin, Height - 25, Math.Max(0, Width - margin * 2), 4);

                using (var path = CreateRoundedRectangle(track, 2))
                using (var brush = new SolidBrush(Color.FromArgb(55, Color.White)))
                {
                    graphics.FillPath(brush, path);
                }

                var segmentWidth = Math.Max(80, track.Width / 3);
                var segmentX = track.Left + _animationOffset - segmentWidth;
                var segment = new Rectangle(segmentX, track.Top, segmentWidth, track.Height);

                graphics.SetClip(track);
                using (var brush = new LinearGradientBrush(segment, Color.FromArgb(0, _versionColor), Color.FromArgb(245, _versionColor), LinearGradientMode.Horizontal))
                using (var path = CreateRoundedRectangle(segment, 2))
                {
                    graphics.FillPath(brush, path);
                }

                graphics.ResetClip();
            }

            private int ResolveAlignedX(ContentAlignment alignment, int textWidth, int margin)
            {
                switch (alignment)
                {
                    case ContentAlignment.TopRight:
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.BottomRight:
                        return Math.Max(margin, Width - margin - textWidth);

                    case ContentAlignment.TopCenter:
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.BottomCenter:
                        return Math.Max(margin, (Width - textWidth) / 2);

                    default:
                        return margin;
                }
            }

            private void ApplyRoundedRegion()
            {
                if (ClientSize.Width <= 0 || ClientSize.Height <= 0)
                {
                    return;
                }

                var bounds = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
                using (var path = CreateRoundedRectangle(bounds, 18))
                {
                    var previousRegion = Region;
                    Region = new Region(path);
                    previousRegion?.Dispose();
                }
            }

            private static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
            {
                var path = new GraphicsPath();
                var diameter = radius * 2;
                var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

                path.AddArc(arc, 180, 90);
                arc.X = bounds.Right - diameter;
                path.AddArc(arc, 270, 90);
                arc.Y = bounds.Bottom - diameter;
                path.AddArc(arc, 0, 90);
                arc.X = bounds.Left;
                path.AddArc(arc, 90, 90);
                path.CloseFigure();

                return path;
            }
        }
    }
}
