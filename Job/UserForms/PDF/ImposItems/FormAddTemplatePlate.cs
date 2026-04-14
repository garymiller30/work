using JobSpace.Profiles;
using JobSpace.Static.Pdf.Imposition.Models;
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
    public partial class FormAddTemplatePlate : Form
    {
        public TemplatePlate SelectedTemplatePlate { get; private set; }
        Profile _profile;
        public FormAddTemplatePlate(Profile profile)
        {
            _profile = profile;
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            objectListView1.AddObjects(LoadForms());
        }

        private ICollection LoadForms()
        {
            return _profile.ImposService.LoadTemplatePates();
        }

        private void btn_saveToList_Click(object sender, EventArgs e)
        {
            UnbindParameters();
        }

        private void UnbindParameters()
        {
            if (string.IsNullOrEmpty(tb_name.Text))
            {
                MessageBox.Show("Name is empty");
                return;
            }

            bool isNew = false;

            if (SelectedTemplatePlate == null)
            {
                SelectedTemplatePlate = new TemplatePlate();
                isNew = true;
            }

            if (tb_name.Text != SelectedTemplatePlate.Name)
            {
                isNew = true;             
            }

            SelectedTemplatePlate.Name = tb_name.Text;
            SelectedTemplatePlate.W = (double)nud_w.Value;
            SelectedTemplatePlate.H = (double)nud_h.Value;
            SelectedTemplatePlate.Xofs = (double)nud_xOfs.Value;
            SelectedTemplatePlate.Yofs = (double)nud_yOfs.Value;
            SelectedTemplatePlate.IsCenterHorizontal = cb_centerX.Checked;
            SelectedTemplatePlate.IsCenterVertical = cb_centerY.Checked;

            if (isNew)
            {
                objectListView1.AddObject(SelectedTemplatePlate);
            }
            else
            {
                objectListView1.RefreshObject(SelectedTemplatePlate);
            }

            _profile.ImposService.SaveTemplatePlates(objectListView1.Objects.Cast<TemplatePlate>().ToList());
        }

        private void objectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplatePlate templatePlate)
            {
                SelectedTemplatePlate = templatePlate;

                BindParameters();
            }
        }

        private void BindParameters()
        {
            cb_centerX.Checked = SelectedTemplatePlate.IsCenterHorizontal;
            cb_centerY.Checked = SelectedTemplatePlate.IsCenterVertical;
            nud_h.Value = (decimal)SelectedTemplatePlate.H;
            nud_w.Value = (decimal)SelectedTemplatePlate.W;
            nud_xOfs.Value = (decimal)SelectedTemplatePlate.Xofs;
            nud_yOfs.Value = (decimal)SelectedTemplatePlate.Yofs;
            tb_name.Text = SelectedTemplatePlate.Name;
        }

        private void bnt_ok_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplatePlate templatePlate)
            {
                SelectedTemplatePlate = templatePlate;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Select template plate");
            }
            return;
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is TemplatePlate templatePlate)
            {
                objectListView1.RemoveObject(templatePlate);
                SelectedTemplatePlate = null;
                _profile.ImposService.SaveTemplatePlates(objectListView1.Objects.Cast<TemplatePlate>().ToList());
            }
        }

        private void nud_w_ValueChanged(object sender, EventArgs e)
        {
            UpdateName();
        }

        private void UpdateName()
        {
            if (cb_autoName.Checked)
            {

                tb_name.Text = $"{nud_w.Value}x{nud_h.Value}";

                if (cb_centerX.Checked)
                {
                    tb_name.Text += "_cx";
                    if (nud_xOfs.Value > 0)
                        tb_name.Text += $"{nud_xOfs.Value}";
                    
                }
                else
                {
                    tb_name.Text += $"_x{nud_xOfs.Value}";
                }

                if (cb_centerY.Checked)
                {
                    tb_name.Text += "_cy";
                    if (nud_yOfs.Value > 0)
                        tb_name.Text += $"{nud_yOfs.Value}";
                }
                else
                {
                    tb_name.Text += $"_y{nud_yOfs.Value}";
                }
            }
        }

        private void cb_centerX_CheckedChanged(object sender, EventArgs e)
        {
            UpdateName();
        }
    }
}
