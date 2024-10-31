namespace JobSpace.UserForms.PDF
{
    partial class FormSelectSpotColor
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
            this.comboBoxListColor = new System.Windows.Forms.ComboBox();
            this.comboBoxListTables = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelColor = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxListColor
            // 
            this.comboBoxListColor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxListColor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxListColor.FormattingEnabled = true;
            this.comboBoxListColor.Location = new System.Drawing.Point(19, 80);
            this.comboBoxListColor.Name = "comboBoxListColor";
            this.comboBoxListColor.Size = new System.Drawing.Size(184, 21);
            this.comboBoxListColor.TabIndex = 15;
            this.comboBoxListColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxListColor_SelectedIndexChanged);
            // 
            // comboBoxListTables
            // 
            this.comboBoxListTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxListTables.FormattingEnabled = true;
            this.comboBoxListTables.Location = new System.Drawing.Point(6, 19);
            this.comboBoxListTables.Name = "comboBoxListTables";
            this.comboBoxListTables.Size = new System.Drawing.Size(239, 21);
            this.comboBoxListTables.TabIndex = 14;
            this.comboBoxListTables.SelectedIndexChanged += new System.EventHandler(this.comboBoxListTables_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(107, 118);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 33);
            this.button1.TabIndex = 16;
            this.button1.Text = "Вибрати";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxListTables);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 52);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тип";
            // 
            // panelColor
            // 
            this.panelColor.Location = new System.Drawing.Point(209, 74);
            this.panelColor.Name = "panelColor";
            this.panelColor.Size = new System.Drawing.Size(61, 33);
            this.panelColor.TabIndex = 18;
            // 
            // FormSelectSpotColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 159);
            this.Controls.Add(this.panelColor);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxListColor);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectSpotColor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Вибрати пантон";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxListColor;
        private System.Windows.Forms.ComboBox comboBoxListTables;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panelColor;
    }
}