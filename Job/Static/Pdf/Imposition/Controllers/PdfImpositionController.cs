using Job.Static.Pdf.Imposition.Common;
using Job.Static.Pdf.Imposition.Product;
using Job.Static.Pdf.Imposition.Scheme;
using Job.Static.Pdf.Imposition.Sheet;
using Job.Static.Pdf.Imposition.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Controllers
{
    public class PdfImpositionController
    {
        
        public static PdfSubject PlaceMax(PdfProductPart part, PdfSheetRun sheet, PdfMasterScheme masterScheme)
        {
            PdfSubject subject = new PdfSubject();

            // дізнатися, який розмір має схема
            PdfFormat schemeFormat = masterScheme.GetFormat(part.MasterPage);

            // отримати оптимальне розташування. Поки що для простих виробів
            PdfPlacementInstruction placementInstruction = GetOptimalSimply(sheet,schemeFormat);

            // створити схеми
            for (int i = 0; i < placementInstruction.Rows; i++)
            {
                for (int j = 0; j < placementInstruction.Columns; j++)
                {
                    PdfScheme scheme = new PdfScheme(masterScheme, part.MasterPage);
                    scheme.RowIdx = i;
                    scheme.ColumnIdx = j;
                    scheme.Rotate = placementInstruction.Rotate;
                    subject.AddScheme(scheme);
                }
            }

            return subject;
        }

        private static PdfPlacementInstruction GetOptimalSimply(PdfSheetRun sheet, PdfFormat schemeFormat)
        {
            PdfPlacementInstruction instructionV1 = new PdfPlacementInstruction(schemeFormat);
            PdfPlacementInstruction instructionV2 = new PdfPlacementInstruction(schemeFormat) {Rotate = 90 };

            if (instructionV1.GetCount(sheet) >= instructionV2.GetCount(sheet))
            {
                return instructionV1;
            }
            return instructionV2;
        }
    }
}
