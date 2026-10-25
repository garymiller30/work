using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JobSpace.UC.PDF.Visual
{
    public partial class Uc_SelectFileAndPageControl : UserControl
    {
        public string? SelectedFilePath { get; private set; }
        public int SelectedPageNo { get; private set; } = 1;
        public bool IsFileSelected => !string.IsNullOrEmpty(SelectedFilePath);
        public event EventHandler? FileSelectionChanged = delegate { };
        public event EventHandler<Tuple<decimal, decimal>>? PositionChanged = delegate { };

        class FileItem
        {
            public string FileName { get; set; } = "вибери файл";
            public string? FilePath { get; set; }
            public int PageNo { get; set; } = 0;
            public int PageCount { get; set; } = 0;
            public override string ToString() => FileName;
        }


        public Uc_SelectFileAndPageControl()
        {
            InitializeComponent();

            cb_selectFile.Items.Add(new FileItem());
            cb_selectFile.SelectedIndex = 0;

            cb_selectFile.SelectedIndexChanged += (s, e) =>
            {
                var item = cb_selectFile.SelectedItem as FileItem;
                if (item != null)
                {
                    SelectedFilePath = item.FilePath;
                    SelectedPageNo = item.PageNo;
                    nud_pageNo.Maximum = item.PageCount;
                    nud_pageNo.Value = item.PageNo;

                    FileSelectionChanged?.Invoke(this, EventArgs.Empty);
                }
            };


            nud_pageNo.ValueChanged += (s, e) =>
            {
                var item = cb_selectFile.SelectedItem as FileItem;
                if (item != null)
                {
                    SelectedPageNo = (int)nud_pageNo.Value;
                    item.PageNo = SelectedPageNo;

                    FileSelectionChanged?.Invoke(this, EventArgs.Empty);
                }
            };

            uc_PositionControl1.OnPositionChanged += (s, p) => PositionChanged?.Invoke(this, p);
         
            var toolTip = new ToolTip();
            toolTip.SetToolTip(cb_position_enable, "Включити/виключити позицію для обраного файлу та сторінки");

        }

        public void AddFiles(List<string> files)
        {
            foreach (var f in files)
            {
                var fi = new System.IO.FileInfo(f);

                var fileItem = new FileItem() { FileName = fi.Name, FilePath = f, PageNo = 1 };
                fileItem.PageCount = PdfHelper.GetPageCount(f);
                cb_selectFile.Items.Add(fileItem);
            }

            AdjustComboBoxWidth(cb_selectFile);
        }

        private void AdjustComboBoxWidth(ComboBox box)
        {
            int maxWidth = box.Width;
            Graphics g = box.CreateGraphics();

            foreach (var item in box.Items)
            {
                // Вимірюємо ширину тексту кожного елемента з урахуванням шрифту
                int itemWidth = (int)g.MeasureString(item.ToString(), box.Font).Width;

                if (itemWidth > maxWidth)
                {
                    maxWidth = itemWidth;
                }
            }

            // Додаємо невеликий запас для смуги прокручування (приблизно 20-30 пікселів)
            box.DropDownWidth = maxWidth + SystemInformation.VerticalScrollBarWidth;
        }

        private void cb_position_enable_CheckedChanged(object sender, EventArgs e)
        {
            uc_PositionControl1.Visible = cb_position_enable.Checked;
        }
    }
}
