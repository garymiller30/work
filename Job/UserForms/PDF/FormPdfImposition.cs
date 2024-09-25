using BrightIdeasSoftware;
using Job.Static.Pdf.Imposition.Drawers.PDF;
using Job.Static.Pdf.Imposition.Drawers.Screen;
using Job.Static.Pdf.Imposition.Models;
using Job.Static.Pdf.Imposition.Services;
using Job.Static.Pdf.Imposition.Services.Impos;
using Job.Static.Pdf.Imposition.Services.Impos.Binding;
using Job.UserForms.PDF.ImposItems;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Job.UserForms.PDF
{
    public partial class FormPdfImposition : KryptonForm
    {
        string _curJobFolder;
        private IEnumerable<string> _files;
        ImposRunList _runList = new ImposRunList();
        List<PdfFile> _pdfFiles = new List<PdfFile>();
        Form _tools;
        TemplatePage _hover;
        ImposToolsParameters _parameters = new ImposToolsParameters();

        ProductPart _productPart;

        public FormPdfImposition()
        {
            InitializeComponent();
            InitFileListTree();
            InitRunList();
            InitSheets();
        }

        private void InitImposTools()
        {
            _tools = new FormImposTools(_parameters);
            _tools.Show();
        }

        private void InitSheets()
        {
            comboBoxSheetPlaceType.DataSource = Enum.GetNames(typeof(TemplateSheetPlaceType));

            var sheets = SaveLoadService.LoadSheets();
            if (sheets.Any())
            {
                comboBoxSheets.Items.AddRange(sheets.ToArray());
            }
        }

        private void InitRunList()
        {
            objectListViewRunList.IsSimpleDragSource = true;
            objectListViewRunList.DropSink = new RearrangingDropSink(true);
            objectListViewRunList.ModelCanDrop += ObjectListViewRunList_ModelCanDrop;
            objectListViewRunList.ModelDropped += ObjectListViewRunList_ModelDropped;
            olvColumnRunListPages.AspectGetter += delegate (object r)
            {
                if (r is ImposRunPage page)
                {
                    return $"{page.FileId}:{page.PageIdx}";
                }
                return r.ToString();
            };

            olvColumIdx.AspectGetter += delegate (object r)
            {
                var page = (ImposRunPage)r;

                var list = objectListViewRunList.Objects.Cast<ImposRunPage>().ToList();

                int idx = list.IndexOf(page) + 1;

                return idx;
            };
        }

        private void ObjectListViewRunList_ModelDropped(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            if (e.SourceListView == treeListViewFiles)
            {
                foreach (var item in e.SourceModels)
                {
                    if (item is PdfFile file)
                    {
                        var pages = _runList.AddFile(file);
                        objectListViewRunList.AddObjects(pages);
                    }
                    else if (item is PdfFilePage page)
                    {
                        int fileId = PdfFile.GetParentId(_pdfFiles, page);
                        if (fileId == -1) throw new Exception("No file id");
                        ImposRunPage runPage = new ImposRunPage(fileId, page.Idx);
                        _runList.AddPage(runPage);
                        objectListViewRunList.AddObject(runPage);
                    }
                }
                e.Handled = true;
            }
        }

        private void ObjectListViewRunList_ModelCanDrop(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (e.TargetModel == null) return;

            if (e.TargetModel is PdfFile || e.TargetModel is PdfFilePage)
            {
                e.Effect = e.StandardDropActionFromKeys;
            }
        }

        private void InitFileListTree()
        {

            treeListViewFiles.IsSimpleDragSource = true;
            //treeListViewFiles.DragSource = new SimpleDragSource();
            treeListViewFiles.CanExpandGetter += (r) => r is PdfFile;
            treeListViewFiles.ChildrenGetter += (r) => ((PdfFile)r).Pages;
            olvColumnName.AspectGetter = delegate (object r)
            {
                if (r is PdfFile file)
                {
                    return $"{file.Id}: {file.ShortName}";
                }
                else if (r is PdfFilePage page)
                {
                    return $"Page {page.Idx}";
                }
                return "???";
            };

            olvColumnTrim.AspectGetter = delegate (object r)
            {
                if (r is PdfFilePage p)
                {
                    return $"{p.Trim.W.ToString("N1")} x {p.Trim.H.ToString("N1")}";
                }
                return null;
            };

            treeListViewFiles.ModelCanDrop += TreeListViewFiles_ModelCanDrop;
        }

        private void TreeListViewFiles_ModelCanDrop(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.TargetModel == null) return;

            if (e.TargetModel is PdfFile || e.TargetModel is PdfFilePage)
            {
                e.Effect = e.StandardDropActionFromKeys;
            }


        }

        public FormPdfImposition(IEnumerable<string> files, string curFolder) : this()
        {
            _files = files;
            _curJobFolder = curFolder;
            int id = 1;
            foreach (var file in _files)
            {

                var pdfFile = new PdfFile(file) { Id = id++ };
                _pdfFiles.Add(pdfFile);
                _runList.AddFile(pdfFile);
            }
            treeListViewFiles.AddObjects(_pdfFiles);
            objectListViewRunList.AddObjects(_runList.RunPages);
            nud_page_w.Value = (decimal)_pdfFiles[0].Pages[0].Trim.W;
            nud_page_h.Value = (decimal)_pdfFiles[0].Pages[0].Trim.H;

            LoadImposFromFile();

        }

        private void LoadImposFromFile()
        {
            string folderPath = Path.Combine(_curJobFolder, ".impos");
            if (Directory.Exists(folderPath))
            {
                string filePath = Path.Combine(folderPath,"imposition.json");

                if (File.Exists(filePath))
                {
                    var str = File.ReadAllText(filePath);
                    _productPart = JsonSerializer.Deserialize<ProductPart>(str);

                    RedrawProductPart();

                }
            }
        }

        private void tsb_RemovePage_Click(object sender, EventArgs e)
        {
            if (objectListViewRunList.SelectedObjects.Count > 0)
            {
                objectListViewRunList.RemoveObjects(objectListViewRunList.SelectedObjects);
            }
        }

        private void tsb_AddEmptyPage_Click(object sender, EventArgs e)
        {
            objectListViewRunList.AddObject(new ImposRunPage() { FileId = 0, PageIdx = 0 });
        }

        private void buttonAddSheet_Click(object sender, EventArgs e)
        {
            AddSheet();
        }

        private void AddSheet()
        {
            using (var form = new FormAddSheet())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SaveLoadService.SaveSheet(form.Sheet);
                    comboBoxSheets.Items.Add(form.Sheet);
                }
            }
        }

        private void buttonEditSheet_Click(object sender, EventArgs e)
        {
            EditSheet();
        }

        private void EditSheet()
        {
            if (comboBoxSheets.SelectedItem is TemplateSheet sheet)
            {
                using (var form = new FormAddSheet(sheet))
                {
                    string old_desc = sheet.Description;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        if (old_desc != sheet.Description)
                        {
                            SaveLoadService.SaveSheet(form.Sheet);
                            comboBoxSheets.Items.Add(form.Sheet);
                        }
                        else
                        {
                            SaveLoadService.SaveSheet(form.Sheet);
                        }
                    }
                }
            }
        }

        private void comboBoxSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSheets.SelectedItem is TemplateSheet sheet)
            {
                nud_info_w.Value = (decimal)sheet.W;
                nud_info_h.Value = (decimal)sheet.H;

                nud_info_fieldBottom.Value = (decimal)sheet.SafeFields.Bottom;
                nud_info_fieldLeft.Value = (decimal)sheet.SafeFields.Left;
                nud_info_fieldRight.Value = (decimal)sheet.SafeFields.Right;
                nud_info_fieldTop.Value = (decimal)sheet.SafeFields.Top;

                nud_info_extraSpace.Value = (decimal)sheet.ExtraSpace;
            }
        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            _productPart = new ProductPart();
            TemplateSheet sheet = _productPart.Sheet;


            sheet.SheetPlaceType = (TemplateSheetPlaceType)comboBoxSheetPlaceType.SelectedIndex;
            sheet.W = (double)nud_info_w.Value;
            sheet.H = (double)nud_info_h.Value;
            sheet.ExtraSpace = (double)nud_info_extraSpace.Value;
            sheet.SafeFields.Top = (double)nud_info_fieldTop.Value;
            sheet.SafeFields.Bottom = (double)nud_info_fieldBottom.Value;
            sheet.SafeFields.Left = (double)nud_info_fieldLeft.Value;
            sheet.SafeFields.Right = (double)nud_info_fieldRight.Value;

            var tp = new TemplatePage();
            tp.W = (double)nud_page_w.Value;
            tp.H = (double)nud_page_h.Value;
            tp.Bleeds = (double)nud_page_bleed.Value;
            tp.Margins.Set(tp.Bleeds);

            LooseBindingParameters bindingParameters = new LooseBindingParameters();
            bindingParameters.IsCenterHorizontal = cb_centerWidth.Checked;
            bindingParameters.IsCenterVertical = cb_centerHeight.Checked;
            bindingParameters.Xofs = (double)nud_Xofs.Value;
            bindingParameters.Yofs = (double)nud_Yofs.Value;
            bindingParameters.Sheet = sheet;
            bindingParameters.TemplatePage = tp;

            sheet.TemplatePageContainer = BindingService.Impos(bindingParameters);

            RedrawProductPart();


        }

        void RedrawProductPart()
        {
            var screenDrawer = new ScreenDrawer();

            if (pb_preview.Image != null) pb_preview.Image.Dispose();

            pb_preview.Width = (int)_productPart.Sheet.W + 1;
            pb_preview.Height = (int)_productPart.Sheet.H + 1;
            pb_preview.Image = screenDrawer.Draw(_productPart);
        }

        private void pb_preview_MouseMove(object sender, MouseEventArgs e)
        {
            if (_productPart != null)
            {
                tsl_coord.Text = $"x: {e.Location.X}, y: {_productPart.Sheet.H - e.Location.Y}";
                CheckHover(e.X, e.Y);
            }
        }

        private void CheckHover(int x, int y)
        {
            _hover = null;

            foreach (var page in _productPart.Sheet.TemplatePageContainer.TemplatePages)
            {
                RectangleF rect = new RectangleF
                {
                    X = (float)page.GetPageDrawX(),
                    Y = (float)(_productPart.Sheet.H - page.GetPageDrawY() - page.GetPageDrawH()),
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
            if (_hover != null && _productPart != null)
            {
                Pen pen = new Pen(Color.Black, 3);

                e.Graphics.DrawRectangle(pen, new Rectangle(
                    (int)_hover.GetPageDrawX(),
                    (int)_productPart.Sheet.H - (int)_hover.GetPageDrawY() - (int)_hover.GetPageDrawH(),
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
                        RedrawProductPart();
                    }
                }
                else if (_parameters.IsNumering)
                {
                    if (_hover != null)
                    {
                        _hover.FrontIdx = _parameters.FrontNum;

                        if (_productPart.Sheet.SheetPlaceType != TemplateSheetPlaceType.SingleSide)
                        {
                            _hover.BackIdx = _parameters.BackNum;
                        }
                        RedrawProductPart();
                    }
                }

            }
            else if (e.Button == MouseButtons.Right)
            {
                if (_parameters.IsFlipAngle)
                {
                    if (_hover != null)
                    {
                        _productPart.Sheet.TemplatePageContainer.FlipPagesAngle(_hover);
                        RedrawProductPart();
                    }
                }
                if (_parameters.IsNumering)
                {
                    if (_hover != null)
                    {
                        if (_productPart.Sheet.SheetPlaceType == TemplateSheetPlaceType.SingleSide)
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

                        RedrawProductPart();
                    }
                }
            }
        }

        private void btn_SaveToPdf_Click(object sender, EventArgs e)
        {
            _productPart.Proof.Enable = cb_DrawProofColor.Checked;
            SaveToPdf();
        }

        private void SaveToPdf()
        {
            if (_productPart != null)
            {
                _productPart.RunList.RunPages = objectListViewRunList.Objects.Cast<ImposRunPage>().ToList();
                _productPart.PdfFiles = _pdfFiles;

                var drawer = new PdfDrawer(_files.ToList()[0] + ".impos.pdf");
                drawer.Draw(_productPart);
                MessageBox.Show("Виконано!");
            }
        }
        private void FormPdfImposition_FormClosing(object sender, FormClosingEventArgs e)
        {
            _tools.Close();
            if (_productPart != null)
            {
                string pathImpos = Path.Combine(_curJobFolder, ".impos");
                Directory.CreateDirectory(pathImpos);

                string fileImpos = Path.Combine(pathImpos, "imposition.json");

                _productPart.Save(fileImpos);
            }
        }

        private void FormPdfImposition_Shown(object sender, EventArgs e)
        {
            InitImposTools();
        }
    }
}
