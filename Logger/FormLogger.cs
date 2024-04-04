using Krypton.Toolkit;
using System;
using System.Windows.Forms;

namespace Logger
{
    public partial class FormLogger : KryptonForm
    {
        readonly object _lock = new object();

        

        public FormLogger()
        {
            InitializeComponent();
            objectListView1.Sort(olvColumnDate, SortOrder.Descending);
        }

        internal void WriteLine(LogRow logRow)
        {
            lock(_lock)
            {
                objectListView1.Invoke((MethodInvoker) delegate { objectListView1.AddObject(logRow); });
            }
        }

        private void clearWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                objectListView1.ClearObjects();    
            }
            
        }

        private void objectListView1_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            LogRow row = e.Model as LogRow;
            if (row.Status == "ERROR")
            {
                e.Item.BackColor = System.Drawing.Color.Red;
                e.Item.ForeColor = System.Drawing.Color.White;
            }
            else if (row.Status == "WARNING")
            {
                e.Item.BackColor = System.Drawing.Color.Yellow;
                e.Item.ForeColor = System.Drawing.Color.Black;
            }
            else if (row.Status == "INFO")
            {
                e.Item.BackColor = System.Drawing.Color.LightGray;
                e.Item.ForeColor = System.Drawing.Color.Black;
            }
        }
    }
}
