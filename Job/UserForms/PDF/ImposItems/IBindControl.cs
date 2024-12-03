using JobSpace.Static.Pdf.Imposition.Models;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public interface IBindControl
    {
        void SetControlBindParameters(ControlBindParameters parameters);
    }
}