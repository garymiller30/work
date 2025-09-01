using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Pdf.Imposition
{
    public interface IImpositionFactory
    {
        IImpositionFactory AddProductPart();

        IImpositionFactory AddPrintSheet(double w, double h);

        IImpositionFactory AddMasterPage(double w,double h, double bleed);
    }
}
