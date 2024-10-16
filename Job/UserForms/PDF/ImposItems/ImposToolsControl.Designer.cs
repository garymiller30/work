namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class ImposToolsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImposToolsControl));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cb_select = new System.Windows.Forms.CheckBox();
            this.cb_rotate_180 = new System.Windows.Forms.CheckBox();
            this.tb_front = new System.Windows.Forms.TextBox();
            this.tb_back = new System.Windows.Forms.TextBox();
            this.btn_switch_front_back = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_EnableNumering = new System.Windows.Forms.CheckBox();
            this.btn_sameNumber = new System.Windows.Forms.Button();
            this.btn_listNumber = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.Controls.Add(this.cb_select, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cb_rotate_180, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tb_front, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tb_back, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.btn_switch_front_back, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.cb_EnableNumering, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.btn_sameNumber, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.btn_listNumber, 2, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(136, 290);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // cb_select
            // 
            this.cb_select.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_select.AutoSize = true;
            this.cb_select.Checked = true;
            this.cb_select.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_select.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_select.Image = ((System.Drawing.Image)(resources.GetObject("cb_select.Image")));
            this.cb_select.Location = new System.Drawing.Point(3, 3);
            this.cb_select.Name = "cb_select";
            this.cb_select.Size = new System.Drawing.Size(41, 33);
            this.cb_select.TabIndex = 0;
            this.cb_select.UseVisualStyleBackColor = true;
            this.cb_select.CheckedChanged += new System.EventHandler(this.cb_select_CheckedChanged_1);
            // 
            // cb_rotate_180
            // 
            this.cb_rotate_180.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_rotate_180.AutoSize = true;
            this.cb_rotate_180.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_rotate_180.Image = ((System.Drawing.Image)(resources.GetObject("cb_rotate_180.Image")));
            this.cb_rotate_180.Location = new System.Drawing.Point(3, 42);
            this.cb_rotate_180.Name = "cb_rotate_180";
            this.cb_rotate_180.Size = new System.Drawing.Size(41, 34);
            this.cb_rotate_180.TabIndex = 1;
            this.cb_rotate_180.UseVisualStyleBackColor = true;
            // 
            // tb_front
            // 
            this.tb_front.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_front.Location = new System.Drawing.Point(3, 239);
            this.tb_front.Name = "tb_front";
            this.tb_front.Size = new System.Drawing.Size(41, 20);
            this.tb_front.TabIndex = 2;
            this.tb_front.Text = "1";
            this.tb_front.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_back
            // 
            this.tb_back.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_back.Location = new System.Drawing.Point(90, 239);
            this.tb_back.Name = "tb_back";
            this.tb_back.Size = new System.Drawing.Size(43, 20);
            this.tb_back.TabIndex = 3;
            this.tb_back.Text = "0";
            this.tb_back.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_switch_front_back
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btn_switch_front_back, 2);
            this.btn_switch_front_back.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_switch_front_back.FlatAppearance.BorderSize = 0;
            this.btn_switch_front_back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_switch_front_back.Image = ((System.Drawing.Image)(resources.GetObject("btn_switch_front_back.Image")));
            this.btn_switch_front_back.Location = new System.Drawing.Point(50, 239);
            this.btn_switch_front_back.Name = "btn_switch_front_back";
            this.btn_switch_front_back.Size = new System.Drawing.Size(34, 21);
            this.btn_switch_front_back.TabIndex = 4;
            this.btn_switch_front_back.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 223);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "лице";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(90, 223);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "зворот";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_EnableNumering
            // 
            this.cb_EnableNumering.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_EnableNumering.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.cb_EnableNumering, 2);
            this.cb_EnableNumering.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_EnableNumering.Image = ((System.Drawing.Image)(resources.GetObject("cb_EnableNumering.Image")));
            this.cb_EnableNumering.Location = new System.Drawing.Point(50, 191);
            this.cb_EnableNumering.Name = "cb_EnableNumering";
            this.cb_EnableNumering.Size = new System.Drawing.Size(34, 29);
            this.cb_EnableNumering.TabIndex = 7;
            this.cb_EnableNumering.UseVisualStyleBackColor = true;
            // 
            // btn_sameNumber
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btn_sameNumber, 2);
            this.btn_sameNumber.Location = new System.Drawing.Point(3, 266);
            this.btn_sameNumber.Name = "btn_sameNumber";
            this.btn_sameNumber.Size = new System.Drawing.Size(61, 21);
            this.btn_sameNumber.TabIndex = 8;
            this.btn_sameNumber.Text = "1-1-1-1";
            this.btn_sameNumber.UseVisualStyleBackColor = true;
            this.btn_sameNumber.Click += new System.EventHandler(this.btn_sameNumber_Click);
            // 
            // btn_listNumber
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btn_listNumber, 2);
            this.btn_listNumber.Location = new System.Drawing.Point(70, 266);
            this.btn_listNumber.Name = "btn_listNumber";
            this.btn_listNumber.Size = new System.Drawing.Size(63, 21);
            this.btn_listNumber.TabIndex = 9;
            this.btn_listNumber.Text = "1-2-3-4";
            this.btn_listNumber.UseVisualStyleBackColor = true;
            this.btn_listNumber.Click += new System.EventHandler(this.btn_listNumber_Click);
            // 
            // ImposToolsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ImposToolsControl";
            this.Size = new System.Drawing.Size(136, 290);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox cb_select;
        private System.Windows.Forms.CheckBox cb_rotate_180;
        private System.Windows.Forms.TextBox tb_front;
        private System.Windows.Forms.TextBox tb_back;
        private System.Windows.Forms.Button btn_switch_front_back;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cb_EnableNumering;
        private System.Windows.Forms.Button btn_sameNumber;
        private System.Windows.Forms.Button btn_listNumber;
    }
}
