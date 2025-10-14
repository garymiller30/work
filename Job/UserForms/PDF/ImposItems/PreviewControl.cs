using Interfaces.Pdf.Imposition;
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

        //TemplatePage _hover;
        ImposToolsParameters _toolParams;

        bool isDragMode { get; set; } = false;

        double hover_x;
        double hover_y;

        PointF clickPoint;
        PointF lastLocation;
        double snapDistance = 7;

        public PreviewControl()
        {
            InitializeComponent();
            pb_preview.MouseWheel += Pb_preview_MouseWheel;
            pb_preview.MouseMove += pb_preview_MouseMove;
            pb_preview.Paint += pb_preview_Paint;
            pb_preview.MouseClick += pb_preview_MouseClick;
            pb_preview.MouseDown += Pb_preview_MouseDown;
            pb_preview.MouseUp += Pb_preview_MouseUp;

        }

        private void Pb_preview_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ScreenDrawer.ZoomFactor += 0.1;
            }
            else
            {
                if (ScreenDrawer.ZoomFactor > 1)
                    ScreenDrawer.ZoomFactor -= 0.1f;
            }
            RedrawSheet();

        }

        private void Pb_preview_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && _toolParams.CurTool == ImposToolEnum.Select)
            {
                parameters.SelectedPreviewPage = null;
            }
            else if (_toolParams.CurTool == ImposToolEnum.AddPageToGroup)
            {
                // вибрати всі сторінки, що входять в виділену область і задати їм групу
                if (e.Button == MouseButtons.Right)
                {
                    if (parameters.HoverPage != null)
                    {
                        parameters.HoverPage.Group = 0;
                    }
                }
                else
                {
                    float x = clickPoint.X < e.X ? clickPoint.X : e.X;
                    float y = clickPoint.Y < e.Y ? clickPoint.Y : e.Y;
                    float w = Math.Abs(e.X - clickPoint.X); ;
                    float h = Math.Abs(e.Y - clickPoint.Y); ;

                    var selectedRect = new RectangleF(
                        (float)(x / ScreenDrawer.ZoomFactor),
                        (float)(y / ScreenDrawer.ZoomFactor),
                        (float)(w / ScreenDrawer.ZoomFactor),
                        (float)(h / ScreenDrawer.ZoomFactor)
                        );
                    var selectedPages = parameters.Sheet.TemplatePageContainer.TemplatePages.Where(p =>
                    {
                        var (page_x, page_y, page_w, page_h) = ScreenDrawCommons.GetPageDraw(p, p.Front);
                        var pageRect = new RectangleF((float)page_x, (float)(parameters.Sheet.H - page_y - page_h), (float)page_w, (float)page_h);
                        return selectedRect.Contains(pageRect);
                    });
                    selectedPages.ToList().ForEach(p => p.Group = _toolParams.CurGroup);
                }

                parameters.UpdateSheet();
            }
            isDragMode = false;
        }

        private void Pb_preview_MouseDown(object sender, MouseEventArgs e)
        {
            if (_toolParams.CurTool == ImposToolEnum.AddPageToGroup)
            {
                isDragMode = true;
                clickPoint = e.Location;
                return;
            }

            if (parameters.HoverPage == null) return;

            hover_x = parameters.HoverPage.Front.X;
            hover_y = parameters.HoverPage.Front.Y;

            if (_toolParams.CurTool == ImposToolEnum.Select)
            {
                isDragMode = true;
                clickPoint = e.Location;
                lastLocation = e.Location;

                if (ModifierKeys.HasFlag(Keys.Alt))
                {
                    var clonePage = parameters.HoverPage.Copy();
                    parameters.Sheet.TemplatePageContainer.AddPage(clonePage);
                    parameters.HoverPage = clonePage;
                }
            }
        }

        public void RedrawSheet()
        {
            if (pb_preview.Image != null) pb_preview.Image.Dispose();
            pb_preview.Image = null;
            if (parameters.Sheet == null) return;

            pb_preview.Width = (int)((parameters.Sheet.W + 1) * ScreenDrawer.ZoomFactor);
            pb_preview.Height = (int)((parameters.Sheet.H + 1) * ScreenDrawer.ZoomFactor);

            pb_preview.Image = ScreenDrawer.Draw(parameters.Sheet);
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
            if (isDragMode == true && parameters.HoverPage != null && _toolParams.CurTool == ImposToolEnum.Select)
            {
                parameters.HoverPage.Front.X += e.X - lastLocation.X;
                parameters.HoverPage.Front.Y -= e.Y - lastLocation.Y;
                ProcessFixPageBackPosition.FixPosition(parameters.Sheet, parameters.HoverPage);

                lastLocation = e.Location;
                SnapPages();

                parameters.UpdateSheet(false);
            }
            else if (_toolParams.CurTool == ImposToolEnum.AddPageToGroup && isDragMode)
            {
                lastLocation = e.Location;
                pb_preview.Invalidate();
            }
            else
            {
                CheckHover((int)(e.X / ScreenDrawer.ZoomFactor), (int)(e.Y / ScreenDrawer.ZoomFactor));
            }
        }

        private void SnapPages()
        {
            var x_ofs = lastLocation.X - clickPoint.X;
            var y_ofs = lastLocation.Y - clickPoint.Y;

            //for x_snap
            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages.AsEnumerable().Reverse())
            {
                if (page == parameters.HoverPage) continue;

                var pageR = ScreenDrawCommons.GetDrawBleedRightFront(page);
                var pageL = ScreenDrawCommons.GetDrawBleedLeftFront(page);

                //// вибрана сторінка - права сторона <--> ліва сторона
                if (Math.Abs(pageR.X2 - hover_x - x_ofs) < snapDistance)
                {
                    parameters.HoverPage.Front.X = page.Front.X + page.GetClippedWByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, parameters.HoverPage);
                    break;
                }
                //// вибрана сторінка - ліва сторона сторона <--> права сторона
                else if (Math.Abs(hover_x + x_ofs + parameters.HoverPage.GetClippedWByRotate() - pageL.X1) < snapDistance)
                {

                    parameters.HoverPage.Front.X = page.Front.X - parameters.HoverPage.GetClippedWByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, parameters.HoverPage);
                    break;
                }
                // вибрана сторінка - ліва сторона <--> ліва сторона
                else if (Math.Abs(pageL.X1 - hover_x - x_ofs) < snapDistance)
                {
                    parameters.HoverPage.Front.X = page.Front.X;
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, parameters.HoverPage);
                    break;
                }
                // вибрана сторінка - ліва сторона сторона <--> ліва сторона
                else if (Math.Abs(hover_x + x_ofs + parameters.HoverPage.GetClippedWByRotate() - pageR.X2) < snapDistance)
                {
                    parameters.HoverPage.Front.X = page.Front.X + page.GetClippedWByRotate() - parameters.HoverPage.GetClippedWByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, parameters.HoverPage);
                    break;
                }
            }


            //for y_snap
            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages.AsEnumerable().Reverse())
            {
                if (page == parameters.HoverPage) continue;

                var pageT = ScreenDrawCommons.GetDrawBleedTopFront(page);
                var pageB = ScreenDrawCommons.GetDrawBleedBottomFront(page);

                // top -> bottom
                if (Math.Abs(pageB.Y1 - hover_y + y_ofs - parameters.HoverPage.GetClippedHByRotate()) < snapDistance)
                {
                    parameters.HoverPage.Front.Y = page.Front.Y - parameters.HoverPage.GetClippedHByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, parameters.HoverPage);
                    break;
                }
                // bottom ->top
                else if (Math.Abs(pageT.Y2 - hover_y + y_ofs) < snapDistance)
                {
                    parameters.HoverPage.Front.Y = page.Front.Y + page.GetClippedHByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, parameters.HoverPage);
                    break;
                }
                // top -> top
                else if (Math.Abs(pageT.Y2 - hover_y + y_ofs - parameters.HoverPage.GetClippedHByRotate()) < snapDistance)
                {
                    parameters.HoverPage.Front.Y = (page.Front.Y + page.GetClippedHByRotate()) - parameters.HoverPage.GetClippedHByRotate();
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, parameters.HoverPage);
                    break;
                }
                //bottom -> bottom
                else if (Math.Abs(pageB.Y1 - hover_y + y_ofs) < snapDistance)
                {
                    parameters.HoverPage.Front.Y = page.Front.Y;
                    ProcessFixPageBackPosition.FixPosition(parameters.Sheet, parameters.HoverPage);
                    break;
                }
            }
        }

        private void CheckHover(int x, int y)
        {
            parameters.HoverPage = null;

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
                    parameters.HoverPage = page;
                    break;
                }
            }
            pb_preview.Refresh();
        }

        private void pb_preview_Paint(object sender, PaintEventArgs e)
        {
            if (parameters == null || parameters.Sheet == null) return;

            if (parameters.SelectedPreviewPage != null)
            {
                Pen pen = new Pen(Color.IndianRed, 3);

                PageSide side = parameters.SelectedPreviewPage.Front;
                (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(parameters.SelectedPreviewPage, side);


                var rect = new RectangleF(
                    (float)page_x,
                    (float)(parameters.Sheet.H - page_y - page_h),
                    (float)page_w,
                    (float)page_h);

                ScreenDrawer.DrawRectangle(e.Graphics, rect, pen);
                pen.Dispose();
            }

            if (parameters.HoverPage != null)
            {
                (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(parameters.HoverPage, parameters.HoverPage.Front);

                Pen pen = new Pen(Color.Black, 2);

                var rect = new RectangleF(
                    (float)page_x,
                    (float)(parameters.Sheet.H - page_y - page_h),
                    (float)page_w,
                    (float)page_h);

                ScreenDrawer.DrawRectangle(e.Graphics, rect, pen);

                pen.Dispose();
            }

            if (_toolParams.CurTool == ImposToolEnum.AddPageToGroup && isDragMode)
            {

                float x = clickPoint.X < lastLocation.X ? clickPoint.X : lastLocation.X;
                float y = clickPoint.Y < lastLocation.Y ? clickPoint.Y : lastLocation.Y;
                float w = Math.Abs(lastLocation.X - clickPoint.X);
                float h = Math.Abs(lastLocation.Y - clickPoint.Y);

                using (Brush brush = new SolidBrush(Color.FromArgb(64, Color.Orange)))
                {
                    e.Graphics.FillRectangle(brush, x, y, w, h);
                }
            }
        }

        private void pb_preview_MouseClick(object sender, MouseEventArgs e)
        {
            if (parameters.HoverPage == null) return;

            if (e.Button == MouseButtons.Left)
            {
                if (_toolParams.CurTool == ImposToolEnum.FlipAngle)
                {
                    _toolParams.OnFlipAngle(this, parameters.HoverPage);
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
                    parameters.SelectedPreviewPage = parameters.HoverPage;
                }
                else if (_toolParams.CurTool == ImposToolEnum.SwitchHW)
                {
                    ToolSwitchWH();
                }
                else if (_toolParams.CurTool == ImposToolEnum.AddPageToGroup)
                {
                    _toolParams.OnAddPageToGroup(this, parameters.HoverPage);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (_toolParams.CurTool == ImposToolEnum.FlipAngle)
                {
                    _toolParams.OnFlipRowAngle(this, parameters.HoverPage);
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
            if (parameters.HoverPage == null) return;
            parameters.HoverPage.SwitchWH(parameters.Sheet.SheetPlaceType);
            parameters.CheckRunListPages();
            parameters.UpdateSheet();
        }

        private void ToolDeletePage()
        {
            parameters.Sheet.TemplatePageContainer.DeletePage(parameters.Sheet, parameters.HoverPage);
            parameters.UpdateSheet();
        }

        private void ToolNumericWithContinue()
        {
            if (parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.SingleSide ||
                       parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.WorkAndTurn)
            {
                if (parameters.Sheet is PrintSheet)
                {
                    _toolParams.FrontNum++;
                    parameters.HoverPage.Front.PrintIdx = _toolParams.FrontNum;
                    parameters.CheckRunListPages();
                }
                else
                {
                    _toolParams.FrontNum++;
                    parameters.HoverPage.Front.MasterIdx = _toolParams.FrontNum;
                }
            }
            else
            {
                _toolParams.FrontNum += 2;
                _toolParams.BackNum += 2;

                if (parameters.Sheet is PrintSheet)
                {
                    parameters.HoverPage.Front.PrintIdx = _toolParams.FrontNum;
                    parameters.HoverPage.Back.PrintIdx = _toolParams.BackNum;
                    parameters.CheckRunListPages();
                }
                else
                {
                    parameters.HoverPage.Front.MasterIdx = _toolParams.FrontNum;
                    parameters.HoverPage.Back.MasterIdx = _toolParams.BackNum;

                }
               
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
                    parameters.HoverPage.Front.PrintIdx = _toolParams.FrontNum;
                    parameters.CheckRunListPages();

                }
                else
                {
                    parameters.HoverPage.Front.MasterIdx = _toolParams.FrontNum;
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
                        parameters.HoverPage.Back.PrintIdx = _toolParams.FrontNum;
                        parameters.CheckRunListPages();
                    }
                    else
                    {
                        parameters.HoverPage.Back.MasterIdx = _toolParams.FrontNum;
                    }
                    _toolParams.FrontNum++;
                }
            }
            else
            {
                if (parameters.Sheet is PrintSheet sheet)
                {
                    parameters.HoverPage.Front.PrintIdx = _toolParams.FrontNum;
                    parameters.CheckRunListPages();
                }
                else
                {
                    parameters.HoverPage.Front.MasterIdx = _toolParams.FrontNum;
                }

                if (parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide ||
                    parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.WorkAndTurn)
                {
                    if (parameters.Sheet is PrintSheet psheet)
                    {

                        parameters.HoverPage.Back.PrintIdx = _toolParams.BackNum;
                        parameters.CheckRunListPages();
                    }
                    else
                    {
                        parameters.HoverPage.Back.MasterIdx = _toolParams.BackNum;

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
