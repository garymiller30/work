using JobSpace.Ext;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
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
    public partial class AddTemplateSheetControl : UserControl
    {
        public EventHandler<TemplateSheet> OnSheetAdded = delegate { };
        public EventHandler<TemplateSheet> OnSheetEdited = delegate { };
        public EventHandler<TemplateSheet> OnSheetSelected { get;set;} = delegate { };
        public EventHandler<TemplateSheet> OnSheetAddToPrint { get;set;} = delegate { };
        public EventHandler<TemplateSheet> OnSheetAddManyToPrint = delegate { };

        List<TemplateSheet> _quickAccess = new List<TemplateSheet> { };

        ControlBindParameters _parameters;

        int idx = 1;
        public AddTemplateSheetControl()
        {
            InitializeComponent();
            olvColumnId.AspectGetter += (r) => ((TemplateSheet)r).Id;
            olvColumnDesc.AspectGetter += (r) => ((TemplateSheet)r).Description;
            olvColumnPrintType.AspectGetter += (r) => ((TemplateSheet)r).SheetPlaceType.GetDescription();
            objectListView1.SelectionChanged += ObjectListView1_SelectionChanged;
            olvColumnFormat.AspectGetter += (r) => $"{((TemplateSheet)r).W} x {((TemplateSheet)r).H}";

            InitQuickAccessMenu();

            InitContextMenu();

            InitSheets();
        }

        private void InitQuickAccessMenu()
        {
            _quickAccess = SaveLoadService.LoadQuickAccessTemplateSheets();
            AssignQuickAccessMenuItems();
        }

        private void AssignQuickAccessMenuItems()
        {
            tsb_QuickAccess.DropDownItems.Clear();

            foreach (TemplateSheet sheet in _quickAccess)
            {
                ToolStripItem item = tsb_QuickAccess.DropDownItems.Add(sheet.Description);
                item.Click += (s, e) =>
                {
                    objectListView1.AddObject(sheet.Copy());
                };
                tsb_QuickAccess.DropDownItems.Add(item);
            }
        }

        private void tsb_addToQuickAccess_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplateSheet sheet)
            {
                HashSet<TemplateSheet> sheets = new HashSet<TemplateSheet>(_quickAccess, new TemplateSheetComparer());
                sheets.Add(sheet);

                _quickAccess.Clear();
                _quickAccess.AddRange(sheets);
                SaveLoadService.SaveQuickAccessTemplateSheets(_quickAccess);
                AssignQuickAccessMenuItems();            }
        }

        class TemplateSheetComparer : IEqualityComparer<TemplateSheet>
        {
            public bool Equals(TemplateSheet x, TemplateSheet y)
            {
                return x.Description == y.Description;
            }

            public int GetHashCode(TemplateSheet obj)
            {
                return obj.Description.GetHashCode();
            }
        }

        private void InitContextMenu()
        {
            List<string> desc = Extensions.GetDescriptions(typeof(TemplateSheetPlaceType)).ToList();

            foreach (string desc2 in desc) {
                ToolStripMenuItem item = new ToolStripMenuItem(desc2);
                item.Tag = desc.IndexOf(desc2);
                item.Click += (s, e) =>
                {
                    if (objectListView1.SelectedObject is TemplateSheet sheet)
                    {
                        sheet.SheetPlaceType = (TemplateSheetPlaceType)item.Tag;
                        objectListView1.RefreshObject(sheet);
                    }
                };
                cms_SheetSideType.Items.Add(item);
            }
        }

        private void InitSheets()
        {

            tscb_sheetType.ComboBox.DataSource = Extensions.GetDescriptions(typeof(TemplateSheetPlaceType));

            var sheets = SaveLoadService.LoadSheets();
            if (sheets.Any())
            {
                tscb_sheetTemplates.ComboBox.DisplayMember = "Description";
                tscb_sheetTemplates.ComboBox.Items.AddRange(sheets.ToArray());
            }
        }

        private void ObjectListView1_SelectionChanged(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplateSheet sheet)
            {
                OnSheetSelected(this, sheet);
            }
        }

        public void SetControlBindParameters(ControlBindParameters parameters)
        {
            _parameters = parameters;
        }

        private void tsb_add_Click(object sender, EventArgs e)
        {
            using (var form = new FormAddSheet())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SaveLoadService.SaveSheet(form.Sheet);
                    tscb_sheetTemplates.Items.Add(form.Sheet);
                }
            }
        }

        private void tsb_edit_Click(object sender, EventArgs e)
        {

            if (tscb_sheetTemplates.SelectedItem is TemplateSheet templateSheet)
            {
                using (var form = new FormAddSheet(templateSheet))
                {
                    string old_desc = templateSheet.Description;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        if (old_desc != templateSheet.Description)
                        {
                            SaveLoadService.SaveSheet(form.Sheet);
                            tscb_sheetTemplates.Items.Add(form.Sheet);
                        }
                        else
                        {
                            SaveLoadService.SaveSheet(form.Sheet);
                        }
                    }
                }
            }
        }

        private void tsb_toPrint_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplateSheet sheet)
                OnSheetAddToPrint(this, sheet);
        }

        public List<TemplateSheet> GetSheets()
        {
            if (objectListView1.Objects == null) return new List<TemplateSheet>();
            return objectListView1.Objects.Cast<TemplateSheet>().ToList();
        }

        private void tsb_fillAll_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplateSheet sheet)
                OnSheetAddManyToPrint(this, sheet);
        }

        private void tbs_dublicate_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplateSheet sheet)
            {
                TemplateSheet s = TemplateSheet.Duplicate(sheet);
                objectListView1.AddObject(s);
            }

        }

        private void tsb_delete_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplateSheet sheet)
            {
                if (ModifierKeys == Keys.Shift)
                {
                    objectListView1.ClearObjects();
                }
                else
                {
                    objectListView1.RemoveObject(sheet);
                }

                _parameters.Sheet = null;
               

            }
        }


        private void tsb_loadTemplate_Click(object sender, EventArgs e)
        {
            var sheets = SaveLoadService.LoadSheetTemplates();
            if (sheets.Count >0)
            {
                objectListView1.AddObjects(sheets);
            }
        }

        private void tsb_addToList_Click(object sender, EventArgs e)
        {
            if (tscb_sheetTemplates.SelectedItem is TemplateSheet t_sheet)
            {
                var sheet = t_sheet.Copy();
                sheet.SheetPlaceType = (TemplateSheetPlaceType)tscb_sheetType.SelectedIndex;
                objectListView1.AddObject(sheet);
                objectListView1.SelectObject(sheet);
                
                OnSheetAdded(this, sheet);
            }
        }

        private void tsb_saveTemplate_Click_1(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            SaveLoadService.SaveSheetTemplates(objectListView1.SelectedObjects.Cast<TemplateSheet>().ToList());
          
        }

        private void objectListView1_DoubleClick(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplateSheet sheet)
            {
                using (var form = new FormAddSheet(sheet))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        objectListView1.RefreshObject(sheet);
                        _parameters.UpdatePreview();
                    }
                }
            }
        }

        private void tsb_deleteTemplateSheet_Click(object sender, EventArgs e)
        {
            if (tscb_sheetTemplates.SelectedItem is TemplateSheet sheet)
            {
                tscb_sheetTemplates.Items.Remove(sheet);
                SaveLoadService.DeleteSheet(sheet);
                
            }
        }

        private void tsb_QuickAccess_ButtonClick(object sender, EventArgs e)
        {
            tsb_QuickAccess.ShowDropDown();
        }
    }
}
