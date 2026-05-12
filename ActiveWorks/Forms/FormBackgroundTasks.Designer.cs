namespace ActiveWorks.Forms
{
    partial class FormBackgroundTasks
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
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnTaskName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnStatus = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDetails = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnTaskName);
            this.objectListView1.AllColumns.Add(this.olvColumnStatus);
            this.objectListView1.AllColumns.Add(this.olvColumnDetails);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnTaskName,
            this.olvColumnStatus,
            this.olvColumnDetails});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(0, 0);
            this.objectListView1.MultiSelect = false;
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(650, 268);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnTaskName
            // 
            this.olvColumnTaskName.AspectName = "Name";
            this.olvColumnTaskName.Text = "Задача";
            this.olvColumnTaskName.Width = 190;
            // 
            // olvColumnStatus
            // 
            this.olvColumnStatus.AspectName = "Status";
            this.olvColumnStatus.Text = "Стан";
            this.olvColumnStatus.Width = 90;
            // 
            // olvColumnDetails
            // 
            this.olvColumnDetails.AspectName = "Details";
            this.olvColumnDetails.FillsFreeSpace = true;
            this.olvColumnDetails.Text = "Деталі";
            this.olvColumnDetails.Width = 340;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonCancel.Location = new System.Drawing.Point(0, 268);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(650, 32);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Скасувати вибрану задачу";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FormBackgroundTasks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 300);
            this.Controls.Add(this.objectListView1);
            this.Controls.Add(this.buttonCancel);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(520, 220);
            this.Name = "FormBackgroundTasks";
            this.ShowIcon = false;
            this.Text = "Фонові процеси";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormBackgroundTasks_FormClosed);
            this.Shown += new System.EventHandler(this.FormBackgroundTasks_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnTaskName;
        private BrightIdeasSoftware.OLVColumn olvColumnStatus;
        private BrightIdeasSoftware.OLVColumn olvColumnDetails;
        private System.Windows.Forms.Button buttonCancel;
    }
}
