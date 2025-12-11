using Interfaces;
using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.Static.Pdf.Rearange
{
    public class RearangePagesForQuartalCalendar
    {
        int _cntMonthinBlock;

        int[] _3monthsInBlock = new int[36]
        {   1,2,3,
            2,3,4,
            3,4,5,
            4,5,6,
            5,6,7,
            6,7,8,
            7,8,9,
            8,9,10,
            9,10,11,
            10,11,12,
            11,12,13,
            12,13,14
        };


        public RearangePagesForQuartalCalendar(int cntMonthinBlock = 3)
        {
            _cntMonthinBlock = cntMonthinBlock;
        }

        public void Run(string file)
        {
            string targetfile = Path.Combine(
                Path.GetDirectoryName(file),
                $"{Path.GetFileNameWithoutExtension(file)}_{_cntMonthinBlock}x12{Path.GetExtension(file)}");

            PDFlib p = new PDFlib();
            try
            {
                p.begin_document(targetfile, "optimize=true");
                int indoc = p.open_pdi_document(file, "");
                double pagecount = p.pcos_get_number(indoc, "length:pages");
                if (pagecount != 14) throw new PDFlibException(-1, "RearangePagesForQuartalCalendar", "The document must contain 14 pages.");

                for (int i = 0; i < _3monthsInBlock.Length; i++)
                {
                    p.begin_page_ext(0, 0, "");

                    int pagehdl = p.open_pdi_page(indoc, _3monthsInBlock[i], "cloneboxes");
                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);

                    p.end_page_ext("");
                }

                p.close_pdi_document(indoc);
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                PdfHelper.LogException(e, "RearangePagesForQuartalCalendar");
            }
            finally { p?.Dispose(); }
        }
    }
}
