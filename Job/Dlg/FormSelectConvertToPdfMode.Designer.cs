namespace Job.Dlg
{
    partial class FormSelectConvertToPdfMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectConvertToPdfMode));
            this.kryptonButtonMultiple = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButtonSingle = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonCheckBoxMoveToTrash = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.kryptonGroupBox1 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.kryptonRadioButton5mm = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.kryptonRadioButton3mm = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.kryptonRadioButton2mm = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.kryptonRadioButton1mm = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.kryptonRadioButtonNone = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonButtonMultiple
            // 
            this.kryptonButtonMultiple.Location = new System.Drawing.Point(12, 118);
            this.kryptonButtonMultiple.Name = "kryptonButtonMultiple";
            this.kryptonButtonMultiple.Size = new System.Drawing.Size(156, 125);
            this.kryptonButtonMultiple.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.kryptonButtonMultiple.TabIndex = 0;
            this.kryptonButtonMultiple.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonButtonMultiple.Values.Image")));
            this.kryptonButtonMultiple.Values.Text = "все у один файл";
            this.kryptonButtonMultiple.Click += new System.EventHandler(this.kryptonButtonMultiple_Click);
            // 
            // kryptonButtonSingle
            // 
            this.kryptonButtonSingle.Location = new System.Drawing.Point(174, 118);
            this.kryptonButtonSingle.Name = "kryptonButtonSingle";
            this.kryptonButtonSingle.Size = new System.Drawing.Size(156, 125);
            this.kryptonButtonSingle.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.kryptonButtonSingle.TabIndex = 1;
            this.kryptonButtonSingle.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonButtonSingle.Values.Image")));
            this.kryptonButtonSingle.Values.Text = "кожний файл окремо";
            this.kryptonButtonSingle.Click += new System.EventHandler(this.kryptonButtonSingle_Click);
            // 
            // kryptonCheckBoxMoveToTrash
            // 
            this.kryptonCheckBoxMoveToTrash.Location = new System.Drawing.Point(47, 12);
            this.kryptonCheckBoxMoveToTrash.Name = "kryptonCheckBoxMoveToTrash";
            this.kryptonCheckBoxMoveToTrash.Size = new System.Drawing.Size(209, 21);
            this.kryptonCheckBoxMoveToTrash.TabIndex = 2;
            this.kryptonCheckBoxMoveToTrash.Values.Text = "перемістити оригінал у кошик";
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Location = new System.Drawing.Point(12, 43);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonRadioButton5mm);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonRadioButton3mm);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonRadioButton2mm);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonRadioButton1mm);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonRadioButtonNone);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(318, 60);
            this.kryptonGroupBox1.TabIndex = 3;
            this.kryptonGroupBox1.Values.Heading = "Виставити TrimBox";
            // 
            // kryptonRadioButton5mm
            // 
            this.kryptonRadioButton5mm.Location = new System.Drawing.Point(251, 7);
            this.kryptonRadioButton5mm.Name = "kryptonRadioButton5mm";
            this.kryptonRadioButton5mm.Size = new System.Drawing.Size(53, 21);
            this.kryptonRadioButton5mm.TabIndex = 4;
            this.kryptonRadioButton5mm.Values.Text = "5 мм";
            // 
            // kryptonRadioButton3mm
            // 
            this.kryptonRadioButton3mm.Location = new System.Drawing.Point(192, 7);
            this.kryptonRadioButton3mm.Name = "kryptonRadioButton3mm";
            this.kryptonRadioButton3mm.Size = new System.Drawing.Size(53, 21);
            this.kryptonRadioButton3mm.TabIndex = 3;
            this.kryptonRadioButton3mm.Values.Text = "3 мм";
            // 
            // kryptonRadioButton2mm
            // 
            this.kryptonRadioButton2mm.Location = new System.Drawing.Point(133, 7);
            this.kryptonRadioButton2mm.Name = "kryptonRadioButton2mm";
            this.kryptonRadioButton2mm.Size = new System.Drawing.Size(53, 21);
            this.kryptonRadioButton2mm.TabIndex = 2;
            this.kryptonRadioButton2mm.Values.Text = "2 мм";
            // 
            // kryptonRadioButton1mm
            // 
            this.kryptonRadioButton1mm.Location = new System.Drawing.Point(74, 7);
            this.kryptonRadioButton1mm.Name = "kryptonRadioButton1mm";
            this.kryptonRadioButton1mm.Size = new System.Drawing.Size(53, 21);
            this.kryptonRadioButton1mm.TabIndex = 1;
            this.kryptonRadioButton1mm.Values.Text = "1 мм";
            // 
            // kryptonRadioButtonNone
            // 
            this.kryptonRadioButtonNone.Checked = true;
            this.kryptonRadioButtonNone.Location = new System.Drawing.Point(13, 7);
            this.kryptonRadioButtonNone.Name = "kryptonRadioButtonNone";
            this.kryptonRadioButtonNone.Size = new System.Drawing.Size(34, 21);
            this.kryptonRadioButtonNone.TabIndex = 0;
            this.kryptonRadioButtonNone.Values.Text = "ні";
            // 
            // FormSelectConvertToPdfMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 256);
            this.Controls.Add(this.kryptonGroupBox1);
            this.Controls.Add(this.kryptonCheckBoxMoveToTrash);
            this.Controls.Add(this.kryptonButtonSingle);
            this.Controls.Add(this.kryptonButtonMultiple);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectConvertToPdfMode";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Як конвертувати у PDF?";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButtonMultiple;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButtonSingle;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox kryptonCheckBoxMoveToTrash;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton kryptonRadioButton5mm;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton kryptonRadioButton3mm;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton kryptonRadioButton2mm;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton kryptonRadioButton1mm;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton kryptonRadioButtonNone;
    }
}