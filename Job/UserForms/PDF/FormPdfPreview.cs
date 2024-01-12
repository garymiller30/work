using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ghostscript.NET;
using Ghostscript.NET.Processor;
using Ghostscript.NET.Viewer;

namespace Job
{
    public sealed partial  class FormPdfPreview : Form
    {

        // Define GetShortPathName API function.
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint GetShortPathName(string lpszLongPath,
            char[] lpszShortPath, int cchBuffer);

        private GhostscriptViewer _viewer;
        private GhostscriptVersionInfo _lastInstalledVersion;

        private string _fileName;

        public FormPdfPreview()
        {
            InitializeComponent();
            //axAcroPDF1.Size = ClientSize;
            try
            {
                _lastInstalledVersion =
                    GhostscriptVersionInfo.GetLastInstalledVersion(
                        GhostscriptLicense.GPL | GhostscriptLicense.AFPL,
                        GhostscriptLicense.GPL);
                _viewer = new GhostscriptViewer { ProgressiveUpdateInterval = 100 };
                _viewer.DisplaySize += _viewer_DisplaySize;
                _viewer.DisplayUpdate += _viewer_DisplayUpdate;
                _viewer.DisplayPage += _viewer_DisplayPage;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.Source);
            }

        }

        public FormPdfPreview(string fileName):this()
        {
            
            _fileName = fileName;
            //Text = Path.GetFileName(fileName);
            ShowPdf(fileName);

        }

        public void ShowPdf(string fileName)
        {
            Show();
            _fileName = ShortFileName(fileName);
            Text = Path.GetFileName(fileName);

            try
            {
                _viewer?.Open(_fileName, _lastInstalledVersion, false);
                //axAcroPDF1.setView("Fit");
                //axAcroPDF1.setLayoutMode("SinglePage");
                //axAcroPDF1.setShowToolbar(false);
                //axAcroPDF1.LoadFile(_fileName) ;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, @"Ошибка");
            }
        }

        void _viewer_DisplaySize(object sender, GhostscriptViewerViewEventArgs e)
        {
             pictureBox1.Image = e.Image;
            // store PDF page image reference
            
        }
        void _viewer_DisplayUpdate(object sender, GhostscriptViewerViewEventArgs e)
        {
            // if we are displaying the image in the PictureBox we can update 
            // it by calling PictureBox.Invalidate() and PictureBox.Update()
            // methods. We dont need to set image reference again because
            // Ghostscript.NET is changing Image object directly in the
            // memory and does not create new Bitmap instance.
            //pictureBox1.Image = e.Image;

            pictureBox1.Invalidate();
            pictureBox1.Update();
        }
        void _viewer_DisplayPage(object sender, GhostscriptViewerViewEventArgs e)
        {
            // complete PDF page is rasterized and we can update our PictureBox
            // once again by calling PictureBox.Invalidate() and PictureBox.Update()

            //pictureBox1.Image = e.Image;

            pictureBox1.Invalidate();
            pictureBox1.Update();
        }

        private void FormPdfPreview_FormClosing(object sender, FormClosingEventArgs e)
        {

            e.Cancel = true;
            Hide();

        }

        public static string GetInkCoverage(string file)
        {
            GhostscriptPipedOutput gsPipedOutput = new GhostscriptPipedOutput();
            string outputPipeHandle = $"%handle%{int.Parse(gsPipedOutput.ClientHandle):X2}";

            var switches = new List<string> {"-empty", "-q", $"-o{outputPipeHandle}", "-sDEVICE=inkcov", file};

            GhostscriptProcessor proc = new GhostscriptProcessor();
            proc.StartProcessing(switches.ToArray(), null);

            byte[] data = gsPipedOutput.Data;

            proc.Dispose();
            gsPipedOutput.Dispose();

            return System.Text.Encoding.ASCII.GetString(data);
        }

        private void FormPdfPreview_FormClosed(object sender, FormClosedEventArgs e)
        {
            _viewer?.Dispose();
            //axAcroPDF1.Dispose();
        }


        // Return the short file name for a long file name.
        private string ShortFileName(string longName)
        {
            var nameChars = new char[1024];
            long length = GetShortPathName(
                longName, nameChars,
                nameChars.Length);

            var shortName = new string(nameChars);
            return shortName.Substring(0, (int)length);
        }
    }
}
