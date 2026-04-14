namespace JobSpace.UserForms.PDF.Visual
{
    partial class FormVisualCoverRootCalc
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
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonComboBox1 = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonGroupBox2 = new Krypton.Toolkit.KryptonGroupBox();
            this.nud_paper_thickness = new Krypton.Toolkit.KryptonNumericUpDown();
            this.cb_paper_thickness = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.nud_cnt_pages = new Krypton.Toolkit.KryptonNumericUpDown();
            this.label_root = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.nud_glue = new Krypton.Toolkit.KryptonNumericUpDown();
            this.nud_coeficient = new Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cb_paper_thickness)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Location = new System.Drawing.Point(12, 12);
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonComboBox1);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(198, 53);
            this.kryptonGroupBox1.TabIndex = 1;
            this.kryptonGroupBox1.Values.Heading = "тип палітурки";
            // 
            // kryptonComboBox1
            // 
            this.kryptonComboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kryptonComboBox1.DropDownWidth = 194;
            this.kryptonComboBox1.Items.AddRange(new object[] {
            "м\'яка палітурка",
            "тверда палітурка"});
            this.kryptonComboBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonComboBox1.Name = "kryptonComboBox1";
            this.kryptonComboBox1.Size = new System.Drawing.Size(194, 29);
            this.kryptonComboBox1.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.kryptonComboBox1.TabIndex = 0;
            this.kryptonComboBox1.SelectedIndexChanged += new System.EventHandler(this.kryptonComboBox1_SelectedIndexChanged);
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.Location = new System.Drawing.Point(12, 71);
            // 
            // kryptonGroupBox2.Panel
            // 
            this.kryptonGroupBox2.Panel.Controls.Add(this.nud_paper_thickness);
            this.kryptonGroupBox2.Panel.Controls.Add(this.cb_paper_thickness);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(315, 56);
            this.kryptonGroupBox2.TabIndex = 2;
            this.kryptonGroupBox2.Values.Heading = "БЛОК: товщина паперу ";
            // 
            // nud_paper_thickness
            // 
            this.nud_paper_thickness.AllowDecimals = true;
            this.nud_paper_thickness.DecimalPlaces = 2;
            this.nud_paper_thickness.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_paper_thickness.Location = new System.Drawing.Point(237, 3);
            this.nud_paper_thickness.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nud_paper_thickness.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nud_paper_thickness.Name = "nud_paper_thickness";
            this.nud_paper_thickness.Size = new System.Drawing.Size(63, 22);
            this.nud_paper_thickness.TabIndex = 1;
            this.nud_paper_thickness.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nud_paper_thickness.ValueChanged += new System.EventHandler(this.nud_cnt_pages_ValueChanged);
            // 
            // cb_paper_thickness
            // 
            this.cb_paper_thickness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_paper_thickness.DropDownWidth = 228;
            this.cb_paper_thickness.Location = new System.Drawing.Point(3, 3);
            this.cb_paper_thickness.Name = "cb_paper_thickness";
            this.cb_paper_thickness.Size = new System.Drawing.Size(228, 22);
            this.cb_paper_thickness.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.cb_paper_thickness.TabIndex = 0;
            this.cb_paper_thickness.SelectedIndexChanged += new System.EventHandler(this.cb_paper_thickness_SelectedIndexChanged);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(17, 142);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(111, 25);
            this.kryptonLabel1.TabIndex = 3;
            this.kryptonLabel1.Values.Text = "кількість сторінок";
            // 
            // nud_cnt_pages
            // 
            this.nud_cnt_pages.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_cnt_pages.Location = new System.Drawing.Point(134, 143);
            this.nud_cnt_pages.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_cnt_pages.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nud_cnt_pages.Name = "nud_cnt_pages";
            this.nud_cnt_pages.Size = new System.Drawing.Size(85, 22);
            this.nud_cnt_pages.TabIndex = 4;
            this.nud_cnt_pages.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nud_cnt_pages.ValueChanged += new System.EventHandler(this.nud_cnt_pages_ValueChanged);
            // 
            // label_root
            // 
            this.label_root.AutoSize = false;
            this.label_root.Location = new System.Drawing.Point(17, 259);
            this.label_root.Name = "label_root";
            this.label_root.Size = new System.Drawing.Size(310, 73);
            this.label_root.StateCommon.LongText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.label_root.StateCommon.LongText.TextV = Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.label_root.StateCommon.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_root.StateCommon.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.label_root.StateCommon.ShortText.TextV = Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.label_root.StateNormal.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.label_root.StateNormal.ShortText.TextV = Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.label_root.TabIndex = 5;
            this.label_root.Values.Text = "0.0";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(17, 173);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(90, 25);
            this.kryptonLabel3.TabIndex = 6;
            this.kryptonLabel3.Values.Text = "на клей, мм";
            // 
            // nud_glue
            // 
            this.nud_glue.AllowDecimals = true;
            this.nud_glue.DecimalPlaces = 1;
            this.nud_glue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_glue.Location = new System.Drawing.Point(134, 176);
            this.nud_glue.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nud_glue.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nud_glue.Name = "nud_glue";
            this.nud_glue.Size = new System.Drawing.Size(85, 22);
            this.nud_glue.TabIndex = 7;
            this.nud_glue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_glue.ValueChanged += new System.EventHandler(this.nud_cnt_pages_ValueChanged);
            // 
            // nud_coeficient
            // 
            this.nud_coeficient.AllowDecimals = true;
            this.nud_coeficient.DecimalPlaces = 1;
            this.nud_coeficient.Enabled = false;
            this.nud_coeficient.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_coeficient.Location = new System.Drawing.Point(134, 207);
            this.nud_coeficient.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nud_coeficient.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nud_coeficient.Name = "nud_coeficient";
            this.nud_coeficient.Size = new System.Drawing.Size(85, 22);
            this.nud_coeficient.TabIndex = 9;
            this.nud_coeficient.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(17, 204);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(111, 25);
            this.kryptonLabel2.TabIndex = 8;
            this.kryptonLabel2.Values.Text = "коефіцієнт об\'єму";
            // 
            // FormVisualCoverRootCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 344);
            this.Controls.Add(this.nud_coeficient);
            this.Controls.Add(this.kryptonLabel2);
            this.Controls.Add(this.nud_glue);
            this.Controls.Add(this.kryptonLabel3);
            this.Controls.Add(this.label_root);
            this.Controls.Add(this.nud_cnt_pages);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.kryptonGroupBox2);
            this.Controls.Add(this.kryptonGroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVisualCoverRootCalc";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Розрахунок корінця палітурки";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cb_paper_thickness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonComboBox kryptonComboBox1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private Krypton.Toolkit.KryptonNumericUpDown nud_paper_thickness;
        private Krypton.Toolkit.KryptonComboBox cb_paper_thickness;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonNumericUpDown nud_cnt_pages;
        private Krypton.Toolkit.KryptonLabel label_root;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonNumericUpDown nud_glue;
        private Krypton.Toolkit.KryptonNumericUpDown nud_coeficient;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
    }
}