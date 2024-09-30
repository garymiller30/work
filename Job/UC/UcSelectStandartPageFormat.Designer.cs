namespace JobSpace.UC
{
    partial class UcSelectStandartPageFormat
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
            this.kryptonComboBoxNumber = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonComboBox_Serial = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel8 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel7 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBoxNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox_Serial)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonComboBoxNumber
            // 
            this.kryptonComboBoxNumber.CornerRoundingRadius = -1F;
            this.kryptonComboBoxNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kryptonComboBoxNumber.DropDownWidth = 30;
            this.kryptonComboBoxNumber.IntegralHeight = false;
            this.kryptonComboBoxNumber.Location = new System.Drawing.Point(60, 24);
            this.kryptonComboBoxNumber.Name = "kryptonComboBoxNumber";
            this.kryptonComboBoxNumber.Size = new System.Drawing.Size(49, 21);
            this.kryptonComboBoxNumber.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.kryptonComboBoxNumber.TabIndex = 30;
            // 
            // kryptonComboBox_Serial
            // 
            this.kryptonComboBox_Serial.CornerRoundingRadius = -1F;
            this.kryptonComboBox_Serial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kryptonComboBox_Serial.DropDownWidth = 30;
            this.kryptonComboBox_Serial.IntegralHeight = false;
            this.kryptonComboBox_Serial.Location = new System.Drawing.Point(3, 24);
            this.kryptonComboBox_Serial.Name = "kryptonComboBox_Serial";
            this.kryptonComboBox_Serial.Size = new System.Drawing.Size(49, 21);
            this.kryptonComboBox_Serial.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.kryptonComboBox_Serial.TabIndex = 29;
            // 
            // kryptonLabel8
            // 
            this.kryptonLabel8.Location = new System.Drawing.Point(60, 3);
            this.kryptonLabel8.Name = "kryptonLabel8";
            this.kryptonLabel8.Size = new System.Drawing.Size(49, 20);
            this.kryptonLabel8.TabIndex = 28;
            this.kryptonLabel8.Values.Text = "Номер";
            // 
            // kryptonLabel7
            // 
            this.kryptonLabel7.Location = new System.Drawing.Point(3, 3);
            this.kryptonLabel7.Name = "kryptonLabel7";
            this.kryptonLabel7.Size = new System.Drawing.Size(41, 20);
            this.kryptonLabel7.TabIndex = 27;
            this.kryptonLabel7.Values.Text = "Серія";
            // 
            // UcSelectStandartPageFormat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.kryptonComboBoxNumber);
            this.Controls.Add(this.kryptonComboBox_Serial);
            this.Controls.Add(this.kryptonLabel8);
            this.Controls.Add(this.kryptonLabel7);
            this.Name = "UcSelectStandartPageFormat";
            this.Size = new System.Drawing.Size(112, 48);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBoxNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox_Serial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonComboBox kryptonComboBoxNumber;
        private Krypton.Toolkit.KryptonComboBox kryptonComboBox_Serial;
        private Krypton.Toolkit.KryptonLabel kryptonLabel8;
        private Krypton.Toolkit.KryptonLabel kryptonLabel7;
    }
}
