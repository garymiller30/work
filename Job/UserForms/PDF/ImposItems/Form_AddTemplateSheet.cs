using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding;
using JobSpace.Static.Pdf.Imposition.Services.Impos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JobSpace.Ext;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class Form_AddTemplateSheet : Form
    {
        public TemplateSheet Sheet { get; set; }
        //ControlBindParameters _parameters;
        ImposToolsParameters _imposParameters = new ImposToolsParameters();

        public Form_AddTemplateSheet(ControlBindParameters parameters)
        {
            InitializeComponent();
            //_parameters = parameters;
            InitSheets();
            DialogResult = DialogResult.Cancel;
            Sheet = TemplateSheet.Create();
            Sheet.MasterPage = parameters.MasterPage.Copy();
            
            SetSheetToIU();
            InitUIEvents();

        }

        public Form_AddTemplateSheet(TemplateSheet sheet)
        {
            InitializeComponent();
            InitSheets();
            Sheet = sheet;
            DialogResult = DialogResult.Cancel;
           
            SetSheetToIU();
            InitUIEvents();

            

        }

        private void SetSheetToIU()
        {
            comboBoxSheetPlaceType.SelectedIndex = (int)Sheet.SheetPlaceType;
            
        }

        void InitUIEvents()
        {
            buttonAddSheet.Click += buttonAddSheet_Click;
            buttonEditSheet.Click += buttonEditSheet_Click;
        }

   
       

        private void InitSheets()
        {
            //comboBoxSheetPlaceType.DataSource = Extensions.GetDescriptions(typeof(TemplateSheetPlaceType));

            //var sheets = ImposSaveLoadService.LoadSheets();
            //if (sheets.Any())
            //{
            //    comboBoxSheets.Items.AddRange(sheets.ToArray());
            //}
        }
        private void AddSheet()
        {
            //using (var form = new FormAddSheet())
            //{
            //    if (form.ShowDialog() == DialogResult.OK)
            //    {
            //        ImposSaveLoadService.SaveSheet(form.Sheet);
            //        comboBoxSheets.Items.Add(form.Sheet);
            //    }
            //}
        }
        private void buttonAddSheet_Click(object sender, EventArgs e)
        {
            AddSheet();
        }

        private void buttonEditSheet_Click(object sender, EventArgs e)
        {
            EditSheet();
        }
        private void EditSheet()
        {
            //if (comboBoxSheets.SelectedItem is TemplateSheet sheet)
            //{
            //    using (var form = new FormAddSheet(sheet))
            //    {
            //        string old_desc = sheet.Description;

            //        if (form.ShowDialog() == DialogResult.OK)
            //        {
            //            if (old_desc != sheet.Description)
            //            {
            //                ImposSaveLoadService.SaveSheet(form.Sheet);
            //                comboBoxSheets.Items.Add(form.Sheet);
            //            }
            //            else
            //            {
            //                ImposSaveLoadService.SaveSheet(form.Sheet);
            //            }
            //        }
            //    }
            //}
        }
      

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBoxSheets.SelectedItem is TemplateSheet sheet)
            {
                Sheet = sheet.Copy();
                Sheet.Description = $"[{Sheet.W}x{Sheet.H}]";
                Sheet.SheetPlaceType = (TemplateSheetPlaceType)comboBoxSheetPlaceType.SelectedIndex;
                DialogResult = DialogResult.OK;
                Close();
            }
        }


        private void btn_delSheet_Click(object sender, EventArgs e)
        {
            //if (comboBoxSheets.SelectedItem is TemplateSheet sheet)
            //{
            //    if (ImposSaveLoadService.DeleteSheet(sheet)) comboBoxSheets.Items.Remove(sheet);
            //}
        }
    }
}
