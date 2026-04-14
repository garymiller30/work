using Interfaces.Pdf.Imposition;
using JobSpace.Profiles;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections.Generic;
using System.IO;
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

        public IImposResult CreateImpos(string templateName, string filePath)
        {
            var imposResult = new ImposResult();

            string templatePath = Path.Combine( _profile.ImposService.PrintSheetsPath,templateName);

            var sheets= _profile.ImposService.LoadPrintSheets(templatePath);
            var productPart = new ProductPart();
            productPart.ExportParameters.SaveToSourceFileFolder(filePath) ;
            productPart.ExportParameters.SavePrintSheetToOrderFolder = false;
            productPart.PrintSheets = sheets;
            var pdfFile = productPart.AddPdfFile(filePath);
            productPart.RunList.AddFile(pdfFile);
            GlobalImposParameters imposParam = new GlobalImposParameters() { Profile = _profile, ProductPart = productPart };
            var drawer = new PdfDrawer(imposParam);
            //TextVariablesService.SetValue(ValueList.OrderNo, param.Job.Number);
            //TextVariablesService.SetValue(ValueList.Customer, param.Job.Customer);
            //TextVariablesService.SetValue(ValueList.OrderDesc, param.Job.Description);
            drawer.Draw(productPart);
            return imposResult;
        }

        public IProductPart CreateProductPart()
        {
            return new ProductPart();
        }
    }
}
