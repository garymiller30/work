namespace OrderInfo
{
    partial class WindowOut
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
            if (!disposing)
            {
                return;
            }

            if (ucNote1 != null)
            {
                ucNote1.OnLeaveControl -= UcNote1_OnLeave;
            }

            RemoveCheckedEvents();

            if (_isSubscribed && ucAddWorkPluginsContainer1 != null)
            {
                ucAddWorkPluginsContainer1.Unsubscribe(UserProfile);
                _isSubscribed = false;
            }

            if (components != null)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowOut));
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            ucNote1 = new JobSpace.UC.UcNote();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            cb_cut = new System.Windows.Forms.CheckBox();
            cb_uv_lak = new System.Windows.Forms.CheckBox();
            cb_protected_lak = new System.Windows.Forms.CheckBox();
            cb_klishe = new System.Windows.Forms.CheckBox();
            ucAddWorkPluginsContainer1 = new JobSpace.UC.UcAddWorkPluginsContainer();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(ucAddWorkPluginsContainer1);
            splitContainer1.Size = new System.Drawing.Size(412, 509);
            splitContainer1.SplitterDistance = 348;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(412, 345);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            panel1.Controls.Add(ucNote1);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(4, 34);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(404, 308);
            panel1.TabIndex = 5;
            // 
            // ucNote1
            // 
            ucNote1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ucNote1.AutoScroll = true;
            ucNote1.Location = new System.Drawing.Point(4, 3);
            ucNote1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            ucNote1.Name = "ucNote1";
            ucNote1.Size = new System.Drawing.Size(397, 301);
            ucNote1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(cb_cut);
            flowLayoutPanel1.Controls.Add(cb_uv_lak);
            flowLayoutPanel1.Controls.Add(cb_protected_lak);
            flowLayoutPanel1.Controls.Add(cb_klishe);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.Location = new System.Drawing.Point(4, 3);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(404, 25);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // cb_cut
            // 
            cb_cut.AutoSize = true;
            cb_cut.Location = new System.Drawing.Point(4, 3);
            cb_cut.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_cut.Name = "cb_cut";
            cb_cut.Size = new System.Drawing.Size(96, 19);
            cb_cut.TabIndex = 0;
            cb_cut.Text = "контур ножа";
            cb_cut.UseVisualStyleBackColor = true;
            // 
            // cb_uv_lak
            // 
            cb_uv_lak.AutoSize = true;
            cb_uv_lak.Location = new System.Drawing.Point(108, 3);
            cb_uv_lak.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_uv_lak.Name = "cb_uv_lak";
            cb_uv_lak.Size = new System.Drawing.Size(64, 19);
            cb_uv_lak.TabIndex = 1;
            cb_uv_lak.Text = "УФ лак";
            cb_uv_lak.UseVisualStyleBackColor = true;
            // 
            // cb_protected_lak
            // 
            cb_protected_lak.AutoSize = true;
            cb_protected_lak.Location = new System.Drawing.Point(180, 3);
            cb_protected_lak.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_protected_lak.Name = "cb_protected_lak";
            cb_protected_lak.Size = new System.Drawing.Size(99, 19);
            cb_protected_lak.TabIndex = 2;
            cb_protected_lak.Text = "захисний лак";
            cb_protected_lak.UseVisualStyleBackColor = true;
            // 
            // cb_klishe
            // 
            cb_klishe.AutoSize = true;
            cb_klishe.Location = new System.Drawing.Point(287, 3);
            cb_klishe.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_klishe.Name = "cb_klishe";
            cb_klishe.Size = new System.Drawing.Size(59, 19);
            cb_klishe.TabIndex = 3;
            cb_klishe.Text = "кліше";
            cb_klishe.UseVisualStyleBackColor = true;
            // 
            // ucAddWorkPluginsContainer1
            // 
            ucAddWorkPluginsContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            ucAddWorkPluginsContainer1.Location = new System.Drawing.Point(0, 0);
            ucAddWorkPluginsContainer1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            ucAddWorkPluginsContainer1.Name = "ucAddWorkPluginsContainer1";
            ucAddWorkPluginsContainer1.Size = new System.Drawing.Size(412, 156);
            ucAddWorkPluginsContainer1.TabIndex = 0;
            // 
            // WindowOut
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "WindowOut";
            Size = new System.Drawing.Size(412, 509);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private JobSpace.UC.UcAddWorkPluginsContainer ucAddWorkPluginsContainer1;
        private System.Windows.Forms.CheckBox cb_protected_lak;
        private System.Windows.Forms.CheckBox cb_uv_lak;
        private System.Windows.Forms.CheckBox cb_cut;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox cb_klishe;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private JobSpace.UC.UcNote ucNote1;
    }
}
