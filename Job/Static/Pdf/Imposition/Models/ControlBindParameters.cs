using BrightIdeasSoftware;
using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class ControlBindParameters : INotifyPropertyChanged
    {

        public TreeListView PdfFileList { get; set; }
        public List<PdfFile> PdfFiles { get; set; }

        TemplatePage masterPage = new TemplatePage();

        TemplatePage selectedPreviewPage;

        /// <summary>
        /// стрінка, що вибрана у прев'ю
        /// </summary>
        public TemplatePage SelectedPreviewPage
        {
            get => selectedPreviewPage;
            set
            {
                selectedPreviewPage = value;
                NotifyPropertyChanged();
            }
        }

        public TemplatePage MasterPage
        {
            get => masterPage; set
            {
                masterPage = value;
                NotifyPropertyChanged();
            }
        }

        TemplateSheet sheet;

        public TemplateSheet Sheet
        {
            get => sheet;
            set
            {
                sheet = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler NeedRearangePages = delegate{ };
        public event EventHandler NeedCheckRunListPages = delegate { };
        public event EventHandler JustUpdatePreview = delegate { };

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdatePreview()
        {
            JustUpdatePreview(this,null);
        }

        public void UpdateSheet()
        {
            if (sheet != null)
            {
                CropMarksService.FixCropMarksFront(sheet.TemplatePageContainer);
            }
            selectedPreviewPage = null;
            UpdatePreview();
        }

        public void RearangePages()
        {
            NeedRearangePages(this, null);
        }

        public void CheckRunListPages()
        {
            NeedCheckRunListPages(this, null);
        }
    }
}
