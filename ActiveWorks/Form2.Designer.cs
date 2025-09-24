namespace ActiveWorks
{
    partial class Form2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.kryptonRibbon1 = new Krypton.Ribbon.KryptonRibbon();
            this.buttonSpecAnyWhatNew = new Krypton.Toolkit.ButtonSpecAny();
            this.buttonSpecBackgroundTasks = new Krypton.Toolkit.ButtonSpecAny();
            this.buttonSpecAnyIssue = new Krypton.Toolkit.ButtonSpecAny();
            this.kryptonManager1 = new Krypton.Toolkit.KryptonManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonRibbon1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonRibbon1
            // 
            this.kryptonRibbon1.AllowFormIntegrate = true;
            this.kryptonRibbon1.InDesignHelperMode = true;
            this.kryptonRibbon1.Name = "kryptonRibbon1";
            this.kryptonRibbon1.QATUserChange = false;
            this.kryptonRibbon1.RibbonAppButton.AppButtonShowRecentDocs = false;
            this.kryptonRibbon1.RibbonAppButton.AppButtonVisible = false;
            this.kryptonRibbon1.SelectedTab = null;
            this.kryptonRibbon1.Size = new System.Drawing.Size(800, 112);
            this.kryptonRibbon1.TabIndex = 0;
            this.kryptonRibbon1.SelectedTabChanged += new System.EventHandler(this.kryptonRibbon1_SelectedTabChanged_1);
            // 
            // buttonSpecAnyWhatNew
            // 
            this.buttonSpecAnyWhatNew.Text = "Що нового?";
            this.buttonSpecAnyWhatNew.UniqueName = "f5df068580854553895e762f454be351";
            // 
            // buttonSpecBackgroundTasks
            // 
            this.buttonSpecBackgroundTasks.ColorMap = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.buttonSpecBackgroundTasks.Text = "Фонові процеси";
            this.buttonSpecBackgroundTasks.UniqueName = "90dfe7b80d0d4b578e00f70e48de4f2f";
            // 
            // buttonSpecAnyIssue
            // 
            this.buttonSpecAnyIssue.Text = "Знайшли помилку?";
            this.buttonSpecAnyIssue.ToolTipBody = "Якщо є обліковий запис на Github, то можете залишити повідомлення.";
            this.buttonSpecAnyIssue.ToolTipTitle = "Знайшли помилку?";
            this.buttonSpecAnyIssue.UniqueName = "eadddcf11d5943559a8440e83cbba81e";
            // 
            // kryptonManager1
            // 
            this.kryptonManager1.GlobalPaletteMode = Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ButtonSpecs.Add(this.buttonSpecAnyIssue);
            this.ButtonSpecs.Add(this.buttonSpecAnyWhatNew);
            this.ButtonSpecs.Add(this.buttonSpecBackgroundTasks);
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.kryptonRibbon1);
            this.CornerRoundingRadius = 0F;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "Form2";
            this.PaletteMode = Krypton.Toolkit.PaletteMode.SparklePurpleLightMode;
            this.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.StateCommon.Border.Rounding = 0F;
            this.Text = "Active Works";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonRibbon1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Ribbon.KryptonRibbon kryptonRibbon1;
        private Krypton.Toolkit.ButtonSpecAny buttonSpecAnyWhatNew;
        private Krypton.Toolkit.ButtonSpecAny buttonSpecBackgroundTasks;
        private Krypton.Toolkit.ButtonSpecAny buttonSpecAnyIssue;
        private Krypton.Toolkit.KryptonManager kryptonManager1;
    }
}