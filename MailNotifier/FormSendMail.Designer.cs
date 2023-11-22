namespace MailNotifier
{
    partial class FormSendMail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSendMail));
            this.groupBoxTo = new System.Windows.Forms.GroupBox();
            this.comboBoxTo = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxHeader = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBoxMessage = new Krypton.Toolkit.KryptonRichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonRemoveAttach = new System.Windows.Forms.Button();
            this.buttonAddAttach = new System.Windows.Forms.Button();
            this.listBoxAttach = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButtonMailShablons = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveAsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonButtonSend = new Krypton.Toolkit.KryptonButton();
            this.groupBoxTotal = new System.Windows.Forms.GroupBox();
            this.labelTotal = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxTo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.groupBoxTotal.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTo
            // 
            this.groupBoxTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTo.Controls.Add(this.comboBoxTo);
            this.groupBoxTo.Location = new System.Drawing.Point(7, 30);
            this.groupBoxTo.Name = "groupBoxTo";
            this.groupBoxTo.Size = new System.Drawing.Size(416, 48);
            this.groupBoxTo.TabIndex = 0;
            this.groupBoxTo.TabStop = false;
            this.groupBoxTo.Text = "Відправити до (більше одного адресата вказуються через \",\")";
            // 
            // comboBoxTo
            // 
            this.comboBoxTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTo.FormattingEnabled = true;
            this.comboBoxTo.Location = new System.Drawing.Point(6, 19);
            this.comboBoxTo.Name = "comboBoxTo";
            this.comboBoxTo.Size = new System.Drawing.Size(404, 21);
            this.comboBoxTo.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxHeader);
            this.groupBox1.Location = new System.Drawing.Point(7, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 47);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Заголовок листа";
            // 
            // textBoxHeader
            // 
            this.textBoxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxHeader.Location = new System.Drawing.Point(7, 20);
            this.textBoxHeader.Name = "textBoxHeader";
            this.textBoxHeader.Size = new System.Drawing.Size(403, 20);
            this.textBoxHeader.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.richTextBoxMessage);
            this.groupBox2.Location = new System.Drawing.Point(4, 137);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(416, 100);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Повідомлення";
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMessage.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.Size = new System.Drawing.Size(410, 81);
            this.richTextBoxMessage.TabIndex = 0;
            this.richTextBoxMessage.Text = "";
            this.richTextBoxMessage.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.RichTextBoxMessage_LinkClicked);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.buttonRemoveAttach);
            this.groupBox3.Controls.Add(this.buttonAddAttach);
            this.groupBox3.Controls.Add(this.listBoxAttach);
            this.groupBox3.Location = new System.Drawing.Point(7, 243);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(332, 137);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Вкладення";
            // 
            // buttonRemoveAttach
            // 
            this.buttonRemoveAttach.Image = global::MailNotifier.Properties.Resources.Delete;
            this.buttonRemoveAttach.Location = new System.Drawing.Point(3, 48);
            this.buttonRemoveAttach.Name = "buttonRemoveAttach";
            this.buttonRemoveAttach.Size = new System.Drawing.Size(26, 26);
            this.buttonRemoveAttach.TabIndex = 2;
            this.buttonRemoveAttach.Text = "-";
            this.buttonRemoveAttach.UseVisualStyleBackColor = true;
            this.buttonRemoveAttach.Click += new System.EventHandler(this.ButtonRemoveAttach_Click);
            // 
            // buttonAddAttach
            // 
            this.buttonAddAttach.Image = global::MailNotifier.Properties.Resources.Actions_list_add_icon;
            this.buttonAddAttach.Location = new System.Drawing.Point(3, 16);
            this.buttonAddAttach.Name = "buttonAddAttach";
            this.buttonAddAttach.Size = new System.Drawing.Size(26, 26);
            this.buttonAddAttach.TabIndex = 1;
            this.buttonAddAttach.UseVisualStyleBackColor = true;
            this.buttonAddAttach.Click += new System.EventHandler(this.ButtonAddAttach_Click);
            // 
            // listBoxAttach
            // 
            this.listBoxAttach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxAttach.DisplayMember = "Name";
            this.listBoxAttach.FormattingEnabled = true;
            this.listBoxAttach.Location = new System.Drawing.Point(35, 16);
            this.listBoxAttach.Name = "listBoxAttach";
            this.listBoxAttach.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxAttach.Size = new System.Drawing.Size(291, 108);
            this.listBoxAttach.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButtonMailShablons});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(429, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButtonMailShablons
            // 
            this.toolStripDropDownButtonMailShablons.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonMailShablons.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem1,
            this.toolStripSeparator2});
            this.toolStripDropDownButtonMailShablons.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonMailShablons.Image")));
            this.toolStripDropDownButtonMailShablons.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonMailShablons.Name = "toolStripDropDownButtonMailShablons";
            this.toolStripDropDownButtonMailShablons.Size = new System.Drawing.Size(72, 22);
            this.toolStripDropDownButtonMailShablons.Text = "Шаблони";
            // 
            // saveAsToolStripMenuItem1
            // 
            this.saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
            this.saveAsToolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
            this.saveAsToolStripMenuItem1.Text = "Зберегти як...";
            this.saveAsToolStripMenuItem1.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.groupBoxTotal);
            this.kryptonPanel1.Controls.Add(this.kryptonButtonSend);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(429, 383);
            this.kryptonPanel1.TabIndex = 6;
            // 
            // kryptonButtonSend
            // 
            this.kryptonButtonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonButtonSend.Location = new System.Drawing.Point(345, 327);
            this.kryptonButtonSend.Name = "kryptonButtonSend";
            this.kryptonButtonSend.Size = new System.Drawing.Size(81, 53);
            this.kryptonButtonSend.TabIndex = 0;
            this.kryptonButtonSend.Values.Text = "Відправити";
            this.kryptonButtonSend.Click += new System.EventHandler(this.ButtonSend_Click);
            // 
            // groupBoxTotal
            // 
            this.groupBoxTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTotal.Controls.Add(this.label2);
            this.groupBoxTotal.Controls.Add(this.labelTotal);
            this.groupBoxTotal.Location = new System.Drawing.Point(345, 243);
            this.groupBoxTotal.Name = "groupBoxTotal";
            this.groupBoxTotal.Size = new System.Drawing.Size(78, 72);
            this.groupBoxTotal.TabIndex = 1;
            this.groupBoxTotal.TabStop = false;
            this.groupBoxTotal.Text = "Всього";
            // 
            // labelTotal
            // 
            this.labelTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTotal.Location = new System.Drawing.Point(6, 23);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(66, 23);
            this.labelTotal.TabIndex = 0;
            this.labelTotal.Text = "0";
            this.labelTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Мб";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormSendMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 383);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxTo);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "FormSendMail";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Відправка пошти";
            this.groupBoxTo.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.groupBoxTotal.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTo;
        private System.Windows.Forms.ComboBox comboBoxTo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxHeader;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBoxAttach;
        private System.Windows.Forms.Button buttonRemoveAttach;
        private System.Windows.Forms.Button buttonAddAttach;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonMailShablons;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton kryptonButtonSend;
        private Krypton.Toolkit.KryptonRichTextBox richTextBoxMessage;
        private System.Windows.Forms.GroupBox groupBoxTotal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelTotal;
    }
}