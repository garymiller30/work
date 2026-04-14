namespace StaticFiles
{
    partial class WindowOut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowOut));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBoxCustomers = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButtonSetByOrder = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBoxCustomers,
            this.toolStripButtonSetByOrder});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(446, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripComboBoxCustomers
            // 
            this.toolStripComboBoxCustomers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxCustomers.Name = "toolStripComboBoxCustomers";
            this.toolStripComboBoxCustomers.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxCustomers.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxCustomers_SelectedIndexChanged);
            // 
            // toolStripButtonSetByOrder
            // 
            this.toolStripButtonSetByOrder.CheckOnClick = true;
            this.toolStripButtonSetByOrder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSetByOrder.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSetByOrder.Image")));
            this.toolStripButtonSetByOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSetByOrder.Name = "toolStripButtonSetByOrder";
            this.toolStripButtonSetByOrder.Size = new System.Drawing.Size(76, 22);
            this.toolStripButtonSetByOrder.Text = "Set by Order";
            // 
            // WindowOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Name = "WindowOut";
            this.Size = new System.Drawing.Size(446, 263);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxCustomers;
        private System.Windows.Forms.ToolStripButton toolStripButtonSetByOrder;
        
    }
}
