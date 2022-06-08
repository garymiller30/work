namespace Repro
{
    partial class Form1
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_All = new System.Windows.Forms.TabPage();
            this.tabPageInWork = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_All);
            this.tabControl1.Controls.Add(this.tabPageInWork);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1022, 563);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage_All
            // 
            this.tabPage_All.Location = new System.Drawing.Point(4, 22);
            this.tabPage_All.Name = "tabPage_All";
            this.tabPage_All.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_All.Size = new System.Drawing.Size(1014, 537);
            this.tabPage_All.TabIndex = 0;
            this.tabPage_All.Text = "все";
            this.tabPage_All.UseVisualStyleBackColor = true;
            // 
            // tabPageInWork
            // 
            this.tabPageInWork.Location = new System.Drawing.Point(4, 22);
            this.tabPageInWork.Name = "tabPageInWork";
            this.tabPageInWork.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInWork.Size = new System.Drawing.Size(1014, 537);
            this.tabPageInWork.TabIndex = 1;
            this.tabPageInWork.Text = "в работе";
            this.tabPageInWork.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1014, 537);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "выведенные";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 563);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Репро";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_All;
        private System.Windows.Forms.TabPage tabPageInWork;
        private System.Windows.Forms.TabPage tabPage3;
    }
}

