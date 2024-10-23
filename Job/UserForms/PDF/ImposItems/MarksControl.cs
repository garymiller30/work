using BrightIdeasSoftware;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections;
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
    public partial class MarksControl : UserControl
    {
        TemplateSheet _sheet;
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
        }

        private object ProductImageGetterDelegate(object r)
        {
            if (r is SheetRoot sheet) {
                if (sheet.Name == "Лист") return 0; 
                return 1;
                }
            if (r is PdfMark) return 2;
            if (r is TextMark) return 3;
            return null;
        }

        private bool ProductCanExpandGetterDelegate(object model)
        {
            if (model is SheetRoot sheet)
            {
                if (sheet.Marks.Containers.Count > 0 || sheet.Marks.Pdf.Count >0 || sheet.Marks.Text.Count > 0)
                {
                    return true;
                }


            }
            else if (model is MarksContainer container)
            {
                if (container.Containers.Count > 0 || container.Pdf.Count > 0 || container.Text.Count > 0)
                {
                    return true;
                }
            }
            else if (model is PdfMark)
            {

            }

            return false;

        }

        private object ProductAspectNameGetterDelegate(object r)
        {
            if (r is SheetRoot sheet)
            {
                return sheet.Name;
            }
            else if (r is MarksContainer container)
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
            if (model is SheetRoot sheet)
            {
                List<object> list = new List<object>();

                if (sheet.Marks.Containers.Count > 0) list.AddRange(sheet.Marks.Containers);
                if (sheet.Marks.Pdf.Count > 0) list.AddRange(sheet.Marks.Pdf);
                if (sheet.Marks.Text.Count > 0) list.AddRange(sheet.Marks.Text);
                return list;
            }
            else if (model is MarksContainer container)
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
                if (e.TargetModel is SheetRoot sheet)
                {
                    if (item is MarksContainer container)
                    {
                        sheet.Marks.Add(container);
                    }
                    else if (item is PdfMark pdfMark)
                    {
                        sheet.Marks.Add(pdfMark);
                    }
                    else if (item is TextMark textMark)
                    {
                        sheet.Marks.Add(textMark);
                    }
                }
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
            else if (e.TargetModel is SheetRoot)
            {
                e.Effect= e.StandardDropActionFromKeys;
            }
        }



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

        public void SetSheet(TemplateSheet e)
        {
            _sheet = e;
            if (_sheet == null) tlv_ProductMarks.Enabled = false;
            else
            {
                tlv_ProductMarks.Enabled = true;
                tlv_ProductMarks.Roots = new[]
                {
                    new SheetRoot{Name = "Лист",Marks = _sheet.Marks},
                    new SheetRoot{Name = "Сюжет", Marks = _sheet.TemplatePageContainer.Marks}
                    };
            }
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

        private void RefreshSheetTree()
        {
            tlv_ProductMarks.RefreshObjects(tlv_ProductMarks.Objects.Cast<SheetRoot>().ToList());
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
        }

        private void tsb_addPdfMark_Click(object sender, EventArgs e)
        {
            if (tlv_MarksResources.SelectedObject is MarksContainer container)
            {
                using (var form = new FormAddPdfMark())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MarksService.AddPdfMark(container, form.Mark);
                        RefreshResourceTree();
                    }
                }
            }
        }

        private void tsb_addTextMark_Click(object sender, EventArgs e)
        {
            if (tlv_MarksResources.SelectedObject is MarksContainer container)
            {

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
        }


        class SheetRoot
        {
            public string Name { get; set; }
            public MarksContainer Marks { get; set; }
        }
    }
}
