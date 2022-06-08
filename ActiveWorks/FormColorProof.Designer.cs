namespace ActiveWorks
{
    //partial class FormColorProof
    //{
    //    /// <summary>
    //    /// Required designer variable.
    //    /// </summary>
    //    private System.ComponentModel.IContainer components = null;

    //    /// <summary>
    //    /// Clean up any resources being used.
    //    /// </summary>
    //    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing && (components != null))
    //        {
    //            components.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    #region Windows Form Designer generated code

    //    /// <summary>
    //    /// Required method for Designer support - do not modify
    //    /// the contents of this method with the code editor.
    //    /// </summary>
    //    private void InitializeComponent()
    //    {
    //        this.components = new System.ComponentModel.Container();
    //        this.objectListView_Forms = new BrightIdeasSoftware.ObjectListView();
    //        this.olvColumn_Customer = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
    //        this.olvColumn_Description = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
    //        this.olvColumn_Width = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
    //        this.olvColumn_Height = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
    //        this.olvColumn_Date = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
    //        this.olvColumn_Payed = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
    //        this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
    //        this.добавитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
    //        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
    //        this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
    //        this.groupBox1 = new System.Windows.Forms.GroupBox();
    //        this.olvColumn_Cost = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
    //        this.numericUpDown_Width = new System.Windows.Forms.NumericUpDown();
    //        this.numericUpDown_Height = new System.Windows.Forms.NumericUpDown();
    //        this.numericUpDown_Cost = new System.Windows.Forms.NumericUpDown();
    //        this.label1 = new System.Windows.Forms.Label();
    //        this.label2 = new System.Windows.Forms.Label();
    //        this.label3 = new System.Windows.Forms.Label();
    //        this.label4 = new System.Windows.Forms.Label();
    //        this.label5 = new System.Windows.Forms.Label();
    //        ((System.ComponentModel.ISupportInitialize)(this.objectListView_Forms)).BeginInit();
    //        this.contextMenuStrip1.SuspendLayout();
    //        this.groupBox1.SuspendLayout();
    //        ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Width)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Height)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Cost)).BeginInit();
    //        this.SuspendLayout();
    //        // 
    //        // objectListView_Forms
    //        // 
    //        this.objectListView_Forms.AllColumns.Add(this.olvColumn_Customer);
    //        this.objectListView_Forms.AllColumns.Add(this.olvColumn_Description);
    //        this.objectListView_Forms.AllColumns.Add(this.olvColumn_Width);
    //        this.objectListView_Forms.AllColumns.Add(this.olvColumn_Height);
    //        this.objectListView_Forms.AllColumns.Add(this.olvColumn_Date);
    //        this.objectListView_Forms.AllColumns.Add(this.olvColumn_Cost);
    //        this.objectListView_Forms.AllColumns.Add(this.olvColumn_Payed);
    //        this.objectListView_Forms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
    //        | System.Windows.Forms.AnchorStyles.Left) 
    //        | System.Windows.Forms.AnchorStyles.Right)));
    //        this.objectListView_Forms.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
    //        this.objectListView_Forms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
    //        this.olvColumn_Customer,
    //        this.olvColumn_Description,
    //        this.olvColumn_Width,
    //        this.olvColumn_Height,
    //        this.olvColumn_Date,
    //        this.olvColumn_Cost,
    //        this.olvColumn_Payed});
    //        this.objectListView_Forms.ContextMenuStrip = this.contextMenuStrip1;
    //        this.objectListView_Forms.FullRowSelect = true;
    //        this.objectListView_Forms.GridLines = true;
    //        this.objectListView_Forms.HideSelection = false;
    //        this.objectListView_Forms.Location = new System.Drawing.Point(0, 0);
    //        this.objectListView_Forms.Name = "objectListView_Forms";
    //        this.objectListView_Forms.OwnerDraw = true;
    //        this.objectListView_Forms.ShowGroups = false;
    //        this.objectListView_Forms.ShowImagesOnSubItems = true;
    //        this.objectListView_Forms.ShowItemToolTips = true;
    //        this.objectListView_Forms.Size = new System.Drawing.Size(760, 288);
    //        this.objectListView_Forms.TabIndex = 1;
    //        this.objectListView_Forms.UseCellFormatEvents = true;
    //        this.objectListView_Forms.UseCompatibleStateImageBehavior = false;
    //        this.objectListView_Forms.UseFiltering = true;
    //        this.objectListView_Forms.View = System.Windows.Forms.View.Details;
    //        this.objectListView_Forms.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.objectListView_Forms_CellEditFinishing);
    //        this.objectListView_Forms.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.objectListView_Forms_CellEditStarting);
    //        // 
    //        // olvColumn_Customer
    //        // 
    //        this.olvColumn_Customer.Text = "Заказчик";
    //        this.olvColumn_Customer.Width = 120;
    //        // 
    //        // olvColumn_Description
    //        // 
    //        this.olvColumn_Description.AspectName = "Description";
    //        this.olvColumn_Description.Text = "описание";
    //        this.olvColumn_Description.Width = 200;
    //        // 
    //        // olvColumn_Width
    //        // 
    //        this.olvColumn_Width.AspectName = "Width";
    //        this.olvColumn_Width.Text = "Ширина";
    //        // 
    //        // olvColumn_Height
    //        // 
    //        this.olvColumn_Height.AspectName = "Height";
    //        this.olvColumn_Height.Text = "Высота";
    //        // 
    //        // olvColumn_Date
    //        // 
    //        this.olvColumn_Date.AspectName = "Date";
    //        this.olvColumn_Date.Text = "Дата";
    //        this.olvColumn_Date.Width = 120;
    //        // 
    //        // olvColumn_Payed
    //        // 
    //        this.olvColumn_Payed.AspectName = "Payed";
    //        this.olvColumn_Payed.CheckBoxes = true;
    //        this.olvColumn_Payed.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
    //        this.olvColumn_Payed.Text = "оплачено";
    //        this.olvColumn_Payed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
    //        // 
    //        // contextMenuStrip1
    //        // 
    //        this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
    //        this.добавитьToolStripMenuItem,
    //        this.toolStripSeparator1,
    //        this.удалитьToolStripMenuItem});
    //        this.contextMenuStrip1.Name = "contextMenuStrip1";
    //        this.contextMenuStrip1.Size = new System.Drawing.Size(125, 54);
    //        // 
    //        // добавитьToolStripMenuItem
    //        // 
    //        this.добавитьToolStripMenuItem.Image = global::ActiveWorks.Properties.Resources.Create;
    //        this.добавитьToolStripMenuItem.Name = "добавитьToolStripMenuItem";
    //        this.добавитьToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
    //        this.добавитьToolStripMenuItem.Text = "добавить";
    //        this.добавитьToolStripMenuItem.Click += new System.EventHandler(this.добавитьToolStripMenuItem_Click);
    //        // 
    //        // toolStripSeparator1
    //        // 
    //        this.toolStripSeparator1.Name = "toolStripSeparator1";
    //        this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
    //        // 
    //        // удалитьToolStripMenuItem
    //        // 
    //        this.удалитьToolStripMenuItem.Image = global::ActiveWorks.Properties.Resources.Delete;
    //        this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
    //        this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
    //        this.удалитьToolStripMenuItem.Text = "удалить";
    //        this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
    //        // 
    //        // groupBox1
    //        // 
    //        this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
    //        | System.Windows.Forms.AnchorStyles.Right)));
    //        this.groupBox1.Controls.Add(this.label5);
    //        this.groupBox1.Controls.Add(this.label4);
    //        this.groupBox1.Controls.Add(this.label3);
    //        this.groupBox1.Controls.Add(this.label2);
    //        this.groupBox1.Controls.Add(this.label1);
    //        this.groupBox1.Controls.Add(this.numericUpDown_Cost);
    //        this.groupBox1.Controls.Add(this.numericUpDown_Height);
    //        this.groupBox1.Controls.Add(this.numericUpDown_Width);
    //        this.groupBox1.Location = new System.Drawing.Point(12, 294);
    //        this.groupBox1.Name = "groupBox1";
    //        this.groupBox1.Size = new System.Drawing.Size(321, 82);
    //        this.groupBox1.TabIndex = 2;
    //        this.groupBox1.TabStop = false;
    //        // 
    //        // olvColumn_Cost
    //        // 
    //        this.olvColumn_Cost.Text = "Стоимость";
    //        this.olvColumn_Cost.Width = 100;
    //        // 
    //        // numericUpDown_Width
    //        // 
    //        this.numericUpDown_Width.Location = new System.Drawing.Point(7, 38);
    //        this.numericUpDown_Width.Maximum = new decimal(new int[] {
    //        99999999,
    //        0,
    //        0,
    //        0});
    //        this.numericUpDown_Width.Name = "numericUpDown_Width";
    //        this.numericUpDown_Width.Size = new System.Drawing.Size(67, 20);
    //        this.numericUpDown_Width.TabIndex = 0;
    //        this.numericUpDown_Width.ValueChanged += new System.EventHandler(this.numericUpDown_Width_ValueChanged);
    //        // 
    //        // numericUpDown_Height
    //        // 
    //        this.numericUpDown_Height.Location = new System.Drawing.Point(97, 38);
    //        this.numericUpDown_Height.Maximum = new decimal(new int[] {
    //        1410065407,
    //        2,
    //        0,
    //        0});
    //        this.numericUpDown_Height.Name = "numericUpDown_Height";
    //        this.numericUpDown_Height.Size = new System.Drawing.Size(67, 20);
    //        this.numericUpDown_Height.TabIndex = 1;
    //        this.numericUpDown_Height.ValueChanged += new System.EventHandler(this.numericUpDown_Height_ValueChanged);
    //        // 
    //        // numericUpDown_Cost
    //        // 
    //        this.numericUpDown_Cost.Location = new System.Drawing.Point(191, 38);
    //        this.numericUpDown_Cost.Maximum = new decimal(new int[] {
    //        999999999,
    //        0,
    //        0,
    //        0});
    //        this.numericUpDown_Cost.Name = "numericUpDown_Cost";
    //        this.numericUpDown_Cost.Size = new System.Drawing.Size(67, 20);
    //        this.numericUpDown_Cost.TabIndex = 2;
    //        this.numericUpDown_Cost.ValueChanged += new System.EventHandler(this.numericUpDown_Cost_ValueChanged);
    //        // 
    //        // label1
    //        // 
    //        this.label1.AutoSize = true;
    //        this.label1.Location = new System.Drawing.Point(6, 22);
    //        this.label1.Name = "label1";
    //        this.label1.Size = new System.Drawing.Size(70, 13);
    //        this.label1.TabIndex = 3;
    //        this.label1.Text = "ширина (mm)";
    //        // 
    //        // label2
    //        // 
    //        this.label2.AutoSize = true;
    //        this.label2.Location = new System.Drawing.Point(94, 22);
    //        this.label2.Name = "label2";
    //        this.label2.Size = new System.Drawing.Size(69, 13);
    //        this.label2.TabIndex = 4;
    //        this.label2.Text = "высота (mm)";
    //        // 
    //        // label3
    //        // 
    //        this.label3.AutoSize = true;
    //        this.label3.Location = new System.Drawing.Point(188, 22);
    //        this.label3.Name = "label3";
    //        this.label3.Size = new System.Drawing.Size(87, 13);
    //        this.label3.TabIndex = 5;
    //        this.label3.Text = "стоимость (грн)";
    //        // 
    //        // label4
    //        // 
    //        this.label4.AutoSize = true;
    //        this.label4.Location = new System.Drawing.Point(80, 40);
    //        this.label4.Name = "label4";
    //        this.label4.Size = new System.Drawing.Size(12, 13);
    //        this.label4.TabIndex = 6;
    //        this.label4.Text = "x";
    //        // 
    //        // label5
    //        // 
    //        this.label5.AutoSize = true;
    //        this.label5.Location = new System.Drawing.Point(170, 40);
    //        this.label5.Name = "label5";
    //        this.label5.Size = new System.Drawing.Size(13, 13);
    //        this.label5.TabIndex = 7;
    //        this.label5.Text = "=";
    //        // 
    //        // FormColorProof
    //        // 
    //        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
    //        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    //        this.ClientSize = new System.Drawing.Size(760, 388);
    //        this.Controls.Add(this.groupBox1);
    //        this.Controls.Add(this.objectListView_Forms);
    //        this.Name = "FormColorProof";
    //        this.ShowIcon = false;
    //        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
    //        this.Text = "Цветопробы";
    //        ((System.ComponentModel.ISupportInitialize)(this.objectListView_Forms)).EndInit();
    //        this.contextMenuStrip1.ResumeLayout(false);
    //        this.groupBox1.ResumeLayout(false);
    //        this.groupBox1.PerformLayout();
    //        ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Width)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Height)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Cost)).EndInit();
    //        this.ResumeLayout(false);

    //    }

    //    #endregion

    //    private BrightIdeasSoftware.ObjectListView objectListView_Forms;
    //    private BrightIdeasSoftware.OLVColumn olvColumn_Customer;
    //    private BrightIdeasSoftware.OLVColumn olvColumn_Width;
    //    private BrightIdeasSoftware.OLVColumn olvColumn_Height;
    //    private BrightIdeasSoftware.OLVColumn olvColumn_Date;
    //    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    //    private System.Windows.Forms.ToolStripMenuItem добавитьToolStripMenuItem;
    //    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    //    private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
    //    private BrightIdeasSoftware.OLVColumn olvColumn_Payed;
    //    private BrightIdeasSoftware.OLVColumn olvColumn_Description;
    //    private BrightIdeasSoftware.OLVColumn olvColumn_Cost;
    //    private System.Windows.Forms.GroupBox groupBox1;
    //    private System.Windows.Forms.Label label5;
    //    private System.Windows.Forms.Label label4;
    //    private System.Windows.Forms.Label label3;
    //    private System.Windows.Forms.Label label2;
    //    private System.Windows.Forms.Label label1;
    //    private System.Windows.Forms.NumericUpDown numericUpDown_Cost;
    //    private System.Windows.Forms.NumericUpDown numericUpDown_Height;
    //    private System.Windows.Forms.NumericUpDown numericUpDown_Width;
    //}
}