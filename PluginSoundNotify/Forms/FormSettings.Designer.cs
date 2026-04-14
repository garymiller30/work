namespace PluginSoundNotify.Forms
{
    partial class FormSettings
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
            this.listBoxAvailableSoundsEvent = new System.Windows.Forms.ListBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelMail = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxMails = new System.Windows.Forms.TextBox();
            this.panelFile = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonPlayFile = new System.Windows.Forms.Button();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.panelSpeech = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonPlaySpeech = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxVoices = new System.Windows.Forms.ComboBox();
            this.comboBoxSoundType = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelMail.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panelFile.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panelSpeech.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxAvailableSoundsEvent
            // 
            this.listBoxAvailableSoundsEvent.FormattingEnabled = true;
            this.listBoxAvailableSoundsEvent.Location = new System.Drawing.Point(12, 12);
            this.listBoxAvailableSoundsEvent.Name = "listBoxAvailableSoundsEvent";
            this.listBoxAvailableSoundsEvent.Size = new System.Drawing.Size(149, 186);
            this.listBoxAvailableSoundsEvent.TabIndex = 1;
            this.listBoxAvailableSoundsEvent.SelectedIndexChanged += new System.EventHandler(this.listBoxAvailableSoundsEvent_SelectedIndexChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSave.Location = new System.Drawing.Point(208, 206);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(122, 40);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "ОК";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Controls.Add(this.comboBoxSoundType);
            this.groupBox1.Location = new System.Drawing.Point(168, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 186);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "налаштування";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.panelMail);
            this.flowLayoutPanel1.Controls.Add(this.panelFile);
            this.flowLayoutPanel1.Controls.Add(this.panelSpeech);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 46);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(328, 134);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // panelMail
            // 
            this.panelMail.Controls.Add(this.groupBox4);
            this.panelMail.Location = new System.Drawing.Point(3, 3);
            this.panelMail.Name = "panelMail";
            this.panelMail.Size = new System.Drawing.Size(322, 50);
            this.panelMail.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxMails);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(322, 50);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Відправити повідомлення адресатам (через \',\')";
            // 
            // textBoxMails
            // 
            this.textBoxMails.Location = new System.Drawing.Point(6, 19);
            this.textBoxMails.Name = "textBoxMails";
            this.textBoxMails.Size = new System.Drawing.Size(309, 20);
            this.textBoxMails.TabIndex = 0;
            this.textBoxMails.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxMails_KeyUp);
            // 
            // panelFile
            // 
            this.panelFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFile.Controls.Add(this.groupBox2);
            this.panelFile.Location = new System.Drawing.Point(3, 59);
            this.panelFile.Name = "panelFile";
            this.panelFile.Size = new System.Drawing.Size(325, 57);
            this.panelFile.TabIndex = 0;
            this.panelFile.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonPlayFile);
            this.groupBox2.Controls.Add(this.buttonSelectFile);
            this.groupBox2.Controls.Add(this.textBoxFile);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(319, 50);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "програвати файл";
            // 
            // buttonPlayFile
            // 
            this.buttonPlayFile.Location = new System.Drawing.Point(291, 18);
            this.buttonPlayFile.Name = "buttonPlayFile";
            this.buttonPlayFile.Size = new System.Drawing.Size(21, 22);
            this.buttonPlayFile.TabIndex = 3;
            this.buttonPlayFile.Text = ">";
            this.buttonPlayFile.UseVisualStyleBackColor = true;
            this.buttonPlayFile.Click += new System.EventHandler(this.buttonPlayFile_Click);
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(243, 18);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(42, 23);
            this.buttonSelectFile.TabIndex = 1;
            this.buttonSelectFile.Text = "...";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // textBoxFile
            // 
            this.textBoxFile.Location = new System.Drawing.Point(7, 20);
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.ReadOnly = true;
            this.textBoxFile.Size = new System.Drawing.Size(230, 20);
            this.textBoxFile.TabIndex = 0;
            // 
            // panelSpeech
            // 
            this.panelSpeech.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSpeech.Controls.Add(this.groupBox3);
            this.panelSpeech.Location = new System.Drawing.Point(3, 122);
            this.panelSpeech.Name = "panelSpeech";
            this.panelSpeech.Size = new System.Drawing.Size(325, 81);
            this.panelSpeech.TabIndex = 1;
            this.panelSpeech.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonPlaySpeech);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.comboBoxVoices);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(319, 71);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "промовляти речення";
            // 
            // buttonPlaySpeech
            // 
            this.buttonPlaySpeech.Location = new System.Drawing.Point(291, 43);
            this.buttonPlaySpeech.Name = "buttonPlaySpeech";
            this.buttonPlaySpeech.Size = new System.Drawing.Size(21, 22);
            this.buttonPlaySpeech.TabIndex = 4;
            this.buttonPlaySpeech.Text = ">";
            this.buttonPlaySpeech.UseVisualStyleBackColor = true;
            this.buttonPlaySpeech.Click += new System.EventHandler(this.buttonPlaySpeech_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(62, 45);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(223, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "речення";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "голос";
            // 
            // comboBoxVoices
            // 
            this.comboBoxVoices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVoices.FormattingEnabled = true;
            this.comboBoxVoices.Location = new System.Drawing.Point(49, 17);
            this.comboBoxVoices.Name = "comboBoxVoices";
            this.comboBoxVoices.Size = new System.Drawing.Size(264, 21);
            this.comboBoxVoices.TabIndex = 0;
            this.comboBoxVoices.SelectedIndexChanged += new System.EventHandler(this.comboBoxVoices_SelectedIndexChanged);
            // 
            // comboBoxSoundType
            // 
            this.comboBoxSoundType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSoundType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSoundType.FormattingEnabled = true;
            this.comboBoxSoundType.Location = new System.Drawing.Point(76, 19);
            this.comboBoxSoundType.Name = "comboBoxSoundType";
            this.comboBoxSoundType.Size = new System.Drawing.Size(176, 21);
            this.comboBoxSoundType.TabIndex = 0;
            this.comboBoxSoundType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSoundType_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "mp3|*.mp3|wav|*.wav";
            // 
            // FormSettings
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 258);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.listBoxAvailableSoundsEvent);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Налаштування";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panelMail.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panelFile.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panelSpeech.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox listBoxAvailableSoundsEvent;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panelFile;
        private System.Windows.Forms.Panel panelSpeech;
        private System.Windows.Forms.ComboBox comboBoxSoundType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxVoices;
        private System.Windows.Forms.Button buttonPlayFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonPlaySpeech;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelMail;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxMails;
    }
}