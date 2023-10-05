namespace ActiveWorks.Forms
{
    partial class FormMoveSignaFileToOrder
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
            this.kryptonTextBoxSignaJobPath = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonTextBoxSignaJobTemplate = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonButton1 = new Krypton.Toolkit.KryptonButton();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonGroup1 = new Krypton.Toolkit.KryptonGroup();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.kryptonButtonFind = new Krypton.Toolkit.KryptonButton();
            this.kryptonButtonCheckTemplate = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).BeginInit();
            this.kryptonGroup1.Panel.SuspendLayout();
            this.kryptonGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonTextBoxSignaJobPath
            // 
            this.kryptonTextBoxSignaJobPath.Location = new System.Drawing.Point(42, 38);
            this.kryptonTextBoxSignaJobPath.Name = "kryptonTextBoxSignaJobPath";
            this.kryptonTextBoxSignaJobPath.Size = new System.Drawing.Size(334, 23);
            this.kryptonTextBoxSignaJobPath.TabIndex = 0;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(42, 12);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(173, 20);
            this.kryptonLabel1.TabIndex = 1;
            this.kryptonLabel1.Values.Text = "шлях до файлів Signa Station";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(422, 12);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(111, 20);
            this.kryptonLabel2.TabIndex = 2;
            this.kryptonLabel2.Values.Text = "шаблон для робіт";
            // 
            // kryptonTextBoxSignaJobTemplate
            // 
            this.kryptonTextBoxSignaJobTemplate.Location = new System.Drawing.Point(422, 38);
            this.kryptonTextBoxSignaJobTemplate.Name = "kryptonTextBoxSignaJobTemplate";
            this.kryptonTextBoxSignaJobTemplate.Size = new System.Drawing.Size(295, 23);
            this.kryptonTextBoxSignaJobTemplate.TabIndex = 3;
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.CornerRoundingRadius = 5F;
            this.kryptonButton1.Location = new System.Drawing.Point(322, 444);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(162, 39);
            this.kryptonButton1.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton1.StateCommon.Border.Rounding = 5F;
            this.kryptonButton1.TabIndex = 4;
            this.kryptonButton1.Values.Text = "ПЕРЕНЕСТИ ФАЙЛИ";
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonTextBoxSignaJobPath);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonTextBoxSignaJobTemplate);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel2);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(773, 94);
            this.kryptonGroupBox1.TabIndex = 5;
            this.kryptonGroupBox1.Values.Heading = "Налаштування";
            // 
            // kryptonGroup1
            // 
            this.kryptonGroup1.Location = new System.Drawing.Point(12, 157);
            this.kryptonGroup1.Name = "kryptonGroup1";
            // 
            // kryptonGroup1.Panel
            // 
            this.kryptonGroup1.Panel.Controls.Add(this.objectListView1);
            this.kryptonGroup1.Size = new System.Drawing.Size(773, 281);
            this.kryptonGroup1.TabIndex = 6;
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumn1);
            this.objectListView1.AllColumns.Add(this.olvColumn2);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(0, 0);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(771, 279);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Name";
            this.olvColumn1.Text = "Ім\'я файлу";
            this.olvColumn1.Width = 537;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Status";
            this.olvColumn2.Text = "Статус";
            this.olvColumn2.Width = 98;
            // 
            // kryptonButtonFind
            // 
            this.kryptonButtonFind.CornerRoundingRadius = 5F;
            this.kryptonButtonFind.Location = new System.Drawing.Point(13, 113);
            this.kryptonButtonFind.Name = "kryptonButtonFind";
            this.kryptonButtonFind.Size = new System.Drawing.Size(162, 39);
            this.kryptonButtonFind.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButtonFind.StateCommon.Border.Rounding = 5F;
            this.kryptonButtonFind.TabIndex = 7;
            this.kryptonButtonFind.Values.Text = "ЗНАЙТИ ФАЙЛИ";
            this.kryptonButtonFind.Click += new System.EventHandler(this.kryptonButtonFind_Click);
            // 
            // kryptonButtonCheckTemplate
            // 
            this.kryptonButtonCheckTemplate.CornerRoundingRadius = 5F;
            this.kryptonButtonCheckTemplate.Location = new System.Drawing.Point(322, 113);
            this.kryptonButtonCheckTemplate.Name = "kryptonButtonCheckTemplate";
            this.kryptonButtonCheckTemplate.Size = new System.Drawing.Size(162, 39);
            this.kryptonButtonCheckTemplate.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButtonCheckTemplate.StateCommon.Border.Rounding = 5F;
            this.kryptonButtonCheckTemplate.TabIndex = 8;
            this.kryptonButtonCheckTemplate.Values.Text = "ПЕРЕВІРИТИ ШАБЛОН";
            this.kryptonButtonCheckTemplate.Click += new System.EventHandler(this.kryptonButtonCheckTemplate_Click);
            // 
            // FormMoveSignaFileToOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 507);
            this.Controls.Add(this.kryptonButtonCheckTemplate);
            this.Controls.Add(this.kryptonButtonFind);
            this.Controls.Add(this.kryptonGroup1);
            this.Controls.Add(this.kryptonGroupBox1);
            this.Controls.Add(this.kryptonButton1);
            this.Name = "FormMoveSignaFileToOrder";
            this.Text = "Перенос файла Signa Station до папки з замовленням";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).EndInit();
            this.kryptonGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).EndInit();
            this.kryptonGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonTextBox kryptonTextBoxSignaJobPath;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonTextBox kryptonTextBoxSignaJobTemplate;
        private Krypton.Toolkit.KryptonButton kryptonButton1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonGroup kryptonGroup1;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private Krypton.Toolkit.KryptonButton kryptonButtonFind;
        private Krypton.Toolkit.KryptonButton kryptonButtonCheckTemplate;
    }
}