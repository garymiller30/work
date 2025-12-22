using Interfaces.Pdf.Imposition;
using JobSpace.Ext;
using JobSpace.Profiles;
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
        public EventHandler<TemplateSheet> OnSheetSelected { get; set; } = delegate { };
        public EventHandler<TemplateSheet> OnSheetAddToPrint { get; set; } = delegate { };
        public EventHandler<TemplateSheet> OnSheetAddManyToPrint = delegate { };

        List<TemplateSheet> _quickAccess = new List<TemplateSheet> { };

        ControlBindParameters _parameters;

        Profile _profile;

        //int idx = 1;
        public AddTemplateSheetControl()
        {
            InitializeComponent();
            //olvColumnId.AspectGetter += (r) => ((TemplateSheet)r).Id;
            olvColumnDesc.AspectGetter += (r) => ((TemplateSheet)r).Description;
            olvColumnPrintType.AspectGetter += (r) => ((TemplateSheet)r).SheetPlaceType.GetDescription();
            objectListView1.SelectionChanged += ObjectListView1_SelectionChanged;
            olvColumnFormat.AspectGetter += (r) => $"{((TemplateSheet)r).W} x {((TemplateSheet)r).H}";
        }

        private void InitQuickAccessMenu()
        {
            _quickAccess = _profile.ImposService.LoadQuickAccessTemplateSheets();
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
                _profile.ImposService.SaveQuickAccessTemplateSheets(_quickAccess);
                AssignQuickAccessMenuItems();
            }
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

            foreach (string desc2 in desc)
            {
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
            tscb_sheetTemplates.ComboBox.DisplayMember = "Description";

            tscb_sheetType.ComboBox.DataSource = Extensions.GetDescriptions(typeof(TemplateSheetPlaceType));

            var sheets = _profile.ImposService.LoadSheets();
            
            if (sheets.Any())
            {
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

        public void SetControlBindParameters(Profile profile, ControlBindParameters parameters)
        {
            _profile = profile;
            _parameters = parameters;

            InitQuickAccessMenu();

            InitContextMenu();

            InitSheets();

        }

        private void tsb_add_Click(object sender, EventArgs e)
        {
            using (var form = new FormAddSheet())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _profile.ImposService.SaveSheet(form.Sheet);
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
                            var copy = form.Sheet.Copy();

                            _profile.ImposService.SaveSheet(copy);
                            tscb_sheetTemplates.Items.Add(copy);
                        }
                        else
                        {
                            _profile.ImposService.SaveSheet(form.Sheet);
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
            if (ModifierKeys == Keys.Shift)
            {

                _parameters.ProductPart.TemplateSheets.Clear();
                objectListView1.ClearObjects();
            }
            else
            {
                if (objectListView1.SelectedObjects.Count == 0) return;
                foreach (var item in objectListView1.SelectedObjects)
                {
                    if (item is TemplateSheet sheet)
                    {
                        _parameters.ProductPart.TemplateSheets.Remove(sheet);
                        objectListView1.RemoveObject(item);
                    }
                }
            }
            _parameters.Sheet = null;
        }


        private void tsb_loadTemplate_Click(object sender, EventArgs e)
        {
            var sheets = _profile.ImposService.LoadSheetTemplates();
            if (sheets.Count > 0)
            {
                objectListView1.AddObjects(sheets);
                objectListView1.SelectObject(sheets[0]);
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

                _parameters.ProductPart.TemplateSheets.Add(sheet);

                OnSheetAdded(this, sheet);
            }
        }

        private void tsb_saveTemplate_Click_1(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            _profile.ImposService.SaveSheetTemplates(objectListView1.SelectedObjects.Cast<TemplateSheet>().ToList());

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
                _profile.ImposService.DeleteSheet(sheet);
            }
        }

        private void tsb_QuickAccess_ButtonClick(object sender, EventArgs e)
        {
            tsb_QuickAccess.ShowDropDown();
        }

        private void tsb_save_all_Click(object sender, EventArgs e)
        {
            if (objectListView1.Objects == null) return;
            var sheets = objectListView1.Objects.Cast<TemplateSheet>().ToList();
            _profile.ImposService.SaveSheetTemplates(sheets);
        }
    }
}
