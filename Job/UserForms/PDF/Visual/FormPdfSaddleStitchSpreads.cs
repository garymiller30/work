using Interfaces;
using JobSpace.Models;
using JobSpace.Static.Pdf;
using JobSpace.Static.Pdf.Common;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormPdfSaddleStitchSpreads : Form
    {
        private readonly IFileSystemInfoExt _sourceFile;
        private string _previewFile;
        private bool _loaded;

        public double BleedMm => (double)nudBleed.Value;

        public FormPdfSaddleStitchSpreads(IFileSystemInfoExt sourceFile)
        {
            InitializeComponent();
            _sourceFile = sourceFile;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            DeletePreviewFile();
        }

        private void FormPdfSaddleStitchSpreads_Shown(object sender, EventArgs e)
        {
            if (_sourceFile == null)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            BuildPreview();
        }

        private void BuildPreview()
        {
            try
            {
                DeletePreviewFile();

                var pagesInfo = PdfHelper.GetPagesInfo(_sourceFile.FullName);
                int pageCount = pagesInfo.Count;
                var spreads = PdfSaddleStitchSpreads.GetSpreadOrder(pageCount);
                var firstTrimBox = pagesInfo.First().Trimbox;

                lblFileName.Text = _sourceFile.Name;
                lblPageCount.Text = pageCount.ToString();
                lblInputTrimBox.Text = $"{firstTrimBox.wMM():0.##} x {firstTrimBox.hMM():0.##} мм";
                lblOutputTrimBox.Text = $"{firstTrimBox.wMM() * 2:0.##} x {firstTrimBox.hMM():0.##} мм";

                lvSpreads.BeginUpdate();
                lvSpreads.Items.Clear();
                for (int i = 0; i < spreads.Count; i++)
                {
                    var item = new ListViewItem((i + 1).ToString());
                    item.SubItems.Add(spreads[i].LeftPage.ToString());
                    item.SubItems.Add(spreads[i].RightPage.ToString());
                    item.SubItems.Add(spreads[i].ToString());
                    lvSpreads.Items.Add(item);
                }
                lvSpreads.EndUpdate();

                _previewFile = Path.Combine(
                    Path.GetTempPath(),
                    $"{Path.GetFileNameWithoutExtension(_sourceFile.Name)}_saddle_spreads_preview_{Guid.NewGuid():N}.pdf");

                PdfSaddleStitchSpreads.CreateSaddleStitchSpreads(_sourceFile.FullName, _previewFile, BleedMm);
                ucPreview.Show(new FileSystemInfoExt(_previewFile));
                _loaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Макет розворотами на скобу",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void nudBleed_ValueChanged(object sender, EventArgs e)
        {
            if (_loaded)
                BuildPreview();
        }

        private void nudBleed_Enter(object sender, EventArgs e)
        {
            nudBleed.Select(0, nudBleed.Text.Length);
        }

        private void DeletePreviewFile()
        {
            if (string.IsNullOrEmpty(_previewFile))
                return;

            try
            {
                if (File.Exists(_previewFile))
                    File.Delete(_previewFile);
            }
            catch
            {
                // Preview cleanup is best-effort because the async renderer can still hold the file briefly.
            }

            _previewFile = null;
        }
    }
}
