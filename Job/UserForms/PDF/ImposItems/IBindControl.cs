using JobSpace.Static.Pdf.Imposition.Models;
using System.Collections.Generic;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public interface IBindControl
    {
        void Calc();
        void RearangePages(List<PrintSheet> sheets, List<ImposRunPage> pages);
        void SetControlBindParameters(ControlBindParameters parameters);
    }
}