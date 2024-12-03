using BrightIdeasSoftware;
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

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
