using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Pdf.Imposition
{
    public interface IImpositionFactory
    {
        IProductPart CreateProductPart();

        IImposResult CreateImpos(string templateName,string filePath);
    }
}
