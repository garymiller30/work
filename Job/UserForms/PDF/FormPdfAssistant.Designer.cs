namespace JobSpace.UserForms.PDF
{
    partial class FormPdfAssistant
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
            this.rtbChat = new System.Windows.Forms.RichTextBox();
            this.panelInput = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.panelSuggestedTool = new System.Windows.Forms.Panel();
            this.btnRunTool = new System.Windows.Forms.Button();
            this.lblToolSuggestion = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblSelectedFiles = new System.Windows.Forms.Label();
            this.panelInput.SuspendLayout();
            this.panelSuggestedTool.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbChat
            // 
            this.rtbChat.BackColor = System.Drawing.Color.White;
            this.rtbChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbChat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbChat.Location = new System.Drawing.Point(0, 40);
            this.rtbChat.Name = "rtbChat";
            this.rtbChat.ReadOnly = true;
            this.rtbChat.Size = new System.Drawing.Size(484, 331);
            this.rtbChat.TabIndex = 0;
            this.rtbChat.Text = "";
            // 
            // panelInput
            // 
            this.panelInput.Controls.Add(this.btnSettings);
            this.panelInput.Controls.Add(this.btnSend);
            this.panelInput.Controls.Add(this.txtInput);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInput.Location = new System.Drawing.Point(0, 421);
            this.panelInput.Name = "panelInput";
            this.panelInput.Padding = new System.Windows.Forms.Padding(8);
            this.panelInput.Size = new System.Drawing.Size(484, 50);
            this.panelInput.TabIndex = 1;
            // 
            // btnSettings
            // 
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Location = new System.Drawing.Point(8, 8);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(30, 34);
            this.btnSettings.TabIndex = 2;
            this.btnSettings.Text = "⚙";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSend.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(401, 8);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 34);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Надіслати";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtInput.Location = new System.Drawing.Point(44, 12);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(351, 25);
            this.txtInput.TabIndex = 0;
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
            // 
            // panelSuggestedTool
            // 
            this.panelSuggestedTool.BackColor = System.Drawing.Color.Honeydew;
            this.panelSuggestedTool.Controls.Add(this.btnRunTool);
            this.panelSuggestedTool.Controls.Add(this.lblToolSuggestion);
            this.panelSuggestedTool.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSuggestedTool.Location = new System.Drawing.Point(0, 371);
            this.panelSuggestedTool.Name = "panelSuggestedTool";
            this.panelSuggestedTool.Size = new System.Drawing.Size(484, 50);
            this.panelSuggestedTool.TabIndex = 2;
            // 
            // btnRunTool
            // 
            this.btnRunTool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunTool.BackColor = System.Drawing.Color.ForestGreen;
            this.btnRunTool.FlatAppearance.BorderSize = 0;
            this.btnRunTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunTool.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRunTool.ForeColor = System.Drawing.Color.White;
            this.btnRunTool.Location = new System.Drawing.Point(344, 9);
            this.btnRunTool.Name = "btnRunTool";
            this.btnRunTool.Size = new System.Drawing.Size(128, 32);
            this.btnRunTool.TabIndex = 1;
            this.btnRunTool.Text = "Запустити";
            this.btnRunTool.UseVisualStyleBackColor = false;
            this.btnRunTool.Click += new System.EventHandler(this.btnRunTool_Click);
            // 
            // lblToolSuggestion
            // 
            this.lblToolSuggestion.AutoSize = true;
            this.lblToolSuggestion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblToolSuggestion.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblToolSuggestion.Location = new System.Drawing.Point(12, 16);
            this.lblToolSuggestion.Name = "lblToolSuggestion";
            this.lblToolSuggestion.Size = new System.Drawing.Size(189, 17);
            this.lblToolSuggestion.TabIndex = 0;
            this.lblToolSuggestion.Text = "Рекомендовано запустити: ...";
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelHeader.Controls.Add(this.lblSelectedFiles);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(484, 40);
            this.panelHeader.TabIndex = 3;
            // 
            // lblSelectedFiles
            // 
            this.lblSelectedFiles.AutoSize = true;
            this.lblSelectedFiles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSelectedFiles.ForeColor = System.Drawing.Color.DimGray;
            this.lblSelectedFiles.Location = new System.Drawing.Point(12, 13);
            this.lblSelectedFiles.Name = "lblSelectedFiles";
            this.lblSelectedFiles.Size = new System.Drawing.Size(107, 15);
            this.lblSelectedFiles.TabIndex = 0;
            this.lblSelectedFiles.Text = "Вибрано файлів: 0";
            // 
            // FormPdfAssistant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 471);
            this.Controls.Add(this.rtbChat);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSuggestedTool);
            this.Controls.Add(this.panelInput);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPdfAssistant";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Асистент PDF-утиліт";
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            this.panelSuggestedTool.ResumeLayout(false);
            this.panelSuggestedTool.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Panel panelSuggestedTool;
        private System.Windows.Forms.Button btnRunTool;
        private System.Windows.Forms.Label lblToolSuggestion;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblSelectedFiles;
        private System.Windows.Forms.Button btnSettings;
    }
}
