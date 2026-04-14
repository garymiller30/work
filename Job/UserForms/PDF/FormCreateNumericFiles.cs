using BrightIdeasSoftware;
using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public partial class FormCreateNumericFiles : Form
    {
        public List<IFileSystemInfoExt> SortedFiles { get; internal set; }
        public int StartFrom { get; internal set; } = 2;
        public int CntNumbers { get; internal set; } = 1;

        public FormCreateNumericFiles(List<Interfaces.IFileSystemInfoExt> inputFiles)
        {
            InitializeComponent();

            objectListView1.DragSource = new SimpleDragSource();
            objectListView1.DropSink = new RearrangingDropSink(false);

            DialogResult = DialogResult.Cancel;

            objectListView1.AddObjects(inputFiles);

            // Configure the olvColumn_no to display the current index (formatted)
            olvColumn_no.AspectGetter = rowObject =>
            {
                // Snapshot the current models in the control to reflect visible order
                var models = objectListView1.Objects.Cast<object>().ToList();

                // Find index of this model in the current order
                int position = models.IndexOf(rowObject);

                // Determine starting number and digit count from nud controls (fall back to StartFrom if controls not present)
                int start = StartFrom;
                int digits = 1;
                try
                {
                    start = Convert.ToInt32(nud_start_num.Value);
                    if (nud_cnt_numbers != null)
                        digits = Convert.ToInt32(nud_cnt_numbers.Value);
                }
                catch
                {
                    // ignore and use defaults
                }

                // If model not found, use the start value; otherwise add position offset
                int displayNumber = (position >= 0) ? start + position : start;

                // Format with leading zeros according to digits (no truncation)
                string format = displayNumber.ToString().PadLeft(Math.Max(1, digits), '0');
                return format;
            };

            // Refresh indexes when numeric controls change
            nud_start_num.ValueChanged += (s, e) =>
            {
                StartFrom = Convert.ToInt32(nud_start_num.Value);
                RefreshIndexColumn();
            };
            nud_cnt_numbers.ValueChanged += (s, e) =>
            {
                CntNumbers = Convert.ToInt32(nud_cnt_numbers.Value);
                RefreshIndexColumn();
            };

            // Optional: when the list changes (reorder), refresh to ensure numbers update immediately
            objectListView1.ItemsChanged += (s, e) => RefreshIndexColumn();

            RefreshIndexColumn();
        }

        private void RefreshIndexColumn()
        {
            // Refresh all objects so olvColumn_no.AspectGetter is called for each row
            try
            {
                var models = objectListView1.Objects.Cast<object>().ToArray();
                objectListView1.RefreshObjects(models);
            }
            catch
            {
                // If refresh fails for any reason, fall back to a full refresh
                objectListView1.Refresh();
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            // записати в  SortedFiles список файлів у поточному порядку
            SortedFiles = objectListView1.Objects.Cast<IFileSystemInfoExt>().ToList();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
