using JobSpace.Static.Pdf.Imposition.Models;
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
        public EventHandler<TemplateSheet> OnSheetAdded = delegate{ }; 
        public EventHandler<TemplateSheet> OnSheetEdited = delegate { };
        public EventHandler<TemplateSheet> OnSheetSelected = delegate { };
        public EventHandler<TemplateSheet> OnSheetAddToPrint = delegate { };
        public EventHandler<TemplateSheet> OnSheetAddManyToPrint = delegate { };
        

        ControlBindParameters _parameters;

        int idx=1;
        public AddTemplateSheetControl()
        {
            InitializeComponent();
            olvColumnId.AspectGetter += (r) => ((TemplateSheet)r).Id;
            olvColumnDesc.AspectGetter += (r) => ((TemplateSheet)r).Description;
            olvColumnPrintType.AspectGetter += (r) => ((TemplateSheet)r).SheetPlaceType;
            objectListView1.SelectionChanged += ObjectListView1_SelectionChanged;
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
            using (var form = new Form_AddTemplateSheet(_parameters))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var sheet = form.Sheet;
                    sheet.Id = idx++;
                    objectListView1.AddObject(sheet);
                    objectListView1.SelectObject(sheet);
                    OnSheetAdded(this,sheet);
                }
            }
        }

        private void tsb_edit_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplateSheet sheet) {
                using (var form = new Form_AddTemplateSheet(sheet))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        objectListView1.RefreshObject(sheet);
                        OnSheetEdited(this, sheet);
                    }
                }

            }
        }

        private void tsb_toPrint_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplateSheet sheet)
                OnSheetAddToPrint(this,sheet);
        }

        public List<TemplateSheet> GetSheets()
        {
            return objectListView1.Objects.Cast<TemplateSheet>().ToList();
        }

        private void tsb_fillAll_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplateSheet sheet)
                OnSheetAddManyToPrint(this,sheet);
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
                objectListView1.RemoveObject(sheet);
            }
        }

        private void tsb_saveTemplate_Click(object sender, EventArgs e)
        {

        }

        private void tsb_loadTemplate_Click(object sender, EventArgs e)
        {

        }
    }
}
