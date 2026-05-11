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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPdfImposition));
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPageSimple = new System.Windows.Forms.TabPage();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer3 = new System.Windows.Forms.SplitContainer();
            pdfFileListControl1 = new JobSpace.UserForms.PDF.ImposItems.PdfFileListControl();
            imposColorsControl1 = new JobSpace.UserForms.PDF.ImposItems.ImposColorsControl();
            previewControl1 = new JobSpace.UserForms.PDF.ImposItems.PreviewControl();
            runListControl1 = new JobSpace.UserForms.PDF.ImposItems.RunListControl();
            splitContainer5 = new System.Windows.Forms.SplitContainer();
            groupBox1 = new System.Windows.Forms.GroupBox();
            pg_Parameters2 = new Krypton.Toolkit.KryptonPropertyGrid();
            marksControl1 = new JobSpace.UserForms.PDF.ImposItems.MarksControl();
            panel2 = new System.Windows.Forms.Panel();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            btn_SaveAsAutoImpos = new System.Windows.Forms.Button();
            btn_SaveToPdf = new System.Windows.Forms.Button();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            btn_cancel_export = new System.Windows.Forms.Button();
            groupBox3 = new System.Windows.Forms.GroupBox();
            cb_useCustomOutputFolder = new System.Windows.Forms.CheckBox();
            btn_selectCustomFolder = new System.Windows.Forms.Button();
            cb_CustomOutputPath = new System.Windows.Forms.ComboBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            cb_useTemplate = new System.Windows.Forms.CheckBox();
            tb_useTemplate = new System.Windows.Forms.TextBox();
            cb_savePrintSheetInOrder = new System.Windows.Forms.CheckBox();
            cb_UseProofColor = new System.Windows.Forms.CheckBox();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            splitContainer4 = new System.Windows.Forms.SplitContainer();
            panel1 = new System.Windows.Forms.Panel();
            imposBindingControl1 = new JobSpace.UserForms.PDF.ImposItems.ImposBindingControl();
            printSheetsControl1 = new JobSpace.UserForms.PDF.ImposItems.PrintSheetsControl();
            printSheetsControl1 = new JobSpace.UserForms.PDF.ImposItems.PrintSheetsControl();
            masterPageSelectControl1 = new JobSpace.UserForms.PDF.ImposItems.MasterPageSelectControl();
            addTemplateSheetControl1 = new JobSpace.UserForms.PDF.ImposItems.AddTemplateSheetControl();
            tabControl1.SuspendLayout();
            tabPageSimple.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer5).BeginInit();
            splitContainer5.Panel1.SuspendLayout();
            splitContainer5.Panel2.SuspendLayout();
            splitContainer5.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
            splitContainer4.Panel1.SuspendLayout();
            splitContainer4.Panel2.SuspendLayout();
            splitContainer4.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageSimple);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(1750, 969);
            tabControl1.TabIndex = 2;
            // 
            // tabPageSimple
            // 
            tabPageSimple.Controls.Add(splitContainer1);
            tabPageSimple.Location = new System.Drawing.Point(4, 24);
            tabPageSimple.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageSimple.Name = "tabPageSimple";
            tabPageSimple.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageSimple.Size = new System.Drawing.Size(1742, 941);
            tabPageSimple.TabIndex = 0;
            tabPageSimple.Text = "простий";
            tabPageSimple.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(4, 3);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(previewControl1);
            splitContainer1.Panel2.Controls.Add(runListControl1);
            splitContainer1.Panel2.Controls.Add(splitContainer5);
            splitContainer1.Panel2.Controls.Add(panel2);
            splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
            splitContainer1.Size = new System.Drawing.Size(1734, 935);
            splitContainer1.SplitterDistance = 229;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 2;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer3.Location = new System.Drawing.Point(0, 0);
            splitContainer3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer3.Name = "splitContainer3";
            splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(pdfFileListControl1);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(imposColorsControl1);
            splitContainer3.Size = new System.Drawing.Size(229, 935);
            splitContainer3.SplitterDistance = 462;
            splitContainer3.SplitterWidth = 5;
            splitContainer3.TabIndex = 1;
            // 
            // pdfFileListControl1
            // 
            pdfFileListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            pdfFileListControl1.Location = new System.Drawing.Point(0, 0);
            pdfFileListControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pdfFileListControl1.Name = "pdfFileListControl1";
            pdfFileListControl1.Size = new System.Drawing.Size(229, 462);
            pdfFileListControl1.TabIndex = 0;
            // 
            // imposColorsControl1
            // 
            imposColorsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            imposColorsControl1.Location = new System.Drawing.Point(0, 0);
            imposColorsControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            imposColorsControl1.Name = "imposColorsControl1";
            imposColorsControl1.Size = new System.Drawing.Size(229, 468);
            imposColorsControl1.TabIndex = 0;
            // 
            // previewControl1
            // 
            previewControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            previewControl1.Location = new System.Drawing.Point(561, 3);
            previewControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            previewControl1.Name = "previewControl1";
            previewControl1.Size = new System.Drawing.Size(688, 929);
            previewControl1.TabIndex = 8;
            // 
            // runListControl1
            // 
            runListControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            runListControl1.Location = new System.Drawing.Point(0, 0);
            runListControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            runListControl1.Name = "runListControl2";
            runListControl1.Size = new System.Drawing.Size(145, 933);
            runListControl1.TabIndex = 7;
            // 
            // splitContainer5
            // 
            splitContainer5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            splitContainer5.Location = new System.Drawing.Point(1253, 3);
            splitContainer5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer5.Name = "splitContainer5";
            splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            splitContainer5.Panel1.Controls.Add(groupBox1);
            // 
            // splitContainer5.Panel2
            // 
            splitContainer5.Panel2.Controls.Add(marksControl1);
            splitContainer5.Size = new System.Drawing.Size(236, 926);
            splitContainer5.SplitterDistance = 239;
            splitContainer5.SplitterWidth = 5;
            splitContainer5.TabIndex = 6;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(pg_Parameters2);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(236, 239);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Параметри";
            // 
            // pg_Parameters2
            // 
            pg_Parameters2.Dock = System.Windows.Forms.DockStyle.Fill;
            pg_Parameters2.Location = new System.Drawing.Point(4, 19);
            pg_Parameters2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pg_Parameters2.Name = "pg_Parameters2";
            pg_Parameters2.Padding = new System.Windows.Forms.Padding(1);
            pg_Parameters2.Size = new System.Drawing.Size(228, 217);
            pg_Parameters2.TabIndex = 1;
            pg_Parameters2.PropertyValueChanged += pg_Parameters_PropertyValueChanged;
            // 
            // marksControl1
            // 
            marksControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            marksControl1.Location = new System.Drawing.Point(0, 0);
            marksControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            marksControl1.Name = "marksControl1";
            marksControl1.Size = new System.Drawing.Size(236, 682);
            marksControl1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            panel2.Controls.Add(flowLayoutPanel1);
            panel2.Controls.Add(groupBox3);
            panel2.Controls.Add(groupBox2);
            panel2.Controls.Add(cb_savePrintSheetInOrder);
            panel2.Controls.Add(cb_UseProofColor);
            panel2.Controls.Add(splitContainer2);
            panel2.Location = new System.Drawing.Point(153, 0);
            panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(406, 926);
            panel2.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(btn_SaveAsAutoImpos);
            flowLayoutPanel1.Controls.Add(btn_SaveToPdf);
            flowLayoutPanel1.Controls.Add(progressBar1);
            flowLayoutPanel1.Controls.Add(btn_cancel_export);
            flowLayoutPanel1.Location = new System.Drawing.Point(3, 881);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(394, 42);
            flowLayoutPanel1.TabIndex = 21;
            // 
            // btn_SaveAsAutoImpos
            // 
            btn_SaveAsAutoImpos.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btn_SaveAsAutoImpos.Location = new System.Drawing.Point(4, 3);
            btn_SaveAsAutoImpos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_SaveAsAutoImpos.Name = "btn_SaveAsAutoImpos";
            btn_SaveAsAutoImpos.Size = new System.Drawing.Size(99, 40);
            btn_SaveAsAutoImpos.TabIndex = 22;
            btn_SaveAsAutoImpos.Text = "Зберегти як автоспуск";
            btn_SaveAsAutoImpos.UseVisualStyleBackColor = true;
            btn_SaveAsAutoImpos.Click += btn_SaveAsAutoImpos_Click;
            // 
            // btn_SaveToPdf
            // 
            btn_SaveToPdf.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btn_SaveToPdf.BackColor = System.Drawing.Color.SeaGreen;
            btn_SaveToPdf.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            btn_SaveToPdf.ForeColor = System.Drawing.SystemColors.Window;
            btn_SaveToPdf.Location = new System.Drawing.Point(107, 6);
            btn_SaveToPdf.Margin = new System.Windows.Forms.Padding(0);
            btn_SaveToPdf.Name = "btn_SaveToPdf";
            btn_SaveToPdf.Size = new System.Drawing.Size(140, 40);
            btn_SaveToPdf.TabIndex = 3;
            btn_SaveToPdf.Text = "Зберегти в PDF";
            btn_SaveToPdf.UseVisualStyleBackColor = false;
            btn_SaveToPdf.Click += btn_SaveToPdf_Click;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar1.Location = new System.Drawing.Point(251, 14);
            progressBar1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(93, 29);
            progressBar1.TabIndex = 10;
            // 
            // btn_cancel_export
            // 
            btn_cancel_export.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            btn_cancel_export.FlatAppearance.BorderSize = 0;
            btn_cancel_export.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_cancel_export.Image = (System.Drawing.Image)resources.GetObject("btn_cancel_export.Image");
            btn_cancel_export.Location = new System.Drawing.Point(352, 14);
            btn_cancel_export.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_cancel_export.Name = "btn_cancel_export";
            btn_cancel_export.Size = new System.Drawing.Size(33, 29);
            btn_cancel_export.TabIndex = 20;
            btn_cancel_export.UseVisualStyleBackColor = true;
            btn_cancel_export.Click += btn_cancel_export_Click;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            groupBox3.Controls.Add(cb_useCustomOutputFolder);
            groupBox3.Controls.Add(btn_selectCustomFolder);
            groupBox3.Controls.Add(cb_CustomOutputPath);
            groupBox3.Location = new System.Drawing.Point(5, 828);
            groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Size = new System.Drawing.Size(393, 48);
            groupBox3.TabIndex = 19;
            groupBox3.TabStop = false;
            groupBox3.Text = "власний шлях до вивідної папки";
            // 
            // cb_useCustomOutputFolder
            // 
            cb_useCustomOutputFolder.AutoSize = true;
            cb_useCustomOutputFolder.Location = new System.Drawing.Point(24, 20);
            cb_useCustomOutputFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_useCustomOutputFolder.Name = "cb_useCustomOutputFolder";
            cb_useCustomOutputFolder.Size = new System.Drawing.Size(15, 14);
            cb_useCustomOutputFolder.TabIndex = 13;
            cb_useCustomOutputFolder.UseVisualStyleBackColor = true;
            // 
            // btn_selectCustomFolder
            // 
            btn_selectCustomFolder.Location = new System.Drawing.Point(336, 16);
            btn_selectCustomFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_selectCustomFolder.Name = "btn_selectCustomFolder";
            btn_selectCustomFolder.Size = new System.Drawing.Size(41, 27);
            btn_selectCustomFolder.TabIndex = 16;
            btn_selectCustomFolder.Text = "...";
            btn_selectCustomFolder.UseVisualStyleBackColor = true;
            btn_selectCustomFolder.Click += btn_selectCustomFolder_Click;
            // 
            // cb_CustomOutputPath
            // 
            cb_CustomOutputPath.FormattingEnabled = true;
            cb_CustomOutputPath.Location = new System.Drawing.Point(49, 16);
            cb_CustomOutputPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_CustomOutputPath.Name = "cb_CustomOutputPath";
            cb_CustomOutputPath.Size = new System.Drawing.Size(279, 23);
            cb_CustomOutputPath.TabIndex = 17;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            groupBox2.Controls.Add(cb_useTemplate);
            groupBox2.Controls.Add(tb_useTemplate);
            groupBox2.Location = new System.Drawing.Point(3, 775);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(394, 48);
            groupBox2.TabIndex = 18;
            groupBox2.TabStop = false;
            groupBox2.Text = "використовувати шаблон імені файлу";
            // 
            // cb_useTemplate
            // 
            cb_useTemplate.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            cb_useTemplate.AutoSize = true;
            cb_useTemplate.Location = new System.Drawing.Point(26, 24);
            cb_useTemplate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_useTemplate.Name = "cb_useTemplate";
            cb_useTemplate.Size = new System.Drawing.Size(15, 14);
            cb_useTemplate.TabIndex = 12;
            cb_useTemplate.UseVisualStyleBackColor = true;
            // 
            // tb_useTemplate
            // 
            tb_useTemplate.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            tb_useTemplate.Location = new System.Drawing.Point(50, 18);
            tb_useTemplate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tb_useTemplate.Name = "tb_useTemplate";
            tb_useTemplate.Size = new System.Drawing.Size(279, 23);
            tb_useTemplate.TabIndex = 14;
            tb_useTemplate.Text = "$[orderNo]_$[customer]_$[orderDesc]";
            // 
            // cb_savePrintSheetInOrder
            // 
            cb_savePrintSheetInOrder.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            cb_savePrintSheetInOrder.Checked = true;
            cb_savePrintSheetInOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            cb_savePrintSheetInOrder.Location = new System.Drawing.Point(166, 737);
            cb_savePrintSheetInOrder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_savePrintSheetInOrder.Name = "cb_savePrintSheetInOrder";
            cb_savePrintSheetInOrder.Size = new System.Drawing.Size(203, 36);
            cb_savePrintSheetInOrder.TabIndex = 11;
            cb_savePrintSheetInOrder.Text = "Зберігати друк. листи у папці з замовленням";
            cb_savePrintSheetInOrder.UseVisualStyleBackColor = true;
            // 
            // cb_UseProofColor
            // 
            cb_UseProofColor.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            cb_UseProofColor.Checked = true;
            cb_UseProofColor.CheckState = System.Windows.Forms.CheckState.Checked;
            cb_UseProofColor.Location = new System.Drawing.Point(54, 737);
            cb_UseProofColor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_UseProofColor.Name = "cb_UseProofColor";
            cb_UseProofColor.Size = new System.Drawing.Size(92, 36);
            cb_UseProofColor.TabIndex = 9;
            cb_UseProofColor.Text = "малювати ProofColor";
            cb_UseProofColor.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            splitContainer2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            splitContainer2.Location = new System.Drawing.Point(3, 3);
            splitContainer2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(addTemplateSheetControl1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(splitContainer4);
            splitContainer2.Size = new System.Drawing.Size(397, 725);
            splitContainer2.SplitterDistance = 280;
            splitContainer2.SplitterWidth = 5;
            splitContainer2.TabIndex = 8;
            // 
            // splitContainer4
            // 
            splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer4.Location = new System.Drawing.Point(0, 0);
            splitContainer4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer4.Name = "splitContainer4";
            splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            splitContainer4.Panel1.Controls.Add(panel1);
            // 
            // splitContainer4.Panel2
            // 
            splitContainer4.Panel2.Controls.Add(printSheetsControl1);
            splitContainer4.Size = new System.Drawing.Size(397, 440);
            splitContainer4.SplitterDistance = 208;
            splitContainer4.SplitterWidth = 5;
            splitContainer4.TabIndex = 8;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(imposBindingControl1);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(397, 208);
            panel1.TabIndex = 1;
            // 
            // imposBindingControl1
            // 
            imposBindingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            imposBindingControl1.Location = new System.Drawing.Point(0, 0);
            imposBindingControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            imposBindingControl1.Name = "imposBindingControl1";
            imposBindingControl1.Size = new System.Drawing.Size(397, 208);
            imposBindingControl1.TabIndex = 0;
            // 
            // printSheetsControl3
            // 
            printSheetsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            printSheetsControl1.Location = new System.Drawing.Point(0, 0);
            printSheetsControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            printSheetsControl1.Name = "printSheetsControl1";
            printSheetsControl1.Size = new System.Drawing.Size(397, 227);
            printSheetsControl1.TabIndex = 0;
            // 
            // masterPageSelectControl1
            // 
            masterPageSelectControl1.Location = new System.Drawing.Point(0, 0);
            masterPageSelectControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            masterPageSelectControl1.Name = "masterPageSelectControl1";
            masterPageSelectControl1.Size = new System.Drawing.Size(393, 82);
            masterPageSelectControl1.TabIndex = 0;
            
            // 
            // addTemplateSheetControl2
            // 
            addTemplateSheetControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            addTemplateSheetControl1.Location = new System.Drawing.Point(0, 0);
            addTemplateSheetControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            addTemplateSheetControl1.MinimumSize = new System.Drawing.Size(391, 0);
            addTemplateSheetControl1.Name = "addTemplateSheetControl2";
            addTemplateSheetControl1.Size = new System.Drawing.Size(397, 280);
            addTemplateSheetControl1.TabIndex = 0;
            // 
            // FormPdfImposition
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1750, 969);
            Controls.Add(tabControl1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormPdfImposition";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Спуск полос";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            FormClosing += FormPdfImposition_FormClosing;
            Shown += FormPdfImposition_Shown;
            tabControl1.ResumeLayout(false);
            tabPageSimple.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            splitContainer5.Panel1.ResumeLayout(false);
            splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer5).EndInit();
            splitContainer5.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer4.Panel1.ResumeLayout(false);
            splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
            splitContainer4.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSimple;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btn_SaveToPdf;
        private System.Windows.Forms.Panel panel2;
        private JobSpace.UserForms.PDF.ImposItems.PdfFileListControl pdfFileListControl1;
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
        private System.Windows.Forms.Button btn_cancel_export;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btn_SaveAsAutoImpos;
        private ImposItems.RunListControl runListControl1;
        private ImposItems.PreviewControl previewControl1;
    }
}
