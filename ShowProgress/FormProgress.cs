using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShowProgress
{
    public partial class FormProgress : Form
    {
        private readonly Action _action;

        private FormProgress()
        {
            InitializeComponent();
        }

        public FormProgress(Action action):this()
        {

            _action = action;
        }

        private async void FormProgress_ShownAsync(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(_action).ConfigureAwait(true);
            }
            catch
            {
            }
            finally
            {
                Close();
            }
            
        }


        public static void ShowProgress(Action action)
        {
            var formProgress = new FormProgress(action);
            formProgress.ShowDialog();
        }
        
    }
}
