using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Pdf;
using Interfaces.Plugins;
using JobSpace.Models;
using JobSpace.Static.Pdf.SetTrimBox.ByBleed;
using JobSpace.Static.Pdf.SetTrimBox.ByFormat;
using JobSpace.Static.Pdf.SetTrimBox.BySpread;
using JobSpace.UserForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Add
{
    [PdfTool("Додати", "Додати TrimBox до PDF",Icon = "add_trimbox",Description = "додати/змінити обрізне поле (trimbox)", Order = 2)]
    [RequiresFeature(LicenseFeature.ExportPdf)]
    public class PdfAddTrimBox : IPdfTool
    {
        TrimBoxResult? _result;

        public bool Configure(PdfJobContext context)
        {
            if (context.InputFiles.Count == 0)
                return false;

            using (var form = new FormGetTrimBox(context.InputFiles[0]))
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _result = form.Result;
                    return true;
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            if (_result == null) return;

            IPdfTrimTool? tool = _result switch
            {
                { ResultType: TrimBoxResultEnum.byBleed } r =>
                    new PdfSetTrimBoxByBleed(new() { Bleed = r.Bleed }),

                { ResultType: TrimBoxResultEnum.byTrimbox, TrimBox: var t } =>
                    new PdfSetTrimBoxByFormat(new() { Width = t.Width, Height = t.Height }),

                { ResultType: TrimBoxResultEnum.bySpread, Spread: var s } =>
                    new PdfSetTrimBoxBySpread(new() { Top = s.Top, Bottom = s.Bottom, Inside = s.Inside, Outside = s.Outside }),

                _ => null
            };

            if (tool == null) return;

            foreach (var ext in context.InputFiles)
            {
                tool.Run(ext.FileInfo.FullName);
            }
        }
    }
}
