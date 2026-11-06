namespace JobSpace.UserForms
{
    partial class FormTrash
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
            panelTop = new Krypton.Toolkit.KryptonPanel();
            btnRestore = new Krypton.Toolkit.KryptonButton();
            btnDelete = new Krypton.Toolkit.KryptonButton();
            btnEmpty = new Krypton.Toolkit.KryptonButton();
            btnRefresh = new Krypton.Toolkit.KryptonButton();
            objectListView1 = new BrightIdeasSoftware.ObjectListView();
            olvColumnName = new BrightIdeasSoftware.OLVColumn();
            olvColumnGroup = new BrightIdeasSoftware.OLVColumn();
            olvColumnType = new BrightIdeasSoftware.OLVColumn();
            olvColumnSize = new BrightIdeasSoftware.OLVColumn();
            olvColumnDeleted = new BrightIdeasSoftware.OLVColumn();
            ((System.ComponentModel.ISupportInitialize)panelTop).BeginInit();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)objectListView1).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(btnRestore);
            panelTop.Controls.Add(btnDelete);
            panelTop.Controls.Add(btnEmpty);
            panelTop.Controls.Add(btnRefresh);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(784, 45);
            panelTop.TabIndex = 0;
            // 
            // btnRestore
            // 
            btnRestore.Location = new System.Drawing.Point(12, 10);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new System.Drawing.Size(150, 25);
            btnRestore.TabIndex = 0;
            btnRestore.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            btnRestore.Values.Text = "Відновити виділене";
            btnRestore.Click += btnRestore_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(168, 10);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(150, 25);
            btnDelete.TabIndex = 1;
            btnDelete.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            btnDelete.Values.Text = "Видалити назавжди";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnEmpty
            // 
            btnEmpty.Location = new System.Drawing.Point(324, 10);
            btnEmpty.Name = "btnEmpty";
            btnEmpty.Size = new System.Drawing.Size(130, 25);
            btnEmpty.TabIndex = 2;
            btnEmpty.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            btnEmpty.Values.Text = "Очистити кошик";
            btnEmpty.Click += btnEmpty_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRefresh.Location = new System.Drawing.Point(672, 10);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(100, 25);
            btnRefresh.TabIndex = 3;
            btnRefresh.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            btnRefresh.Values.Text = "Оновити";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // objectListView1
            // 
            objectListView1.AllColumns.Add(olvColumnName);
            objectListView1.AllColumns.Add(olvColumnGroup);
            objectListView1.AllColumns.Add(olvColumnType);
            objectListView1.AllColumns.Add(olvColumnSize);
            objectListView1.AllColumns.Add(olvColumnDeleted);
            objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { olvColumnName, olvColumnGroup, olvColumnType, olvColumnSize, olvColumnDeleted });
            objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            objectListView1.FullRowSelect = true;
            objectListView1.GridLines = true;
            objectListView1.Location = new System.Drawing.Point(0, 45);
            objectListView1.Name = "objectListView1";
            objectListView1.Size = new System.Drawing.Size(784, 516);
            objectListView1.TabIndex = 1;
            objectListView1.UseCompatibleStateImageBehavior = false;
            objectListView1.View = System.Windows.Forms.View.Details;
            objectListView1.DoubleClick += objectListView1_DoubleClick;
            // 
            // olvColumnName
            // 
            olvColumnName.AspectName = "Name";
            olvColumnName.Text = "Назва";
            olvColumnName.Width = 320;
            // 
            // olvColumnGroup
            // 
            olvColumnGroup.AspectName = "GroupName";
            olvColumnGroup.Text = "Папка кошика";
            olvColumnGroup.Width = 100;
            // 
            // olvColumnType
            // 
            olvColumnType.AspectName = "IsDirectory";
            olvColumnType.Text = "Тип";
            olvColumnType.Width = 80;
            // 
            // olvColumnSize
            // 
            olvColumnSize.AspectName = "Size";
            olvColumnSize.Text = "Розмір";
            olvColumnSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            olvColumnSize.Width = 100;
            // 
            // olvColumnDeleted
            // 
            olvColumnDeleted.AspectName = "DeletedTime";
            olvColumnDeleted.Text = "Дата видалення";
            olvColumnDeleted.Width = 150;
            // 
            // FormTrash
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(784, 561);
            Controls.Add(objectListView1);
            Controls.Add(panelTop);
            Name = "FormTrash";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Кошик замовлення";
            ((System.ComponentModel.ISupportInitialize)panelTop).EndInit();
            panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)objectListView1).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel panelTop;
        private Krypton.Toolkit.KryptonButton btnRestore;
        private Krypton.Toolkit.KryptonButton btnDelete;
        private Krypton.Toolkit.KryptonButton btnEmpty;
        private Krypton.Toolkit.KryptonButton btnRefresh;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnName;
        private BrightIdeasSoftware.OLVColumn olvColumnGroup;
        private BrightIdeasSoftware.OLVColumn olvColumnType;
        private BrightIdeasSoftware.OLVColumn olvColumnSize;
        private BrightIdeasSoftware.OLVColumn olvColumnDeleted;
    }
}
