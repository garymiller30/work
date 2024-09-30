namespace JobSpace.UserForms.PDF
{
    partial class FormPdfImposition
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSimple = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btn_SaveToPdf = new System.Windows.Forms.Button();
            this.pdfFileListControl1 = new JobSpace.UserForms.PDF.ImposItems.PdfFileListControl();
            this.previewControl1 = new JobSpace.UserForms.PDF.ImposItems.PreviewControl();
            this.addTemplateSheetControl1 = new JobSpace.UserForms.PDF.ImposItems.AddTemplateSheetControl();
            this.printSheetsControl1 = new JobSpace.UserForms.PDF.ImposItems.PrintSheetsControl();
            this.runListControl1 = new JobSpace.UserForms.PDF.ImposItems.RunListControl();
            this.tabControl1.SuspendLayout();
            this.tabPageSimple.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSimple);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1075, 633);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPageSimple
            // 
            this.tabPageSimple.Controls.Add(this.splitContainer1);
            this.tabPageSimple.Location = new System.Drawing.Point(4, 22);
            this.tabPageSimple.Name = "tabPageSimple";
            this.tabPageSimple.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSimple.Size = new System.Drawing.Size(1067, 607);
            this.tabPageSimple.TabIndex = 0;
            this.tabPageSimple.Text = "простий";
            this.tabPageSimple.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pdfFileListControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.previewControl1);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(1061, 601);
            this.splitContainer1.SplitterDistance = 205;
            this.splitContainer1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.Controls.Add(this.splitContainer2);
            this.panel2.Controls.Add(this.runListControl1);
            this.panel2.Controls.Add(this.btn_SaveToPdf);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(468, 593);
            this.panel2.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.splitContainer2.Location = new System.Drawing.Point(120, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.addTemplateSheetControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.printSheetsControl1);
            this.splitContainer2.Size = new System.Drawing.Size(340, 506);
            this.splitContainer2.SplitterDistance = 211;
            this.splitContainer2.TabIndex = 8;
            // 
            // btn_SaveToPdf
            // 
            this.btn_SaveToPdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_SaveToPdf.BackColor = System.Drawing.Color.SeaGreen;
            this.btn_SaveToPdf.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_SaveToPdf.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_SaveToPdf.Location = new System.Drawing.Point(212, 528);
            this.btn_SaveToPdf.Name = "btn_SaveToPdf";
            this.btn_SaveToPdf.Size = new System.Drawing.Size(153, 47);
            this.btn_SaveToPdf.TabIndex = 3;
            this.btn_SaveToPdf.Text = "Зберегти в PDF";
            this.btn_SaveToPdf.UseVisualStyleBackColor = false;
            this.btn_SaveToPdf.Click += new System.EventHandler(this.btn_SaveToPdf_Click);
            // 
            // pdfFileListControl1
            // 
            this.pdfFileListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfFileListControl1.Location = new System.Drawing.Point(0, 0);
            this.pdfFileListControl1.Name = "pdfFileListControl1";
            this.pdfFileListControl1.Size = new System.Drawing.Size(205, 601);
            this.pdfFileListControl1.TabIndex = 0;
            // 
            // previewControl1
            // 
            this.previewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewControl1.Location = new System.Drawing.Point(477, 0);
            this.previewControl1.Name = "previewControl1";
            this.previewControl1.Size = new System.Drawing.Size(375, 596);
            this.previewControl1.TabIndex = 3;
            // 
            // addTemplateSheetControl1
            // 
            this.addTemplateSheetControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addTemplateSheetControl1.Location = new System.Drawing.Point(0, 0);
            this.addTemplateSheetControl1.MinimumSize = new System.Drawing.Size(335, 158);
            this.addTemplateSheetControl1.Name = "addTemplateSheetControl1";
            this.addTemplateSheetControl1.Size = new System.Drawing.Size(340, 211);
            this.addTemplateSheetControl1.TabIndex = 6;
            // 
            // printSheetsControl1
            // 
            this.printSheetsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printSheetsControl1.Location = new System.Drawing.Point(0, 0);
            this.printSheetsControl1.Name = "printSheetsControl1";
            this.printSheetsControl1.Size = new System.Drawing.Size(340, 291);
            this.printSheetsControl1.TabIndex = 7;
            // 
            // runListControl1
            // 
            this.runListControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.runListControl1.Location = new System.Drawing.Point(3, 3);
            this.runListControl1.Name = "runListControl1";
            this.runListControl1.Size = new System.Drawing.Size(111, 587);
            this.runListControl1.TabIndex = 5;
            // 
            // FormPdfImposition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 633);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormPdfImposition";
            this.Text = "Спуск полос";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPdfImposition_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSimple.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSimple;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btn_SaveToPdf;
        private System.Windows.Forms.Panel panel2;
        private JobSpace.UserForms.PDF.ImposItems.PdfFileListControl pdfFileListControl1;
        private JobSpace.UserForms.PDF.ImposItems.RunListControl runListControl1;
        private JobSpace.UserForms.PDF.ImposItems.PreviewControl previewControl1;
        private ImposItems.PrintSheetsControl printSheetsControl1;
        private ImposItems.AddTemplateSheetControl addTemplateSheetControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}