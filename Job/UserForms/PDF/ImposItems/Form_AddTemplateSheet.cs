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

            previewControl1.SetSheet(Sheet);

        }

        private void SetSheetToIU()
        {
            comboBoxSheetPlaceType.SelectedIndex = (int)Sheet.SheetPlaceType;
            nud_info_w.Value = (decimal)Sheet.W;
            nud_info_h.Value = (decimal)Sheet.H;
            nud_info_extraSpace.Value = (decimal)(Sheet.ExtraSpace);
            nud_info_fieldTop.Value = (decimal)Sheet.SafeFields.Top;
            nud_info_fieldBottom.Value = (decimal)Sheet.SafeFields.Bottom;
            nud_info_fieldLeft.Value = (decimal)Sheet.SafeFields.Left;
            nud_info_fieldRight.Value = (decimal)Sheet.SafeFields.Right;

            nud_page_w.Value = (decimal)Sheet.MasterPage.W;
            nud_page_h.Value = (decimal)Sheet.MasterPage.H;
            nud_page_bleed.Value = (decimal)Sheet.MasterPage.Bleeds;
            
        }

        void InitUIEvents()
        {
            buttonAddSheet.Click += buttonAddSheet_Click;
            buttonEditSheet.Click += buttonEditSheet_Click;
            comboBoxSheets.SelectedIndexChanged += comboBoxSheets_SelectedIndexChanged;
            

            nud_info_w.ValueChanged += ValueChanged;
            nud_info_h.ValueChanged += ValueChanged;
            nud_info_extraSpace.ValueChanged += ValueChanged;
            nud_info_fieldTop.ValueChanged += ValueChanged;
            nud_info_fieldBottom.ValueChanged += ValueChanged;
            nud_info_fieldLeft.ValueChanged += ValueChanged;
            nud_info_fieldRight.ValueChanged += ValueChanged;
            nud_page_bleed.ValueChanged += ValueChanged;
            nud_page_w.ValueChanged += ValueChanged;
            nud_page_h.ValueChanged += ValueChanged;
            nud_Xofs.ValueChanged += ValueChanged;
            nud_Yofs.ValueChanged += ValueChanged;

            comboBoxSheetPlaceType.SelectedIndexChanged+= ValueChanged;
            cb_centerHeight.CheckedChanged+= ValueChanged;
            cb_centerWidth.CheckedChanged+= ValueChanged;

            InitPreview();
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void InitPreview()
        {
            previewControl1.InitBindParameters(_imposParameters);
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
            Sheet.SheetPlaceType = (TemplateSheetPlaceType)comboBoxSheetPlaceType.SelectedIndex;
            Sheet.W = (double)nud_info_w.Value;
            Sheet.H = (double)nud_info_h.Value;
            Sheet.ExtraSpace = (double)nud_info_extraSpace.Value;
            Sheet.SafeFields.Top = (double)nud_info_fieldTop.Value;
            Sheet.SafeFields.Bottom = (double)nud_info_fieldBottom.Value;
            Sheet.SafeFields.Left = (double)nud_info_fieldLeft.Value;
            Sheet.SafeFields.Right = (double)nud_info_fieldRight.Value;

            Sheet.MasterPage.W = (double)nud_page_w.Value;
            Sheet.MasterPage.H = (double)nud_page_h.Value;
            Sheet.MasterPage.Bleeds = (double)nud_page_bleed.Value;
            Sheet.MasterPage.SetMarginsLikeBleed();
            

            LooseBindingParameters bindingParameters = new LooseBindingParameters();
            bindingParameters.IsCenterHorizontal = cb_centerWidth.Checked;
            bindingParameters.IsCenterVertical = cb_centerHeight.Checked;
            bindingParameters.Xofs = (double)nud_Xofs.Value;
            bindingParameters.Yofs = (double)nud_Yofs.Value;
            bindingParameters.Sheet = Sheet;

            Sheet.TemplatePageContainer = BindingService.Impos(bindingParameters);
            Sheet.Description = $"[{Sheet.W}x{Sheet.H}]";

            previewControl1.SetSheet(Sheet);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();            
        }
    }
}
