using JobSpace.Static.Pdf.Imposition.Drawers.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class PreviewControl : UserControl
    {
        TemplateSheet _sheet;
        TemplatePage _hover;
        ImposToolsParameters _parameters;

        public PreviewControl()
        {
            InitializeComponent();
            pb_preview.MouseMove += pb_preview_MouseMove;
            pb_preview.Paint += pb_preview_Paint;
            pb_preview.MouseClick += pb_preview_MouseClick;

        }

        public void SetSheet(TemplateSheet sheet)
        {
            _sheet = sheet;
            RedrawSheet();
        }

        public void RedrawSheet()
        {
            var screenDrawer = new ScreenDrawer();

            if (pb_preview.Image != null) pb_preview.Image.Dispose();

            pb_preview.Width = (int)_sheet.W + 1;
            pb_preview.Height = (int)_sheet.H + 1;
            
            pb_preview.Image = screenDrawer.Draw(_sheet);
        }

        public void InitBindParameters(ImposToolsParameters parameters)
        {
            _parameters = parameters;
            imposToolsControl1.InitParameters(parameters);
        }

        private void pb_preview_MouseMove(object sender, MouseEventArgs e)
        {
            if (_sheet != null)
            {
                //tsl_coord.Text = $"x: {e.Location.X}, y: {_productPart.Sheet.H - e.Location.Y}";
                CheckHover(e.X, e.Y);
            }
        }

        private void CheckHover(int x, int y)
        {
            _hover = null;

            foreach (var page in _sheet.TemplatePageContainer.TemplatePages)
            {
                RectangleF rect = new RectangleF
                {
                    X = (float)page.GetPageDrawX(),
                    Y = (float)(_sheet.H - page.GetPageDrawY() - page.GetPageDrawH()),
                    Width = (float)page.GetPageDrawW(),
                    Height = (float)page.GetPageDrawH()
                };

                if (rect.Contains(x, y))
                {
                    _hover = page;
                    break;
                }
            }
            pb_preview.Refresh();
        }

        private void pb_preview_Paint(object sender, PaintEventArgs e)
        {
            if (_hover != null && _sheet != null)
            {
                Pen pen = new Pen(Color.Black, 3);

                e.Graphics.DrawRectangle(pen, new Rectangle(
                    (int)_hover.GetPageDrawX(),
                    (int)_sheet.H - (int)_hover.GetPageDrawY() - (int)_hover.GetPageDrawH(),
                    (int)_hover.GetPageDrawW(),
                    (int)_hover.GetPageDrawH()));
                pen.Dispose();
            }
        }

        private void pb_preview_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_parameters.IsFlipAngle)
                {
                    if (_hover != null)
                    {
                        _hover.FlipAngle();
                        RedrawSheet();
                    }
                }
                else if (_parameters.IsNumering)
                {
                    if (_hover != null)
                    {
                        _hover.FrontIdx = _parameters.FrontNum;

                        if (_sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide ||
                            _sheet.SheetPlaceType != TemplateSheetPlaceType.WorkAndTurn)
                        {
                            _hover.BackIdx = _parameters.BackNum;
                        }
                        RedrawSheet();
                    }
                }

            }
            else if (e.Button == MouseButtons.Right)
            {
                if (_parameters.IsFlipAngle)
                {
                    if (_hover != null)
                    {
                        _sheet.TemplatePageContainer.FlipPagesAngle(_hover);
                        RedrawSheet();
                    }
                }
                if (_parameters.IsNumering)
                {
                    if (_hover != null)
                    {
                        if (_sheet.SheetPlaceType == TemplateSheetPlaceType.SingleSide ||
                            _sheet.SheetPlaceType == TemplateSheetPlaceType.WorkAndTurn)
                        {
                            _hover.FrontIdx = _parameters.FrontNum++;
                        }
                        else

                        {
                            _hover.FrontIdx = _parameters.FrontNum;
                            _parameters.FrontNum += 2;
                            _hover.BackIdx = _parameters.BackNum;
                            _parameters.BackNum += 2;
                        }

                        RedrawSheet();
                    }
                }
            }
        }
    }
}
