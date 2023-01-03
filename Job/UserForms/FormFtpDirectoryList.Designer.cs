namespace Job.UserForms
{
    partial class FormFtpDirectoryList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFtpDirectoryList));
            this.objectListView_Ftp = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonButton1 = new Krypton.Toolkit.KryptonButton();
            this.kryptonButton2 = new Krypton.Toolkit.KryptonButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView_Ftp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectListView_Ftp
            // 
            this.objectListView_Ftp.AllColumns.Add(this.olvColumn1);
            this.objectListView_Ftp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView_Ftp.CellEditUseWholeCell = false;
            this.objectListView_Ftp.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.objectListView_Ftp.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView_Ftp.FullRowSelect = true;
            this.objectListView_Ftp.GridLines = true;
            this.objectListView_Ftp.HideSelection = false;
            this.objectListView_Ftp.Location = new System.Drawing.Point(3, 32);
            this.objectListView_Ftp.Name = "objectListView_Ftp";
            this.objectListView_Ftp.SelectedBackColor = System.Drawing.Color.Gray;
            this.objectListView_Ftp.ShowGroups = false;
            this.objectListView_Ftp.Size = new System.Drawing.Size(338, 184);
            this.objectListView_Ftp.SmallImageList = this.imageList1;
            this.objectListView_Ftp.TabIndex = 0;
            this.objectListView_Ftp.UseCompatibleStateImageBehavior = false;
            this.objectListView_Ftp.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Name";
            this.olvColumn1.Text = "Ім\'я";
            this.olvColumn1.Width = 200;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonButton2);
            this.kryptonPanel1.Controls.Add(this.kryptonButton1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(344, 257);
            this.kryptonPanel1.TabIndex = 3;
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(3, 3);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(80, 25);
            this.kryptonButton1.TabIndex = 3;
            this.kryptonButton1.Values.Image = global::Job.Properties.Resources.arrow_turn_left;
            this.kryptonButton1.Values.Text = "назад";
            this.kryptonButton1.Click += new System.EventHandler(this.button_Up_Click);
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.kryptonButton2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.kryptonButton2.Location = new System.Drawing.Point(129, 222);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.Size = new System.Drawing.Size(90, 32);
            this.kryptonButton2.TabIndex = 4;
            this.kryptonButton2.Values.Text = "Вибрати";
            this.kryptonButton2.Click += new System.EventHandler(this.button_Select_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folder-icon.png");
            // 
            // FormFtpDirectoryList
            // 
            this.AcceptButton = this.kryptonButton2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 257);
            this.Controls.Add(this.objectListView_Ftp);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "FormFtpDirectoryList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Вибрати папку на FTP";
            ((System.ComponentModel.ISupportInitialize)(this.objectListView_Ftp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView_Ftp;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton kryptonButton2;
        private Krypton.Toolkit.KryptonButton kryptonButton1;
        private System.Windows.Forms.ImageList imageList1;
    }
}