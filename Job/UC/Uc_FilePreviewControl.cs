using Interfaces;
using JobSpace.Models;
using JobSpace.Models.PdfDrawer;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace JobSpace.UC
{
    public partial class Uc_FilePreviewControl : UserControl
    {
        int _currentPage = 1;

        PdfDrawerPageCache pdfDrawerPageCache;
       
        PdfPreviewParameters previewParameters;

        public Func<int,List<IScreenPrimitive>> GetScreenPrimitives {get;set;}

        #region [ EVENTS ]
        public event EventHandler<int> OnPageChanged = delegate { };
        #endregion

        public Uc_FilePreviewControl()
        {
            InitializeComponent();
            previewParameters = new PdfPreviewParameters();

            uc_PreviewControl1.SetPreviewParameters(previewParameters);
        }

        public void Show(IFileSystemInfoExt filePath)
        {
            pdfDrawerPageCache = new PdfDrawerPageCache(filePath);

            _currentPage = 1;
            tst_cur_page.Text = _currentPage.ToString();
            tsl_count_pages.Text = $"/{pdfDrawerPageCache.TotalPages}";
      
            GetPreview();
        }

      

        private async void GetPreview()
        {
            uc_PreviewControl1.StartWait(Path.Combine(AppContext.BaseDirectory, "db\\resources\\wait.gif"));

            var res = await PdfDrawerService.GetImageAsync(previewParameters,pdfDrawerPageCache,_currentPage);
            
            uc_PreviewControl1.StopWait();

            if (res != null)
            {
                uc_PreviewControl1.SetImage(res.Item1, res.Item2, res.Item3);
            }
        }

        private void tsb_previous_page_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                tst_cur_page.Text = _currentPage.ToString();
                GetPreview();
                //OnPageChanged(this, _currentPage);
            }
        }

        private void tsb_next_page_Click(object sender, EventArgs e)
        {
            if (_currentPage < pdfDrawerPageCache.TotalPages)
            {
                _currentPage++;
                tst_cur_page.Text = _currentPage.ToString();
                GetPreview();
                //OnPageChanged(this, _currentPage);
            }
        }

        private void tst_cur_page_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tst_cur_page.Text, out int page))
            {
                if (page >= 1 && page <= pdfDrawerPageCache.TotalPages)
                {
                    _currentPage = page;
                    GetPreview();
                    //OnPageChanged(this, _currentPage);
                }
                else
                {
                    tst_cur_page.Text = _currentPage.ToString();
                }
            }
        }

        public void ClearPreview()
        {
            uc_PreviewControl1.SetImage(null, 0, 0);
        }

        private void tsb_fit_to_window_CheckStateChanged(object sender, EventArgs e)
        {
            uc_PreviewControl1.SetFitAndResetZoom(tsb_fit_to_window.Checked);
        }

        public void SetPrimitives(List<IScreenPrimitive> primitives)
        {
            uc_PreviewControl1.Primitives = primitives;
        }

        public PdfPageInfo GetCurrentPageInfo()
        {
            return pdfDrawerPageCache?.GetPageInfo(_currentPage);
        }

        public int GetCurrentPageIdx()
        {
            return _currentPage - 1;
        }

        public int GetTotalPages()
        {
            return pdfDrawerPageCache.TotalPages;
        }


        private void tsb_show_spread_CheckedChanged(object sender, EventArgs e)
        {
            previewParameters.Display = tsb_show_spread.Checked ? PdfPreviewDisplay.Spread : PdfPreviewDisplay.Single;
            GetPreview();
        }

        public void Redraw()
        {
            GetPreview();
            //uc_PreviewControl1.Redraw();
        }

        public void SetFunc_GetScreenPrimitives(Func<int, List<IScreenPrimitive>> getPrimitives)
        {
            previewParameters.GetScreenPrimitives = getPrimitives;
        }
    }
}
