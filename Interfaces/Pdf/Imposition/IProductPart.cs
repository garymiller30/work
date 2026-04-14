using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Pdf.Imposition
{
    public interface IProductPart
    {
        IPrintSheet CreatePrintSheet(double w, double h);
    }
}
