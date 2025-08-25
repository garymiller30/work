using Interfaces.Pdf.Imposition;
using JobSpace.Profiles;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition
{
    public class ImpositionFactory : IImpositionFactory
    {
        Profile _profile;

        public ImpositionFactory(Profile userProfile)
        {
            _profile = userProfile;
        }
        public IProductPart CreateProductPart()
        {
            return new ProductPart();
        }
    }
}
