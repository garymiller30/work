using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job.Static.Pdf.SetTrimBox
{
    public class SetTrimBoxBase
    {

        internal void RewriteFile(string source, string target)
        {
            while (true)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(source, target, overwrite: true);
                    break;
                }
                catch (Exception e)
                {
                    if (MessageBox.Show(e.Message, @"Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) != DialogResult.Retry)
                    {
                        break;
                    }
                }
            }
        }
    }
}
