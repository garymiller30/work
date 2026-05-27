namespace PluginFileshareWeb
{
    partial class WindowOut
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowOut));
            toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            kryptonWorkspace1 = new Krypton.Workspace.KryptonWorkspace();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripTextBoxUrl = new System.Windows.Forms.ToolStripTextBox();
            tsb_go = new System.Windows.Forms.ToolStripButton();
            tsb_paste_go = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tstb_zoomFactor = new System.Windows.Forms.ToolStripTextBox();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            tsb_zoomOk = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton_Add = new System.Windows.Forms.ToolStripButton();
            tabPage1 = new System.Windows.Forms.TabPage();
            tabPage2 = new System.Windows.Forms.TabPage();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.TopToolStripPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonWorkspace1).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(kryptonWorkspace1);
            toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(944, 494);
            toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            toolStripContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            toolStripContainer1.Name = "toolStripContainer1";
            toolStripContainer1.Size = new System.Drawing.Size(944, 523);
            toolStripContainer1.TabIndex = 1;
            toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            toolStripContainer1.TopToolStripPanel.Controls.Add(toolStrip1);
            // 
            // kryptonWorkspace1
            // 
            kryptonWorkspace1.ActivePage = null;
            kryptonWorkspace1.CompactFlags = Krypton.Workspace.CompactFlags.RemoveEmptyCells;
            kryptonWorkspace1.ContainerBackStyle = Krypton.Toolkit.PaletteBackStyle.PanelClient;
            kryptonWorkspace1.Dock = System.Windows.Forms.DockStyle.Fill;
            kryptonWorkspace1.Location = new System.Drawing.Point(0, 0);
            kryptonWorkspace1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonWorkspace1.Name = "kryptonWorkspace1";
            // 
            // 
            // 
            kryptonWorkspace1.Root.UniqueName = "daecbd13ec4a49798f23058a37837f96";
            kryptonWorkspace1.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            kryptonWorkspace1.ShowMaximizeButton = false;
            kryptonWorkspace1.Size = new System.Drawing.Size(944, 494);
            kryptonWorkspace1.SplitterWidth = 5;
            kryptonWorkspace1.TabIndex = 0;
            kryptonWorkspace1.TabStop = true;
            // 
            // toolStrip1
            // 
            toolStrip1.AutoSize = false;
            toolStrip1.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripTextBoxUrl, tsb_go, tsb_paste_go, toolStripSeparator1, tstb_zoomFactor, toolStripLabel1, tsb_zoomOk, toolStripSeparator3, toolStripButton_Add });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            toolStrip1.Size = new System.Drawing.Size(944, 29);
            toolStrip1.Stretch = true;
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripTextBoxUrl
            // 
            toolStripTextBoxUrl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            toolStripTextBoxUrl.Name = "toolStripTextBoxUrl";
            toolStripTextBoxUrl.Size = new System.Drawing.Size(160, 23);
            toolStripTextBoxUrl.ToolTipText = "Адреса сторінки";
            toolStripTextBoxUrl.Click += toolStripTextBoxUrl_Click;
            // 
            // tsb_go
            // 
            tsb_go.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsb_go.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_go.Name = "tsb_go";
            tsb_go.Size = new System.Drawing.Size(58, 20);
            tsb_go.Text = "Перейти";
            tsb_go.ToolTipText = "Відкрити адресу";
            tsb_go.Click += toolStripButtonGo_Click;
            // 
            // tsb_paste_go
            // 
            tsb_paste_go.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsb_paste_go.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_paste_go.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            tsb_paste_go.Name = "tsb_paste_go";
            tsb_paste_go.Size = new System.Drawing.Size(114, 23);
            tsb_paste_go.Text = "Вставити і перейти";
            tsb_paste_go.ToolTipText = "Створити вкладку з посилання з буфера обміну";
            tsb_paste_go.Click += tsb_paste_go_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // tstb_zoomFactor
            // 
            tstb_zoomFactor.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            tstb_zoomFactor.Name = "tstb_zoomFactor";
            tstb_zoomFactor.Size = new System.Drawing.Size(44, 23);
            tstb_zoomFactor.Text = "80";
            tstb_zoomFactor.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(17, 20);
            toolStripLabel1.Text = "%";
            // 
            // tsb_zoomOk
            // 
            tsb_zoomOk.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsb_zoomOk.Image = (System.Drawing.Image)resources.GetObject("tsb_zoomOk.Image");
            tsb_zoomOk.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_zoomOk.Name = "tsb_zoomOk";
            tsb_zoomOk.Size = new System.Drawing.Size(27, 20);
            tsb_zoomOk.Text = "OK";
            tsb_zoomOk.ToolTipText = "Застосувати масштаб";
            tsb_zoomOk.Click += tsb_zoomOk_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButton_Add
            // 
            toolStripButton_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButton_Add.Image = (System.Drawing.Image)resources.GetObject("toolStripButton_Add.Image");
            toolStripButton_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton_Add.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            toolStripButton_Add.Name = "toolStripButton_Add";
            toolStripButton_Add.Size = new System.Drawing.Size(84, 23);
            toolStripButton_Add.Text = "+ Посилання";
            toolStripButton_Add.ToolTipText = "Додати швидке посилання";
            toolStripButton_Add.Click += toolStripButton_Add_Click;
            // 
            // tabPage1
            // 
            tabPage1.Location = new System.Drawing.Point(4, 40);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(795, 378);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new System.Drawing.Point(4, 40);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(801, 384);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // WindowOut
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(toolStripContainer1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "WindowOut";
            Size = new System.Drawing.Size(944, 523);
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonWorkspace1).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Add;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxUrl;
        private System.Windows.Forms.ToolStripButton tsb_go;
        private System.Windows.Forms.ToolStripButton tsb_paste_go;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox tstb_zoomFactor;
        private System.Windows.Forms.ToolStripButton tsb_zoomOk;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspace1;
    }
}
