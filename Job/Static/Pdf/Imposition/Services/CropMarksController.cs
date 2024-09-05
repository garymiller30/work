using Job.Static.Pdf.Imposition.Models.Marks;
using Job.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services
{
    public class CropMarksController
    {
        public CropMarksParam Parameters { get; set; } = new CropMarksParam();

        public List<CropMark> CropMarks { get; set; } = new List<CropMark>();

        public CropDirection[] GetDrawDirectionFront(double angle)
        {
            Dictionary<double, CropDirection[]> drawDirectionFront = new Dictionary<double, CropDirection[]>
            {
                {000, new CropDirection[]{
                    new CropDirection{X=-1},new CropDirection{Y=-1}, //BL
                    new CropDirection{X=-1},new CropDirection{Y= 1}, //Tl
                    new CropDirection{X= 1},new CropDirection{Y= 1}, //TR
                    new CropDirection{X= 1},new CropDirection{Y=-1}, //BR
                }},
                {090, new CropDirection[]
                {
                    new CropDirection{Y=-1},new CropDirection{X= 1}, //BL
                    new CropDirection{Y=-1},new CropDirection{X=-1}, //Tl
                    new CropDirection{Y= 1},new CropDirection{X=-1}, //TR
                    new CropDirection{Y= 1},new CropDirection{X= 1}, //BR
                }},
                {180, new CropDirection[]{
                    new CropDirection{X= 1},new CropDirection{Y= 1}, //BL
                    new CropDirection{X= 1},new CropDirection{Y=-1}, //Tl
                    new CropDirection{X=-1},new CropDirection{Y=-1}, //TR
                    new CropDirection{X=-1},new CropDirection{Y= 1}, //BR
                }},
                 {270, new CropDirection[]
                {
                    new CropDirection{Y= 1},new CropDirection{X=-1}, //BL
                    new CropDirection{Y= 1},new CropDirection{X= 1}, //Tl
                    new CropDirection{Y=-1},new CropDirection{X= 1}, //TR
                    new CropDirection{Y=-1},new CropDirection{X=-1}, //BR
                }},
            };



            return drawDirectionFront[angle];
        }

        public CropDirection[] GetDrawDirectionBack(double angle)
        {


            Dictionary<double, CropDirection[]> drawDirectionBack = new Dictionary<double, CropDirection[]>
            {
                {000, new CropDirection[]{
                    new CropDirection{X=1},new CropDirection{Y=-1}, //BL
                    new CropDirection{X=1},new CropDirection{Y= 1}, //Tl
                    new CropDirection{X=-1},new CropDirection{Y= 1}, //TR
                    new CropDirection{X=-1},new CropDirection{Y=-1}, //BR
                }},
                {090, new CropDirection[]
                {
                    new CropDirection{Y= 1},new CropDirection{X=-1}, //BL
                    new CropDirection{Y= 1},new CropDirection{X= 1}, //Tl
                    new CropDirection{Y=-1},new CropDirection{X= 1}, //TR
                    new CropDirection{Y=-1},new CropDirection{X=-1}, //BR
                }},
                {180, new CropDirection[]{
                    new CropDirection{X= 1},new CropDirection{Y= 1}, //BL
                    new CropDirection{X= 1},new CropDirection{Y=-1}, //Tl
                    new CropDirection{X=-1},new CropDirection{Y=-1}, //TR
                    new CropDirection{X=-1},new CropDirection{Y= 1}, //BR
                }},
                 {270, new CropDirection[]
                {
                    new CropDirection{Y=-1},new CropDirection{X= 1}, //BL
                    new CropDirection{Y=-1},new CropDirection{X=-1}, //Tl
                    new CropDirection{Y= 1},new CropDirection{X=-1}, //TR
                    new CropDirection{Y= 1},new CropDirection{X= 1}, //BR
                }},
        };

            return drawDirectionBack[angle];
        }



        public AnchorOfset[] GetAnchorOfsetsFront(TemplatePage templatePage, double angle)
        {

            double llx = templatePage.X;
            double lly = templatePage.Y;

            CropMarksController crops = templatePage.CropMarksController;
            double len = crops.Parameters.Len;
            double dist = crops.Parameters.Distance;

            Dictionary<double, AnchorOfset[]> AnchorsFront = new Dictionary<double, AnchorOfset[]>
            {
                {000, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx + templatePage.Clip.Left, Y = lly + templatePage.Clip.Bottom}, //BL
                    new AnchorOfset{Y = templatePage.H},                                                   //TL
                    new AnchorOfset{X = templatePage.W},                                                   //TR
                    new AnchorOfset{Y = -templatePage.H}                                                   //BR
                }},
                {090, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx - templatePage.Clip.Bottom + templatePage.GetClippedH, Y = lly + templatePage.Clip.Left}, //BL
                    new AnchorOfset{X = -templatePage.H},                                                   //TL
                    new AnchorOfset{Y = templatePage.W},                                                   //TR
                    new AnchorOfset{X = templatePage.H}                                                   //BR
                }},
                {180, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx  - templatePage.Clip.Left + templatePage.GetClippedW, Y = lly - templatePage.Clip.Bottom + templatePage.GetClippedH}, //BL
                    new AnchorOfset{Y = -templatePage.H},                                                   //TL
                    new AnchorOfset{X = -templatePage.W},                                                   //TR
                    new AnchorOfset{Y = templatePage.H}                                                   //BR
                }},
                {270, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx + templatePage.Clip.Bottom, Y = lly - templatePage.Clip.Left+ templatePage.GetClippedW}, //BL
                    new AnchorOfset{X = templatePage.H},                                                  //TL
                    new AnchorOfset{Y = -templatePage.W},                                                   //TR
                    new AnchorOfset{X = -templatePage.H}                                                    //BR
                }},
            };

            return AnchorsFront[angle];
        }


        public AnchorOfset[] GetAnchorOfsetsBack(TemplatePage templatePage, TemplateSheet sheet, double angle)
        {

            double llx = templatePage.X;
            double lly = templatePage.Y;

            CropMarksController crops = templatePage.CropMarksController;
            double len = crops.Parameters.Len;
            double dist = crops.Parameters.Distance;


            Dictionary<double, AnchorOfset[]> AnchorsBack = new Dictionary<double, AnchorOfset[]>
            {
                {000, new AnchorOfset[]
                {
                    new AnchorOfset{X = sheet.W - (llx + templatePage.Clip.Left), Y = lly + templatePage.Clip.Bottom}, //BL
                    new AnchorOfset{Y = templatePage.H},                                                   //TL
                    new AnchorOfset{X = -templatePage.W},                                                   //TR
                    new AnchorOfset{Y = -templatePage.H}                                                   //BR
                }},
                {090, new AnchorOfset[]
                {
                    new AnchorOfset{X = sheet.W - llx + templatePage.Clip.Bottom - templatePage.GetClippedH , Y = lly + templatePage.GetClippedW - templatePage.Clip.Left  }, //BL
                    new AnchorOfset{X = templatePage.H},                                                   //TL
                    new AnchorOfset{Y = -templatePage.W},                                                   //TR
                    new AnchorOfset{X = -templatePage.H}                                                   //BR
                }},
                {180, new AnchorOfset[]
                {
                    new AnchorOfset{X = sheet.W - llx  - templatePage.Clip.Left, Y = lly + templatePage.GetClippedH - templatePage.Clip.Bottom}, //BL
                    new AnchorOfset{Y = -templatePage.H},                                                   //TL
                    new AnchorOfset{X = -templatePage.W},                                                   //TR
                    new AnchorOfset{Y = templatePage.H}                                                   //BR
                }},
                {270, new AnchorOfset[]
                {
                    new AnchorOfset{X = sheet.W - llx - templatePage.Clip.Bottom, Y = lly + templatePage.Clip.Left}, //BL
                    new AnchorOfset{X = -templatePage.H},                                                  //TL
                    new AnchorOfset{Y = templatePage.W},                                                   //TR
                    new AnchorOfset{X = templatePage.H}                                                    //BR
                }},
            };

            return AnchorsBack[angle];
        }
    }
}
