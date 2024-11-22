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
    public partial class PrintSheetsControl : UserControl
    {
        public EventHandler<PrintSheet> OnPrintSheetsChanged { get; set; } = delegate { };
        public EventHandler OnPrintSheetDeleted { get; set; } = delegate { };

        int id = 1;
        ControlBindParameters _controlBindParameters;
        public PrintSheetsControl()
        {
            InitializeComponent();
            olvColumnId.AspectGetter += (r) => ((PrintSheet)r).Id;
            olvColumnTemplateId.AspectGetter += (r) => ((PrintSheet)r).TemplateId;
            olvColumnDesc.AspectGetter += (r) => ((PrintSheet)r).Description;
            olvColumnPlaceType.AspectGetter += (r) => ((PrintSheet)r).SheetPlaceType;
            objectListView1.SelectionChanged += (o, e) =>
            {
                if (objectListView1.SelectedObject is PrintSheet sheet)
                {
                    OnPrintSheetsChanged(this, sheet);
                }
            };
        }

        public void SetControlBindParameters(ControlBindParameters controlBindParameters)
        {
            _controlBindParameters = controlBindParameters;
        }

        public void AddSheet(PrintSheet sheet)
        {
            objectListView1.AddObject(sheet);
        }

        public List<PrintSheet> GetSheets()
        {
            if (objectListView1.Objects == null) return new List<PrintSheet> { };
            return objectListView1.Objects.Cast<PrintSheet>().ToList();
        }

        private void tsb_delete_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
            {
                objectListView1.ClearObjects();
            }
            else if (objectListView1.SelectedObject is PrintSheet sheet)
            {
                objectListView1.RemoveObject(sheet);
            }
            else
            {
                return;
            }

            OnPrintSheetDeleted(this, null);
        }
    }
}
