using Amazon.Runtime.Internal.Util;
using Interfaces;
using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UC
{
    public partial class Uc_PreviewControl : UserControl
    {
        private Image image;
        private List<IScreenPrimitive> _primitives = new List<IScreenPrimitive>();
        private bool _fitToScreen = true;
        private float _zoomFactor = 1.0f;
        public float ZoomFactor
        {
            get => _zoomFactor; set
            {
                _zoomFactor = value;
                FitToScreen = false;
                
            }
        }
        public bool FitToScreen
        {
            get => _fitToScreen; set
            {
                _fitToScreen = value;
                UpdatePreviewLayout();
            }
        }

        public List<IScreenPrimitive> Primitives
        {
            get => _primitives; set
            {
                if (_primitives != null)
                {
                    foreach (var prim in _primitives)
                    {
                        if (prim is IDisposable disp)
                        {
                            disp.Dispose();
                        }
                    }
                }
                _primitives = value;
                pb_preview.Invalidate();
            }
        }

        private SizeF size = new SizeF(100, 100);
        private Image Image
        {
            get => image;
            set
            {
                if (image != null)
                {
                    image.Dispose();
                }

                if (value != null)
                {
                    image = new Bitmap(value);
                }
                else
                {
                    image = value;
                }
            }
        }

        public void SetImage(Image img, double w, double h)
        {
            Image = img;
            size = new SizeF((float)w, (float)h);
            UpdatePreviewLayout();
        }

        public Uc_PreviewControl()
        {
            InitializeComponent();
        }

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            UpdatePreviewLayout();
        }

        private void pb_preview_Paint(object sender, PaintEventArgs e)
        {
            if (image == null) return;

            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            g.ScaleTransform(ZoomFactor, ZoomFactor);

            g.DrawImage(image, 0, 0, size.Width, size.Height);

            DrawPrimitives(g);
        }

        private void DrawPrimitives(Graphics g)
        {
            foreach (var primitive in Primitives)
            {
                primitive.Draw(g);
            }
        }

        private void UpdatePreviewLayout()
        {
            if (image == null) return;

            float dpi = pb_preview.DeviceDpi;

            // розмір сторінки у пікселях
            float pageWpx = size.Width * dpi / 25.4f;
            float pageHpx = size.Height * dpi / 25.4f;

            if (FitToScreen)
            {
                float availW = pb_preview.Parent.Width;
                float availH = pb_preview.Parent.Height;

                // масштаб
                float scaleX = availW / pageWpx;
                float scaleY = availH / pageHpx;

                _zoomFactor = Math.Min(scaleX, scaleY);

                // нові розміри PictureBox
                int newW = (int)(pageWpx * _zoomFactor) - 1;
                int newH = (int)(pageHpx * _zoomFactor) - 1;

                pb_preview.Width = newW;
                pb_preview.Height = newH;
            }
            else
            {
                // масштаб 1:1
                //_zoomFactor = 1.0f;

                int newW = (int)(pageWpx * _zoomFactor) -1;
                int newH = (int)(pageHpx * _zoomFactor) -1;

                pb_preview.Width = newW;
                pb_preview.Height = newH;

                pb_preview.Left = 0;
                pb_preview.Top = 0;
            }

            pb_preview.Invalidate();
        }

        public void StartWait(string animationFile)
        {
            Image = null;

            pb_preview.SizeMode = PictureBoxSizeMode.CenterImage;
            // Спершу показуємо анімований GIF
            pb_preview.ImageLocation = animationFile;
        }

        public void StopWait()
        {
            pb_preview.ImageLocation = null;
            pb_preview.SizeMode = PictureBoxSizeMode.Normal;
        }

        internal void Redraw()
        {
            pb_preview.Invalidate();
        }
    }
}
