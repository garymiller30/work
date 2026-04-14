using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models
{
    public class PdfPreviewParameters
    {
        public bool FitToWindow { get;set; } = true;
        public float ZoomFactor { get; set; } = 1.0f;
        public PdfPreviewDisplay Display { get; set; } = PdfPreviewDisplay.Single;

        public Func<int,List<IScreenPrimitive>> GetScreenPrimitives; 
    }
}
