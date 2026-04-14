namespace SpeedDevelopingPlate
{
    partial class Form1
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnManufacturer = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnPlateFormat = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSpeed = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnTemperature = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnLastDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьФорматПластиныToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.olvColumnType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnManufacturer);
            this.objectListView1.AllColumns.Add(this.olvColumnType);
            this.objectListView1.AllColumns.Add(this.olvColumnPlateFormat);
            this.objectListView1.AllColumns.Add(this.olvColumnSpeed);
            this.objectListView1.AllColumns.Add(this.olvColumnTemperature);
            this.objectListView1.AllColumns.Add(this.olvColumnLastDate);
            this.objectListView1.AllowColumnReorder = true;
            this.objectListView1.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.F2Only;
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnManufacturer,
            this.olvColumnType,
            this.olvColumnPlateFormat,
            this.olvColumnSpeed,
            this.olvColumnTemperature,
            this.olvColumnLastDate});
            this.objectListView1.ContextMenuStrip = this.contextMenuStrip1;
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HeaderWordWrap = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(0, 0);
            this.objectListView1.Margin = new System.Windows.Forms.Padding(4);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.ShowItemToolTips = true;
            this.objectListView1.Size = new System.Drawing.Size(793, 521);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.UseFiltering = true;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.CellEditFinished += new BrightIdeasSoftware.CellEditEventHandler(this.objectListView1_CellEditFinished);
            this.objectListView1.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.objectListView1_CellEditFinishing);
            this.objectListView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.objectListView1_MouseDoubleClick);
            // 
            // olvColumnManufacturer
            // 
            this.olvColumnManufacturer.AspectName = "Manufacturer";
            this.olvColumnManufacturer.CellEditUseWholeCell = true;
            this.olvColumnManufacturer.Text = "производитель";
            this.olvColumnManufacturer.Width = 114;
            // 
            // olvColumnPlateFormat
            // 
            this.olvColumnPlateFormat.AspectName = "Format";
            this.olvColumnPlateFormat.IsEditable = false;
            this.olvColumnPlateFormat.Text = "формат пластины";
            this.olvColumnPlateFormat.Width = 148;
            // 
            // olvColumnSpeed
            // 
            this.olvColumnSpeed.AspectName = "GetLastSpeed";
            this.olvColumnSpeed.CellEditUseWholeCell = true;
            this.olvColumnSpeed.IsEditable = false;
            this.olvColumnSpeed.Text = "скорость";
            this.olvColumnSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // olvColumnTemperature
            // 
            this.olvColumnTemperature.AspectName = "GetLastTemperature";
            this.olvColumnTemperature.CellEditUseWholeCell = true;
            this.olvColumnTemperature.IsEditable = false;
            this.olvColumnTemperature.Text = "температура";
            this.olvColumnTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnTemperature.Width = 99;
            // 
            // olvColumnLastDate
            // 
            this.olvColumnLastDate.AspectName = "GetLastDate";
            this.olvColumnLastDate.IsEditable = false;
            this.olvColumnLastDate.Text = "дата последнего изменения";
            this.olvColumnLastDate.Width = 170;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьФорматПластиныToolStripMenuItem,
            this.toolStripSeparator1,
            this.удалитьToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(228, 54);
            // 
            // добавитьФорматПластиныToolStripMenuItem
            // 
            this.добавитьФорматПластиныToolStripMenuItem.Name = "добавитьФорматПластиныToolStripMenuItem";
            this.добавитьФорматПластиныToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.добавитьФорматПластиныToolStripMenuItem.Text = "добавить формат пластины";
            this.добавитьФорматПластиныToolStripMenuItem.Click += new System.EventHandler(this.добавитьФорматПластиныToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(224, 6);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.удалитьToolStripMenuItem.Text = "удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
            // 
            // olvColumnType
            // 
            this.olvColumnType.AspectName = "PlateType";
            this.olvColumnType.Text = "тип ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 521);
            this.Controls.Add(this.objectListView1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "скорости проявки пластин";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnPlateFormat;
        private BrightIdeasSoftware.OLVColumn olvColumnSpeed;
        private BrightIdeasSoftware.OLVColumn olvColumnLastDate;
        private BrightIdeasSoftware.OLVColumn olvColumnTemperature;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem добавитьФорматПластиныToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvColumnManufacturer;
        private BrightIdeasSoftware.OLVColumn olvColumnType;
    }
}

