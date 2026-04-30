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

        int _previewRequestId;




        public Uc_FilePreviewControl()
        {
            InitializeComponent();
            previewParameters = new PdfPreviewParameters();

            uc_PreviewControl1.SetPreviewParameters(previewParameters);
        }

        public void Show(IFileSystemInfoExt filePath)
        {
            _previewRequestId++;
            pdfDrawerPageCache?.Dispose();

            pdfDrawerPageCache = new PdfDrawerPageCache(filePath);

            _currentPage = 1;
            tst_cur_page.Text = _currentPage.ToString();
            tsl_count_pages.Text = $"/{pdfDrawerPageCache.TotalPages}";

            GetPreview();
        }



        private async void GetPreview()
        {
            int requestId = _previewRequestId;
            var pageCache = pdfDrawerPageCache;
            int page = _currentPage;

            uc_PreviewControl1.StartWait(Path.Combine(AppContext.BaseDirectory, "db\\resources\\wait.gif"));
            try
            {
                var res = await PdfDrawerService.GetImageAsync(previewParameters, pageCache, page);

                if (requestId != _previewRequestId || pageCache != pdfDrawerPageCache || page != _currentPage)
                    return;

                if (res != null)
                {
                    uc_PreviewControl1.SetImage(res.Item1, res.Item2, res.Item3);
                }
                else
                {
                    uc_PreviewControl1.SetImage(null, 0, 0);
                }

            }
            catch (Exception e)
            {
                if (requestId != _previewRequestId || pageCache != pdfDrawerPageCache || page != _currentPage)
                    return;

                Logger.Log.Error(null, "Uc_FilePreviewControl.GetPreview", e.Message);
                uc_PreviewControl1.SetImage(null, 0, 0);
            }
            finally
            {
                if (requestId == _previewRequestId && pageCache == pdfDrawerPageCache && page == _currentPage)
                    uc_PreviewControl1.StopWait();
                // 3. !!! КРИТИЧНИЙ ШТРИХ: Примусове оновлення інтерфейсу!
                // Це змушує контейнер перемалюватися, витираючи будь-які "примарні" елементи, 
                // які могли залишитися після StopWait().
                //this.Invoke(new Action(() => uc_PreviewControl1.Invalidate())); // Або Refresh()
            }
        }

        private void tsb_previous_page_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                tst_cur_page.Text = _currentPage.ToString();
                GetPreview();
            }
        }

        private void tsb_next_page_Click(object sender, EventArgs e)
        {
            _currentPage++;
            if (previewParameters.Display == PdfPreviewDisplay.Spread)
            {
                _currentPage++;
            }
            
            if (_currentPage > pdfDrawerPageCache.TotalPages)
            { _currentPage = 1; }

            tst_cur_page.Text = _currentPage.ToString();
            GetPreview();
        }

        private void tst_cur_page_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tst_cur_page.Text, out int page))
            {
                if (page >= 1 && page <= pdfDrawerPageCache.TotalPages)
                {
                    _currentPage = page;
                    GetPreview();

                }
                else
                {
                    tst_cur_page.Text = _currentPage.ToString();
                }
            }
        }

        public void ClearPreview()
        {
            _previewRequestId++;
            uc_PreviewControl1.SetImage(null, 0, 0);
        }

        private void tsb_fit_to_window_CheckStateChanged(object sender, EventArgs e)
        {
            uc_PreviewControl1.SetFitAndResetZoom(tsb_fit_to_window.Checked);
        }

        public PdfPageInfo GetCurrentPageInfo()
        {
            return pdfDrawerPageCache?.GetPageInfo(_currentPage);
        }

        public PdfPageInfo GetPageInfo(int pageNo)
        {
            return pdfDrawerPageCache?.GetPageInfo(pageNo);
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
