namespace PluginRabbitMq
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
            this.groupBox_Rabbit = new System.Windows.Forms.GroupBox();
            this.textBox_VirtualHost = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_Pass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_User = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Server = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox_Rabbit.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Rabbit
            // 
            this.groupBox_Rabbit.Controls.Add(this.textBox_VirtualHost);
            this.groupBox_Rabbit.Controls.Add(this.label5);
            this.groupBox_Rabbit.Controls.Add(this.textBox_Pass);
            this.groupBox_Rabbit.Controls.Add(this.label4);
            this.groupBox_Rabbit.Controls.Add(this.textBox_User);
            this.groupBox_Rabbit.Controls.Add(this.label3);
            this.groupBox_Rabbit.Controls.Add(this.textBox_Server);
            this.groupBox_Rabbit.Controls.Add(this.label1);
            this.groupBox_Rabbit.Location = new System.Drawing.Point(12, 12);
            this.groupBox_Rabbit.Name = "groupBox_Rabbit";
            this.groupBox_Rabbit.Size = new System.Drawing.Size(273, 136);
            this.groupBox_Rabbit.TabIndex = 3;
            this.groupBox_Rabbit.TabStop = false;
            this.groupBox_Rabbit.Text = "RabbitMQ";
            // 
            // textBox_VirtualHost
            // 
            this.textBox_VirtualHost.Location = new System.Drawing.Point(118, 98);
            this.textBox_VirtualHost.Name = "textBox_VirtualHost";
            this.textBox_VirtualHost.Size = new System.Drawing.Size(136, 20);
            this.textBox_VirtualHost.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(16, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "віртуальний хост";
            // 
            // textBox_Pass
            // 
            this.textBox_Pass.Location = new System.Drawing.Point(118, 72);
            this.textBox_Pass.Name = "textBox_Pass";
            this.textBox_Pass.Size = new System.Drawing.Size(136, 20);
            this.textBox_Pass.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(65, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "пароль";
            // 
            // textBox_User
            // 
            this.textBox_User.Location = new System.Drawing.Point(118, 46);
            this.textBox_User.Name = "textBox_User";
            this.textBox_User.Size = new System.Drawing.Size(136, 20);
            this.textBox_User.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(44, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "користувач";
            // 
            // textBox_Server
            // 
            this.textBox_Server.Location = new System.Drawing.Point(118, 17);
            this.textBox_Server.Name = "textBox_Server";
            this.textBox_Server.Size = new System.Drawing.Size(136, 20);
            this.textBox_Server.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(64, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Сервер";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(113, 154);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 30);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 196);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBox_Rabbit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowIcon = false;
            this.Text = "Налаштування";
            this.groupBox_Rabbit.ResumeLayout(false);
            this.groupBox_Rabbit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_Rabbit;
        private System.Windows.Forms.TextBox textBox_VirtualHost;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_Pass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_User;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Server;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSave;
    }
}