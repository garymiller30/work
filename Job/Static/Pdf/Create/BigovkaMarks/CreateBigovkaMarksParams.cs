using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Create.BigovkaMarks
{
    public class CreateBigovkaMarksParams
    {
        public DirectionEnum Direction { get;set;} = DirectionEnum.Horizontal;
        public double DistanceFromTrim { get;set;} = 1;
        public double Length { get;set;} = 1;
        public double[] Bigovki { get;set;}
        public double Bleed { get;set;} = 2;
        public MarkColor Color { get;set;} = new MarkColor();

        public bool MirrorEven { get;set;} = true;
    }
}
