using Job.Static.Pdf.Imposition.Models;
using Job.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services
{
    public class CropMarkCreator
    {
        double _baseX;
        double _baseY;

        double xFrom;
        double yFrom;

        public CropMarkCreator(double baseX, double baseY)
        {
            _baseX = baseX;
            _baseY = baseY;
        }

        public CropMarkCreator From(double xOfs, double yOfs)
        {
            xFrom = _baseX + xOfs;
            yFrom = _baseY + yOfs;
            return this;
        }

        public CropMark To(double xOfs, double yOfs)
        {
            return new CropMark
            {
                From = new PointD { X = xFrom, Y = yFrom },
                To = new PointD { X = xFrom + xOfs, Y = yFrom + yOfs }
            };
        }
    }
}
