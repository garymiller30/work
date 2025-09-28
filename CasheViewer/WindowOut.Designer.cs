namespace CasheViewer
{
    partial class WindowOut
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowOut));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_Total = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_TotalDecimal = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_SelectedTxt = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_Selected = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonJobs = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReportYears = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPayed = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTotalPayedByCustomer = new System.Windows.Forms.ToolStripButton();
            this.panelControlReport = new System.Windows.Forms.Panel();
            this.tsb_pay_custom = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_Total,
            this.toolStripStatusLabel_TotalDecimal,
            this.toolStripStatusLabel_SelectedTxt,
            this.toolStripStatusLabel_Selected});
            this.statusStrip1.Location = new System.Drawing.Point(0, 233);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(433, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_Total
            // 
            this.toolStripStatusLabel_Total.Name = "toolStripStatusLabel_Total";
            this.toolStripStatusLabel_Total.Size = new System.Drawing.Size(48, 17);
            this.toolStripStatusLabel_Total.Text = "Всього:";
            // 
            // toolStripStatusLabel_TotalDecimal
            // 
            this.toolStripStatusLabel_TotalDecimal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel_TotalDecimal.Name = "toolStripStatusLabel_TotalDecimal";
            this.toolStripStatusLabel_TotalDecimal.Size = new System.Drawing.Size(14, 17);
            this.toolStripStatusLabel_TotalDecimal.Text = "0";
            // 
            // toolStripStatusLabel_SelectedTxt
            // 
            this.toolStripStatusLabel_SelectedTxt.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.toolStripStatusLabel_SelectedTxt.Name = "toolStripStatusLabel_SelectedTxt";
            this.toolStripStatusLabel_SelectedTxt.Size = new System.Drawing.Size(58, 17);
            this.toolStripStatusLabel_SelectedTxt.Text = "Вибрано:";
            // 
            // toolStripStatusLabel_Selected
            // 
            this.toolStripStatusLabel_Selected.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel_Selected.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.toolStripStatusLabel_Selected.Name = "toolStripStatusLabel_Selected";
            this.toolStripStatusLabel_Selected.Size = new System.Drawing.Size(14, 17);
            this.toolStripStatusLabel_Selected.Text = "0";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonJobs,
            this.toolStripButtonReportYears,
            this.toolStripSeparator4,
            this.toolStripButtonPayed,
            this.tsb_pay_custom,
            this.toolStripSeparator5,
            this.toolStripButtonSettings,
            this.toolStripButtonTotalPayedByCustomer});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(433, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonJobs
            // 
            this.toolStripButtonJobs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonJobs.Image = global::CasheViewer.Properties.Resources.Food_List_Ingredients_icon;
            this.toolStripButtonJobs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonJobs.Name = "toolStripButtonJobs";
            this.toolStripButtonJobs.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonJobs.Text = "Показати по роботам";
            this.toolStripButtonJobs.Click += new System.EventHandler(this.toolStripButtonJobs_Click);
            // 
            // toolStripButtonReportYears
            // 
            this.toolStripButtonReportYears.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReportYears.Image = global::CasheViewer.Properties.Resources.Calendar_selection_all_icon;
            this.toolStripButtonReportYears.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReportYears.Name = "toolStripButtonReportYears";
            this.toolStripButtonReportYears.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonReportYears.Text = "Показати по роках";
            this.toolStripButtonReportYears.Click += new System.EventHandler(this.toolStripButtonReportYears_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonPayed
            // 
            this.toolStripButtonPayed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPayed.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPayed.Image")));
            this.toolStripButtonPayed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPayed.Name = "toolStripButtonPayed";
            this.toolStripButtonPayed.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPayed.Text = "сплатити вибрані";
            this.toolStripButtonPayed.Click += new System.EventHandler(this.toolStripButtonPayed_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSettings
            // 
            this.toolStripButtonSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSettings.Image")));
            this.toolStripButtonSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSettings.Name = "toolStripButtonSettings";
            this.toolStripButtonSettings.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSettings.Text = "Налаштування";
            this.toolStripButtonSettings.Click += new System.EventHandler(this.toolStripButtonSettings_Click);
            // 
            // toolStripButtonTotalPayedByCustomer
            // 
            this.toolStripButtonTotalPayedByCustomer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTotalPayedByCustomer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTotalPayedByCustomer.Image")));
            this.toolStripButtonTotalPayedByCustomer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTotalPayedByCustomer.Name = "toolStripButtonTotalPayedByCustomer";
            this.toolStripButtonTotalPayedByCustomer.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTotalPayedByCustomer.Text = "Показати скільки було сплачено взагалі";
            this.toolStripButtonTotalPayedByCustomer.Click += new System.EventHandler(this.toolStripButtonTotalPayedByCustomer_Click);
            // 
            // panelControlReport
            // 
            this.panelControlReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlReport.Location = new System.Drawing.Point(0, 25);
            this.panelControlReport.Name = "panelControlReport";
            this.panelControlReport.Size = new System.Drawing.Size(433, 208);
            this.panelControlReport.TabIndex = 3;
            // 
            // tsb_pay_custom
            // 
            this.tsb_pay_custom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_pay_custom.Image = ((System.Drawing.Image)(resources.GetObject("tsb_pay_custom.Image")));
            this.tsb_pay_custom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_pay_custom.Name = "tsb_pay_custom";
            this.tsb_pay_custom.Size = new System.Drawing.Size(23, 22);
            this.tsb_pay_custom.Text = "Відмітити певну суму";
            this.tsb_pay_custom.Click += new System.EventHandler(this.tsb_pay_custom_Click);
            // 
            // WindowOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControlReport);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "WindowOut";
            this.Size = new System.Drawing.Size(433, 255);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Total;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_TotalDecimal;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_SelectedTxt;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Selected;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panelControlReport;
        private System.Windows.Forms.ToolStripButton toolStripButtonSettings;
        private System.Windows.Forms.ToolStripButton toolStripButtonPayed;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonJobs;
        private System.Windows.Forms.ToolStripButton toolStripButtonReportYears;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButtonTotalPayedByCustomer;
        private System.Windows.Forms.ToolStripButton tsb_pay_custom;
    }
}
