using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Windows.Forms;

namespace ActiveWorks
{
    static class Program
    {
        /// <summary>
        /// 
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form2());
        }
    }
}
