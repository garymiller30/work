using Amazon.Runtime.Internal.Util;
using Interfaces;
using JobSpace.Models;
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
        
        private bool _dragging = false;
        private Point _dragStartPoint;
        private Point _scrollStartPoint;

        PdfPreviewParameters _previewParameters = new PdfPreviewParameters();

        public bool FitToScreen
        {
            get => _previewParameters.FitToWindow; 
            
            
            set
            {
                _previewParameters.FitToWindow = value;
                UpdatePreviewLayout();
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
        /// <summary>
        /// Sets the image and updates its display size.
        /// </summary>
        /// <param name="img">The image to display.</param>
        /// <param name="w">The width of the image in device-independent units.</param>
        /// <param name="h">The height of the image in device-independent units.</param>
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
            g.ScaleTransform(_previewParameters.ZoomFactor, _previewParameters.ZoomFactor);
            
            g.DrawImage(image, 0, 0, size.Width, size.Height);
        }
        private void UpdatePreviewLayout()
        {
            if (image == null) return;

            float dpi = pb_preview.DeviceDpi;

            // розмір сторінки у пікселях
            float pageWpx = size.Width * dpi / 25.4f;
            float pageHpx = size.Height * dpi / 25.4f;

            if (_previewParameters.FitToWindow)
            {
                int availW = pb_preview.Parent.Width;
                int availH = pb_preview.Parent.Height;

                // масштаб
                float scaleX = availW / pageWpx;
                float scaleY = availH / pageHpx;

                 _previewParameters.ZoomFactor = Math.Min(scaleX, scaleY);
            }
            // при діленні перевірити чи є залишок. Якщо є - округлити в більшу сторону

            var newW = pageWpx * _previewParameters.ZoomFactor;
            var newH = pageHpx * _previewParameters.ZoomFactor;

            pb_preview.Width = (int)Math.Ceiling(newW);
            pb_preview.Height = (int)Math.Ceiling(newH); 

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

        public void Redraw()
        {
            pb_preview.Invalidate();
        }

        public void SetFitAndResetZoom(bool fit)
        {
            if (fit)
            {
                _previewParameters.FitToWindow = true;
            }
            else
            {
                _previewParameters.FitToWindow = false;
                _previewParameters.ZoomFactor = 1.0f;
            }
            UpdatePreviewLayout();
        }

        public void SetPreviewParameters(PdfPreviewParameters param)
        {
            _previewParameters = param;
            UpdatePreviewLayout();
        }

        private void pb_preview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            _dragging = true;
            pb_preview.Cursor = Cursors.Hand;
            // початкова позиція миші (у координатах panel)
            _dragStartPoint = panel1.PointToClient(Cursor.Position);

            // поточний скрол
            _scrollStartPoint = new Point(
                panel1.AutoScrollPosition.X,
                panel1.AutoScrollPosition.Y);
        }

        private void pb_preview_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_dragging)
                return;

            Point currentPoint = panel1.PointToClient(Cursor.Position);

            int dx = currentPoint.X - _dragStartPoint.X;
            int dy = currentPoint.Y - _dragStartPoint.Y;

            panel1.AutoScrollPosition = new Point(
                -_scrollStartPoint.X - dx,
                -_scrollStartPoint.Y - dy);
        }

        private void pb_preview_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _dragging = false;
                pb_preview.Cursor = Cursors.Default;
            }
        }
    }
}
