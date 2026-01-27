namespace JobSpace.UserForms.PDF.Visual
{
    partial class FormVisualSoftCover
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
            this.nud_bleed = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nud_total_height = new System.Windows.Forms.NumericUpDown();
            this.nud_total_width = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nud_right_klapan = new System.Windows.Forms.NumericUpDown();
            this.nud_left_klapan = new System.Windows.Forms.NumericUpDown();
            this.nud_root = new System.Windows.Forms.NumericUpDown();
            this.nud_height = new System.Windows.Forms.NumericUpDown();
            this.nud_width = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_apply_schema = new System.Windows.Forms.Button();
            this.btn_create_schema = new System.Windows.Forms.Button();
            this.uc_PreviewBrowserFile1 = new JobSpace.UC.Uc_FilePreviewControl();
            this.btn_load_schema = new System.Windows.Forms.Button();
            this.btn_save_schema = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_bleed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_total_height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_total_width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_right_klapan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_left_klapan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_width)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.nud_bleed);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.nud_total_height);
            this.groupBox1.Controls.Add(this.nud_total_width);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.nud_right_klapan);
            this.groupBox1.Controls.Add(this.nud_left_klapan);
            this.groupBox1.Controls.Add(this.nud_root);
            this.groupBox1.Controls.Add(this.nud_height);
            this.groupBox1.Controls.Add(this.nud_width);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 268);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "параметри";
            // 
            // nud_bleed
            // 
            this.nud_bleed.DecimalPlaces = 1;
            this.nud_bleed.Location = new System.Drawing.Point(126, 164);
            this.nud_bleed.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_bleed.Name = "nud_bleed";
            this.nud_bleed.Size = new System.Drawing.Size(74, 20);
            this.nud_bleed.TabIndex = 5;
            this.nud_bleed.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nud_bleed.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            this.nud_bleed.Enter += new System.EventHandler(this.nud_Enter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 166);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "на обріз, мм";
            // 
            // nud_total_height
            // 
            this.nud_total_height.DecimalPlaces = 1;
            this.nud_total_height.Location = new System.Drawing.Point(126, 242);
            this.nud_total_height.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_total_height.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_total_height.Name = "nud_total_height";
            this.nud_total_height.ReadOnly = true;
            this.nud_total_height.Size = new System.Drawing.Size(74, 20);
            this.nud_total_height.TabIndex = 13;
            this.nud_total_height.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // nud_total_width
            // 
            this.nud_total_width.DecimalPlaces = 1;
            this.nud_total_width.Location = new System.Drawing.Point(126, 220);
            this.nud_total_width.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_total_width.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_total_width.Name = "nud_total_width";
            this.nud_total_width.ReadOnly = true;
            this.nud_total_width.Size = new System.Drawing.Size(74, 20);
            this.nud_total_width.TabIndex = 12;
            this.nud_total_width.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 244);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "загальна висота, мм";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 222);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "загальна ширина, мм";
            // 
            // nud_right_klapan
            // 
            this.nud_right_klapan.DecimalPlaces = 1;
            this.nud_right_klapan.Location = new System.Drawing.Point(126, 137);
            this.nud_right_klapan.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_right_klapan.Name = "nud_right_klapan";
            this.nud_right_klapan.Size = new System.Drawing.Size(74, 20);
            this.nud_right_klapan.TabIndex = 4;
            this.nud_right_klapan.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            this.nud_right_klapan.Enter += new System.EventHandler(this.nud_Enter);
            // 
            // nud_left_klapan
            // 
            this.nud_left_klapan.DecimalPlaces = 1;
            this.nud_left_klapan.Location = new System.Drawing.Point(126, 108);
            this.nud_left_klapan.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_left_klapan.Name = "nud_left_klapan";
            this.nud_left_klapan.Size = new System.Drawing.Size(74, 20);
            this.nud_left_klapan.TabIndex = 3;
            this.nud_left_klapan.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            this.nud_left_klapan.Enter += new System.EventHandler(this.nud_Enter);
            // 
            // nud_root
            // 
            this.nud_root.DecimalPlaces = 1;
            this.nud_root.Location = new System.Drawing.Point(126, 80);
            this.nud_root.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_root.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_root.Name = "nud_root";
            this.nud_root.Size = new System.Drawing.Size(74, 20);
            this.nud_root.TabIndex = 2;
            this.nud_root.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_root.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            this.nud_root.Enter += new System.EventHandler(this.nud_Enter);
            // 
            // nud_height
            // 
            this.nud_height.DecimalPlaces = 1;
            this.nud_height.Location = new System.Drawing.Point(126, 53);
            this.nud_height.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_height.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_height.Name = "nud_height";
            this.nud_height.Size = new System.Drawing.Size(74, 20);
            this.nud_height.TabIndex = 1;
            this.nud_height.Value = new decimal(new int[] {
            297,
            0,
            0,
            0});
            this.nud_height.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            this.nud_height.Enter += new System.EventHandler(this.nud_Enter);
            // 
            // nud_width
            // 
            this.nud_width.DecimalPlaces = 1;
            this.nud_width.Location = new System.Drawing.Point(126, 27);
            this.nud_width.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_width.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_width.Name = "nud_width";
            this.nud_width.Size = new System.Drawing.Size(74, 20);
            this.nud_width.TabIndex = 0;
            this.nud_width.Value = new decimal(new int[] {
            210,
            0,
            0,
            0});
            this.nud_width.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            this.nud_width.Enter += new System.EventHandler(this.nud_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "правий клапан, мм";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "лівий клапан, мм";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "корінець, мм";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "висота сторінки, мм";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ширина сторінки, мм";
            // 
            // btn_apply_schema
            // 
            this.btn_apply_schema.Location = new System.Drawing.Point(116, 329);
            this.btn_apply_schema.Name = "btn_apply_schema";
            this.btn_apply_schema.Size = new System.Drawing.Size(102, 37);
            this.btn_apply_schema.TabIndex = 1;
            this.btn_apply_schema.Text = "файл + схема";
            this.btn_apply_schema.UseVisualStyleBackColor = true;
            this.btn_apply_schema.Click += new System.EventHandler(this.btn_apply_schema_Click);
            // 
            // btn_create_schema
            // 
            this.btn_create_schema.Location = new System.Drawing.Point(116, 286);
            this.btn_create_schema.Name = "btn_create_schema";
            this.btn_create_schema.Size = new System.Drawing.Size(102, 37);
            this.btn_create_schema.TabIndex = 0;
            this.btn_create_schema.Text = "створити схему";
            this.btn_create_schema.UseVisualStyleBackColor = true;
            this.btn_create_schema.Click += new System.EventHandler(this.btn_create_schema_Click);
            // 
            // uc_PreviewBrowserFile1
            // 
            this.uc_PreviewBrowserFile1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_PreviewBrowserFile1.Location = new System.Drawing.Point(224, 12);
            this.uc_PreviewBrowserFile1.Name = "uc_PreviewBrowserFile1";
            this.uc_PreviewBrowserFile1.Size = new System.Drawing.Size(564, 426);
            this.uc_PreviewBrowserFile1.TabIndex = 2;
            // 
            // btn_load_schema
            // 
            this.btn_load_schema.Location = new System.Drawing.Point(12, 329);
            this.btn_load_schema.Name = "btn_load_schema";
            this.btn_load_schema.Size = new System.Drawing.Size(93, 37);
            this.btn_load_schema.TabIndex = 5;
            this.btn_load_schema.Text = "завантажити схему";
            this.btn_load_schema.UseVisualStyleBackColor = true;
            this.btn_load_schema.Click += new System.EventHandler(this.btn_load_schema_Click);
            // 
            // btn_save_schema
            // 
            this.btn_save_schema.Location = new System.Drawing.Point(12, 286);
            this.btn_save_schema.Name = "btn_save_schema";
            this.btn_save_schema.Size = new System.Drawing.Size(93, 37);
            this.btn_save_schema.TabIndex = 4;
            this.btn_save_schema.Text = "зберегти схему";
            this.btn_save_schema.UseVisualStyleBackColor = true;
            this.btn_save_schema.Click += new System.EventHandler(this.btn_save_schema_Click);
            // 
            // FormVisualSoftCover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_load_schema);
            this.Controls.Add(this.btn_save_schema);
            this.Controls.Add(this.btn_apply_schema);
            this.Controls.Add(this.btn_create_schema);
            this.Controls.Add(this.uc_PreviewBrowserFile1);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormVisualSoftCover";
            this.ShowIcon = false;
            this.Text = "М\'яка обкладинка";
            this.Shown += new System.EventHandler(this.FormVisualSoftCover_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_bleed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_total_height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_total_width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_right_klapan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_left_klapan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_width)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nud_total_height;
        private System.Windows.Forms.NumericUpDown nud_total_width;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nud_right_klapan;
        private System.Windows.Forms.NumericUpDown nud_left_klapan;
        private System.Windows.Forms.NumericUpDown nud_root;
        private System.Windows.Forms.NumericUpDown nud_height;
        private System.Windows.Forms.NumericUpDown nud_width;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nud_bleed;
        private System.Windows.Forms.Label label8;
        private UC.Uc_FilePreviewControl uc_PreviewBrowserFile1;
        private System.Windows.Forms.Button btn_apply_schema;
        private System.Windows.Forms.Button btn_create_schema;
        private System.Windows.Forms.Button btn_load_schema;
        private System.Windows.Forms.Button btn_save_schema;
    }
}