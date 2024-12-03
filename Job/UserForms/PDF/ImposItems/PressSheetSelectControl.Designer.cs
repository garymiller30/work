namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class PressSheetSelectControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PressSheetSelectControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxSheets = new System.Windows.Forms.ComboBox();
            this.btn_delSheet = new System.Windows.Forms.Button();
            this.buttonAddSheet = new System.Windows.Forms.Button();
            this.buttonEditSheet = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBoxSheetPlaceType = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxSheets);
            this.groupBox1.Controls.Add(this.btn_delSheet);
            this.groupBox1.Controls.Add(this.buttonAddSheet);
            this.groupBox1.Controls.Add(this.buttonEditSheet);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 53);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Шаблони форматів";
            // 
            // comboBoxSheets
            // 
            this.comboBoxSheets.DisplayMember = "Description";
            this.comboBoxSheets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSheets.FormattingEnabled = true;
            this.comboBoxSheets.Location = new System.Drawing.Point(8, 19);
            this.comboBoxSheets.Name = "comboBoxSheets";
            this.comboBoxSheets.Size = new System.Drawing.Size(144, 21);
            this.comboBoxSheets.TabIndex = 0;
            // 
            // btn_delSheet
            // 
            this.btn_delSheet.Image = ((System.Drawing.Image)(resources.GetObject("btn_delSheet.Image")));
            this.btn_delSheet.Location = new System.Drawing.Point(212, 15);
            this.btn_delSheet.Name = "btn_delSheet";
            this.btn_delSheet.Size = new System.Drawing.Size(27, 27);
            this.btn_delSheet.TabIndex = 5;
            this.btn_delSheet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_delSheet.UseVisualStyleBackColor = true;
            // 
            // buttonAddSheet
            // 
            this.buttonAddSheet.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddSheet.Image")));
            this.buttonAddSheet.Location = new System.Drawing.Point(155, 15);
            this.buttonAddSheet.Name = "buttonAddSheet";
            this.buttonAddSheet.Size = new System.Drawing.Size(27, 27);
            this.buttonAddSheet.TabIndex = 2;
            this.buttonAddSheet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonAddSheet.UseVisualStyleBackColor = true;
            // 
            // buttonEditSheet
            // 
            this.buttonEditSheet.Image = ((System.Drawing.Image)(resources.GetObject("buttonEditSheet.Image")));
            this.buttonEditSheet.Location = new System.Drawing.Point(182, 15);
            this.buttonEditSheet.Name = "buttonEditSheet";
            this.buttonEditSheet.Size = new System.Drawing.Size(27, 27);
            this.buttonEditSheet.TabIndex = 3;
            this.buttonEditSheet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonEditSheet.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(23, 70);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(55, 13);
            this.label15.TabIndex = 9;
            this.label15.Text = "тип друку";
            // 
            // comboBoxSheetPlaceType
            // 
            this.comboBoxSheetPlaceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSheetPlaceType.FormattingEnabled = true;
            this.comboBoxSheetPlaceType.Location = new System.Drawing.Point(83, 67);
            this.comboBoxSheetPlaceType.Name = "comboBoxSheetPlaceType";
            this.comboBoxSheetPlaceType.Size = new System.Drawing.Size(144, 21);
            this.comboBoxSheetPlaceType.TabIndex = 8;
            // 
            // PressSheetSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label15);
            this.Controls.Add(this.comboBoxSheetPlaceType);
            this.Controls.Add(this.groupBox1);
            this.Name = "PressSheetSelectControl";
            this.Size = new System.Drawing.Size(256, 101);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxSheets;
        private System.Windows.Forms.Button btn_delSheet;
        private System.Windows.Forms.Button buttonAddSheet;
        private System.Windows.Forms.Button buttonEditSheet;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBoxSheetPlaceType;
    }
}
