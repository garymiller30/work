namespace JobSpace.UserForms
{
    partial class FormEnterTirag
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            objectListView1 = new BrightIdeasSoftware.ObjectListView();
            olvColumn_name = new BrightIdeasSoftware.OLVColumn();
            olvColumn_tirag = new BrightIdeasSoftware.OLVColumn();
            btn_ok = new System.Windows.Forms.Button();
            btn_set_tirag = new System.Windows.Forms.Button();
            nud_tirag = new System.Windows.Forms.NumericUpDown();
            btn_paste = new System.Windows.Forms.Button();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            l_total = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            txt_filter = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            uc_FilePreviewControl1 = new JobSpace.UC.Uc_FilePreviewControl();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)objectListView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_tirag).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // objectListView1
            // 
            objectListView1.AllColumns.Add(olvColumn_name);
            objectListView1.AllColumns.Add(olvColumn_tirag);
            objectListView1.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            objectListView1.CellEditUseWholeCell = false;
            objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { olvColumn_name, olvColumn_tirag });
            objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            objectListView1.FullRowSelect = true;
            objectListView1.GridLines = true;
            objectListView1.Location = new System.Drawing.Point(0, 0);
            objectListView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            objectListView1.Name = "objectListView1";
            objectListView1.ShowGroups = false;
            objectListView1.Size = new System.Drawing.Size(577, 491);
            objectListView1.TabIndex = 2;
            objectListView1.UseCompatibleStateImageBehavior = false;
            objectListView1.UseFiltering = true;
            objectListView1.View = System.Windows.Forms.View.Details;
            objectListView1.CellEditFinished += objectListView1_CellEditFinished;
            objectListView1.SelectedIndexChanged += objectListView1_SelectedIndexChanged;
            // 
            // olvColumn_name
            // 
            olvColumn_name.AspectName = "FileInfo.FileInfo.Name";
            olvColumn_name.IsEditable = false;
            olvColumn_name.Text = "Ім'я файлу";
            olvColumn_name.Width = 300;
            // 
            // olvColumn_tirag
            // 
            olvColumn_tirag.AspectName = "Tirag";
            olvColumn_tirag.CellEditUseWholeCell = true;
            olvColumn_tirag.Text = "Тираж";
            olvColumn_tirag.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            olvColumn_tirag.Width = 108;
            // 
            // btn_ok
            // 
            btn_ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            btn_ok.Location = new System.Drawing.Point(401, 590);
            btn_ok.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_ok.Name = "btn_ok";
            btn_ok.Size = new System.Drawing.Size(135, 43);
            btn_ok.TabIndex = 3;
            btn_ok.Text = "OK";
            btn_ok.UseVisualStyleBackColor = true;
            btn_ok.Click += btn_ok_Click;
            // 
            // btn_set_tirag
            // 
            btn_set_tirag.Location = new System.Drawing.Point(120, 17);
            btn_set_tirag.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_set_tirag.Name = "btn_set_tirag";
            btn_set_tirag.Size = new System.Drawing.Size(169, 27);
            btn_set_tirag.TabIndex = 1;
            btn_set_tirag.Text = "примінити до вибраних";
            btn_set_tirag.UseVisualStyleBackColor = true;
            btn_set_tirag.Click += btn_set_tirag_Click;
            // 
            // nud_tirag
            // 
            nud_tirag.Location = new System.Drawing.Point(14, 18);
            nud_tirag.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_tirag.Maximum = new decimal(new int[] { 276447232, 23283, 0, 0 });
            nud_tirag.Name = "nud_tirag";
            nud_tirag.Size = new System.Drawing.Size(99, 23);
            nud_tirag.TabIndex = 0;
            // 
            // btn_paste
            // 
            btn_paste.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btn_paste.Location = new System.Drawing.Point(766, 14);
            btn_paste.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_paste.Name = "btn_paste";
            btn_paste.Size = new System.Drawing.Size(149, 27);
            btn_paste.TabIndex = 4;
            btn_paste.Text = "вставити з буфера";
            btn_paste.UseVisualStyleBackColor = true;
            btn_paste.Click += btn_paste_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(l_total);
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new System.Drawing.Point(14, 547);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(903, 36);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // l_total
            // 
            l_total.AutoSize = true;
            l_total.Location = new System.Drawing.Point(886, 0);
            l_total.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            l_total.Name = "l_total";
            l_total.Size = new System.Drawing.Size(13, 15);
            l_total.TabIndex = 1;
            l_total.Text = "0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(834, 0);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(48, 15);
            label1.TabIndex = 0;
            label1.Text = "Всього:";
            // 
            // txt_filter
            // 
            txt_filter.Location = new System.Drawing.Point(390, 20);
            txt_filter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txt_filter.Name = "txt_filter";
            txt_filter.Size = new System.Drawing.Size(116, 23);
            txt_filter.TabIndex = 6;
            txt_filter.TextChanged += txt_filter_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(336, 25);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(44, 15);
            label2.TabIndex = 7;
            label2.Text = "фільтр";
            // 
            // uc_FilePreviewControl1
            // 
            uc_FilePreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            uc_FilePreviewControl1.Location = new System.Drawing.Point(0, 0);
            uc_FilePreviewControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            uc_FilePreviewControl1.Name = "uc_FilePreviewControl1";
            uc_FilePreviewControl1.Size = new System.Drawing.Size(322, 491);
            uc_FilePreviewControl1.TabIndex = 8;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            splitContainer1.Location = new System.Drawing.Point(14, 50);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(objectListView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(uc_FilePreviewControl1);
            splitContainer1.Size = new System.Drawing.Size(903, 491);
            splitContainer1.SplitterDistance = 577;
            splitContainer1.TabIndex = 9;
            // 
            // FormEnterTirag
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(929, 646);
            Controls.Add(splitContainer1);
            Controls.Add(label2);
            Controls.Add(txt_filter);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(btn_paste);
            Controls.Add(nud_tirag);
            Controls.Add(btn_set_tirag);
            Controls.Add(btn_ok);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormEnterTirag";
            ShowIcon = false;
            Text = "Виставити тиражі";
            ((System.ComponentModel.ISupportInitialize)objectListView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_tirag).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private System.Windows.Forms.Button btn_ok;
        private BrightIdeasSoftware.OLVColumn olvColumn_name;
        private BrightIdeasSoftware.OLVColumn olvColumn_tirag;
        private System.Windows.Forms.Button btn_set_tirag;
        private System.Windows.Forms.NumericUpDown nud_tirag;
        private System.Windows.Forms.Button btn_paste;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label l_total;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_filter;
        private System.Windows.Forms.Label label2;
        private UC.Uc_FilePreviewControl uc_FilePreviewControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}