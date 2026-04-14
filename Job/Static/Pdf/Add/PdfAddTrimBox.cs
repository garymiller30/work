using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Models;
using JobSpace.Static.Pdf.SetTrimBox.ByBleed;
using JobSpace.Static.Pdf.SetTrimBox.ByFormat;
using JobSpace.Static.Pdf.SetTrimBox.BySpread;
using JobSpace.UserForms;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Add
{
    [PdfTool("", "Додати TrimBox до PDF",Icon = "add_trimbox",Order = 1)]
    public class PdfAddTrimBox : IPdfTool
    {
        TrimBoxResult _result;

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
            Action<string> action = null;

            switch (_result.ResultType)
            {
                case TrimBoxResultEnum.byBleed:
                    var bleedParam = new PdfSetTrimBoxByBleedParams { Bleed = _result.Bleed };
                    var bleedTool = new PdfSetTrimBoxByBleed(bleedParam);
                    action = file => bleedTool.Run(file);
                    break;

                case TrimBoxResultEnum.byTrimbox:
                    var formatParam = new PdfSetTrimBoxByFormatParams
                    {
                        Width = _result.TrimBox.Width,
                        Height = _result.TrimBox.Height
                    };
                    var formatTool = new PdfSetTrimBoxByFormat(formatParam);
                    action = file => formatTool.Run(file);
                    break;

                case TrimBoxResultEnum.bySpread:
                    var spreadParam = new PdfSetTrimBoxBySpreadParams
                    {
                        Top = _result.Spread.Top,
                        Bottom = _result.Spread.Bottom,
                        Inside = _result.Spread.Inside,
                        Outside = _result.Spread.Outside
                    };
                    var spreadTool = new PdfSetTrimBoxBySpread(spreadParam);
                    action = file => spreadTool.Run(file);
                    break;
            }
            if (action == null)
                return;

            foreach (var ext in context.InputFiles)
            {
                action(ext.FileInfo.FullName);
            }

        }
    }
}
