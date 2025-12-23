using JobSpace.Static.Pdf.Imposition;
using JobSpace.Static.Pdf.Imposition.Models;
using System.Collections.Generic;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public interface IBindControl
    {
        void Calc();
        void CheckRunListPages(List<PrintSheet> printSheets, List<ImposRunPage> imposRunPages);
        void RearangePages(List<PrintSheet> sheets, List<ImposRunPage> pages);
        void SetControlBindParameters(GlobalImposParameters imposParam);
    }
}