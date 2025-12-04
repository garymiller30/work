namespace JobSpace.UserForms
{
    partial class FormRegexRenameFiles
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_find = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_replace = new System.Windows.Forms.TextBox();
            this.olv_find = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn_name = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_new_name = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btn_rename = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olv_find)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_find);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(241, 47);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "пошук";
            // 
            // tb_find
            // 
            this.tb_find.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_find.Location = new System.Drawing.Point(6, 19);
            this.tb_find.Name = "tb_find";
            this.tb_find.Size = new System.Drawing.Size(229, 20);
            this.tb_find.TabIndex = 0;
            this.tb_find.TextChanged += new System.EventHandler(this.tb_find_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_replace);
            this.groupBox2.Location = new System.Drawing.Point(259, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(241, 47);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "заміна";
            // 
            // tb_replace
            // 
            this.tb_replace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_replace.Location = new System.Drawing.Point(6, 19);
            this.tb_replace.Name = "tb_replace";
            this.tb_replace.Size = new System.Drawing.Size(229, 20);
            this.tb_replace.TabIndex = 0;
            this.tb_replace.Text = "$0#$1";
            this.tb_replace.TextChanged += new System.EventHandler(this.tb_replace_TextChanged);
            // 
            // olv_find
            // 
            this.olv_find.AllColumns.Add(this.olvColumn_name);
            this.olv_find.AllColumns.Add(this.olvColumn_new_name);
            this.olv_find.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olv_find.CellEditUseWholeCell = false;
            this.olv_find.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn_name,
            this.olvColumn_new_name});
            this.olv_find.Cursor = System.Windows.Forms.Cursors.Default;
            this.olv_find.FullRowSelect = true;
            this.olv_find.GridLines = true;
            this.olv_find.HideSelection = false;
            this.olv_find.Location = new System.Drawing.Point(12, 113);
            this.olv_find.Name = "olv_find";
            this.olv_find.ShowGroups = false;
            this.olv_find.Size = new System.Drawing.Size(488, 282);
            this.olv_find.TabIndex = 1;
            this.olv_find.UseCompatibleStateImageBehavior = false;
            this.olv_find.UseFiltering = true;
            this.olv_find.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn_name
            // 
            this.olvColumn_name.AspectName = "OriginalName";
            this.olvColumn_name.Text = "Ім\'я";
            this.olvColumn_name.Width = 244;
            // 
            // olvColumn_new_name
            // 
            this.olvColumn_new_name.AspectName = "NewName";
            this.olvColumn_new_name.Searchable = false;
            this.olvColumn_new_name.Text = "Нове ім\'я";
            this.olvColumn_new_name.UseFiltering = false;
            this.olvColumn_new_name.Width = 237;
            // 
            // btn_rename
            // 
            this.btn_rename.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_rename.Location = new System.Drawing.Point(202, 401);
            this.btn_rename.Name = "btn_rename";
            this.btn_rename.Size = new System.Drawing.Size(113, 38);
            this.btn_rename.TabIndex = 2;
            this.btn_rename.Text = "перейменувати";
            this.btn_rename.UseVisualStyleBackColor = true;
            this.btn_rename.Click += new System.EventHandler(this.btn_rename_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(15, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(485, 48);
            this.label1.TabIndex = 3;
            this.label1.Text = "$0 - оригінальне ім\'я\r\n$1 і більше - знайдене співпадіння";
            // 
            // FormRegexRenameFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 451);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_rename);
            this.Controls.Add(this.olv_find);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormRegexRenameFiles";
            this.Text = "FormRegexRenameFiles";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olv_find)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.ObjectListView olv_find;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_find;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tb_replace;
        private BrightIdeasSoftware.OLVColumn olvColumn_name;
        private BrightIdeasSoftware.OLVColumn olvColumn_new_name;
        private System.Windows.Forms.Button btn_rename;
        private System.Windows.Forms.Label label1;
    }
}