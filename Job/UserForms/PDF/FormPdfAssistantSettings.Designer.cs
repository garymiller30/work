namespace JobSpace.UserForms.PDF
{
    partial class FormPdfAssistantSettings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            txtApiUrl = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            txtApiKey = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            txtModelName = new System.Windows.Forms.TextBox();
            btnSave = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            txtAudioModel = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 17);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(52, 15);
            label1.TabIndex = 0;
            label1.Text = "API URL:";
            // 
            // txtApiUrl
            // 
            txtApiUrl.Location = new System.Drawing.Point(117, 14);
            txtApiUrl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtApiUrl.Name = "txtApiUrl";
            txtApiUrl.Size = new System.Drawing.Size(317, 23);
            txtApiUrl.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(14, 47);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(50, 15);
            label2.TabIndex = 2;
            label2.Text = "API Key:";
            // 
            // txtApiKey
            // 
            txtApiKey.Location = new System.Drawing.Point(117, 44);
            txtApiKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtApiKey.Name = "txtApiKey";
            txtApiKey.Size = new System.Drawing.Size(317, 23);
            txtApiKey.TabIndex = 3;
            txtApiKey.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(14, 77);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(79, 15);
            label3.TabIndex = 4;
            label3.Text = "Model Name:";
            // 
            // txtModelName
            // 
            txtModelName.Location = new System.Drawing.Point(117, 74);
            txtModelName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtModelName.Name = "txtModelName";
            txtModelName.Size = new System.Drawing.Size(317, 23);
            txtModelName.TabIndex = 5;
            // 
            // btnSave
            // 
            btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnSave.Location = new System.Drawing.Point(253, 194);
            btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(88, 27);
            btnSave.TabIndex = 6;
            btnSave.Text = "Зберегти";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(347, 194);
            btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(88, 27);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Скасувати";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtAudioModel
            // 
            txtAudioModel.Location = new System.Drawing.Point(117, 137);
            txtAudioModel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtAudioModel.Name = "txtAudioModel";
            txtAudioModel.Size = new System.Drawing.Size(317, 23);
            txtAudioModel.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(117, 119);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(187, 15);
            label4.TabIndex = 8;
            label4.Text = "модель для розпізнавання мови:";
            // 
            // FormPdfAssistantSettings
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(448, 233);
            Controls.Add(txtAudioModel);
            Controls.Add(label4);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtModelName);
            Controls.Add(label3);
            Controls.Add(txtApiKey);
            Controls.Add(label2);
            Controls.Add(txtApiUrl);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPdfAssistantSettings";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Налаштування ШІ асистента";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtApiUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtAudioModel;
        private System.Windows.Forms.Label label4;
    }
}
