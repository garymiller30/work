using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class MasterPageSelectControl : UserControl
    {
        List<PdfFile> _files;
        public EventHandler<PageFormatView> OnMasterPageChanged { get; set; } = delegate { };
        public EventHandler<PageFormatView> OnMasterPageAdded { get; set; } = delegate { };
        public MasterPageSelectControl()
        {
            InitializeComponent();
        }

        public void SetFormats(List<PdfFile> pdfFiles)
        {
            List<PageFormatView> formats = new List<PageFormatView>();

            foreach (PdfFile pdfFile in pdfFiles)
            {
                formats.Add(new PageFormatView(pdfFile));
            }

            cb_FileFormats.DataSource = formats;
        }

        private void cb_FileFormats_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_FileFormats.SelectedItem is PageFormatView page)
            {
                nud_page_w.Value = page.Width;
                nud_page_h.Value = page.Height;
                nud_page_bleed.Value = page.Bleed;

                OnMasterPageChanged(this, page);
            }
        }

        private void nud_page_w_ValueChanged(object sender, EventArgs e)
        {
            if (cb_FileFormats.SelectedItem is PageFormatView page)
            {
                page.Width = ((NumericUpDown)sender).Value;

                OnMasterPageChanged(this, page);
            }
        }

        private void nud_page_h_ValueChanged(object sender, EventArgs e)
        {
            if (cb_FileFormats.SelectedItem is PageFormatView page)
            {
                page.Height = ((NumericUpDown)sender).Value;

                OnMasterPageChanged(this, page);
            }

        }

        private void nud_page_bleed_ValueChanged(object sender, EventArgs e)
        {
            if (cb_FileFormats.SelectedItem is PageFormatView page)
            {
                page.Bleed = ((NumericUpDown)sender).Value;

                OnMasterPageChanged(this, page);
            }
        }

        private void nud_page_w_Click(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void btn_add_page_Click(object sender, EventArgs e)
        {

            if (cb_FileFormats.SelectedItem is PageFormatView page)
            {
                OnMasterPageAdded(this, page);
            }
        }
    }
}
