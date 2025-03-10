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
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.pdfFileListControl1 = new JobSpace.UserForms.PDF.ImposItems.PdfFileListControl();
            this.imposColorsControl1 = new JobSpace.UserForms.PDF.ImposItems.ImposColorsControl();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pg_Parameters2 = new Krypton.Toolkit.KryptonPropertyGrid();
            this.marksControl1 = new JobSpace.UserForms.PDF.ImposItems.MarksControl();
            this.previewControl1 = new JobSpace.UserForms.PDF.ImposItems.PreviewControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_useCustomOutputFolder = new System.Windows.Forms.CheckBox();
            this.btn_selectCustomFolder = new System.Windows.Forms.Button();
            this.cb_CustomOutputPath = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_useTemplate = new System.Windows.Forms.CheckBox();
            this.tb_useTemplate = new System.Windows.Forms.TextBox();
            this.cb_savePrintSheetInOrder = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.cb_UseProofColor = new System.Windows.Forms.CheckBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.masterPageSelectControl1 = new JobSpace.UserForms.PDF.ImposItems.MasterPageSelectControl();
            this.addTemplateSheetControl1 = new JobSpace.UserForms.PDF.ImposItems.AddTemplateSheetControl();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.imposBindingControl1 = new JobSpace.UserForms.PDF.ImposItems.ImposBindingControl();
            this.printSheetsControl1 = new JobSpace.UserForms.PDF.ImposItems.PrintSheetsControl();
            this.runListControl1 = new JobSpace.UserForms.PDF.ImposItems.RunListControl();
            this.btn_SaveToPdf = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageSimple.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSimple);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1075, 708);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPageSimple
            // 
            this.tabPageSimple.Controls.Add(this.splitContainer1);
            this.tabPageSimple.Location = new System.Drawing.Point(4, 22);
            this.tabPageSimple.Name = "tabPageSimple";
            this.tabPageSimple.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSimple.Size = new System.Drawing.Size(1067, 682);
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer1.Panel2.Controls.Add(this.previewControl1);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(1061, 676);
            this.splitContainer1.SplitterDistance = 141;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.pdfFileListControl1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.imposColorsControl1);
            this.splitContainer3.Size = new System.Drawing.Size(141, 676);
            this.splitContainer3.SplitterDistance = 336;
            this.splitContainer3.TabIndex = 1;
            // 
            // pdfFileListControl1
            // 
            this.pdfFileListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfFileListControl1.Location = new System.Drawing.Point(0, 0);
            this.pdfFileListControl1.Name = "pdfFileListControl1";
            this.pdfFileListControl1.Size = new System.Drawing.Size(141, 336);
            this.pdfFileListControl1.TabIndex = 0;
            // 
            // imposColorsControl1
            // 
            this.imposColorsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imposColorsControl1.Location = new System.Drawing.Point(0, 0);
            this.imposColorsControl1.Name = "imposColorsControl1";
            this.imposColorsControl1.Size = new System.Drawing.Size(141, 336);
            this.imposColorsControl1.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer5.Location = new System.Drawing.Point(722, 3);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.marksControl1);
            this.splitContainer5.Size = new System.Drawing.Size(189, 668);
            this.splitContainer5.SplitterDistance = 173;
            this.splitContainer5.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pg_Parameters2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 173);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметри";
            // 
            // pg_Parameters2
            // 
            this.pg_Parameters2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pg_Parameters2.CategoryForeColor = System.Drawing.Color.White;
            this.pg_Parameters2.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pg_Parameters2.CommandsForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.pg_Parameters2.DisabledItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.pg_Parameters2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pg_Parameters2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pg_Parameters2.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pg_Parameters2.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.pg_Parameters2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(196)))), ((int)(((byte)(216)))));
            this.pg_Parameters2.Location = new System.Drawing.Point(3, 16);
            this.pg_Parameters2.Name = "pg_Parameters2";
            this.pg_Parameters2.Size = new System.Drawing.Size(183, 154);
            this.pg_Parameters2.TabIndex = 1;
            this.pg_Parameters2.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pg_Parameters2.ViewForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.pg_Parameters2.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pg_Parameters_PropertyValueChanged);
            // 
            // marksControl1
            // 
            this.marksControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marksControl1.Location = new System.Drawing.Point(0, 0);
            this.marksControl1.Name = "marksControl1";
            this.marksControl1.Size = new System.Drawing.Size(189, 491);
            this.marksControl1.TabIndex = 4;
            // 
            // previewControl1
            // 
            this.previewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewControl1.Location = new System.Drawing.Point(477, 0);
            this.previewControl1.Name = "previewControl1";
            this.previewControl1.Size = new System.Drawing.Size(203, 671);
            this.previewControl1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.cb_savePrintSheetInOrder);
            this.panel2.Controls.Add(this.progressBar1);
            this.panel2.Controls.Add(this.cb_UseProofColor);
            this.panel2.Controls.Add(this.splitContainer2);
            this.panel2.Controls.Add(this.runListControl1);
            this.panel2.Controls.Add(this.btn_SaveToPdf);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(474, 668);
            this.panel2.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.cb_useCustomOutputFolder);
            this.groupBox3.Controls.Add(this.btn_selectCustomFolder);
            this.groupBox3.Controls.Add(this.cb_CustomOutputPath);
            this.groupBox3.Location = new System.Drawing.Point(130, 582);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(337, 42);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "власний шлях до вивідної папки";
            // 
            // cb_useCustomOutputFolder
            // 
            this.cb_useCustomOutputFolder.AutoSize = true;
            this.cb_useCustomOutputFolder.Location = new System.Drawing.Point(21, 17);
            this.cb_useCustomOutputFolder.Name = "cb_useCustomOutputFolder";
            this.cb_useCustomOutputFolder.Size = new System.Drawing.Size(15, 14);
            this.cb_useCustomOutputFolder.TabIndex = 13;
            this.cb_useCustomOutputFolder.UseVisualStyleBackColor = true;
            // 
            // btn_selectCustomFolder
            // 
            this.btn_selectCustomFolder.Location = new System.Drawing.Point(288, 14);
            this.btn_selectCustomFolder.Name = "btn_selectCustomFolder";
            this.btn_selectCustomFolder.Size = new System.Drawing.Size(35, 23);
            this.btn_selectCustomFolder.TabIndex = 16;
            this.btn_selectCustomFolder.Text = "...";
            this.btn_selectCustomFolder.UseVisualStyleBackColor = true;
            this.btn_selectCustomFolder.Click += new System.EventHandler(this.btn_selectCustomFolder_Click);
            // 
            // cb_CustomOutputPath
            // 
            this.cb_CustomOutputPath.FormattingEnabled = true;
            this.cb_CustomOutputPath.Location = new System.Drawing.Point(42, 14);
            this.cb_CustomOutputPath.Name = "cb_CustomOutputPath";
            this.cb_CustomOutputPath.Size = new System.Drawing.Size(240, 21);
            this.cb_CustomOutputPath.TabIndex = 17;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.cb_useTemplate);
            this.groupBox2.Controls.Add(this.tb_useTemplate);
            this.groupBox2.Location = new System.Drawing.Point(129, 536);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(338, 42);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "використовувати шаблон імені файлу";
            // 
            // cb_useTemplate
            // 
            this.cb_useTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_useTemplate.AutoSize = true;
            this.cb_useTemplate.Location = new System.Drawing.Point(22, 19);
            this.cb_useTemplate.Name = "cb_useTemplate";
            this.cb_useTemplate.Size = new System.Drawing.Size(15, 14);
            this.cb_useTemplate.TabIndex = 12;
            this.cb_useTemplate.UseVisualStyleBackColor = true;
            // 
            // tb_useTemplate
            // 
            this.tb_useTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_useTemplate.Location = new System.Drawing.Point(43, 16);
            this.tb_useTemplate.Name = "tb_useTemplate";
            this.tb_useTemplate.Size = new System.Drawing.Size(240, 20);
            this.tb_useTemplate.TabIndex = 14;
            this.tb_useTemplate.Text = "$[orderNo]_$[customer]_$[orderDesc]";
            // 
            // cb_savePrintSheetInOrder
            // 
            this.cb_savePrintSheetInOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_savePrintSheetInOrder.Checked = true;
            this.cb_savePrintSheetInOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_savePrintSheetInOrder.Location = new System.Drawing.Point(268, 503);
            this.cb_savePrintSheetInOrder.Name = "cb_savePrintSheetInOrder";
            this.cb_savePrintSheetInOrder.Size = new System.Drawing.Size(174, 31);
            this.cb_savePrintSheetInOrder.TabIndex = 11;
            this.cb_savePrintSheetInOrder.Text = "Зберігати друк. листи у папці з замовленням";
            this.cb_savePrintSheetInOrder.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(285, 634);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(182, 25);
            this.progressBar1.TabIndex = 10;
            // 
            // cb_UseProofColor
            // 
            this.cb_UseProofColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_UseProofColor.Checked = true;
            this.cb_UseProofColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_UseProofColor.Location = new System.Drawing.Point(172, 503);
            this.cb_UseProofColor.Name = "cb_UseProofColor";
            this.cb_UseProofColor.Size = new System.Drawing.Size(79, 31);
            this.cb_UseProofColor.TabIndex = 9;
            this.cb_UseProofColor.Text = "малювати ProofColor";
            this.cb_UseProofColor.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.splitContainer2.Location = new System.Drawing.Point(129, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.masterPageSelectControl1);
            this.splitContainer2.Panel1.Controls.Add(this.addTemplateSheetControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(340, 494);
            this.splitContainer2.SplitterDistance = 191;
            this.splitContainer2.TabIndex = 8;
            // 
            // masterPageSelectControl1
            // 
            this.masterPageSelectControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.masterPageSelectControl1.Location = new System.Drawing.Point(1, 119);
            this.masterPageSelectControl1.Name = "masterPageSelectControl1";
            this.masterPageSelectControl1.OnMasterPageAdded = null;
            this.masterPageSelectControl1.OnMasterPageChanged = null;
            this.masterPageSelectControl1.Size = new System.Drawing.Size(339, 71);
            this.masterPageSelectControl1.TabIndex = 1;
            // 
            // addTemplateSheetControl1
            // 
            this.addTemplateSheetControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addTemplateSheetControl1.Location = new System.Drawing.Point(0, 3);
            this.addTemplateSheetControl1.MinimumSize = new System.Drawing.Size(335, 0);
            this.addTemplateSheetControl1.Name = "addTemplateSheetControl1";
            this.addTemplateSheetControl1.OnSheetAddToPrint = null;
            this.addTemplateSheetControl1.OnSheetSelected = null;
            this.addTemplateSheetControl1.Size = new System.Drawing.Size(340, 110);
            this.addTemplateSheetControl1.TabIndex = 6;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.printSheetsControl1);
            this.splitContainer4.Size = new System.Drawing.Size(340, 299);
            this.splitContainer4.SplitterDistance = 142;
            this.splitContainer4.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.imposBindingControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(340, 142);
            this.panel1.TabIndex = 1;
            // 
            // imposBindingControl1
            // 
            this.imposBindingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imposBindingControl1.Location = new System.Drawing.Point(0, 0);
            this.imposBindingControl1.Name = "imposBindingControl1";
            this.imposBindingControl1.Size = new System.Drawing.Size(340, 142);
            this.imposBindingControl1.TabIndex = 1;
            // 
            // printSheetsControl1
            // 
            this.printSheetsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printSheetsControl1.JustReassignPages = null;
            this.printSheetsControl1.Location = new System.Drawing.Point(0, 0);
            this.printSheetsControl1.Name = "printSheetsControl1";
            this.printSheetsControl1.OnPrintSheetDeleted = null;
            this.printSheetsControl1.OnPrintSheetsChanged = null;
            this.printSheetsControl1.Size = new System.Drawing.Size(340, 153);
            this.printSheetsControl1.TabIndex = 7;
            // 
            // runListControl1
            // 
            this.runListControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.runListControl1.Location = new System.Drawing.Point(3, 3);
            this.runListControl1.Name = "runListControl1";
            this.runListControl1.Size = new System.Drawing.Size(120, 662);
            this.runListControl1.TabIndex = 5;
            // 
            // btn_SaveToPdf
            // 
            this.btn_SaveToPdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_SaveToPdf.BackColor = System.Drawing.Color.SeaGreen;
            this.btn_SaveToPdf.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_SaveToPdf.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_SaveToPdf.Location = new System.Drawing.Point(130, 629);
            this.btn_SaveToPdf.Margin = new System.Windows.Forms.Padding(0);
            this.btn_SaveToPdf.Name = "btn_SaveToPdf";
            this.btn_SaveToPdf.Size = new System.Drawing.Size(153, 35);
            this.btn_SaveToPdf.TabIndex = 3;
            this.btn_SaveToPdf.Text = "Зберегти в PDF";
            this.btn_SaveToPdf.UseVisualStyleBackColor = false;
            this.btn_SaveToPdf.Click += new System.EventHandler(this.btn_SaveToPdf_Click);
            // 
            // FormPdfImposition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 708);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormPdfImposition";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Спуск полос";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPdfImposition_FormClosing);
            this.Shown += new System.EventHandler(this.FormPdfImposition_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSimple.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.CheckBox cb_UseProofColor;
        private ImposItems.MarksControl marksControl1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private ImposItems.ImposColorsControl imposColorsControl1;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Panel panel1;
        private ImposItems.MasterPageSelectControl masterPageSelectControl1;
        private ImposItems.ImposBindingControl imposBindingControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox cb_savePrintSheetInOrder;
        private System.Windows.Forms.CheckBox cb_useCustomOutputFolder;
        private System.Windows.Forms.CheckBox cb_useTemplate;
        private System.Windows.Forms.TextBox tb_useTemplate;
        private System.Windows.Forms.Button btn_selectCustomFolder;
        private System.Windows.Forms.ComboBox cb_CustomOutputPath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private Krypton.Toolkit.KryptonPropertyGrid pg_Parameters2;
    }
}