namespace JobSpace.UserForms.PDF.Visual
{
    partial class FormVisualQuarterlyCalendar
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmi_file = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_new = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi_save = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_save_as = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_tools = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_autolayout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_add_module = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_remove_module = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_export = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_export_image = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssl_dimensions = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_zoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_mouse_coords = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_selected = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_left = new System.Windows.Forms.SplitContainer();
            this.gb_structure = new System.Windows.Forms.GroupBox();
            this.tv_structure = new System.Windows.Forms.TreeView();
            this.panel_module_buttons = new System.Windows.Forms.Panel();
            this.btn_remove_module = new System.Windows.Forms.Button();
            this.btn_add_module = new System.Windows.Forms.Button();
            this.gb_properties = new System.Windows.Forms.GroupBox();
            this.panel_props_container = new System.Windows.Forms.Panel();
            this.panel_canvas_toolbar = new System.Windows.Forms.Panel();
            this.btn_autolayout = new System.Windows.Forms.Button();
            this.btn_zoom_out = new System.Windows.Forms.Button();
            this.btn_zoom_in = new System.Windows.Forms.Button();
            this.btn_zoom_100 = new System.Windows.Forms.Button();
            this.btn_fit = new System.Windows.Forms.Button();
            this.btn_export_quick = new System.Windows.Forms.Button();
            this.panel_canvas_viewport = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_left)).BeginInit();
            this.splitContainer_left.Panel1.SuspendLayout();
            this.splitContainer_left.Panel2.SuspendLayout();
            this.splitContainer_left.SuspendLayout();
            this.gb_structure.SuspendLayout();
            this.panel_module_buttons.SuspendLayout();
            this.gb_properties.SuspendLayout();
            this.panel_canvas_toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_file,
            this.tsmi_tools,
            this.tsmi_export});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmi_file
            // 
            this.tsmi_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_new,
            this.tsmi_open,
            this.toolStripSeparator1,
            this.tsmi_save,
            this.tsmi_save_as,
            this.toolStripSeparator2,
            this.tsmi_exit});
            this.tsmi_file.Name = "tsmi_file";
            this.tsmi_file.Size = new System.Drawing.Size(48, 20);
            this.tsmi_file.Text = "Файл";
            // 
            // tsmi_new
            // 
            this.tsmi_new.Name = "tsmi_new";
            this.tsmi_new.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.tsmi_new.Size = new System.Drawing.Size(225, 22);
            this.tsmi_new.Text = "Новий календар";
            this.tsmi_new.Click += new System.EventHandler(this.tsmi_new_Click);
            // 
            // tsmi_open
            // 
            this.tsmi_open.Name = "tsmi_open";
            this.tsmi_open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmi_open.Size = new System.Drawing.Size(225, 22);
            this.tsmi_open.Text = "Відкрити...";
            this.tsmi_open.Click += new System.EventHandler(this.tsmi_open_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(222, 6);
            // 
            // tsmi_save
            // 
            this.tsmi_save.Name = "tsmi_save";
            this.tsmi_save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmi_save.Size = new System.Drawing.Size(225, 22);
            this.tsmi_save.Text = "Зберегти";
            this.tsmi_save.Click += new System.EventHandler(this.tsmi_save_Click);
            // 
            // tsmi_save_as
            // 
            this.tsmi_save_as.Name = "tsmi_save_as";
            this.tsmi_save_as.Size = new System.Drawing.Size(225, 22);
            this.tsmi_save_as.Text = "Зберегти як...";
            this.tsmi_save_as.Click += new System.EventHandler(this.tsmi_save_as_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(222, 6);
            // 
            // tsmi_exit
            // 
            this.tsmi_exit.Name = "tsmi_exit";
            this.tsmi_exit.Size = new System.Drawing.Size(225, 22);
            this.tsmi_exit.Text = "Закрити";
            this.tsmi_exit.Click += new System.EventHandler(this.tsmi_exit_Click);
            // 
            // tsmi_tools
            // 
            this.tsmi_tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_autolayout,
            this.tsmi_add_module,
            this.tsmi_remove_module});
            this.tsmi_tools.Name = "tsmi_tools";
            this.tsmi_tools.Size = new System.Drawing.Size(87, 20);
            this.tsmi_tools.Text = "Конструкція";
            // 
            // tsmi_autolayout
            // 
            this.tsmi_autolayout.Name = "tsmi_autolayout";
            this.tsmi_autolayout.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.tsmi_autolayout.Size = new System.Drawing.Size(228, 22);
            this.tsmi_autolayout.Text = "Автовирівнювання";
            this.tsmi_autolayout.Click += new System.EventHandler(this.tsmi_autolayout_Click);
            // 
            // tsmi_add_module
            // 
            this.tsmi_add_module.Name = "tsmi_add_module";
            this.tsmi_add_module.Size = new System.Drawing.Size(228, 22);
            this.tsmi_add_module.Text = "Додати модуль";
            this.tsmi_add_module.Click += new System.EventHandler(this.btn_add_module_Click);
            // 
            // tsmi_remove_module
            // 
            this.tsmi_remove_module.Name = "tsmi_remove_module";
            this.tsmi_remove_module.Size = new System.Drawing.Size(228, 22);
            this.tsmi_remove_module.Text = "Видалити останній модуль";
            this.tsmi_remove_module.Click += new System.EventHandler(this.btn_remove_module_Click);
            // 
            // tsmi_export
            // 
            this.tsmi_export.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_export_image});
            this.tsmi_export.Name = "tsmi_export";
            this.tsmi_export.Size = new System.Drawing.Size(65, 20);
            this.tsmi_export.Text = "Експорт";
            // 
            // tsmi_export_image
            // 
            this.tsmi_export_image.Name = "tsmi_export_image";
            this.tsmi_export_image.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.tsmi_export_image.Size = new System.Drawing.Size(262, 22);
            this.tsmi_export_image.Text = "Зберегти зображення...";
            this.tsmi_export_image.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssl_dimensions,
            this.tssl_zoom,
            this.tssl_mouse_coords,
            this.tssl_selected});
            this.statusStrip1.Location = new System.Drawing.Point(0, 778);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1200, 24);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssl_dimensions
            // 
            this.tssl_dimensions.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tssl_dimensions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tssl_dimensions.Name = "tssl_dimensions";
            this.tssl_dimensions.Size = new System.Drawing.Size(190, 19);
            this.tssl_dimensions.Text = "Габарити виробу: 0.0 × 0.0 мм";
            // 
            // tssl_zoom
            // 
            this.tssl_zoom.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tssl_zoom.Name = "tssl_zoom";
            this.tssl_zoom.Size = new System.Drawing.Size(95, 19);
            this.tssl_zoom.Text = "Масштаб: 100%";
            // 
            // tssl_mouse_coords
            // 
            this.tssl_mouse_coords.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tssl_mouse_coords.Name = "tssl_mouse_coords";
            this.tssl_mouse_coords.Size = new System.Drawing.Size(120, 19);
            this.tssl_mouse_coords.Text = "X: 0.0 мм, Y: 0.0 мм";
            // 
            // tssl_selected
            // 
            this.tssl_selected.Name = "tssl_selected";
            this.tssl_selected.Size = new System.Drawing.Size(117, 19);
            this.tssl_selected.Text = "Вибрано: (нічого)";
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 24);
            this.splitContainer_main.Name = "splitContainer_main";
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.splitContainer_left);
            this.splitContainer_main.Panel1MinSize = 340;
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.panel_canvas_viewport);
            this.splitContainer_main.Panel2.Controls.Add(this.panel_canvas_toolbar);
            this.splitContainer_main.Size = new System.Drawing.Size(1200, 754);
            this.splitContainer_main.SplitterDistance = 370;
            this.splitContainer_main.TabIndex = 2;
            // 
            // splitContainer_left
            // 
            this.splitContainer_left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_left.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_left.Name = "splitContainer_left";
            this.splitContainer_left.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_left.Panel1
            // 
            this.splitContainer_left.Panel1.Controls.Add(this.gb_structure);
            this.splitContainer_left.Panel1MinSize = 200;
            // 
            // splitContainer_left.Panel2
            // 
            this.splitContainer_left.Panel2.Controls.Add(this.gb_properties);
            this.splitContainer_left.Panel2MinSize = 250;
            this.splitContainer_left.Size = new System.Drawing.Size(370, 754);
            this.splitContainer_left.SplitterDistance = 270;
            this.splitContainer_left.TabIndex = 0;
            // 
            // gb_structure
            // 
            this.gb_structure.Controls.Add(this.tv_structure);
            this.gb_structure.Controls.Add(this.panel_module_buttons);
            this.gb_structure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_structure.Location = new System.Drawing.Point(0, 0);
            this.gb_structure.Name = "gb_structure";
            this.gb_structure.Padding = new System.Windows.Forms.Padding(6);
            this.gb_structure.Size = new System.Drawing.Size(370, 270);
            this.gb_structure.TabIndex = 0;
            this.gb_structure.TabStop = false;
            this.gb_structure.Text = "Конструкція календаря";
            // 
            // tv_structure
            // 
            this.tv_structure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv_structure.HideSelection = false;
            this.tv_structure.Location = new System.Drawing.Point(6, 22);
            this.tv_structure.Name = "tv_structure";
            this.tv_structure.Size = new System.Drawing.Size(358, 206);
            this.tv_structure.TabIndex = 0;
            this.tv_structure.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_structure_AfterSelect);
            // 
            // panel_module_buttons
            // 
            this.panel_module_buttons.Controls.Add(this.btn_remove_module);
            this.panel_module_buttons.Controls.Add(this.btn_add_module);
            this.panel_module_buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_module_buttons.Location = new System.Drawing.Point(6, 228);
            this.panel_module_buttons.Name = "panel_module_buttons";
            this.panel_module_buttons.Size = new System.Drawing.Size(358, 36);
            this.panel_module_buttons.TabIndex = 1;
            // 
            // btn_remove_module
            // 
            this.btn_remove_module.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_remove_module.Location = new System.Drawing.Point(184, 5);
            this.btn_remove_module.Name = "btn_remove_module";
            this.btn_remove_module.Size = new System.Drawing.Size(168, 27);
            this.btn_remove_module.TabIndex = 1;
            this.btn_remove_module.Text = "- Видалити модуль";
            this.btn_remove_module.UseVisualStyleBackColor = true;
            this.btn_remove_module.Click += new System.EventHandler(this.btn_remove_module_Click);
            // 
            // btn_add_module
            // 
            this.btn_add_module.Location = new System.Drawing.Point(6, 5);
            this.btn_add_module.Name = "btn_add_module";
            this.btn_add_module.Size = new System.Drawing.Size(168, 27);
            this.btn_add_module.TabIndex = 0;
            this.btn_add_module.Text = "+ Додати модуль";
            this.btn_add_module.UseVisualStyleBackColor = true;
            this.btn_add_module.Click += new System.EventHandler(this.btn_add_module_Click);
            // 
            // gb_properties
            // 
            this.gb_properties.Controls.Add(this.panel_props_container);
            this.gb_properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_properties.Location = new System.Drawing.Point(0, 0);
            this.gb_properties.Name = "gb_properties";
            this.gb_properties.Padding = new System.Windows.Forms.Padding(6);
            this.gb_properties.Size = new System.Drawing.Size(370, 480);
            this.gb_properties.TabIndex = 0;
            this.gb_properties.TabStop = false;
            this.gb_properties.Text = "Властивості вибраного елемента";
            // 
            // panel_props_container
            // 
            this.panel_props_container.AutoScroll = true;
            this.panel_props_container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_props_container.Location = new System.Drawing.Point(6, 22);
            this.panel_props_container.Name = "panel_props_container";
            this.panel_props_container.Size = new System.Drawing.Size(358, 452);
            this.panel_props_container.TabIndex = 0;
            // 
            // panel_canvas_toolbar
            // 
            this.panel_canvas_toolbar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel_canvas_toolbar.Controls.Add(this.btn_export_quick);
            this.panel_canvas_toolbar.Controls.Add(this.btn_autolayout);
            this.panel_canvas_toolbar.Controls.Add(this.btn_zoom_out);
            this.panel_canvas_toolbar.Controls.Add(this.btn_zoom_in);
            this.panel_canvas_toolbar.Controls.Add(this.btn_zoom_100);
            this.panel_canvas_toolbar.Controls.Add(this.btn_fit);
            this.panel_canvas_toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_canvas_toolbar.Location = new System.Drawing.Point(0, 0);
            this.panel_canvas_toolbar.Name = "panel_canvas_toolbar";
            this.panel_canvas_toolbar.Size = new System.Drawing.Size(826, 36);
            this.panel_canvas_toolbar.TabIndex = 0;
            // 
            // btn_autolayout
            // 
            this.btn_autolayout.Location = new System.Drawing.Point(315, 5);
            this.btn_autolayout.Name = "btn_autolayout";
            this.btn_autolayout.Size = new System.Drawing.Size(130, 26);
            this.btn_autolayout.TabIndex = 4;
            this.btn_autolayout.Text = "Автовирівнювання";
            this.btn_autolayout.UseVisualStyleBackColor = true;
            this.btn_autolayout.Click += new System.EventHandler(this.tsmi_autolayout_Click);
            // 
            // btn_zoom_out
            // 
            this.btn_zoom_out.Location = new System.Drawing.Point(245, 5);
            this.btn_zoom_out.Name = "btn_zoom_out";
            this.btn_zoom_out.Size = new System.Drawing.Size(60, 26);
            this.btn_zoom_out.TabIndex = 3;
            this.btn_zoom_out.Text = "-";
            this.btn_zoom_out.UseVisualStyleBackColor = true;
            this.btn_zoom_out.Click += new System.EventHandler(this.btn_zoom_out_Click);
            // 
            // btn_zoom_in
            // 
            this.btn_zoom_in.Location = new System.Drawing.Point(180, 5);
            this.btn_zoom_in.Name = "btn_zoom_in";
            this.btn_zoom_in.Size = new System.Drawing.Size(60, 26);
            this.btn_zoom_in.TabIndex = 2;
            this.btn_zoom_in.Text = "+";
            this.btn_zoom_in.UseVisualStyleBackColor = true;
            this.btn_zoom_in.Click += new System.EventHandler(this.btn_zoom_in_Click);
            // 
            // btn_zoom_100
            // 
            this.btn_zoom_100.Location = new System.Drawing.Point(105, 5);
            this.btn_zoom_100.Name = "btn_zoom_100";
            this.btn_zoom_100.Size = new System.Drawing.Size(70, 26);
            this.btn_zoom_100.TabIndex = 1;
            this.btn_zoom_100.Text = "100%";
            this.btn_zoom_100.UseVisualStyleBackColor = true;
            this.btn_zoom_100.Click += new System.EventHandler(this.btn_zoom_100_Click);
            // 
            // btn_fit
            // 
            this.btn_fit.Location = new System.Drawing.Point(8, 5);
            this.btn_fit.Name = "btn_fit";
            this.btn_fit.Size = new System.Drawing.Size(92, 26);
            this.btn_fit.TabIndex = 0;
            this.btn_fit.Text = "Вписати";
            this.btn_fit.UseVisualStyleBackColor = true;
            this.btn_fit.Click += new System.EventHandler(this.btn_fit_Click);
            // 
            // btn_export_quick
            // 
            this.btn_export_quick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_export_quick.Location = new System.Drawing.Point(665, 5);
            this.btn_export_quick.Name = "btn_export_quick";
            this.btn_export_quick.Size = new System.Drawing.Size(150, 26);
            this.btn_export_quick.TabIndex = 5;
            this.btn_export_quick.Text = "Експорт зображення...";
            this.btn_export_quick.UseVisualStyleBackColor = true;
            this.btn_export_quick.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // panel_canvas_viewport
            // 
            this.panel_canvas_viewport.BackColor = System.Drawing.Color.FromArgb(220, 225, 230);
            this.panel_canvas_viewport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_canvas_viewport.Location = new System.Drawing.Point(0, 36);
            this.panel_canvas_viewport.Name = "panel_canvas_viewport";
            this.panel_canvas_viewport.Size = new System.Drawing.Size(826, 718);
            this.panel_canvas_viewport.TabIndex = 1;
            // 
            // FormVisualQuarterlyCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 802);
            this.Controls.Add(this.splitContainer_main);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(950, 600);
            this.Name = "FormVisualQuarterlyCalendar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Конструктор візуалізації квартального календаря";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.splitContainer_left.Panel1.ResumeLayout(false);
            this.splitContainer_left.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_left)).EndInit();
            this.splitContainer_left.ResumeLayout(false);
            this.gb_structure.ResumeLayout(false);
            this.panel_module_buttons.ResumeLayout(false);
            this.gb_properties.ResumeLayout(false);
            this.panel_canvas_toolbar.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmi_file;
        private System.Windows.Forms.ToolStripMenuItem tsmi_new;
        private System.Windows.Forms.ToolStripMenuItem tsmi_open;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmi_save;
        private System.Windows.Forms.ToolStripMenuItem tsmi_save_as;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmi_exit;
        private System.Windows.Forms.ToolStripMenuItem tsmi_tools;
        private System.Windows.Forms.ToolStripMenuItem tsmi_autolayout;
        private System.Windows.Forms.ToolStripMenuItem tsmi_add_module;
        private System.Windows.Forms.ToolStripMenuItem tsmi_remove_module;
        private System.Windows.Forms.ToolStripMenuItem tsmi_export;
        private System.Windows.Forms.ToolStripMenuItem tsmi_export_image;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssl_dimensions;
        private System.Windows.Forms.ToolStripStatusLabel tssl_zoom;
        private System.Windows.Forms.ToolStripStatusLabel tssl_mouse_coords;
        private System.Windows.Forms.ToolStripStatusLabel tssl_selected;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.SplitContainer splitContainer_left;
        private System.Windows.Forms.GroupBox gb_structure;
        private System.Windows.Forms.TreeView tv_structure;
        private System.Windows.Forms.Panel panel_module_buttons;
        private System.Windows.Forms.Button btn_remove_module;
        private System.Windows.Forms.Button btn_add_module;
        private System.Windows.Forms.GroupBox gb_properties;
        private System.Windows.Forms.Panel panel_props_container;
        private System.Windows.Forms.Panel panel_canvas_toolbar;
        private System.Windows.Forms.Button btn_fit;
        private System.Windows.Forms.Button btn_zoom_100;
        private System.Windows.Forms.Button btn_zoom_in;
        private System.Windows.Forms.Button btn_zoom_out;
        private System.Windows.Forms.Button btn_autolayout;
        private System.Windows.Forms.Button btn_export_quick;
        private System.Windows.Forms.Panel panel_canvas_viewport;
    }
}
