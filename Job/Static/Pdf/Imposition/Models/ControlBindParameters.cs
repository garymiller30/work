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

        public TemplatePage HoverPage { get;set;}

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

        private int selectedImposRunPageIdx { get; set; }

        public int SelectedImposRunPageIdx
        {
            get => selectedImposRunPageIdx;
            set
            {
                selectedImposRunPageIdx = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler NeedRearangePages = delegate { };
        public event EventHandler NeedCheckRunListPages = delegate { };
        public event EventHandler JustUpdatePreview = delegate { };

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdatePreview()
        {
            JustUpdatePreview(this, null);
        }

        public void UpdateSheet(bool resetHover = true)
        {
            if (sheet != null)
            {
                CropMarksService.FixCropMarks(sheet);
            }
            selectedPreviewPage = null;
            if(resetHover) HoverPage = null;
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

        public void SetSheet(TemplateSheet e)
        {
            selectedPreviewPage = null;
            Sheet = e;
        }
        

    }
}
