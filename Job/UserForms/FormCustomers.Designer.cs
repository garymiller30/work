namespace Job.UserForms
{
    partial class FormCustomers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCustomers));
            this.objectListView_Customers = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn_Customer = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFTP = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStrip_Customers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавиьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectListView_FtpServers = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.kryptonButtonNotify = new Krypton.Toolkit.KryptonButton();
            this.kryptonGroupBoxParameters = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonGroupBoxFTP = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonCheckBoxUseFtp = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonTextBoxCustomerFilter = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView_Customers)).BeginInit();
            this.contextMenuStrip_Customers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView_FtpServers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxParameters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxParameters.Panel)).BeginInit();
            this.kryptonGroupBoxParameters.Panel.SuspendLayout();
            this.kryptonGroupBoxParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxFTP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxFTP.Panel)).BeginInit();
            this.kryptonGroupBoxFTP.Panel.SuspendLayout();
            this.kryptonGroupBoxFTP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectListView_Customers
            // 
            this.objectListView_Customers.AllColumns.Add(this.olvColumn_Customer);
            this.objectListView_Customers.AllColumns.Add(this.olvColumnFTP);
            this.objectListView_Customers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView_Customers.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.objectListView_Customers.CellEditUseWholeCell = false;
            this.objectListView_Customers.CheckBoxes = true;
            this.objectListView_Customers.CheckedAspectName = "Show";
            this.objectListView_Customers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn_Customer,
            this.olvColumnFTP});
            this.objectListView_Customers.ContextMenuStrip = this.contextMenuStrip_Customers;
            this.objectListView_Customers.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView_Customers.FullRowSelect = true;
            this.objectListView_Customers.GridLines = true;
            this.objectListView_Customers.HideSelection = false;
            this.objectListView_Customers.Location = new System.Drawing.Point(3, 32);
            this.objectListView_Customers.Name = "objectListView_Customers";
            this.objectListView_Customers.ShowGroups = false;
            this.objectListView_Customers.ShowImagesOnSubItems = true;
            this.objectListView_Customers.Size = new System.Drawing.Size(209, 436);
            this.objectListView_Customers.TabIndex = 0;
            this.objectListView_Customers.UseCompatibleStateImageBehavior = false;
            this.objectListView_Customers.UseFiltering = true;
            this.objectListView_Customers.UseSubItemCheckBoxes = true;
            this.objectListView_Customers.View = System.Windows.Forms.View.Details;
            this.objectListView_Customers.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.objectListView_Customers_CellEditFinishing);
            this.objectListView_Customers.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.objectListView_Customers_CellEditStarting);
            this.objectListView_Customers.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.objectListView_Customers_ItemChecked);
            this.objectListView_Customers.Click += new System.EventHandler(this.objectListView_Customers_Click);
            // 
            // olvColumn_Customer
            // 
            this.olvColumn_Customer.AspectName = "Name";
            this.olvColumn_Customer.Text = "Замовник";
            this.olvColumn_Customer.Width = 161;
            // 
            // olvColumnFTP
            // 
            this.olvColumnFTP.AspectName = "IsFtpEnable";
            this.olvColumnFTP.CheckBoxes = true;
            this.olvColumnFTP.IsEditable = false;
            this.olvColumnFTP.Text = "FTP";
            this.olvColumnFTP.Width = 50;
            // 
            // contextMenuStrip_Customers
            // 
            this.contextMenuStrip_Customers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавиьToolStripMenuItem,
            this.toolStripSeparator1,
            this.удалитьToolStripMenuItem});
            this.contextMenuStrip_Customers.Name = "contextMenuStrip1";
            this.contextMenuStrip_Customers.Size = new System.Drawing.Size(133, 54);
            // 
            // добавиьToolStripMenuItem
            // 
            this.добавиьToolStripMenuItem.Name = "добавиьToolStripMenuItem";
            this.добавиьToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.добавиьToolStripMenuItem.Text = "добавить";
            this.добавиьToolStripMenuItem.Click += new System.EventHandler(this.добавиьToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(129, 6);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.удалитьToolStripMenuItem.Text = "удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
            // 
            // objectListView_FtpServers
            // 
            this.objectListView_FtpServers.AllColumns.Add(this.olvColumn1);
            this.objectListView_FtpServers.CellEditUseWholeCell = false;
            this.objectListView_FtpServers.CheckBoxes = true;
            this.objectListView_FtpServers.CheckedAspectName = "";
            this.objectListView_FtpServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.objectListView_FtpServers.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView_FtpServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView_FtpServers.FullRowSelect = true;
            this.objectListView_FtpServers.GridLines = true;
            this.objectListView_FtpServers.HideSelection = false;
            this.objectListView_FtpServers.Location = new System.Drawing.Point(0, 0);
            this.objectListView_FtpServers.Name = "objectListView_FtpServers";
            this.objectListView_FtpServers.ShowGroups = false;
            this.objectListView_FtpServers.Size = new System.Drawing.Size(218, 323);
            this.objectListView_FtpServers.TabIndex = 1;
            this.objectListView_FtpServers.UseCompatibleStateImageBehavior = false;
            this.objectListView_FtpServers.View = System.Windows.Forms.View.Details;
            this.objectListView_FtpServers.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.objectListView_FtpServers_ItemCheck);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Name";
            this.olvColumn1.Text = "FTP сервери";
            this.olvColumn1.Width = 178;
            // 
            // kryptonButtonNotify
            // 
            this.kryptonButtonNotify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonButtonNotify.Location = new System.Drawing.Point(17, 394);
            this.kryptonButtonNotify.Name = "kryptonButtonNotify";
            this.kryptonButtonNotify.Size = new System.Drawing.Size(220, 43);
            this.kryptonButtonNotify.StateNormal.Content.LongText.MultiLine = Krypton.Toolkit.InheritBool.True;
            this.kryptonButtonNotify.StatePressed.Content.LongText.MultiLine = Krypton.Toolkit.InheritBool.True;
            this.kryptonButtonNotify.StateTracking.Content.LongText.MultiLine = Krypton.Toolkit.InheritBool.True;
            this.kryptonButtonNotify.TabIndex = 10;
            this.kryptonButtonNotify.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonButtonNotify.Values.Image")));
            this.kryptonButtonNotify.Values.Text = "настройка відправки \r\nповідомлень";
            this.kryptonButtonNotify.Click += new System.EventHandler(this.buttonNotify_Click);
            // 
            // kryptonGroupBoxParameters
            // 
            this.kryptonGroupBoxParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBoxParameters.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBoxParameters.Name = "kryptonGroupBoxParameters";
            // 
            // kryptonGroupBoxParameters.Panel
            // 
            this.kryptonGroupBoxParameters.Panel.Controls.Add(this.kryptonGroupBoxFTP);
            this.kryptonGroupBoxParameters.Panel.Controls.Add(this.kryptonCheckBoxUseFtp);
            this.kryptonGroupBoxParameters.Panel.Controls.Add(this.kryptonButtonNotify);
            this.kryptonGroupBoxParameters.Size = new System.Drawing.Size(252, 471);
            this.kryptonGroupBoxParameters.TabIndex = 5;
            this.kryptonGroupBoxParameters.Values.Heading = "Параметри замовника";
            // 
            // kryptonGroupBoxFTP
            // 
            this.kryptonGroupBoxFTP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonGroupBoxFTP.Enabled = false;
            this.kryptonGroupBoxFTP.Location = new System.Drawing.Point(15, 36);
            this.kryptonGroupBoxFTP.Name = "kryptonGroupBoxFTP";
            // 
            // kryptonGroupBoxFTP.Panel
            // 
            this.kryptonGroupBoxFTP.Panel.Controls.Add(this.objectListView_FtpServers);
            this.kryptonGroupBoxFTP.Size = new System.Drawing.Size(222, 348);
            this.kryptonGroupBoxFTP.TabIndex = 15;
            this.kryptonGroupBoxFTP.Values.Heading = "FTP";
            // 
            // kryptonCheckBoxUseFtp
            // 
            this.kryptonCheckBoxUseFtp.Location = new System.Drawing.Point(15, 10);
            this.kryptonCheckBoxUseFtp.Name = "kryptonCheckBoxUseFtp";
            this.kryptonCheckBoxUseFtp.Size = new System.Drawing.Size(154, 21);
            this.kryptonCheckBoxUseFtp.TabIndex = 14;
            this.kryptonCheckBoxUseFtp.Values.Text = "використовувати FTP";
            this.kryptonCheckBoxUseFtp.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonSplitContainer1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(472, 471);
            this.kryptonPanel1.TabIndex = 6;
            // 
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
            // 
            // kryptonSplitContainer1.Panel1
            // 
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.objectListView_Customers);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonTextBoxCustomerFilter);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonLabel1);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.kryptonGroupBoxParameters);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(472, 471);
            this.kryptonSplitContainer1.SplitterDistance = 215;
            this.kryptonSplitContainer1.TabIndex = 8;
            // 
            // kryptonTextBoxCustomerFilter
            // 
            this.kryptonTextBoxCustomerFilter.Location = new System.Drawing.Point(32, 3);
            this.kryptonTextBoxCustomerFilter.Name = "kryptonTextBoxCustomerFilter";
            this.kryptonTextBoxCustomerFilter.Size = new System.Drawing.Size(136, 25);
            this.kryptonTextBoxCustomerFilter.TabIndex = 7;
            this.kryptonTextBoxCustomerFilter.TextChanged += new System.EventHandler(this.textBoxCustomerFilter_TextChanged);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(3, 6);
            this.kryptonLabel1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(22, 18);
            this.kryptonLabel1.TabIndex = 6;
            this.kryptonLabel1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonLabel1.Values.Image")));
            this.kryptonLabel1.Values.Text = "";
            // 
            // FormCustomers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 471);
            this.Controls.Add(this.kryptonPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCustomers";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Замовники";
            ((System.ComponentModel.ISupportInitialize)(this.objectListView_Customers)).EndInit();
            this.contextMenuStrip_Customers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView_FtpServers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxParameters.Panel)).EndInit();
            this.kryptonGroupBoxParameters.Panel.ResumeLayout(false);
            this.kryptonGroupBoxParameters.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxParameters)).EndInit();
            this.kryptonGroupBoxParameters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxFTP.Panel)).EndInit();
            this.kryptonGroupBoxFTP.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxFTP)).EndInit();
            this.kryptonGroupBoxFTP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            this.kryptonSplitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView_Customers;
        private BrightIdeasSoftware.OLVColumn olvColumn_Customer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Customers;
        private System.Windows.Forms.ToolStripMenuItem добавиьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        private BrightIdeasSoftware.ObjectListView objectListView_FtpServers;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private Krypton.Toolkit.KryptonButton kryptonButtonNotify;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBoxParameters;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBoxFTP;
        private Krypton.Toolkit.KryptonCheckBox kryptonCheckBoxUseFtp;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonTextBox kryptonTextBoxCustomerFilter;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private BrightIdeasSoftware.OLVColumn olvColumnFTP;
    }
}