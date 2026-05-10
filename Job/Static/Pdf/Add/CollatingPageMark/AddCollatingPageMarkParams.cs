using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Add.CollatingPageMark
{
    public class AddCollatingPageMarkParams
    {
        public PageCollatingMarkPositionEnum Position { get;set;} = PageCollatingMarkPositionEnum.LEFT;
        public double X { get;set;} = 0;
         public double Y { get;set;} = 10;
        public double MarkWidth { get;set;} = 4;
       public double MarkHeight { get;set;} = 1;
       public double PathLen { get;set;} = 100;
    }
}
