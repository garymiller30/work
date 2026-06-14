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
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            panelInput = new System.Windows.Forms.Panel();
            txtInput = new System.Windows.Forms.TextBox();
            btnRecord = new System.Windows.Forms.Button();
            btnSend = new System.Windows.Forms.Button();
            btnSettings = new System.Windows.Forms.Button();
            panelSuggestedTool = new System.Windows.Forms.Panel();
            btnRunTool = new System.Windows.Forms.Button();
            lblToolSuggestion = new System.Windows.Forms.Label();
            panelHeader = new System.Windows.Forms.Panel();
            lblSelectedFiles = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            panelInput.SuspendLayout();
            panelSuggestedTool.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = System.Drawing.Color.White;
            webView21.Dock = System.Windows.Forms.DockStyle.Fill;
            webView21.Location = new System.Drawing.Point(0, 46);
            webView21.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            webView21.Name = "webView21";
            webView21.Size = new System.Drawing.Size(653, 381);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // panelInput
            // 
            panelInput.Controls.Add(txtInput);
            panelInput.Controls.Add(btnRecord);
            panelInput.Controls.Add(btnSend);
            panelInput.Controls.Add(btnSettings);
            panelInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelInput.Location = new System.Drawing.Point(0, 485);
            panelInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelInput.Name = "panelInput";
            panelInput.Padding = new System.Windows.Forms.Padding(9);
            panelInput.Size = new System.Drawing.Size(653, 58);
            panelInput.TabIndex = 1;
            // 
            // txtInput
            // 
            txtInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtInput.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            txtInput.Location = new System.Drawing.Point(51, 14);
            txtInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtInput.Name = "txtInput";
            txtInput.Size = new System.Drawing.Size(394, 25);
            txtInput.TabIndex = 0;
            txtInput.KeyDown += txtInput_KeyDown;
            // 
            // btnRecord
            // 
            btnRecord.BackColor = System.Drawing.Color.WhiteSmoke;
            btnRecord.Dock = System.Windows.Forms.DockStyle.Right;
            btnRecord.FlatAppearance.BorderSize = 0;
            btnRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRecord.Font = new System.Drawing.Font("Segoe UI", 13F);
            btnRecord.Location = new System.Drawing.Point(510, 9);
            btnRecord.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new System.Drawing.Size(42, 40);
            btnRecord.TabIndex = 3;
            btnRecord.Text = "🎙";
            btnRecord.UseVisualStyleBackColor = false;
            btnRecord.Click += btnRecord_Click;
            // 
            // btnSend
            // 
            btnSend.BackColor = System.Drawing.Color.DodgerBlue;
            btnSend.Dock = System.Windows.Forms.DockStyle.Right;
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSend.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            btnSend.ForeColor = System.Drawing.Color.White;
            btnSend.Location = new System.Drawing.Point(552, 9);
            btnSend.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSend.Name = "btnSend";
            btnSend.Size = new System.Drawing.Size(92, 40);
            btnSend.TabIndex = 1;
            btnSend.Text = "Надіслати";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // btnSettings
            // 
            btnSettings.Dock = System.Windows.Forms.DockStyle.Left;
            btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSettings.Location = new System.Drawing.Point(9, 9);
            btnSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new System.Drawing.Size(35, 40);
            btnSettings.TabIndex = 2;
            btnSettings.Text = "⚙";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // panelSuggestedTool
            // 
            panelSuggestedTool.BackColor = System.Drawing.Color.Honeydew;
            panelSuggestedTool.Controls.Add(btnRunTool);
            panelSuggestedTool.Controls.Add(lblToolSuggestion);
            panelSuggestedTool.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelSuggestedTool.Location = new System.Drawing.Point(0, 427);
            panelSuggestedTool.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelSuggestedTool.Name = "panelSuggestedTool";
            panelSuggestedTool.Size = new System.Drawing.Size(653, 58);
            panelSuggestedTool.TabIndex = 2;
            // 
            // btnRunTool
            // 
            btnRunTool.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRunTool.BackColor = System.Drawing.Color.ForestGreen;
            btnRunTool.FlatAppearance.BorderSize = 0;
            btnRunTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRunTool.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            btnRunTool.ForeColor = System.Drawing.Color.White;
            btnRunTool.Location = new System.Drawing.Point(490, 10);
            btnRunTool.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnRunTool.Name = "btnRunTool";
            btnRunTool.Size = new System.Drawing.Size(149, 37);
            btnRunTool.TabIndex = 1;
            btnRunTool.Text = "▶  Запустити";
            btnRunTool.UseVisualStyleBackColor = false;
            btnRunTool.Click += btnRunTool_Click;
            // 
            // lblToolSuggestion
            // 
            lblToolSuggestion.AutoSize = true;
            lblToolSuggestion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            lblToolSuggestion.ForeColor = System.Drawing.Color.DarkGreen;
            lblToolSuggestion.Location = new System.Drawing.Point(14, 18);
            lblToolSuggestion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblToolSuggestion.Name = "lblToolSuggestion";
            lblToolSuggestion.Size = new System.Drawing.Size(196, 17);
            lblToolSuggestion.TabIndex = 0;
            lblToolSuggestion.Text = "Рекомендовано запустити: ...";
            // 
            // panelHeader
            // 
            panelHeader.BackColor = System.Drawing.Color.FromArgb(40, 40, 55);
            panelHeader.Controls.Add(lblSelectedFiles);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(653, 46);
            panelHeader.TabIndex = 3;
            // 
            // lblSelectedFiles
            // 
            lblSelectedFiles.AutoSize = true;
            lblSelectedFiles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            lblSelectedFiles.ForeColor = System.Drawing.Color.Silver;
            lblSelectedFiles.Location = new System.Drawing.Point(14, 14);
            lblSelectedFiles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSelectedFiles.Name = "lblSelectedFiles";
            lblSelectedFiles.Size = new System.Drawing.Size(108, 15);
            lblSelectedFiles.TabIndex = 0;
            lblSelectedFiles.Text = "Вибрано файлів: 0";
            // 
            // FormPdfAssistant
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(653, 543);
            Controls.Add(webView21);
            Controls.Add(panelHeader);
            Controls.Add(panelSuggestedTool);
            Controls.Add(panelInput);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(487, 456);
            Name = "FormPdfAssistant";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "🤖 Асистент PDF-утиліт";
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            panelInput.ResumeLayout(false);
            panelInput.PerformLayout();
            panelSuggestedTool.ResumeLayout(false);
            panelSuggestedTool.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Panel panelSuggestedTool;
        private System.Windows.Forms.Button btnRunTool;
        private System.Windows.Forms.Label lblToolSuggestion;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblSelectedFiles;
        private System.Windows.Forms.Button btnSettings;
    }
}
