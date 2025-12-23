using Interfaces.Pdf.Imposition;
using JobSpace.Static.Pdf.Imposition;
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
        GlobalImposParameters _imposParam;

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
            if (e.Button == MouseButtons.Right && _imposParam.ImposTools.CurTool == ImposToolEnum.Select)
            {
                _imposParam.ControlsBind.SelectedPreviewPage = null;
            }
            else if (_imposParam.ImposTools.CurTool == ImposToolEnum.AddPageToGroup)
            {
                // вибрати всі сторінки, що входять в виділену область і задати їм групу
                if (e.Button == MouseButtons.Right)
                {
                    if (_imposParam.ControlsBind.HoverPage != null)
                    {
                        _imposParam.ControlsBind.HoverPage.Group = 0;
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
                    var selectedPages = _imposParam.ControlsBind.Sheet.TemplatePageContainer.TemplatePages.Where(p =>
                    {
                        var (page_x, page_y, page_w, page_h) = ScreenDrawCommons.GetPageDraw(p, p.Front);
                        var pageRect = new RectangleF((float)page_x, (float)(_imposParam.ControlsBind.Sheet.H - page_y - page_h), (float)page_w, (float)page_h);
                        return selectedRect.Contains(pageRect);
                    });
                    selectedPages.ToList().ForEach(p => p.Group = _imposParam.ImposTools.CurGroup);
                }

                _imposParam.ControlsBind.UpdateSheet();
            }
            isDragMode = false;
        }

        private void Pb_preview_MouseDown(object sender, MouseEventArgs e)
        {
            if (_imposParam.ImposTools.CurTool == ImposToolEnum.AddPageToGroup)
            {
                isDragMode = true;
                clickPoint = e.Location;
                return;
            }

            if (_imposParam.ControlsBind.HoverPage == null) return;

            hover_x = _imposParam.ControlsBind.HoverPage.Front.X;
            hover_y = _imposParam.ControlsBind.HoverPage.Front.Y;

            if (_imposParam.ImposTools.CurTool == ImposToolEnum.Select)
            {
                isDragMode = true;
                clickPoint = e.Location;
                lastLocation = e.Location;

                if (ModifierKeys.HasFlag(Keys.Alt))
                {
                    var clonePage = _imposParam.ControlsBind.HoverPage.Copy();
                    _imposParam.ControlsBind.Sheet.TemplatePageContainer.AddPage(clonePage);
                    _imposParam.ControlsBind.HoverPage = clonePage;
                }
            }
        }

        public void RedrawSheet()
        {
            if (pb_preview.Image != null) pb_preview.Image.Dispose();
            pb_preview.Image = null;
            if (_imposParam.ControlsBind.Sheet == null) return;

            pb_preview.Width = (int)((_imposParam.ControlsBind.Sheet.W + 1) * ScreenDrawer.ZoomFactor);
            pb_preview.Height = (int)((_imposParam.ControlsBind.Sheet.H + 1) * ScreenDrawer.ZoomFactor);

            pb_preview.Image = ScreenDrawer.Draw(_imposParam.ControlsBind.Sheet);
        }

        private void OnCropMarksChanged(object sender, EventArgs e)
        {
            if (_imposParam.ControlsBind.Sheet == null) return;

            _imposParam.ControlsBind.Sheet.TemplatePageContainer.SetCropMarksLen(_imposParam.ImposTools.CropMarksParameters.Len);
            _imposParam.ControlsBind.Sheet.TemplatePageContainer.SetCropMarksDistance(_imposParam.ImposTools.CropMarksParameters.Distance);

            _imposParam.ControlsBind.UpdateSheet();

        }

        private void OnTheSameNumberClick(object sender, EventArgs e)
        {
            if (_imposParam.ControlsBind.Sheet == null) return;
            foreach (var page in _imposParam.ControlsBind.Sheet.TemplatePageContainer.TemplatePages)
            {
                if (_imposParam.ControlsBind.Sheet is PrintSheet)
                {
                    page.Front.PrintIdx = _imposParam.ImposTools.FrontNum;
                }
                else
                {
                    page.Front.MasterIdx = _imposParam.ImposTools.FrontNum;

                }

                if (_imposParam.ControlsBind.Sheet.SheetPlaceType == TemplateSheetPlaceType.Sheetwise)
                {
                    if (_imposParam.ControlsBind.Sheet is PrintSheet)
                    {
                        page.Back.PrintIdx = _imposParam.ImposTools.BackNum;
                    }
                    else
                    {
                        page.Back.MasterIdx = _imposParam.ImposTools.BackNum;
                    }
                }
            }
            _imposParam.ControlsBind.CheckRunListPages();
            RedrawSheet();
        }

        private void OnToolsListNumberClick(object sender, EventArgs e)
        {
            if (_imposParam.ControlsBind.Sheet == null) return;

            int front = _imposParam.ImposTools.FrontNum;
            int back = _imposParam.ImposTools.BackNum;

            foreach (var page in _imposParam.ControlsBind.Sheet.TemplatePageContainer.TemplatePages)
            {
                if (_imposParam.ControlsBind.Sheet.SheetPlaceType == TemplateSheetPlaceType.Sheetwise)
                {
                    if (_imposParam.ControlsBind.Sheet is PrintSheet tsheet)
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
                    if (_imposParam.ControlsBind.Sheet is PrintSheet tsheet)
                    {
                        page.Front.PrintIdx = front++;
                    }
                    else
                    {
                        page.Front.MasterIdx = front++;

                    }
                }
            }
            _imposParam.ControlsBind.CheckRunListPages();
            RedrawSheet();
        }

        private void pb_preview_MouseMove(object sender, MouseEventArgs e)
        {
            if (_imposParam.ControlsBind.Sheet == null) return;
            if (isDragMode == true && _imposParam.ControlsBind.HoverPage != null && _imposParam.ImposTools.CurTool == ImposToolEnum.Select)
            {
                _imposParam.ControlsBind.HoverPage.Front.X += e.X - lastLocation.X;
                _imposParam.ControlsBind.HoverPage.Front.Y -= e.Y - lastLocation.Y;
                ProcessFixPageBackPosition.FixPosition(_imposParam.ControlsBind.Sheet, _imposParam.ControlsBind.HoverPage);

                lastLocation = e.Location;
                SnapPages();

                _imposParam.ControlsBind.UpdateSheet(false);
            }
            else if (_imposParam.ImposTools.CurTool == ImposToolEnum.AddPageToGroup && isDragMode)
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
            foreach (var page in _imposParam.ControlsBind.Sheet.TemplatePageContainer.TemplatePages.AsEnumerable().Reverse())
            {
                if (page == _imposParam.ControlsBind.HoverPage) continue;

                var pageR = ScreenDrawCommons.GetDrawBleedRightFront(page);
                var pageL = ScreenDrawCommons.GetDrawBleedLeftFront(page);

                //// вибрана сторінка - права сторона <--> ліва сторона
                if (Math.Abs(pageR.X2 - hover_x - x_ofs) < snapDistance)
                {
                    _imposParam.ControlsBind.HoverPage.Front.X = page.Front.X + page.GetClippedWByRotate();
                    ProcessFixPageBackPosition.FixPosition(_imposParam.ControlsBind.Sheet, _imposParam.ControlsBind.HoverPage);
                    break;
                }
                //// вибрана сторінка - ліва сторона сторона <--> права сторона
                else if (Math.Abs(hover_x + x_ofs + _imposParam.ControlsBind.HoverPage.GetClippedWByRotate() - pageL.X1) < snapDistance)
                {

                    _imposParam.ControlsBind.HoverPage.Front.X = page.Front.X - _imposParam.ControlsBind.HoverPage.GetClippedWByRotate();
                    ProcessFixPageBackPosition.FixPosition(_imposParam.ControlsBind.Sheet, _imposParam.ControlsBind.HoverPage);
                    break;
                }
                // вибрана сторінка - ліва сторона <--> ліва сторона
                else if (Math.Abs(pageL.X1 - hover_x - x_ofs) < snapDistance)
                {
                    _imposParam.ControlsBind.HoverPage.Front.X = page.Front.X;
                    ProcessFixPageBackPosition.FixPosition(_imposParam.ControlsBind.Sheet, _imposParam.ControlsBind.HoverPage);
                    break;
                }
                // вибрана сторінка - ліва сторона сторона <--> ліва сторона
                else if (Math.Abs(hover_x + x_ofs + _imposParam.ControlsBind.HoverPage.GetClippedWByRotate() - pageR.X2) < snapDistance)
                {
                    _imposParam.ControlsBind.HoverPage.Front.X = page.Front.X + page.GetClippedWByRotate() - _imposParam.ControlsBind.HoverPage.GetClippedWByRotate();
                    ProcessFixPageBackPosition.FixPosition(_imposParam.ControlsBind.Sheet, _imposParam.ControlsBind.HoverPage);
                    break;
                }
            }


            //for y_snap
            foreach (var page in _imposParam.ControlsBind.Sheet.TemplatePageContainer.TemplatePages.AsEnumerable().Reverse())
            {
                if (page == _imposParam.ControlsBind.HoverPage) continue;

                var pageT = ScreenDrawCommons.GetDrawBleedTopFront(page);
                var pageB = ScreenDrawCommons.GetDrawBleedBottomFront(page);

                // top -> bottom
                if (Math.Abs(pageB.Y1 - hover_y + y_ofs - _imposParam.ControlsBind.HoverPage.GetClippedHByRotate()) < snapDistance)
                {
                    _imposParam.ControlsBind.HoverPage.Front.Y = page.Front.Y - _imposParam.ControlsBind.HoverPage.GetClippedHByRotate();
                    ProcessFixPageBackPosition.FixPosition(_imposParam.ControlsBind.Sheet, _imposParam.ControlsBind.HoverPage);
                    break;
                }
                // bottom ->top
                else if (Math.Abs(pageT.Y2 - hover_y + y_ofs) < snapDistance)
                {
                    _imposParam.ControlsBind.HoverPage.Front.Y = page.Front.Y + page.GetClippedHByRotate();
                    ProcessFixPageBackPosition.FixPosition(_imposParam.ControlsBind.Sheet, _imposParam.ControlsBind.HoverPage);
                    break;
                }
                // top -> top
                else if (Math.Abs(pageT.Y2 - hover_y + y_ofs - _imposParam.ControlsBind.HoverPage.GetClippedHByRotate()) < snapDistance)
                {
                    _imposParam.ControlsBind.HoverPage.Front.Y = (page.Front.Y + page.GetClippedHByRotate()) - _imposParam.ControlsBind.HoverPage.GetClippedHByRotate();
                    ProcessFixPageBackPosition.FixPosition(_imposParam.ControlsBind.Sheet, _imposParam.ControlsBind.HoverPage);
                    break;
                }
                //bottom -> bottom
                else if (Math.Abs(pageB.Y1 - hover_y + y_ofs) < snapDistance)
                {
                    _imposParam.ControlsBind.HoverPage.Front.Y = page.Front.Y;
                    ProcessFixPageBackPosition.FixPosition(_imposParam.ControlsBind.Sheet, _imposParam.ControlsBind.HoverPage);
                    break;
                }
            }
        }

        private void CheckHover(int x, int y)
        {
            _imposParam.ControlsBind.HoverPage = null;

            foreach (var page in _imposParam.ControlsBind.Sheet.TemplatePageContainer.TemplatePages.AsEnumerable().Reverse())
            {
                PageSide side = page.Front;
                (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(page, side);

                RectangleF rect = new RectangleF
                {
                    X = (float)page_x,
                    Y = (float)(_imposParam.ControlsBind.Sheet.H - page_y - page_h),
                    Width = (float)page_w,
                    Height = (float)page_h
                };

                if (rect.Contains(x, y))
                {
                    _imposParam.ControlsBind.HoverPage = page;
                    break;
                }
            }
            pb_preview.Refresh();
        }

        private void pb_preview_Paint(object sender, PaintEventArgs e)
        {
            if (_imposParam.ControlsBind == null || _imposParam.ControlsBind.Sheet == null) return;

            if (_imposParam.ControlsBind.SelectedPreviewPage != null)
            {
                Pen pen = new Pen(Color.IndianRed, 3);

                PageSide side = _imposParam.ControlsBind.SelectedPreviewPage.Front;
                (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(_imposParam.ControlsBind.SelectedPreviewPage, side);


                var rect = new RectangleF(
                    (float)page_x,
                    (float)(_imposParam.ControlsBind.Sheet.H - page_y - page_h),
                    (float)page_w,
                    (float)page_h);

                ScreenDrawer.DrawRectangle(e.Graphics, rect, pen);
                pen.Dispose();
            }

            if (_imposParam.ControlsBind.HoverPage != null)
            {
                (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(_imposParam.ControlsBind.HoverPage, _imposParam.ControlsBind.HoverPage.Front);

                Pen pen = new Pen(Color.Black, 2);

                var rect = new RectangleF(
                    (float)page_x,
                    (float)(_imposParam.ControlsBind.Sheet.H - page_y - page_h),
                    (float)page_w,
                    (float)page_h);

                ScreenDrawer.DrawRectangle(e.Graphics, rect, pen);

                pen.Dispose();
            }

            if (_imposParam.ImposTools.CurTool == ImposToolEnum.AddPageToGroup && isDragMode)
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
            if (_imposParam.ControlsBind.HoverPage == null) return;

            if (e.Button == MouseButtons.Left)
            {
                if (_imposParam.ImposTools.CurTool == ImposToolEnum.FlipAngle)
                {
                    _imposParam.ImposTools.OnFlipAngle(this, _imposParam.ControlsBind.HoverPage);
                }
                else if (_imposParam.ImposTools.CurTool == ImposToolEnum.Numeration)
                {
                    ToolNumeringSinglePage();
                }
                else if (_imposParam.ImposTools.CurTool == ImposToolEnum.DeletePage)
                {
                    ToolDeletePage();
                }
                else if (_imposParam.ImposTools.CurTool == ImposToolEnum.Select)
                {
                    _imposParam.ControlsBind.SelectedPreviewPage = _imposParam.ControlsBind.HoverPage;
                }
                else if (_imposParam.ImposTools.CurTool == ImposToolEnum.SwitchHW)
                {
                    ToolSwitchWH();
                }
                else if (_imposParam.ImposTools.CurTool == ImposToolEnum.AddPageToGroup)
                {
                    _imposParam.ImposTools.OnAddPageToGroup(this, _imposParam.ControlsBind.HoverPage);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (_imposParam.ImposTools.CurTool == ImposToolEnum.FlipAngle)
                {
                    _imposParam.ImposTools.OnFlipRowAngle(this, _imposParam.ControlsBind.HoverPage);
                    ToolFlipPageRow();
                }
                if (_imposParam.ImposTools.CurTool == ImposToolEnum.Numeration)
                {
                    ToolNumericWithContinue();
                }
            }
        }

        private void ToolSwitchWH()
        {
            if (_imposParam.ControlsBind.HoverPage == null) return;
            _imposParam.ControlsBind.HoverPage.SwitchWH(_imposParam.ControlsBind.Sheet.SheetPlaceType);
            _imposParam.ControlsBind.CheckRunListPages();
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void ToolDeletePage()
        {
            _imposParam.ControlsBind.Sheet.TemplatePageContainer.DeletePage(_imposParam.ControlsBind.Sheet, _imposParam.ControlsBind.HoverPage);
            _imposParam.ControlsBind.UpdateSheet();
        }

        private void ToolNumericWithContinue()
        {
            if (_imposParam.ControlsBind.Sheet.SheetPlaceType == TemplateSheetPlaceType.SingleSide ||
                       _imposParam.ControlsBind.Sheet.SheetPlaceType == TemplateSheetPlaceType.WorkAndTurn)
            {
                if (_imposParam.ControlsBind.Sheet is PrintSheet)
                {
                    _imposParam.ImposTools.FrontNum++;
                    _imposParam.ControlsBind.HoverPage.Front.PrintIdx = _imposParam.ImposTools.FrontNum;
                    _imposParam.ControlsBind.CheckRunListPages();
                }
                else
                {
                    _imposParam.ImposTools.FrontNum++;
                    _imposParam.ControlsBind.HoverPage.Front.MasterIdx = _imposParam.ImposTools.FrontNum;
                }
            }
            else
            {
                _imposParam.ImposTools.FrontNum += 2;
                _imposParam.ImposTools.BackNum += 2;

                if (_imposParam.ControlsBind.Sheet is PrintSheet)
                {
                    _imposParam.ControlsBind.HoverPage.Front.PrintIdx = _imposParam.ImposTools.FrontNum;
                    _imposParam.ControlsBind.HoverPage.Back.PrintIdx = _imposParam.ImposTools.BackNum;
                    _imposParam.ControlsBind.CheckRunListPages();
                }
                else
                {
                    _imposParam.ControlsBind.HoverPage.Front.MasterIdx = _imposParam.ImposTools.FrontNum;
                    _imposParam.ControlsBind.HoverPage.Back.MasterIdx = _imposParam.ImposTools.BackNum;

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
                if (_imposParam.ControlsBind.Sheet is PrintSheet sheet)
                {
                    _imposParam.ControlsBind.HoverPage.Front.PrintIdx = _imposParam.ImposTools.FrontNum;
                    _imposParam.ControlsBind.CheckRunListPages();

                }
                else
                {
                    _imposParam.ControlsBind.HoverPage.Front.MasterIdx = _imposParam.ImposTools.FrontNum;
                }

                _imposParam.ImposTools.FrontNum++;

            }
            else if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (_imposParam.ControlsBind.Sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide ||
                    _imposParam.ControlsBind.Sheet.SheetPlaceType != TemplateSheetPlaceType.WorkAndTurn)
                {

                    if (_imposParam.ControlsBind.Sheet is PrintSheet sheet)
                    {
                        _imposParam.ControlsBind.HoverPage.Back.PrintIdx = _imposParam.ImposTools.FrontNum;
                        _imposParam.ControlsBind.CheckRunListPages();
                    }
                    else
                    {
                        _imposParam.ControlsBind.HoverPage.Back.MasterIdx = _imposParam.ImposTools.FrontNum;
                    }
                    _imposParam.ImposTools.FrontNum++;
                }
            }
            else
            {
                if (_imposParam.ControlsBind.Sheet is PrintSheet sheet)
                {
                    _imposParam.ControlsBind.HoverPage.Front.PrintIdx = _imposParam.ImposTools.FrontNum;
                    _imposParam.ControlsBind.CheckRunListPages();
                }
                else
                {
                    _imposParam.ControlsBind.HoverPage.Front.MasterIdx = _imposParam.ImposTools.FrontNum;
                }

                if (_imposParam.ControlsBind.Sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide ||
                    _imposParam.ControlsBind.Sheet.SheetPlaceType != TemplateSheetPlaceType.WorkAndTurn)
                {
                    if (_imposParam.ControlsBind.Sheet is PrintSheet psheet)
                    {

                        _imposParam.ControlsBind.HoverPage.Back.PrintIdx = _imposParam.ImposTools.BackNum;
                        _imposParam.ControlsBind.CheckRunListPages();
                    }
                    else
                    {
                        _imposParam.ControlsBind.HoverPage.Back.MasterIdx = _imposParam.ImposTools.BackNum;

                    }
                }
            }
            RedrawSheet();

        }

        public void SetControlBindParameters(GlobalImposParameters imposParam)
        {
            _imposParam = imposParam;
            
            _imposParam.ControlsBind.PropertyChanged += Parameters_PropertyChanged;
            _imposParam.ControlsBind.JustUpdatePreview += Parameters_JustUpdatePreview;

            _imposParam.ImposTools.OnListNumberClick += OnToolsListNumberClick;
            _imposParam.ImposTools.OnTheSameNumberClick += OnTheSameNumberClick;
            _imposParam.ImposTools.OnCropMarksChanged += OnCropMarksChanged;
            imposToolsControl1.InitParameters(_imposParam);
        }

        private void Parameters_JustUpdatePreview(object sender, EventArgs e)
        {
            RedrawSheet();
        }

        private void Parameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_imposParam.ImposTools.CurTool == ImposToolEnum.Numeration && e.PropertyName == "SelectedImposRunPageIdx")
            {
                _imposParam.ImposTools.FrontNum = _imposParam.ControlsBind.SelectedImposRunPageIdx;
            }
            RedrawSheet();
        }
    }
}
