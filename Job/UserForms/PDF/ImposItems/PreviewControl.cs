using JobSpace.Static.Pdf.Imposition.Drawers.Screen;
using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Processes;
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
            hover_x = _hover.Front.X;
            hover_y = _hover.Front.Y;

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
                    page.Front.PrintIdx = _toolParams.FrontNum;
                }
                else
                {
                    page.Front.MasterIdx = _toolParams.FrontNum;

                }

                if (parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.Sheetwise)
                {
                    if (parameters.Sheet is PrintSheet)
                    {
                        page.Back.PrintIdx = _toolParams.BackNum;
                    }
                    else
                    {
                        page.Back.MasterIdx = _toolParams.BackNum;
                    }
                }
            }
            parameters.CheckRunListPages();
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
                        page.Front.PrintIdx = front;
                        page.Back.PrintIdx = back;
                    }
                    else
                    {
                        page.Front.MasterIdx = front;
                        page.Back.MasterIdx = back;


                    }

                    front += 2;
                    back += 2;
                }
                else
                {
                    if (parameters.Sheet is PrintSheet tsheet)
                    {
                        page.Front.PrintIdx = front++;
                    }
                    else
                    {
                        page.Front.MasterIdx = front++;

                    }
                }
            }
            parameters.CheckRunListPages();
            RedrawSheet();
        }

        private void pb_preview_MouseMove(object sender, MouseEventArgs e)
        {
            if (parameters.Sheet == null) return;
            if (isDragMode && _hover != null)
            {
                _hover.Front.X += e.X - lastLocation.X;
                _hover.Front.Y -= e.Y - lastLocation.Y;
                ProcessFixPageBackPosition.FixPosition(parameters.Sheet, _hover);

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

            //for x_snap
            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages.AsEnumerable().Reverse())
            {
                if (page == _hover) continue;

                var pageR = ScreenDrawCommons.GetDrawBleedRightFront(page);
                var pageL = ScreenDrawCommons.GetDrawBleedLeftFront(page);

                //// вибрана сторінка - права сторона <--> ліва сторона
                if (Math.Abs(pageR.X2 - hover_x - x_ofs) < snapDistance)
                {
                    _hover.Front.X = page.Front.X + page.GetClippedWByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, _hover);
                    break;
                }
                //// вибрана сторінка - ліва сторона сторона <--> права сторона
                else if (Math.Abs(hover_x + x_ofs + _hover.GetClippedWByRotate() - pageL.X1) < snapDistance)
                {

                    _hover.Front.X = page.Front.X - _hover.GetClippedWByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, _hover);
                    break;
                }
                // вибрана сторінка - ліва сторона <--> ліва сторона
                else if (Math.Abs(pageL.X1 - hover_x - x_ofs) < snapDistance)
                {
                    _hover.Front.X = page.Front.X;
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, _hover);
                    break;
                }
                // вибрана сторінка - ліва сторона сторона <--> ліва сторона
                else if (Math.Abs(hover_x + x_ofs + _hover.GetClippedWByRotate() - pageR.X2) < snapDistance)
                {
                    _hover.Front.X = page.Front.X + page.GetClippedWByRotate() - _hover.GetClippedWByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, _hover);
                    break;
                }
            }


            //for y_snap
            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages.AsEnumerable().Reverse())
            {
                if (page == _hover) continue;

                var pageT = ScreenDrawCommons.GetDrawBleedTopFront(page);
                var pageB = ScreenDrawCommons.GetDrawBleedBottomFront(page);

                // top -> bottom
                if (Math.Abs(pageB.Y1 - hover_y + y_ofs - _hover.GetClippedHByRotate()) < snapDistance)
                {
                    _hover.Front.Y = page.Front.Y - _hover.GetClippedHByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, _hover);
                    break;
                }
                // bottom ->top
                else if (Math.Abs(pageT.Y2 - hover_y + y_ofs) < snapDistance)
                {
                    _hover.Front.Y = page.Front.Y + page.GetClippedHByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, _hover);
                    break;
                }
                // top -> top
                else if (Math.Abs(pageT.Y2 - hover_y + y_ofs - _hover.GetClippedHByRotate()) < snapDistance)
                {
                    _hover.Front.Y = (page.Front.Y + page.GetClippedHByRotate()) - _hover.GetClippedHByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, _hover);
                    break;
                }
                //bottom -> bottom
                else if (Math.Abs(pageB.Y1 - hover_y + y_ofs) < snapDistance)
                {
                    _hover.Front.Y = page.Front.Y;
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, _hover);
                    break;
                }
            }
        }

        private void CheckHover(int x, int y)
        {
            _hover = null;

            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages.AsEnumerable().Reverse())
            {
                PageSide side = page.Front;
                (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(page, side);

                RectangleF rect = new RectangleF
                {
                    X = (float)page_x,
                    Y = (float)(parameters.Sheet.H - page_y - page_h),
                    Width = (float)page_w,
                    Height = (float)page_h
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

                PageSide side = parameters.SelectedPreviewPage.Front;
                (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(parameters.SelectedPreviewPage, side);

                e.Graphics.DrawRectangle(pen, new Rectangle(
                    (int)page_x,
                    (int)parameters.Sheet.H - (int)page_y - (int)page_h,
                    (int)page_w,
                    (int)page_h));
                pen.Dispose();
            }

            if (_hover != null)
            {
                (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(_hover, _hover.Front);

                Pen pen = new Pen(Color.Black, 2);

                e.Graphics.DrawRectangle(pen, new Rectangle(
                    (int)page_x,
                    (int)parameters.Sheet.H - (int)page_y - (int)page_h,
                    (int)page_w,
                    (int)page_h));
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
                    _toolParams.OnFlipAngle(this,_hover);
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
                    _toolParams.OnFlipRowAngle(this, _hover);
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
                    _hover.Front.PrintIdx = _toolParams.FrontNum++;
                    parameters.CheckRunListPages();
                }
                else
                {
                    _hover.Front.MasterIdx = _toolParams.FrontNum++;
                }
            }
            else
            {
                if (parameters.Sheet is PrintSheet)
                {
                    _hover.Front.PrintIdx = _toolParams.FrontNum;
                    _hover.Back.PrintIdx = _toolParams.BackNum;
                    parameters.CheckRunListPages();
                }
                else
                {
                    _hover.Front.MasterIdx = _toolParams.FrontNum;
                    _hover.Back.MasterIdx = _toolParams.BackNum;

                }
                _toolParams.FrontNum += 2;
                _toolParams.BackNum += 2;
            }

            RedrawSheet();
        }

        private void ToolFlipPageRow()
        {
            
            RedrawSheet();

        }

        private void ToolNumeringSinglePage()
        {

            if (ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift))
            {
                if (parameters.Sheet is PrintSheet sheet)
                {
                    _hover.Front.PrintIdx = _toolParams.FrontNum;
                    parameters.CheckRunListPages();

                }
                else
                {
                    _hover.Front.MasterIdx = _toolParams.FrontNum;
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
                        _hover.Back.PrintIdx = _toolParams.FrontNum;
                        parameters.CheckRunListPages();
                    }
                    else
                    {
                        _hover.Back.MasterIdx = _toolParams.FrontNum;
                    }
                    _toolParams.FrontNum++;
                }
            }
            else
            {
                if (parameters.Sheet is PrintSheet sheet)
                {
                    _hover.Front.PrintIdx = _toolParams.FrontNum;
                    parameters.CheckRunListPages();
                }
                else
                {
                    _hover.Front.MasterIdx = _toolParams.FrontNum;
                }

                if (parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide ||
                    parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.WorkAndTurn)
                {
                    if (parameters.Sheet is PrintSheet psheet)
                    {

                        _hover.Back.PrintIdx = _toolParams.BackNum;
                        parameters.CheckRunListPages();
                    }
                    else
                    {
                        _hover.Back.MasterIdx = _toolParams.BackNum;

                    }
                }
            }
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
            if (_toolParams.CurTool == ImposToolEnum.Numeration && e.PropertyName == "SelectedImposRunPageIdx")
            {
                _toolParams.FrontNum = parameters.SelectedImposRunPageIdx;
            }


            RedrawSheet();
        }
    }
}
