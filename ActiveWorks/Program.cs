using Job.Static.Pdf.Imposition;
using System;
using System.Windows.Forms;

namespace ActiveWorks
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //var test = new PdfImposTest();
           // test.Run();

            Application.Run(new Form2());
        }
    }
}
