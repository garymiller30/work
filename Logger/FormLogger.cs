using System;
using System.Windows.Forms;

namespace Logger
{
    public partial class FormLogger : Form
    {
        readonly object _lock = new object();

        

        public FormLogger()
        {
            InitializeComponent();
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
    }
}
