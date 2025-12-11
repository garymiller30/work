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
        List<ProductPart> _productParts = new List<ProductPart>();
        int curProductPartIdx = -1;
        int curPrintSheetIdx = -1;

        public ImpositionFactory(Profile userProfile)
        {
            _profile = userProfile;
        }

        public IImpositionFactory AddMasterPage(double w, double h, double bleed)
        {
            if (curProductPartIdx < 0) throw new Exception("Add Product Part first.");
            if (curPrintSheetIdx < 0) throw new Exception("Add Print Sheet first.");
            var s = new TemplatePage() { W = w, H = h, Bleeds = new ClipBox() { Default = bleed } };
            _productParts[curProductPartIdx].PrintSheets[curPrintSheetIdx].MasterPage = s;
            return this;
        }

        public IImpositionFactory AddPrintSheet(double w, double h, TemplateSheetPlaceType sheetPlaceType)
        {
            if (curProductPartIdx < 0) throw new Exception("Add Product Part first.");

            var s = new PrintSheet() { W = w, H = h,SheetPlaceType = sheetPlaceType };
            _productParts[curProductPartIdx].PrintSheets.Add(s);


            return this;
        }

        public IImpositionFactory AddProductPart()
        {
            var pp = new ProductPart();
            _productParts.Add(pp);

            curProductPartIdx++;

            return this;
        }
    }
}
