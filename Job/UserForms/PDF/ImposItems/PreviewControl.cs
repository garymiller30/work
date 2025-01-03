﻿using JobSpace.Static.Pdf.Imposition.Drawers.Screen;
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
        ImposToolsParameters _parameters;
        bool isDragMode = false;

        double hover_x;
        double hover_y;

        PointF clickPoint;
        PointF lastLocation;
        double snapDistance = 6;




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

            if (_parameters.CurTool == ImposToolEnum.Select)
            {
                isDragMode = true;
                clickPoint = e.Location;
                lastLocation = e.Location;
            }
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
                if (parameters.Sheet is PrintSheet)
                {
                    page.PrintFrontIdx = _parameters.FrontNum;
                }
                else
                {
                    page.MasterFrontIdx = _parameters.FrontNum;
                    
                }

                if (parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.Sheetwise)
                {
                    if (parameters.Sheet is PrintSheet)
                    {
                        page.PrintBackIdx = _parameters.BackNum;
                    }
                    else
                    {
                        page.MasterBackIdx = _parameters.BackNum;
                        
                    }


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

            foreach (var page in parameters.Sheet.TemplatePageContainer.TemplatePages)
            {
                if (page == _hover) continue;

                if (!x_snap)
                {
                    var pageR = page.GetDrawBleedRight();
                    var pageL = page.GetDrawBleedLeft();

                    // вибрана сторінка - права сторона <--> ліва сторона
                    if (Math.Abs(pageR.X2 - hover_x - x_ofs) < snapDistance)
                    {
                        x_snap = true;
                        _hover.X = page.X + page.GetClippedWByRotate();
                    }
                    // вибрана сторінка - ліва сторона сторона <--> права сторона
                    else if (Math.Abs(hover_x + x_ofs + _hover.GetClippedWByRotate() - pageL.X1) < snapDistance)
                    {
                        x_snap = true;
                        _hover.X = page.X - _hover.GetClippedWByRotate();
                    }
                    // вибрана сторінка - права сторона <--> права сторона
                    else if (Math.Abs(pageL.X1 - hover_x - x_ofs) < snapDistance)
                    {
                        x_snap = true;
                        _hover.X = page.X;
                    }
                    // вибрана сторінка - ліва сторона сторона <--> ліва сторона
                    else if (Math.Abs(hover_x + x_ofs + _hover.GetClippedWByRotate() - pageR.X2) < snapDistance)
                    {
                        x_snap = true;
                        _hover.X = page.X;
                    }
                }

                if (!y_snap)
                {
                    var pageT = page.GetDrawBleedTop();
                    var pageB = page.GetDrawBleedBottom();

                    // вибрана сторінка - верх <--> низ
                    if (Math.Abs(pageB.Y1 - hover_y - y_ofs - _hover.GetClippedHByRotate()) < snapDistance)
                    {
                        y_snap = true;
                        _hover.Y = page.Y - _hover.GetClippedHByRotate();
                    }
                    // вибрана сторінка - низ <--> верх
                    else if (Math.Abs(hover_y - y_ofs - pageB.Y2) < snapDistance)
                    {
                        y_snap = true;
                        _hover.Y = page.Y + page.GetClippedHByRotate();
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
            if (parameters == null || parameters.Sheet == null) return;

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
                    _hover.PrintFrontIdx = _parameters.FrontNum++;
                    parameters.CheckRunListPages();
                }
                else
                {
                    _hover.MasterFrontIdx = _parameters.FrontNum++;
                    
                }
                    
            }
            else
            {
                if (parameters.Sheet is PrintSheet)
                {
                    _hover.PrintFrontIdx = _parameters.FrontNum;
                    _hover.PrintBackIdx = _parameters.BackNum;
                    parameters.CheckRunListPages();
                }
                else
                {
                    _hover.MasterFrontIdx = _parameters.FrontNum;
                    _hover.MasterBackIdx = _parameters.BackNum;
                   
                }
                _parameters.FrontNum += 2;
                _parameters.BackNum += 2;
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
                    _hover.PrintFrontIdx = _parameters.FrontNum;
                    
                    parameters.CheckRunListPages();
                    
                }
                else
                {
                    _hover.MasterFrontIdx = _parameters.FrontNum;

                }
                    
                _parameters.FrontNum++;

            }
            else if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide ||
                    parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.WorkAndTurn)
                {

                    if (parameters.Sheet is PrintSheet sheet)
                    {

                        _hover.PrintBackIdx = _parameters.FrontNum;
                        parameters.CheckRunListPages();
                    }
                    else
                    {
                        _hover.MasterBackIdx = _parameters.FrontNum;
                        
                    }
                    _parameters.FrontNum++;
                }
            }
            else
            {
                if (parameters.Sheet is PrintSheet sheet)
                {
                    _hover.PrintFrontIdx = _parameters.FrontNum;
                    parameters.CheckRunListPages();
                }
                else
                {
                    _hover.MasterFrontIdx = _parameters.FrontNum;
                   

                }

                if (parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide ||
                    parameters.Sheet.SheetPlaceType != TemplateSheetPlaceType.WorkAndTurn)
                {
                    if (parameters.Sheet is PrintSheet psheet)
                    {
                        
                        _hover.PrintBackIdx = _parameters.BackNum;
                        parameters.CheckRunListPages();
                    }
                    else
                    {
                        _hover.MasterBackIdx = _parameters.BackNum;
                        
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
            Debug.WriteLine("-->PreviewControl: Parameters_PropertyChanged");
            RedrawSheet();
            Debug.WriteLine("<--PreviewControl: Parameters_PropertyChanged");
        }
    }
}
