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
            this.kryptonRibbon1 = new ComponentFactory.Krypton.Ribbon.KryptonRibbon();
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.buttonSpecAnyWhatNew = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.buttonSpecBackgroundTasks = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonRibbon1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonRibbon1
            // 
            this.kryptonRibbon1.InDesignHelperMode = true;
            this.kryptonRibbon1.Name = "kryptonRibbon1";
            this.kryptonRibbon1.QATUserChange = false;
            this.kryptonRibbon1.RibbonAppButton.AppButtonShowRecentDocs = false;
            this.kryptonRibbon1.RibbonAppButton.AppButtonVisible = false;
            this.kryptonRibbon1.SelectedContext = null;
            this.kryptonRibbon1.SelectedTab = null;
            this.kryptonRibbon1.Size = new System.Drawing.Size(800, 112);
            this.kryptonRibbon1.TabIndex = 0;
            // 
            // kryptonManager1
            // 
            this.kryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Silver;
            this.kryptonManager1.GlobalStrings.Abort = "Перервати";
            this.kryptonManager1.GlobalStrings.Cancel = "Скасувати";
            this.kryptonManager1.GlobalStrings.Close = "Закрити";
            this.kryptonManager1.GlobalStrings.Help = "Допомога";
            this.kryptonManager1.GlobalStrings.Ignore = "Ігнорувати";
            this.kryptonManager1.GlobalStrings.No = "Ні";
            this.kryptonManager1.GlobalStrings.Retry = "Повторити";
            this.kryptonManager1.GlobalStrings.Today = "Сьогодні";
            this.kryptonManager1.GlobalStrings.Yes = "Так";
            // 
            // buttonSpecAnyWhatNew
            // 
            this.buttonSpecAnyWhatNew.Text = "Що нового?";
            this.buttonSpecAnyWhatNew.UniqueName = "f5df068580854553895e762f454be351";
            // 
            // buttonSpecBackgroundTasks
            // 
            this.buttonSpecBackgroundTasks.Text = "Фонові процеси";
            this.buttonSpecBackgroundTasks.UniqueName = "90dfe7b80d0d4b578e00f70e48de4f2f";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BracketType = ComponentFactory.Krypton.Toolkit.BracketType.NOBRACKET;
            this.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.buttonSpecAnyWhatNew,
            this.buttonSpecBackgroundTasks});
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.kryptonRibbon1);
            this.CornerRoundingRadius = 10;
            this.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Primary;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "Form2";
            this.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.StateCommon.Border.Rounding = 10;
            this.Text = "Active Works";
            this.TextExtra = "";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonRibbon1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Ribbon.KryptonRibbon kryptonRibbon1;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonSpecAnyWhatNew;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonSpecBackgroundTasks;
    }
}