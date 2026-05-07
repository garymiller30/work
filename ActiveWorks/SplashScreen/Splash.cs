using System;
using System.Drawing;
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
            private readonly PictureBox _image = new PictureBox();
            private readonly Label _version = new Label();
            private readonly Label _header = new Label();
            private readonly Label _status = new Label();

            public SplashForm()
            {
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.CenterScreen;
                ShowInTaskbar = false;
                TopMost = true;
                DoubleBuffered = true;
                BackColor = Color.Black;

                _image.Dock = DockStyle.Fill;
                _image.SizeMode = PictureBoxSizeMode.StretchImage;
                Controls.Add(_image);

                ConfigureOverlayLabel(_version);
                _version.AutoSize = true;

                ConfigureOverlayLabel(_header);
                _header.Font = new Font(Font.FontFamily, 18f, FontStyle.Bold);
                _header.TextAlign = ContentAlignment.TopLeft;

                ConfigureOverlayLabel(_status);
                _status.Font = new Font(Font.FontFamily, 11f, FontStyle.Regular);
                _status.TextAlign = ContentAlignment.BottomLeft;

                _image.Controls.Add(_version);
                _image.Controls.Add(_header);
                _image.Controls.Add(_status);
            }

            public void SetImage(Image image)
            {
                if (image == null)
                {
                    return;
                }

                _image.Image = image;
                ClientSize = image.Size;
                CenterToScreen();
                LayoutOverlayLabels();
            }

            public void SetVersion(string version, Color color, int x, int y)
            {
                _version.Text = version ?? string.Empty;
                _version.ForeColor = color;
                _version.Location = new Point(x, y);
                _version.BringToFront();
            }

            public void SetHeader(string header, ContentAlignment alignment)
            {
                _header.Text = header ?? string.Empty;
                _header.TextAlign = alignment;
                LayoutOverlayLabels();
                _header.BringToFront();
            }

            public void SetStatus(string status)
            {
                _status.Text = status ?? string.Empty;
                LayoutOverlayLabels();
                _status.BringToFront();
            }

            protected override void OnClientSizeChanged(EventArgs e)
            {
                base.OnClientSizeChanged(e);
                LayoutOverlayLabels();
            }

            private static void ConfigureOverlayLabel(Label label)
            {
                label.BackColor = Color.Transparent;
                label.ForeColor = Color.White;
                label.UseMnemonic = false;
            }

            private void LayoutOverlayLabels()
            {
                const int margin = 12;

                _header.Bounds = new Rectangle(
                    margin,
                    margin,
                    Math.Max(0, ClientSize.Width - margin * 2),
                    40);

                _status.Bounds = new Rectangle(
                    margin,
                    Math.Max(margin, ClientSize.Height - 40 - margin),
                    Math.Max(0, ClientSize.Width - margin * 2),
                    40);

                if (_header.TextAlign == ContentAlignment.TopRight ||
                    _header.TextAlign == ContentAlignment.MiddleRight ||
                    _header.TextAlign == ContentAlignment.BottomRight)
                {
                    _header.Left = margin;
                    _header.Width = Math.Max(0, ClientSize.Width - margin * 2);
                }
            }
        }
    }
}
