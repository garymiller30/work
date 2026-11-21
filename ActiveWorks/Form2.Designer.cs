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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            kryptonRibbon1 = new Krypton.Ribbon.KryptonRibbon();
            buttonSpecAnyWhatNew = new Krypton.Toolkit.ButtonSpecAny();
            buttonSpecBackgroundTasks = new Krypton.Toolkit.ButtonSpecAny();
            buttonSpecAnyIssue = new Krypton.Toolkit.ButtonSpecAny();
            kryptonManager1 = new Krypton.Toolkit.KryptonManager(components);
            statusStripMain = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabelUpdate = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripSplitButton_check_update = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)kryptonRibbon1).BeginInit();
            statusStripMain.SuspendLayout();
            SuspendLayout();
            // 
            // kryptonRibbon1
            // 
            kryptonRibbon1.Name = "kryptonRibbon1";
            kryptonRibbon1.QATUserChange = false;
            kryptonRibbon1.RibbonFileAppButton.AppButtonShowRecentDocs = false;
            kryptonRibbon1.RibbonFileAppButton.AppButtonVisible = false;
            kryptonRibbon1.RibbonFileAppButton.FormCloseBoxVisible = true;
            kryptonRibbon1.SelectedTab = null;
            kryptonRibbon1.Size = new System.Drawing.Size(933, 112);
            kryptonRibbon1.TabIndex = 0;
            // 
            // buttonSpecAnyWhatNew
            // 
            buttonSpecAnyWhatNew.Text = "Що нового?";
            buttonSpecAnyWhatNew.UniqueName = "f5df068580854553895e762f454be351";
            // 
            // buttonSpecBackgroundTasks
            // 
            buttonSpecBackgroundTasks.ColorMap = System.Drawing.Color.FromArgb(0, 0, 192);
            buttonSpecBackgroundTasks.Text = "Фонові процеси";
            buttonSpecBackgroundTasks.UniqueName = "90dfe7b80d0d4b578e00f70e48de4f2f";
            // 
            // buttonSpecAnyIssue
            // 
            buttonSpecAnyIssue.Text = "Знайшли помилку?";
            buttonSpecAnyIssue.ToolTipBody = "Якщо є обліковий запис на Github, то можете залишити повідомлення.";
            buttonSpecAnyIssue.ToolTipTitle = "Знайшли помилку?";
            buttonSpecAnyIssue.UniqueName = "eadddcf11d5943559a8440e83cbba81e";
            // 
            // kryptonManager1
            // 
            kryptonManager1.GlobalPaletteMode = Krypton.Toolkit.PaletteMode.Office2010Silver;
            kryptonManager1.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
            kryptonManager1.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
            // 
            // statusStripMain
            // 
            statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabelUpdate, toolStripSplitButton_check_update });
            statusStripMain.Location = new System.Drawing.Point(0, 497);
            statusStripMain.Name = "statusStripMain";
            statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStripMain.Size = new System.Drawing.Size(933, 22);
            statusStripMain.TabIndex = 1;
            // 
            // toolStripStatusLabelUpdate
            // 
            toolStripStatusLabelUpdate.IsLink = true;
            toolStripStatusLabelUpdate.Name = "toolStripStatusLabelUpdate";
            toolStripStatusLabelUpdate.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripSplitButton_check_update
            // 
            toolStripSplitButton_check_update.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripSplitButton_check_update.Image = Properties.Resources.Refresh;
            toolStripSplitButton_check_update.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripSplitButton_check_update.Name = "toolStripSplitButton_check_update";
            toolStripSplitButton_check_update.Size = new System.Drawing.Size(23, 20);
            toolStripSplitButton_check_update.Text = "Перевірити оновлення";
            toolStripSplitButton_check_update.ToolTipText = "Примусова перевірка оновлення";
            toolStripSplitButton_check_update.Click += toolStripSplitButton_check_update_ClickAsync;
            // 
            // Form2
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ButtonSpecs.Add(buttonSpecAnyIssue);
            ButtonSpecs.Add(buttonSpecAnyWhatNew);
            ButtonSpecs.Add(buttonSpecBackgroundTasks);
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(statusStripMain);
            Controls.Add(kryptonRibbon1);
            FormTitleAlign = Krypton.Toolkit.PaletteRelativeAlign.Center;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "Form2";
            StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom | Krypton.Toolkit.PaletteDrawBorders.Left | Krypton.Toolkit.PaletteDrawBorders.Right;
            Text = "Active Works";
            TransparencyKey = System.Drawing.Color.Empty;
            FormClosing += Form2_FormClosing;
            Load += Form2_Load;
            ((System.ComponentModel.ISupportInitialize)kryptonRibbon1).EndInit();
            statusStripMain.ResumeLayout(false);
            statusStripMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Krypton.Ribbon.KryptonRibbon kryptonRibbon1;
        private Krypton.Toolkit.ButtonSpecAny buttonSpecAnyWhatNew;
        private Krypton.Toolkit.ButtonSpecAny buttonSpecBackgroundTasks;
        private Krypton.Toolkit.ButtonSpecAny buttonSpecAnyIssue;
        private Krypton.Toolkit.KryptonManager kryptonManager1;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelUpdate;
        private System.Windows.Forms.ToolStripButton toolStripSplitButton_check_update;
    }
}
