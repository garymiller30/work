namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class ImposBindingControl
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_SelectBindType = new System.Windows.Forms.ComboBox();
            this.panelBindingControl = new System.Windows.Forms.Panel();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_SelectBindType);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(299, 47);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "як розкладати?";
            // 
            // cb_SelectBindType
            // 
            this.cb_SelectBindType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_SelectBindType.FormattingEnabled = true;
            this.cb_SelectBindType.Location = new System.Drawing.Point(7, 19);
            this.cb_SelectBindType.Name = "cb_SelectBindType";
            this.cb_SelectBindType.Size = new System.Drawing.Size(166, 21);
            this.cb_SelectBindType.TabIndex = 0;
            this.cb_SelectBindType.SelectedIndexChanged += new System.EventHandler(this.cb_SelectBindType_SelectedIndexChanged);
            // 
            // panelBindingControl
            // 
            this.panelBindingControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBindingControl.AutoScroll = true;
            this.panelBindingControl.Location = new System.Drawing.Point(0, 56);
            this.panelBindingControl.Name = "panelBindingControl";
            this.panelBindingControl.Size = new System.Drawing.Size(299, 123);
            this.panelBindingControl.TabIndex = 9;
            // 
            // ImposBindingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBindingControl);
            this.Controls.Add(this.groupBox3);
            this.Name = "ImposBindingControl";
            this.Size = new System.Drawing.Size(305, 182);
            
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cb_SelectBindType;
        private System.Windows.Forms.Panel panelBindingControl;
    }
}
