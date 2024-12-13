using JobSpace.Static.Pdf.Imposition.Drawers.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos;
using Krypton.Toolkit;
using Org.BouncyCastle.Crypto.Parameters;
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
        ControlBindParameters parameters;

        //TemplateSheet _sheet;
        TemplatePage _hover;
        ImposToolsParameters _parameters;
        bool isDragMode = false;

        double hover_x;
        double hover_y;

        PointF clickPoint;
        PointF lastLocation;
        double snapDistance = 16;




        public PreviewControl()
        {
            InitializeComponent();
            pb_preview.MouseMove += pb_preview_MouseMove;
            pb_preview.Paint += pb_preview_Paint;
            pb_preview.MouseClick += pb_preview_MouseClick;
            pb_preview.MouseDown += Pb_preview_MouseDown;
            pb_preview.MouseUp += Pb_preview_MouseUp;

        }

        private void Pb_preview_MouseUp(object sender, MouseEventArgs e)
        {
            isDragMode = false;
        }

        private void Pb_preview_MouseDown(object sender, MouseEventArgs e)
        {
            if (_hover == null) return;
            hover_x = _hover.X;
            hover_y = _hover.Y;
            isDragMode = true;
            clickPoint = e.Location;
            lastLocation = e.Location;
        }


        public void SetSheet(TemplateSheet sheet)
        {
            //_sheet = sheet;
            //RedrawSheet();
        }

        public void RedrawSheet()
        {
            if (parameters.Sheet == null) return;

            var screenDrawer = new ScreenDrawer();

            if (pb_preview.Image != null) pb_preview.Image.Dispose();

            pb_preview.Width = (int)parameters.Sheet.W + 1;
            pb_preview.Height = (int)parameters.Sheet.H + 1;

            pb_preview.Image = screenDrawer.Draw(parameters.Sheet);
        }

        public void InitBindParameters(ImposToolsParameters parameters)
        {
            _parameters = parameters;
            _parameters.OnListNumberClick += OnToolsListNumberClick;
            _parameters.OnTheSameNumberClick += OnTheSameNumberClick;
            imposToolsControl1.InitParameters(parameters);
        }

        private void OnTheSameNumberClick(object sender, EventArgs e)
        {
            if (parameters.Sheet == null) return;
            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages)
            {
                page.FrontIdx = _parameters.FrontNum;
                if (parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.Sheetwise)
                {
                    page.BackIdx = _parameters.BackNum;
                }
            }
            RedrawSheet();

        }

        private void OnToolsListNumberClick(object sender, EventArgs e)
        {
            if (parameters.Sheet == null) return;

            int front = _parameters.FrontNum;
            int back = _parameters.BackNum;

            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages)
            {
                if (parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.Sheetwise)
                {
                    page.FrontIdx = front;
                    page.BackIdx = back;

                    front += 2;
                    back += 2;
                }
                else
                {
                    page.FrontIdx = front++;
                }
            }
            RedrawSheet();
        }

        private void pb_preview_MouseMove(object sender, MouseEventArgs e)
        {
            if (parameters.Sheet == null) return;
            if (isDragMode && _hover != null)
            {
                _hover.X += e.X - lastLocation.X;
                _hover.Y -= e.Y - lastLocation.Y;
                lastLocation = e.Location;
                SnapPages();

                parameters.UpdateSheet();
            }
            else
            {
                //tsl_coord.Text = $"x: {e.Location.X}, y: {_productPart.Sheet.H - e.Location.Y}";
                CheckHover(e.X, e.Y);
            }
        }

        private void SnapPages()
        {
            var x_ofs = lastLocation.X - clickPoint.X;
            var y_ofs = lastLocation.Y - clickPoint.Y;

            bool x_snap = false;
            bool y_snap = false;

            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages)
            {
                if (page == _hover) continue;

                if (!x_snap)
                {
                    var pageR = page.GetDrawBleedRight();
                    var pageL = page.GetDrawBleedLeft();
                    // вибрана сторінка - права сторона
                    if (Math.Abs(pageR.X2 - hover_x - x_ofs) < snapDistance)
                    {

                        x_snap = true;
                        _hover.X = page.X + page.GetClippedWByRotate();

                    }
                    // вибрана сторінка - ліва сторона сторона
                    else if (Math.Abs(hover_x + x_ofs + _hover.GetClippedWByRotate() - pageL.X1) < snapDistance)
                    {
                        x_snap = true;
                        _hover.X = pageL.X1 - _hover.GetClippedWByRotate();
                    }
                }

                if (!y_snap)
                {
                    var pageT = page.GetDrawBleedTop();
                    var pageB = page.GetDrawBleedBottom();

                    // вибрана сторінка - верх
                    if (Math.Abs(pageB.Y1 - hover_y - y_ofs - _hover.GetClippedHByRotate()) < snapDistance)
                    {
                        y_snap = true;
                        _hover.Y = pageB.Y1 - _hover.GetClippedHByRotate();
                    }
                }


                if (x_snap && y_snap) break;
            }
        }

        private void CheckHover(int x, int y)
        {
            _hover = null;

            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages)
            {
                RectangleF rect = new RectangleF
                {
                    X = (float)page.GetPageDrawX(),
                    Y = (float)(parameters.Sheet.H - page.GetPageDrawY() - page.GetPageDrawH()),
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
            if (parameters.Sheet == null) return;

            if (parameters.SelectedPreviewPage != null)
            {
                Pen pen = new Pen(Color.IndianRed, 3);

                e.Graphics.DrawRectangle(pen, new Rectangle(
                    (int)parameters.SelectedPreviewPage.GetPageDrawX(),
                    (int)parameters.Sheet.H - (int)parameters.SelectedPreviewPage.GetPageDrawY() - (int)parameters.SelectedPreviewPage.GetPageDrawH(),
                    (int)parameters.SelectedPreviewPage.GetPageDrawW(),
                    (int)parameters.SelectedPreviewPage.GetPageDrawH()));
                pen.Dispose();
            }

            if (_hover != null)
            {
                Pen pen = new Pen(Color.Black, 2);

                e.Graphics.DrawRectangle(pen, new Rectangle(
                    (int)_hover.GetPageDrawX(),
                    (int)parameters.Sheet.H - (int)_hover.GetPageDrawY() - (int)_hover.GetPageDrawH(),
                    (int)_hover.GetPageDrawW(),
                    (int)_hover.GetPageDrawH()));
                pen.Dispose();
            }

        }

        private void pb_preview_MouseClick(object sender, MouseEventArgs e)
        {
            if (_hover == null) return;

            if (e.Button == MouseButtons.Left)
            {
                if (_parameters.CurTool == ImposToolEnum.FlipAngle)
                {
                    ToolFlipSinglePage();
                }
                else if (_parameters.CurTool == ImposToolEnum.Numeration)
                {
                    ToolNumeringSinglePage();
                }
                else if (_parameters.CurTool == ImposToolEnum.DeletePage)
                {
                    ToolDeletePage();
                }
                else if (_parameters.CurTool == ImposToolEnum.Select)
                {
                    parameters.SelectedPreviewPage = _hover;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (_parameters.CurTool == ImposToolEnum.FlipAngle)
                {
                    ToolFlipPageRow();
                }
                if (_parameters.CurTool == ImposToolEnum.Numeration)
                {
                    ToolNumericWithContinue();
                }
            }
        }

        private void ToolDeletePage()
        {
            parameters.Sheet.TemplatePageContainer.DeletePage(parameters.SelectedPreviewPage);
            RedrawSheet();
        }

        private void ToolNumericWithContinue()
        {
            if (parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.SingleSide ||
                       parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.WorkAndTurn)
            {
                parameters.SelectedPreviewPage.FrontIdx = _parameters.FrontNum++;
            }
            else
            {
                parameters.SelectedPreviewPage.FrontIdx = _parameters.FrontNum;
                _parameters.FrontNum += 2;
                parameters.SelectedPreviewPage.BackIdx = _parameters.BackNum;
                _parameters.BackNum += 2;
            }

            RedrawSheet();
        }

        private void ToolFlipPageRow()
        {
            parameters.Sheet.TemplatePageContainer.FlipPagesAngle(parameters.SelectedPreviewPage);
            LooseBindingSingleSide.FixBleedsFront(parameters.Sheet.TemplatePageContainer);
            RedrawSheet();

        }

        private void ToolNumeringSinglePage()
        {

            if (ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift))
            {
                parameters.SelectedPreviewPage.FrontIdx = _parameters.FrontNum;
                _parameters.FrontNum++;

            }
            else if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide ||
                    parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.WorkAndTurn)
                {
                    parameters.SelectedPreviewPage.BackIdx = _parameters.FrontNum;
                    _parameters.FrontNum++;
                }
            }
            else
            {
                parameters.SelectedPreviewPage.FrontIdx = _parameters.FrontNum;

                if (parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide ||
                    parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.WorkAndTurn)
                {
                    parameters.SelectedPreviewPage.BackIdx = _parameters.BackNum;
                }
            }

            RedrawSheet();

        }

        private void ToolFlipSinglePage()
        {
            parameters.SelectedPreviewPage.FlipAngle();
            LooseBindingSingleSide.FixBleedsFront(parameters.Sheet.TemplatePageContainer);
            RedrawSheet();
        }

        public void SetControlBindParameters(ControlBindParameters controlBindParameters)
        {
            parameters = controlBindParameters;
            parameters.PropertyChanged += Parameters_PropertyChanged;
        }

        private void Parameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RedrawSheet();
        }
    }
}
