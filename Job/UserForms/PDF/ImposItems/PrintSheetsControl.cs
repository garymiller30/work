using BrightIdeasSoftware;
using JobSpace.Dlg;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using Ookii.Dialogs.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class PrintSheetsControl : UserControl
    {
        public EventHandler<PrintSheet> OnPrintSheetsChanged { get; set; } = delegate { };
        public EventHandler JustReassignPages { get; set; } = delegate { };
        public EventHandler OnPrintSheetDeleted { get; set; } = delegate { };

        int id = 1;
        ControlBindParameters _controlBindParameters;
        public PrintSheetsControl()
        {
            InitializeComponent();
            olvColumnId.AspectGetter += (r) => objectListView1.Objects?.Cast<PrintSheet>().ToList().IndexOf( (PrintSheet)r)+1;
            olvColumnFormat.AspectGetter += (r) => ((PrintSheet)r).GetFormatStr();
            olvColumnDesc.AspectGetter += (r) => ((PrintSheet)r).Description;
            olvColumnPlaceType.AspectGetter += (r) => ((PrintSheet)r).SheetPlaceType;
            objectListView1.SelectionChanged += (o, e) =>
            {
                if (objectListView1.SelectedObject is PrintSheet sheet)
                {
                    OnPrintSheetsChanged(this, sheet);
                }
            };

            olvColumnTemplatePlate.AspectGetter += (r) => ((PrintSheet)r).TemplatePlate?.Name;
            olvColumnCount.AspectGetter += (r) => ((PrintSheet)r).Count;

            objectListView1.DragSource = new SimpleDragSource();
            objectListView1.DropSink = new RearrangingDropSink(false);
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

        public void AddSheets(PrintSheet sheet, int sheetCnt)
        {
            List<PrintSheet> list = new List<PrintSheet>();
            for (int i = 0; i < sheetCnt; i++)
            {
                var l = sheet.Copy();
                list.Add(l);
            }
            objectListView1.AddObjects(list);
        }

        private void tsb_savePrintSheet_Click(object sender, EventArgs e)
        {
            if (objectListView1.Objects == null) return;
            SaveLoadService.SavePrintSheets(objectListView1.Objects.Cast<PrintSheet>().ToList());
        }

        private void tsb_loadPrintSheet_Click(object sender, EventArgs e)
        {
             List<PrintSheet> list = SaveLoadService.LoadPrintSheets();

            objectListView1.AddObjects(list);

            JustReassignPages(this,null);

        }

        private void tsb_setPlate_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            using (var form = new FormAddTemplatePlate())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.SelectedTemplatePlate == null) return;
                    foreach (PrintSheet sheet in objectListView1.SelectedObjects)
                    {
                        sheet.TemplatePlate = form.SelectedTemplatePlate;
                    }

                    objectListView1.RefreshObjects(objectListView1.SelectedObjects.Cast<object>().ToList());
                }
                
            }
        }

        private void tsb_removeTemplatePlate_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;
            foreach (PrintSheet sheet in objectListView1.SelectedObjects)
            {
                sheet.TemplatePlate = null;
            }
            objectListView1.RefreshObjects(objectListView1.SelectedObjects.Cast<object>().ToList());
        }

        private void tsb_count_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            using (var form = new FormTirag())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    foreach (PrintSheet sheet in objectListView1.SelectedObjects)
                    {
                        sheet.Count = form.Tirag;
                    }
                    objectListView1.RefreshObjects(objectListView1.SelectedObjects.Cast<object>().ToList());
                }
            }
        }

        private void tsb_loadFromOrderFolder_Click(object sender, EventArgs e)
        {
            using (var form = new VistaOpenFileDialog())
            {
                form.CheckFileExists = true;
                form.Filter = "JSON files (*.json)|*.json";
                form.FileName = Path.GetDirectoryName(_controlBindParameters.PdfFiles[0].FileName)+ "\\";

                if (form.ShowDialog() == DialogResult.OK)
                {
                    var list = SaveLoadService.LoadPrintSheets(form.FileName);
                    objectListView1.AddObjects(list);
                    JustReassignPages(this, null);
                }
            }
        }
    }
}
