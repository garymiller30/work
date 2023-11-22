namespace FtpClient
{
    partial class UcFtpExplorer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcFtpExplorer));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBoxFilter = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonClearFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBoxScripts = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButtonStartScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxCheckTime = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelMin = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonAutoCheck = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelAutoCheck = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCntFiles = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSelectedFiles = new System.Windows.Forms.ToolStripStatusLabel();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnStatus = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFileName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStripBrowser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.новыйЗаказToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новыйЗаказпапкаОписаниеЗаказаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новыйЗаказстрокаНовыйЗаказToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьВТекущийЗаказToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.удалитьФайлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageListStatus2 = new System.Windows.Forms.ImageList(this.components);
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip2FtpScripts = new System.Windows.Forms.ToolStrip();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonBack = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonDisconnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonCreateNewFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonCopySelectedFileListToClipboard = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonResetStatus = new System.Windows.Forms.ToolStripButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.contextMenuStripBrowser.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.LeftToolStripPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBoxFilter,
            this.toolStripButtonClearFilter,
            this.toolStripSeparator1,
            this.toolStripSeparator5,
            this.toolStripComboBoxScripts,
            this.toolStripButtonStartScript,
            this.toolStripTextBoxCheckTime,
            this.toolStripLabelMin,
            this.toolStripButtonAutoCheck});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(375, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripTextBoxFilter
            // 
            this.toolStripTextBoxFilter.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.toolStripTextBoxFilter.Name = "toolStripTextBoxFilter";
            this.toolStripTextBoxFilter.Size = new System.Drawing.Size(70, 25);
            this.toolStripTextBoxFilter.ToolTipText = "фільтр списку";
            this.toolStripTextBoxFilter.TextChanged += new System.EventHandler(this.ToolStripTextBoxFilter_TextChanged);
            // 
            // toolStripButtonClearFilter
            // 
            this.toolStripButtonClearFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonClearFilter.Image = global::FtpClient.Properties.Resources.filter_clear;
            this.toolStripButtonClearFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClearFilter.Name = "toolStripButtonClearFilter";
            this.toolStripButtonClearFilter.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonClearFilter.Text = "очистити фільтр";
            this.toolStripButtonClearFilter.Click += new System.EventHandler(this.ToolStripButtonClearFilter_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripComboBoxScripts
            // 
            this.toolStripComboBoxScripts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxScripts.Name = "toolStripComboBoxScripts";
            this.toolStripComboBoxScripts.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxScripts.ToolTipText = "список скриптів";
            // 
            // toolStripButtonStartScript
            // 
            this.toolStripButtonStartScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStartScript.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStartScript.Image")));
            this.toolStripButtonStartScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStartScript.Name = "toolStripButtonStartScript";
            this.toolStripButtonStartScript.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonStartScript.Text = "Запустити скрипт";
            this.toolStripButtonStartScript.Click += new System.EventHandler(this.toolStripButtonStartScript_Click);
            // 
            // toolStripTextBoxCheckTime
            // 
            this.toolStripTextBoxCheckTime.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.toolStripTextBoxCheckTime.Name = "toolStripTextBoxCheckTime";
            this.toolStripTextBoxCheckTime.Size = new System.Drawing.Size(30, 25);
            this.toolStripTextBoxCheckTime.Text = "2";
            this.toolStripTextBoxCheckTime.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolStripTextBoxCheckTime.ToolTipText = "Через скільки хвилин перевіряти поточну папку";
            // 
            // toolStripLabelMin
            // 
            this.toolStripLabelMin.Name = "toolStripLabelMin";
            this.toolStripLabelMin.Size = new System.Drawing.Size(24, 22);
            this.toolStripLabelMin.Text = "хв.";
            // 
            // toolStripButtonAutoCheck
            // 
            this.toolStripButtonAutoCheck.CheckOnClick = true;
            this.toolStripButtonAutoCheck.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAutoCheck.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAutoCheck.Image")));
            this.toolStripButtonAutoCheck.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAutoCheck.Name = "toolStripButtonAutoCheck";
            this.toolStripButtonAutoCheck.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAutoCheck.Text = "Автоматично перевіряти поточну папку";
            this.toolStripButtonAutoCheck.CheckedChanged += new System.EventHandler(this.ToolStripButtonAutoCheck_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelPath,
            this.toolStripProgressBar1,
            this.toolStripStatusLabelAutoCheck,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelCntFiles,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabelSelectedFiles});
            this.statusStrip1.Location = new System.Drawing.Point(0, 352);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(627, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelPath
            // 
            this.toolStripStatusLabelPath.Name = "toolStripStatusLabelPath";
            this.toolStripStatusLabelPath.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripStatusLabelAutoCheck
            // 
            this.toolStripStatusLabelAutoCheck.Image = global::FtpClient.Properties.Resources.Programming_Watch_icon;
            this.toolStripStatusLabelAutoCheck.Name = "toolStripStatusLabelAutoCheck";
            this.toolStripStatusLabelAutoCheck.Size = new System.Drawing.Size(16, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(51, 17);
            this.toolStripStatusLabel1.Text = "Файлів:";
            // 
            // toolStripStatusLabelCntFiles
            // 
            this.toolStripStatusLabelCntFiles.Name = "toolStripStatusLabelCntFiles";
            this.toolStripStatusLabelCntFiles.Size = new System.Drawing.Size(15, 17);
            this.toolStripStatusLabelCntFiles.Text = "0";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(63, 17);
            this.toolStripStatusLabel2.Text = "Вибрано:";
            // 
            // toolStripStatusLabelSelectedFiles
            // 
            this.toolStripStatusLabelSelectedFiles.Name = "toolStripStatusLabelSelectedFiles";
            this.toolStripStatusLabelSelectedFiles.Size = new System.Drawing.Size(15, 17);
            this.toolStripStatusLabelSelectedFiles.Text = "0";
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnStatus);
            this.objectListView1.AllColumns.Add(this.olvColumnFileName);
            this.objectListView1.AllColumns.Add(this.olvColumnSize);
            this.objectListView1.AllColumns.Add(this.olvColumnDate);
            this.objectListView1.AllowDrop = true;
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnStatus,
            this.olvColumnFileName,
            this.olvColumnSize,
            this.olvColumnDate});
            this.objectListView1.ContextMenuStrip = this.contextMenuStripBrowser;
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.IsSimpleDragSource = true;
            this.objectListView1.IsSimpleDropSink = true;
            this.objectListView1.Location = new System.Drawing.Point(0, 0);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(603, 302);
            this.objectListView1.SmallImageList = this.imageList1;
            this.objectListView1.TabIndex = 2;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.UseFiltering = true;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.CanDrop += new System.EventHandler<BrightIdeasSoftware.OlvDropEventArgs>(this.ObjectListView1_CanDrop);
            this.objectListView1.Dropped += new System.EventHandler<BrightIdeasSoftware.OlvDropEventArgs>(this.ObjectListView1_DroppedAsync);
            this.objectListView1.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.ObjectListView1_FormatRow);
            this.objectListView1.ModelCanDrop += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.ObjectListView1_ModelCanDrop);
            this.objectListView1.ModelDropped += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.ObjectListView1_ModelDropped);
            this.objectListView1.SelectionChanged += new System.EventHandler(this.objectListView1_SelectionChanged);
            this.objectListView1.DoubleClick += new System.EventHandler(this.ObjectListView1_DoubleClick);
            // 
            // olvColumnStatus
            // 
            this.olvColumnStatus.Text = "Status";
            this.olvColumnStatus.Width = 26;
            // 
            // olvColumnFileName
            // 
            this.olvColumnFileName.Text = "FileName";
            this.olvColumnFileName.Width = 300;
            // 
            // olvColumnSize
            // 
            this.olvColumnSize.Text = "Size";
            this.olvColumnSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumnSize.Width = 100;
            // 
            // olvColumnDate
            // 
            this.olvColumnDate.Text = "Date";
            this.olvColumnDate.Width = 100;
            // 
            // contextMenuStripBrowser
            // 
            this.contextMenuStripBrowser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.новыйЗаказToolStripMenuItem,
            this.новыйЗаказпапкаОписаниеЗаказаToolStripMenuItem,
            this.новыйЗаказстрокаНовыйЗаказToolStripMenuItem,
            this.добавитьВТекущийЗаказToolStripMenuItem,
            this.downloadToToolStripMenuItem,
            this.toolStripSeparator2,
            this.удалитьФайлToolStripMenuItem,
            this.markToolStripMenuItem});
            this.contextMenuStripBrowser.Name = "contextMenuStripBrowser";
            this.contextMenuStripBrowser.Size = new System.Drawing.Size(360, 164);
            // 
            // новыйЗаказToolStripMenuItem
            // 
            this.новыйЗаказToolStripMenuItem.Name = "новыйЗаказToolStripMenuItem";
            this.новыйЗаказToolStripMenuItem.Size = new System.Drawing.Size(359, 22);
            this.новыйЗаказToolStripMenuItem.Text = "нове замовлення (папка = номер замовлення)";
            this.новыйЗаказToolStripMenuItem.Click += new System.EventHandler(this.НовыйЗаказToolStripMenuItem_Click);
            // 
            // новыйЗаказпапкаОписаниеЗаказаToolStripMenuItem
            // 
            this.новыйЗаказпапкаОписаниеЗаказаToolStripMenuItem.Name = "новыйЗаказпапкаОписаниеЗаказаToolStripMenuItem";
            this.новыйЗаказпапкаОписаниеЗаказаToolStripMenuItem.Size = new System.Drawing.Size(359, 22);
            this.новыйЗаказпапкаОписаниеЗаказаToolStripMenuItem.Text = "нове замовлення (папка = опис замовлення)";
            this.новыйЗаказпапкаОписаниеЗаказаToolStripMenuItem.Click += new System.EventHandler(this.НовыйЗаказпапкаОписаниеЗаказаToolStripMenuItem_Click);
            // 
            // новыйЗаказстрокаНовыйЗаказToolStripMenuItem
            // 
            this.новыйЗаказстрокаНовыйЗаказToolStripMenuItem.Name = "новыйЗаказстрокаНовыйЗаказToolStripMenuItem";
            this.новыйЗаказстрокаНовыйЗаказToolStripMenuItem.Size = new System.Drawing.Size(359, 22);
            this.новыйЗаказстрокаНовыйЗаказToolStripMenuItem.Text = "нове замовлення (рядок = нове замовлення)";
            this.новыйЗаказстрокаНовыйЗаказToolStripMenuItem.Click += new System.EventHandler(this.НовыйЗаказстрокаНовыйЗаказToolStripMenuItem_Click);
            // 
            // добавитьВТекущийЗаказToolStripMenuItem
            // 
            this.добавитьВТекущийЗаказToolStripMenuItem.Name = "добавитьВТекущийЗаказToolStripMenuItem";
            this.добавитьВТекущийЗаказToolStripMenuItem.Size = new System.Drawing.Size(359, 22);
            this.добавитьВТекущийЗаказToolStripMenuItem.Text = "додати файли в поточне замовлення";
            this.добавитьВТекущийЗаказToolStripMenuItem.Click += new System.EventHandler(this.ДобавитьВТекущийЗаказToolStripMenuItem_Click);
            // 
            // downloadToToolStripMenuItem
            // 
            this.downloadToToolStripMenuItem.Name = "downloadToToolStripMenuItem";
            this.downloadToToolStripMenuItem.Size = new System.Drawing.Size(359, 22);
            this.downloadToToolStripMenuItem.Text = "Завантажити до...";
            this.downloadToToolStripMenuItem.Click += new System.EventHandler(this.DownloadToToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(356, 6);
            // 
            // удалитьФайлToolStripMenuItem
            // 
            this.удалитьФайлToolStripMenuItem.Image = global::FtpClient.Properties.Resources.Erase;
            this.удалитьФайлToolStripMenuItem.Name = "удалитьФайлToolStripMenuItem";
            this.удалитьФайлToolStripMenuItem.Size = new System.Drawing.Size(359, 22);
            this.удалитьФайлToolStripMenuItem.Text = "видалити файл";
            this.удалитьФайлToolStripMenuItem.Click += new System.EventHandler(this.УдалитьФайлToolStripMenuItem_Click);
            // 
            // markToolStripMenuItem
            // 
            this.markToolStripMenuItem.Name = "markToolStripMenuItem";
            this.markToolStripMenuItem.Size = new System.Drawing.Size(359, 22);
            this.markToolStripMenuItem.Text = "прибрати мітку \"новий\"";
            this.markToolStripMenuItem.Click += new System.EventHandler(this.MarkToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folder.png");
            this.imageList1.Images.SetKeyName(1, "Document-txt-icon.png");
            // 
            // imageListStatus2
            // 
            this.imageListStatus2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListStatus2.ImageStream")));
            this.imageListStatus2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListStatus2.Images.SetKeyName(0, "Actions-dialog-ok-icon.png");
            this.imageListStatus2.Images.SetKeyName(1, "Actions-list-add-icon.png");
            this.imageListStatus2.Images.SetKeyName(2, "Status-user-away-icon.png");
            this.imageListStatus2.Images.SetKeyName(3, "Actions-edit-delete-icon.png");
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.toolStrip2FtpScripts);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.objectListView1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(603, 302);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolStripContainer1.LeftToolStripPanel
            // 
            this.toolStripContainer1.LeftToolStripPanel.Controls.Add(this.toolStrip2);
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(627, 352);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip2FtpScripts
            // 
            this.toolStrip2FtpScripts.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2FtpScripts.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.toolStrip2FtpScripts.Location = new System.Drawing.Point(3, 0);
            this.toolStrip2FtpScripts.Name = "toolStrip2FtpScripts";
            this.toolStrip2FtpScripts.Size = new System.Drawing.Size(111, 25);
            this.toolStrip2FtpScripts.TabIndex = 0;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRefresh,
            this.toolStripButtonBack,
            this.toolStripSeparator7,
            this.toolStripButtonDisconnect,
            this.toolStripSeparator8,
            this.toolStripButtonCreateNewFolder,
            this.toolStripSeparator9,
            this.toolStripButtonCopySelectedFileListToClipboard,
            this.toolStripButtonResetStatus});
            this.toolStrip2.Location = new System.Drawing.Point(0, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(24, 167);
            this.toolStrip2.TabIndex = 0;
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Image = global::FtpClient.Properties.Resources.arrow_refresh;
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(22, 20);
            this.toolStripButtonRefresh.Text = "Оновити";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.ToolStripButtonRefresh_Click);
            // 
            // toolStripButtonBack
            // 
            this.toolStripButtonBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBack.Image = global::FtpClient.Properties.Resources.arrow_turn_left;
            this.toolStripButtonBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBack.Name = "toolStripButtonBack";
            this.toolStripButtonBack.Size = new System.Drawing.Size(22, 20);
            this.toolStripButtonBack.Text = "Назад";
            this.toolStripButtonBack.Click += new System.EventHandler(this.ToolStripButtonBack_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(22, 6);
            // 
            // toolStripButtonDisconnect
            // 
            this.toolStripButtonDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDisconnect.Image = global::FtpClient.Properties.Resources.plug_disconnect_prohibition_icon;
            this.toolStripButtonDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDisconnect.Name = "toolStripButtonDisconnect";
            this.toolStripButtonDisconnect.Size = new System.Drawing.Size(22, 20);
            this.toolStripButtonDisconnect.Text = "Роз\'єднати";
            this.toolStripButtonDisconnect.Click += new System.EventHandler(this.ToolStripButtonDisconnect_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(22, 6);
            // 
            // toolStripButtonCreateNewFolder
            // 
            this.toolStripButtonCreateNewFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCreateNewFolder.Image = global::FtpClient.Properties.Resources.folder_new_icon;
            this.toolStripButtonCreateNewFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCreateNewFolder.Name = "toolStripButtonCreateNewFolder";
            this.toolStripButtonCreateNewFolder.Size = new System.Drawing.Size(22, 20);
            this.toolStripButtonCreateNewFolder.Text = "Створити папку";
            this.toolStripButtonCreateNewFolder.Click += new System.EventHandler(this.ToolStripButtonNewFolder_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(22, 6);
            // 
            // toolStripButtonCopySelectedFileListToClipboard
            // 
            this.toolStripButtonCopySelectedFileListToClipboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCopySelectedFileListToClipboard.Image = global::FtpClient.Properties.Resources.application_view_list_icon;
            this.toolStripButtonCopySelectedFileListToClipboard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCopySelectedFileListToClipboard.Name = "toolStripButtonCopySelectedFileListToClipboard";
            this.toolStripButtonCopySelectedFileListToClipboard.Size = new System.Drawing.Size(22, 20);
            this.toolStripButtonCopySelectedFileListToClipboard.Text = "скопіювати у буфер імена файлів";
            this.toolStripButtonCopySelectedFileListToClipboard.ToolTipText = "скопіювати у буфер імена вибраних файлів";
            this.toolStripButtonCopySelectedFileListToClipboard.Click += new System.EventHandler(this.toolStripButtonCopySelectedFileListToClipboard_Click);
            // 
            // toolStripButtonResetStatus
            // 
            this.toolStripButtonResetStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonResetStatus.Image = global::FtpClient.Properties.Resources.Actions_dialog_ok_icon;
            this.toolStripButtonResetStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonResetStatus.Name = "toolStripButtonResetStatus";
            this.toolStripButtonResetStatus.Size = new System.Drawing.Size(22, 20);
            this.toolStripButtonResetStatus.Text = "прибрати мітку \"новий\" у файлах";
            this.toolStripButtonResetStatus.Click += new System.EventHandler(this.MarkToolStripMenuItem_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(627, 374);
            this.kryptonPanel1.TabIndex = 4;
            // 
            // UcFtpExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UcFtpExplorer";
            this.Size = new System.Drawing.Size(627, 374);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.contextMenuStripBrowser.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.LeftToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.LeftToolStripPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnFileName;
        private BrightIdeasSoftware.OLVColumn olvColumnSize;
        private BrightIdeasSoftware.OLVColumn olvColumnDate;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBrowser;
        private System.Windows.Forms.ToolStripMenuItem новыйЗаказToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPath;
        private System.Windows.Forms.ToolStripMenuItem добавитьВТекущийЗаказToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxFilter;
        private System.Windows.Forms.ToolStripButton toolStripButtonClearFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem удалитьФайлToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvColumnStatus;
        private System.Windows.Forms.ToolStripMenuItem новыйЗаказпапкаОписаниеЗаказаToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButtonAutoCheck;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelAutoCheck;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxCheckTime;
        private System.Windows.Forms.ToolStripLabel toolStripLabelMin;
        private System.Windows.Forms.ToolStripMenuItem markToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListStatus2;
        private System.Windows.Forms.ToolStripMenuItem downloadToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem новыйЗаказстрокаНовыйЗаказToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip2FtpScripts;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCntFiles;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSelectedFiles;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxScripts;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripButton toolStripButtonBack;
        private System.Windows.Forms.ToolStripButton toolStripButtonCreateNewFolder;
        private System.Windows.Forms.ToolStripButton toolStripButtonDisconnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton toolStripButtonCopySelectedFileListToClipboard;
        private System.Windows.Forms.ToolStripButton toolStripButtonResetStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonStartScript;
    }
}
