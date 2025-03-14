using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.ImposItems
{


    public partial class ImposToolsControl : UserControl
    {
        ImposToolsParameters parameters;
        public ImposToolsControl()
        {
            InitializeComponent();

            tb_front.MouseClick += tb_front_MouseClick;
            tb_back.MouseClick += tb_back_MouseClick;
            rb_select.CheckedChanged += Rb_select_CheckedChanged;
            rb_rotate_180.CheckedChanged += cb_rotate_180_CheckedChanged;
            rb_EnableNumering.CheckedChanged += cb_EnableNumering_CheckedChanged;
            rb_deletePage.CheckedChanged += Rb_deletePage_CheckedChanged;
            tb_front.TextChanged += tb_front_TextChanged;
            tb_back.TextChanged += tb_back_TextChanged;
            rb_centerH.Click += Rb_centerH_CheckedChanged;
            rb_centerV.Click += Rb_centerV_CheckedChanged;
            btn_switch_front_back.Click += btn_switch_front_back_Click;
            rb_switchHW.CheckedChanged += Rb_switchHW_CheckedChanged;
            rb_add_page_to_group.CheckedChanged += Rb_add_page_to_group_CheckedChanged;

            tb_front.DataBindings.Add("Enabled", rb_EnableNumering, "Checked");
            tb_back.DataBindings.Add("Enabled", rb_EnableNumering, "Checked");
            btn_switch_front_back.DataBindings.Add("Enabled", rb_EnableNumering, "Checked");
            btn_sameNumber.DataBindings.Add("Enabled", rb_EnableNumering, "Checked");
            btn_listNumber.DataBindings.Add("Enabled", rb_EnableNumering, "Checked");

        }

        private void Rb_add_page_to_group_CheckedChanged(object sender, EventArgs e)
        {
            bool check = rb_add_page_to_group.Checked;
            if (check)
            {
                parameters.CurTool = ImposToolEnum.AddPageToGroup;
            }
        }

        private void Rb_switchHW_CheckedChanged(object sender, EventArgs e)
        {
            bool check = rb_switchHW.Checked;
            if (check)
            {
                parameters.CurTool = ImposToolEnum.SwitchHW;
            }
        }

        private void Rb_centerV_CheckedChanged(object sender, EventArgs e)
        {
            parameters.OnClickCenterV(this, null);
        }

        private void Rb_centerH_CheckedChanged(object sender, EventArgs e)
        {
            parameters.OnClickCenterH(this, null);
        }

        private void Rb_select_CheckedChanged(object sender, EventArgs e)
        {
            bool check = rb_select.Checked;
            if (check)
            {
                parameters.CurTool = ImposToolEnum.Select;
            }
        }

        private void Rb_deletePage_CheckedChanged(object sender, EventArgs e)
        {
            bool check = rb_deletePage.Checked;
            if (check)
            {
                parameters.CurTool = ImposToolEnum.DeletePage;
            }
        }

        public void InitParameters(ImposToolsParameters param)
        {
            parameters = param;
            parameters.BackNumChanged += delegate (object sender, int num)
            {
                tb_back.Text = num.ToString();
            };

            parameters.FrontNumChanged += delegate (object sender, int num)
            {
                tb_front.Text = num.ToString();
            };

            nud_cropLen.Value = (decimal)parameters.CropMarksParameters.Len;
            nud_cropDist.Value = (decimal)parameters.CropMarksParameters.Distance;
        }

        private void tb_front_MouseClick(object sender, MouseEventArgs e)
        {
            tb_front.SelectAll();
        }

        private void tb_back_MouseClick(object sender, MouseEventArgs e)
        {
            tb_back.SelectAll();
        }

        private void cb_rotate_180_CheckedChanged(object sender, EventArgs e)
        {
            bool check = rb_rotate_180.Checked;
            if (check)
            {
                parameters.CurTool = ImposToolEnum.FlipAngle;
            }
        }

        private void cb_EnableNumering_CheckedChanged(object sender, EventArgs e)
        {
            bool check = rb_EnableNumering.Checked;
            if (check)
            {
                parameters.CurTool = ImposToolEnum.Numeration;
            }
        }

        private void tb_front_TextChanged(object sender, EventArgs e)
        {
            bool res = int.TryParse(tb_front.Text, out int val);
            if (res)
            {
                parameters.FrontNum = val;
            }
        }

        private void tb_back_TextChanged(object sender, EventArgs e)
        {
            bool res = int.TryParse(tb_back.Text, out int val);
            if (res)
            {
                parameters.BackNum = val;
            }
        }

        private void btn_switch_front_back_Click(object sender, EventArgs e)
        {
            (tb_front.Text, tb_back.Text) = (tb_back.Text, tb_front.Text);
        }

        private void btn_sameNumber_Click(object sender, EventArgs e)
        {
            parameters.OnTheSameNumberClick(sender, e);
        }

        private void btn_listNumber_Click(object sender, EventArgs e)
        {
            parameters.OnListNumberClick(sender, e);
        }

        private void btn_ApplyCropMark_Click(object sender, EventArgs e)
        {
            parameters.CropMarksParameters.Distance = (double)nud_cropDist.Value;
            parameters.CropMarksParameters.Len = (double)nud_cropLen.Value;

            parameters.OnCropMarksChanged(this, null);
        }

        private void btn_left_Click(object sender, EventArgs e)
        {
            parameters.OnMoveLeftClick(this, 1);
        }

        private void btn_right_Click(object sender, EventArgs e)
        {
            parameters.OnMoveRightClick(this, 1);
        }

        private void btn_up_Click(object sender, EventArgs e)
        {
            parameters.OnMoveUpClick(this, 1);
        }

        private void btn_down_Click(object sender, EventArgs e)
        {
            parameters.OnMoveDownClick(this, 1);
        }

        private void btn_rotateLeft_Click(object sender, EventArgs e)
        {
            parameters.OnRotateLeft(this, null);
        }

        private void btn_rotateRight_Click(object sender, EventArgs e)
        {
            parameters.OnRotateRight(this, null);
        }

        private void b_switchWH_Click(object sender, EventArgs e)
        {
            parameters.OnSwitchWH(this, null);
        }

        private void btn_add_group_Click(object sender, EventArgs e)
        {
            if (olv_groups.GetItemCount() == 0)
            {
                olv_groups.AddObject(PageGroupsService.CreateGroup( 1));
            }
            else
            {
                olv_groups.AddObject(PageGroupsService.CreateGroup(olv_groups.Objects.Cast<PageGroup>().Max(x => x.Id) + 1));
            }
            
        }

        private void olv_groups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (olv_groups.SelectedObject != null)
            {
                parameters.CurGroup = (olv_groups.SelectedObject as PageGroup).Id;
            }
            else
            {
                parameters.CurGroup = 0;
            }
        }

        private void btn_distribute_hor_Click(object sender, EventArgs e)
        {
            if (olv_groups.SelectedObjects.Count < 2) return;

            parameters.OnPageGroupDistributeHor(this,olv_groups.SelectedObjects.Cast<PageGroup>().ToList());
        }
    }
}
