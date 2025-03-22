﻿namespace JobSpace.UserForms
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
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn_name = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_tirag = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_set_tirag = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btn_paste = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumn_name);
            this.objectListView1.AllColumns.Add(this.olvColumn_tirag);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn_name,
            this.olvColumn_tirag});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(12, 44);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(432, 320);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.CellEditFinished += new BrightIdeasSoftware.CellEditEventHandler(this.objectListView1_CellEditFinished);
            // 
            // olvColumn_name
            // 
            this.olvColumn_name.AspectName = "FileInfo.FileInfo.Name";
            this.olvColumn_name.IsEditable = false;
            this.olvColumn_name.Text = "Ім\'я файлу";
            this.olvColumn_name.Width = 300;
            // 
            // olvColumn_tirag
            // 
            this.olvColumn_tirag.AspectName = "Tirag";
            this.olvColumn_tirag.CellEditUseWholeCell = true;
            this.olvColumn_tirag.Text = "Тираж";
            this.olvColumn_tirag.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumn_tirag.Width = 108;
            // 
            // btn_ok
            // 
            this.btn_ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_ok.Location = new System.Drawing.Point(174, 396);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(116, 37);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_set_tirag
            // 
            this.btn_set_tirag.Location = new System.Drawing.Point(103, 15);
            this.btn_set_tirag.Name = "btn_set_tirag";
            this.btn_set_tirag.Size = new System.Drawing.Size(145, 23);
            this.btn_set_tirag.TabIndex = 2;
            this.btn_set_tirag.Text = "примінити до вибраних";
            this.btn_set_tirag.UseVisualStyleBackColor = true;
            this.btn_set_tirag.Click += new System.EventHandler(this.btn_set_tirag_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(12, 16);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            276447232,
            23283,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(85, 20);
            this.numericUpDown1.TabIndex = 3;
            // 
            // btn_paste
            // 
            this.btn_paste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_paste.Location = new System.Drawing.Point(316, 12);
            this.btn_paste.Name = "btn_paste";
            this.btn_paste.Size = new System.Drawing.Size(128, 23);
            this.btn_paste.TabIndex = 4;
            this.btn_paste.Text = "вставити з буфера";
            this.btn_paste.UseVisualStyleBackColor = true;
            this.btn_paste.Click += new System.EventHandler(this.btn_paste_Click);
            // 
            // FormEnterTirag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 445);
            this.Controls.Add(this.btn_paste);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.btn_set_tirag);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.objectListView1);
            this.Name = "FormEnterTirag";
            this.ShowIcon = false;
            this.Text = "Виставити тиражі";
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private System.Windows.Forms.Button btn_ok;
        private BrightIdeasSoftware.OLVColumn olvColumn_name;
        private BrightIdeasSoftware.OLVColumn olvColumn_tirag;
        private System.Windows.Forms.Button btn_set_tirag;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btn_paste;
    }
}