using JobSpace.Profiles;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition
{
    public sealed class GlobalImposParameters
    {
        public ImposInputParam ImposInput { get; set; } = new ImposInputParam();
        public ImposToolsParameters ImposTools { get; set; } = new ImposToolsParameters();
        public ControlBindParameters ControlsBind { get; set; } 
        public ProductPart ProductPart { get; set; } = new ProductPart();
        public Profile Profile { get; set; }

        public GlobalImposParameters()
        {
            ControlsBind = new ControlBindParameters(this);
        }
    }
}
