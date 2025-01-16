using JobSpace.Static.Pdf.Imposition.Drawers.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos;
using Krypton.Toolkit;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        ImposToolsParameters _toolParams;
        bool isDragMode = false;

        double hover_x;
        double hover_y;

        PointF clickPoint;
        PointF lastLocation;
        double snapDistance = 7;

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
            if (e.Button == MouseButtons.Right && _toolParams.CurTool == ImposToolEnum.Select)
            {
                parameters.SelectedPreviewPage = null;
            }
        }

        private void Pb_preview_MouseDown(object sender, MouseEventArgs e)
        {
            if (_hover == null) return;
            hover_x = _hover.X;
            hover_y = _hover.Y;

            if (_toolParams.CurTool == ImposToolEnum.Select)
            {
                isDragMode = true;
                clickPoint = e.Location;
                lastLocation = e.Location;

                if (ModifierKeys.HasFlag(Keys.Alt))
                {
                    var clonePage = _hover.Copy();
                    parameters.Sheet.TemplatePageContainer.AddPage(clonePage);
                    _hover = clonePage;
                }
            }
        }

        public void RedrawSheet()
        {
            if (pb_preview.Image != null) pb_preview.Image.Dispose();
            pb_preview.Image = null;
            if (parameters.Sheet == null) return;
            var screenDrawer = new ScreenDrawer();

            pb_preview.Width = (int)parameters.Sheet.W + 1;
            pb_preview.Height = (int)parameters.Sheet.H + 1;

            pb_preview.Image = screenDrawer.Draw(parameters.Sheet);


        }

        public void InitBindParameters(ImposToolsParameters parameters)
        {
            _toolParams = parameters;
            _toolParams.OnListNumberClick += OnToolsListNumberClick;
            _toolParams.OnTheSameNumberClick += OnTheSameNumberClick;
            _toolParams.OnCropMarksChanged += OnCropMarksChanged;
            imposToolsControl1.InitParameters(parameters);
        }

        private void OnCropMarksChanged(object sender, EventArgs e)
        {
            if (parameters.Sheet == null) return;

            parameters.Sheet.TemplatePageContainer.SetCropMarksLen(_toolParams.CropMarksParameters.Len);
            parameters.Sheet.TemplatePageContainer.SetCropMarksDistance(_toolParams.CropMarksParameters.Distance);

            parameters.UpdateSheet();

        }

        private void OnTheSameNumberClick(object sender, EventArgs e)
        {
            if (parameters.Sheet == null) return;
            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages)
            {
                if (parameters.Sheet is PrintSheet)
                {
                    page.PrintFrontIdx = _toolParams.FrontNum;
                }
                else
                {
                    page.MasterFrontIdx = _toolParams.FrontNum;

                }

                if (parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.Sheetwise)
                {
                    if (parameters.Sheet is PrintSheet)
                    {
                        page.PrintBackIdx = _toolParams.BackNum;
                    }
                    else
                    {
                        page.MasterBackIdx = _toolParams.BackNum;

                    }


                }
            }
            RedrawSheet();

        }

        private void OnToolsListNumberClick(object sender, EventArgs e)
        {
            if (parameters.Sheet == null) return;

            int front = _toolParams.FrontNum;
            int back = _toolParams.BackNum;

            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages)
            {
                if (parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.Sheetwise)
                {
                    if (parameters.Sheet is PrintSheet tsheet)
                    {
                        page.PrintFrontIdx = front;
                        page.PrintBackIdx = back;
                    }
                    else
                    {
                        page.MasterFrontIdx = front;
                        page.MasterBackIdx = back;


                    }

                    front += 2;
                    back += 2;
                }
                else
                {
                    if (parameters.Sheet is PrintSheet tsheet)
                    {
                        page.PrintFrontIdx = front++;
                    }
                    else
                    {
                        page.MasterFrontIdx = front++;

                    }
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

            //for x_snap
            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages.AsEnumerable().Reverse())
            {
                if (page == _hover) continue;

                var pageR = page.GetDrawBleedRight();
                var pageL = page.GetDrawBleedLeft();

                //// вибрана сторінка - права сторона <--> ліва сторона
                if (Math.Abs(pageR.X2 - hover_x - x_ofs) < snapDistance)
                {
                    _hover.X = page.X + page.GetClippedWByRotate();
                    break;
                }
                //// вибрана сторінка - ліва сторона сторона <--> права сторона
                else if (Math.Abs(hover_x + x_ofs + _hover.GetClippedWByRotate() - pageL.X1) < snapDistance)
                {

                    _hover.X = page.X - _hover.GetClippedWByRotate();
                    break;
                }
                // вибрана сторінка - ліва сторона <--> ліва сторона
                else if (Math.Abs(pageL.X1 - hover_x - x_ofs) < snapDistance)
                {

                    _hover.X = page.X;
                    break;
                }
                // вибрана сторінка - ліва сторона сторона <--> ліва сторона
                else if (Math.Abs(hover_x + x_ofs + _hover.GetClippedWByRotate() - pageR.X2) < snapDistance)
                {
                    _hover.X = page.X + page.GetClippedWByRotate() - _hover.GetClippedWByRotate();
                    break;
                }
            }


            //for y_snap
            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages.AsEnumerable().Reverse())
            {
                if (page == _hover) continue;

                var pageT = page.GetDrawBleedTop();
                var pageB = page.GetDrawBleedBottom();

                // top -> bottom
                if (Math.Abs(pageB.Y1 - hover_y + y_ofs - _hover.GetClippedHByRotate()) < snapDistance)
                {
                    _hover.Y = page.Y - _hover.GetClippedHByRotate();
                    break;
                }
                // bottom ->top
                else if (Math.Abs(pageT.Y2 - hover_y + y_ofs) < snapDistance)
                {
                    _hover.Y = page.Y + page.GetClippedHByRotate();
                    break;
                }
                // top -> top
                else if (Math.Abs(pageT.Y2 - hover_y + y_ofs - _hover.GetClippedHByRotate()) < snapDistance)
                {
                    _hover.Y = (page.Y + page.GetClippedHByRotate()) - _hover.GetClippedHByRotate();
                    break;
                }
                //bottom -> bottom
                else if (Math.Abs(pageB.Y1 - hover_y + y_ofs) < snapDistance)
                {
                    _hover.Y = page.Y;
                    break;
                }
            }
        }

        private void CheckHover(int x, int y)
        {
            _hover = null;

            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages.AsEnumerable().Reverse())
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
            if (parameters == null || parameters.Sheet == null) return;

            //e.Graphics.PageUnit = GraphicsUnit.Millimeter;

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
                if (_toolParams.CurTool == ImposToolEnum.FlipAngle)
                {
                    ToolFlipSinglePage();
                }
                else if (_toolParams.CurTool == ImposToolEnum.Numeration)
                {
                    ToolNumeringSinglePage();
                }
                else if (_toolParams.CurTool == ImposToolEnum.DeletePage)
                {
                    ToolDeletePage();
                }
                else if (_toolParams.CurTool == ImposToolEnum.Select)
                {
                    parameters.SelectedPreviewPage = _hover;
                }
                else if (_toolParams.CurTool == ImposToolEnum.SwitchHW)
                {
                    ToolSwitchWH();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (_toolParams.CurTool == ImposToolEnum.FlipAngle)
                {
                    ToolFlipPageRow();
                }
                if (_toolParams.CurTool == ImposToolEnum.Numeration)
                {
                    ToolNumericWithContinue();
                }
            }
        }

        private void ToolSwitchWH()
        {
            if (_hover == null) return;
            _hover.SwitchWH();
            parameters.CheckRunListPages();
            parameters.UpdateSheet();
        }

        private void ToolDeletePage()
        {
            parameters.Sheet.TemplatePageContainer.DeletePage(_hover);
            parameters.UpdateSheet();
        }

        private void ToolNumericWithContinue()
        {
            if (parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.SingleSide ||
                       parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.WorkAndTurn)
            {
                if (parameters.Sheet is PrintSheet)
                {
                    _hover.PrintFrontIdx = _toolParams.FrontNum++;
                    parameters.CheckRunListPages();
                }
                else
                {
                    _hover.MasterFrontIdx = _toolParams.FrontNum++;

                }

            }
            else
            {
                if (parameters.Sheet is PrintSheet)
                {
                    _hover.PrintFrontIdx = _toolParams.FrontNum;
                    _hover.PrintBackIdx = _toolParams.BackNum;
                    parameters.CheckRunListPages();
                }
                else
                {
                    _hover.MasterFrontIdx = _toolParams.FrontNum;
                    _hover.MasterBackIdx = _toolParams.BackNum;

                }
                _toolParams.FrontNum += 2;
                _toolParams.BackNum += 2;
            }

            RedrawSheet();
        }

        private void ToolFlipPageRow()
        {
            parameters.Sheet.TemplatePageContainer.FlipPagesAngle(_hover);
            LooseBindingSingleSide.FixBleedsFront(parameters.Sheet.TemplatePageContainer);
            RedrawSheet();

        }

        private void ToolNumeringSinglePage()
        {

            if (ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift))
            {
                if (parameters.Sheet is PrintSheet sheet)
                {
                    _hover.PrintFrontIdx = _toolParams.FrontNum;

                    parameters.CheckRunListPages();

                }
                else
                {
                    _hover.MasterFrontIdx = _toolParams.FrontNum;

                }

                _toolParams.FrontNum++;

            }
            else if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide ||
                    parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.WorkAndTurn)
                {

                    if (parameters.Sheet is PrintSheet sheet)
                    {

                        _hover.PrintBackIdx = _toolParams.FrontNum;
                        parameters.CheckRunListPages();
                    }
                    else
                    {
                        _hover.MasterBackIdx = _toolParams.FrontNum;

                    }
                    _toolParams.FrontNum++;
                }
            }
            else
            {
                if (parameters.Sheet is PrintSheet sheet)
                {
                    _hover.PrintFrontIdx = _toolParams.FrontNum;
                    parameters.CheckRunListPages();
                }
                else
                {
                    _hover.MasterFrontIdx = _toolParams.FrontNum;
                }

                if (parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide ||
                    parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.WorkAndTurn)
                {
                    if (parameters.Sheet is PrintSheet psheet)
                    {

                        _hover.PrintBackIdx = _toolParams.BackNum;
                        parameters.CheckRunListPages();
                    }
                    else
                    {
                        _hover.MasterBackIdx = _toolParams.BackNum;

                    }
                }
            }
            RedrawSheet();

        }

        private void ToolFlipSinglePage()
        {
            _hover.FlipAngle();
            LooseBindingSingleSide.FixBleedsFront(parameters.Sheet.TemplatePageContainer);
            RedrawSheet();
        }

        public void SetControlBindParameters(ControlBindParameters controlBindParameters)
        {
            parameters = controlBindParameters;
            parameters.PropertyChanged += Parameters_PropertyChanged;
            parameters.JustUpdatePreview += Parameters_JustUpdatePreview;
        }

        private void Parameters_JustUpdatePreview(object sender, EventArgs e)
        {
            RedrawSheet();
        }

        private void Parameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RedrawSheet();
        }
    }
}
