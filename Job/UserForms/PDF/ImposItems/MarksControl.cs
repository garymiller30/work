using BrightIdeasSoftware;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections;
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
    public partial class MarksControl : UserControl
    {
        ControlBindParameters parameters;

        //TemplateSheet _sheet;
        public MarksControl()
        {
            InitializeComponent();

            tlv_MarksResources.CanExpandGetter += CanExpandGetterHandle;
            tlv_MarksResources.ChildrenGetter += ChildrenGetterDelegate;
            ovl_ResourceMarks.AspectGetter += NameAspectGetterDelegate;
            ovl_ResourceMarks.ImageGetter += ImageGetterDelegate;

            tlv_ProductMarks.ModelCanDrop += Tlv_ProductMarks_ModelCanDrop;
            tlv_ProductMarks.IsSimpleDragSource = true;
            tlv_ProductMarks.DropSink = new SimpleDropSink();
            tlv_ProductMarks.ModelDropped += Tlv_ProductMarks_ModelDropped;
            tlv_ProductMarks.ChildrenGetter += ProductChildrenGetterDelegate;
            tlv_ProductMarks.CanExpandGetter += ProductCanExpandGetterDelegate;
            olv_ProductMarkName.AspectGetter += ProductAspectNameGetterDelegate;
            olv_ProductMarkName.ImageGetter += ProductImageGetterDelegate;

            List<MarksContainer> marks = MarksService.GetResourceMarks();
            tlv_MarksResources.AddObjects(marks);
            tlv_MarksResources.ExpandAll();
        }




        #region [RESOURCE MARKS]

        private object ImageGetterDelegate(object model)
        {
            if (model is MarksContainer || model is List<MarksContainer>)
            {
                return 0;
            }
            else if (model is PdfMark)
            {
                return 1;
            }
            else if (model is TextMark)
            {
                return 2;
            }

            return null;

        }

        private IEnumerable ChildrenGetterDelegate(object model)
        {
            if (model is MarksContainer group)
            {
                List<object> list = new List<object>();

                if (group.Containers.Count > 0) list.AddRange(group.Containers);
                if (group.Pdf.Count > 0) list.AddRange(group.Pdf);
                if (group.Text.Count > 0) list.AddRange(group.Text);

                return list;
            }
            if (model is List<MarksContainer> groups)
            {
                return groups;
            }
            if (model is List<PdfMark> pdfMarks)
            {
                return pdfMarks;
            }
            else if (model is List<TextMark> textMarks)
            {
                return textMarks;
            }
            return null;
        }
        private object NameAspectGetterDelegate(object rowObject)
        {
            if (rowObject is MarksContainer container)
            {
                return container.Name;
            }
            else if (rowObject is List<MarksContainer> groups)
            {
                return "Групи";
            }
            else if (rowObject is List<PdfMark>)
            {
                return "PDF";
            }
            else if (rowObject is List<TextMark>)
            {
                return "Текст";
            }
            else if (rowObject is PdfMark pdfMark)
            {
                return pdfMark.Name;
            }
            else if (rowObject is TextMark textMark)
            {
                return textMark.Name;
            }

            return null;
        }

        private bool CanExpandGetterHandle(object model)
        {
            if (model is MarksContainer container)
            {
                return true;
            }
            else if (model is List<MarksContainer> groups && groups.Count > 0)
            {
                return true;
            }
            else if (model is List<PdfMark> pdfMarks && pdfMarks.Count > 0)
            {
                return true;
            }
            else if (model is List<TextMark> textMarks && textMarks.Count > 0)
            {
                return true;
            }
            return false;
        }

        private void tsb_addGroup_Click(object sender, EventArgs e)
        {
            using (var form = new FormEditFolder("група", "назва групи"))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    AddGroup(form.NewName);
                }
            }
        }

        private void RefreshResourceTree()
        {
            tlv_MarksResources.RefreshObjects(tlv_MarksResources.Objects.Cast<MarksContainer>().ToList());
        }
        private void AddGroup(string name)
        {
            MarksContainer group;

            if (tlv_MarksResources.SelectedObject is MarksContainer container)
            {
                group = MarksService.CreateGroup(name, container);
                RefreshResourceTree();
            }
            else if (tlv_MarksResources.SelectedObject is List<MarksContainer> groups)
            {
                var parent = tlv_MarksResources.GetParent(tlv_MarksResources.SelectedObject);
                if (parent is MarksContainer container1)
                {
                    group = MarksService.CreateGroup(name, container1);
                    RefreshResourceTree();
                }
            }
            else
            {
                group = MarksService.CreateGroup(name);
                tlv_MarksResources.AddObject(group);
            }
        }
        private void tsb_Delete_Click(object sender, EventArgs e)
        {
            if (tlv_MarksResources.SelectedObject is MarksContainer group)
            {
                MarksService.DeleteGroup(group);

                if (group.ParentId == null)
                {
                    tlv_MarksResources.RemoveObject(group);
                }

                RefreshResourceTree();
            }
            else if (tlv_MarksResources.SelectedObject is PdfMark pdfMark)
            {
                MarksService.DeleteMark(pdfMark);
                RefreshResourceTree();
            }
            else if (tlv_MarksResources.SelectedObject is TextMark textMark)
            {
                MarksService.DeleteMark(textMark);
                RefreshResourceTree();
            }
        }
        private void tsb_addPdfMark_Click(object sender, EventArgs e)
        {
            if (tlv_MarksResources.SelectedObject is MarksContainer container)
            {
                using (var form = new FormAddPdfMark())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MarksService.AddMark(container, form.Mark);
                        RefreshResourceTree();
                    }
                }
            }
        }

        private void tsb_addTextMark_Click(object sender, EventArgs e)
        {
            if (tlv_MarksResources.SelectedObject is MarksContainer container)
            {
                using (var form = new FormAddTextMark())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MarksService.AddMark(container, form.Mark);
                        RefreshResourceTree();
                    }
                }
            }
        }

        private void tlv_MarksResources_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tlv_MarksResources.SelectedObject is PdfMark mark)
            {
                using (var form = new FormAddPdfMark(mark))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MarksService.SaveResourceMarks();
                        RefreshResourceTree();
                    }
                }
            }
            else if (tlv_MarksResources.SelectedObject is TextMark textMark)
            {
                using (var form = new FormAddTextMark(textMark))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MarksService.SaveResourceMarks();
                        RefreshResourceTree();
                    }
                }
            }
        }
        #endregion

        #region [SHEET MARKS]

        private void tsb_sheet_deleteMark_Click(object sender, EventArgs e)
        {
            if (tlv_ProductMarks.SelectedObject is MarksContainer group)
            {
                // знайти групу
                DeleteMark(group);
                RefreshSheetTree();
            }
            else if (tlv_ProductMarks.SelectedObject is PdfMark pdfMark)
            {
                //знайти мітку
                DeleteMark(pdfMark);
                RefreshSheetTree();
            }
            else if (tlv_ProductMarks.SelectedObject is TextMark textMark)
            {
                //знайти мітку
                DeleteMark(textMark);
                RefreshSheetTree();
            }
        }

        private void DeleteMark(TextMark textMark)
        {
            parameters.Sheet.Marks.Delete(textMark);
        }

        private void DeleteMark(PdfMark pdfMark)
        {
            parameters.Sheet.Marks.Delete(pdfMark);
        }

        private void DeleteMark(MarksContainer group)
        {
            parameters.Sheet.Marks.Delete(group);
        }

        private void RefreshSheetTree()
        {
            tlv_ProductMarks.RefreshObjects(tlv_ProductMarks.Objects.Cast<MarksContainer>().ToList());
            parameters.UpdatePreview();
        }


        private object ProductImageGetterDelegate(object r)
        {
            if (r is MarksContainer container) return 1;
            if (r is PdfMark) return 2;
            if (r is TextMark) return 3;
            return null;
        }
        private bool ProductCanExpandGetterDelegate(object model)
        {
            if (model is MarksContainer container)
            {
                if (container.Containers.Count > 0 || container.Pdf.Count > 0 || container.Text.Count > 0)
                {
                    return true;
                }
            }

            return false;

        }
        private object ProductAspectNameGetterDelegate(object r)
        {
            if (r is MarksContainer container)
            {
                return container.Name;
            }
            else if (r is PdfMark pdfMark)
            {
                return pdfMark.Name;
            }
            else if (r is TextMark textMark)
            {
                return textMark.Name;
            }

            return null;
        }

        private IEnumerable ProductChildrenGetterDelegate(object model)
        {
            if (model is MarksContainer container)
            {
                List<object> list = new List<object>();
                if (container.Containers.Count > 0) list.AddRange(container.Containers);
                if (container.Pdf.Count > 0) list.AddRange(container.Pdf);
                if (container.Text.Count > 0) list.AddRange(container.Text);
                return list;
            }
            return null;
        }

        private void Tlv_ProductMarks_ModelDropped(object sender, ModelDropEventArgs e)
        {
            if (e.SourceListView != tlv_MarksResources) return;

            foreach (var item in e.SourceModels)
            {
                if (e.TargetModel is MarksContainer container)
                {
                    if (item is MarksContainer group)
                    {
                        var g = MarksService.Duplicate(group);
                        g.ParentId = container.Id;
                        container.Containers.Add(g);
                    }
                    else if (item is PdfMark pdfMark)
                    {
                        var p = MarksService.Duplicate(pdfMark);
                        container.Pdf.Add(p);
                    }
                    else if (item is TextMark textMark)
                    {
                        var t = MarksService.Duplicate(textMark);
                        container.Text.Add(t);
                    }
                }


                //if (e.TargetModel is SheetRootAbstract sheet)
                //{
                //    if (item is MarksContainer container)
                //    {
                //        var c = MarksService.Duplicate(container);
                //        c.ParentId = sheet.Id;
                //        sheet.Marks.Add(c);
                //    }
                //    else if (item is PdfMark pdfMark)
                //    {
                //        var p = MarksService.Duplicate(pdfMark);
                //        sheet.Marks.Add(p);
                //    }
                //    else if (item is TextMark textMark)
                //    {
                //        var t = MarksService.Duplicate(textMark);

                //        sheet.Marks.Add(t);
                //    }
                //}
                //else if (e.TargetModel is SubjetRoot subject)
                //{
                //    if (item is MarksContainer container)
                //    {
                //        var c = MarksService.Duplicate(container);
                //        c.ParentId = subject.Id;
                //        subject.Marks.Add(c);
                //    }
                //    else if (item is PdfMark pdfMark)
                //    {
                //        var p = MarksService.Duplicate(pdfMark);
                //        subject.Marks.Add(p);
                //    }
                //    else if (item is TextMark textMark)
                //    {
                //        var t = MarksService.Duplicate(textMark);
                //        subject.Marks.Add(t);
                //    }
                //}
            }

            e.Handled = true;
            RefreshSheetTree();
        }
        private void Tlv_ProductMarks_ModelCanDrop(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.TargetModel == null) return;

            if (e.TargetModel is PdfMark || e.TargetModel is MarksContainer)
            {
                e.Effect = e.StandardDropActionFromKeys;
            }
          
        }
        private void tlv_ProductMarks_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tlv_ProductMarks.SelectedObject is PdfMark pdfMark)
            {
                using (var form = new FormAddPdfMark(pdfMark))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // update preview
                        parameters.UpdatePreview();
                    }
                }
            }
            else if (tlv_ProductMarks.SelectedObject is TextMark textmark)
            {
                using (var form = new FormAddTextMark(textmark))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // update preview
                        parameters.UpdatePreview();
                    }
                }
            }
        }

        #endregion

        private void tlv_ProductMarks_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (((OLVListItem)e.Item).RowObject is PdfMark pdfMark)
            {
                pdfMark.Enable = e.Item.Checked;
            }
            else if (((OLVListItem)e.Item).RowObject is TextMark textMark)
            {
                textMark.Enable = e.Item.Checked;
            }
            parameters.UpdatePreview();
        }

        public void SetControlBindParameters(ControlBindParameters controlBindParameters)
        {
            parameters = controlBindParameters;
            parameters.PropertyChanged += Parameters_PropertyChanged;

        }



        private void Parameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName != "Sheet") return;

            if (parameters.Sheet == null) tlv_ProductMarks.Enabled = false;
            else
            {
                Debug.WriteLine("-->MarksControl: Parameters_PropertyChanged");
                tlv_ProductMarks.Enabled = true;
                tlv_ProductMarks.Roots =


                    new object[]
                {
                    parameters.Sheet.Marks,
                    //new SheetRoot(){Marks = parameters.Sheet.Marks},
                    //new SubjetRoot(){Marks = parameters.Sheet.TemplatePageContainer.Marks}
                    };
                Debug.WriteLine("<--MarksControl: Parameters_PropertyChanged");
            }
        }

        private void tlv_ProductMarks_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //parameters.UpdateSheet();
        }
    }
}
