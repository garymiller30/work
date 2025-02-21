using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krypton.Toolkit;
using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;

namespace JobSpace.Static.Pdf.Imposition.Services
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

        public CropDirection[] GetDrawDirectionWorkandTumbleBack(double angle)
        {
            Dictionary<double, CropDirection[]> drawDirectionBack = new Dictionary<double, CropDirection[]>
            {
                {0, new CropDirection[]{
                    new CropDirection{X=-1},new CropDirection{Y=-1}, //BL
                    new CropDirection{X=-1},new CropDirection{Y= 1}, //Tl
                    new CropDirection{X=1},new CropDirection{Y= 1}, //TR
                    new CropDirection{X=1},new CropDirection{Y=-1}, //BR
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

            return drawDirectionBack[angle];
        }



        public AnchorOfset[] GetAnchorOfsetsFront(TemplatePage page, double angle)
        {

            PageSide side = page.Front;

            double llx = ScreenDrawCommons.GetPageDrawX(page,side);// page.Front.X;
            double lly = ScreenDrawCommons.GetPageDrawY(page,side);// page.Front.Y;

            CropMarksController crops = page.CropMarksController;
            double len = crops.Parameters.Len;
            double dist = crops.Parameters.Distance;

            Dictionary<double, AnchorOfset[]> AnchorsFront = new Dictionary<double, AnchorOfset[]>
            {
                {000, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx, Y = lly}, //BL
                    new AnchorOfset{Y = page.H},                                                   //TL
                    new AnchorOfset{X = page.W},                                                   //TR
                    new AnchorOfset{Y = -page.H}                                                   //BR
                }},
                {090, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx + page.H, Y = lly}, //BL
                    new AnchorOfset{X = -page.H},                                                   //TL
                    new AnchorOfset{Y = +page.W},                                                   //TR
                    new AnchorOfset{X = +page.H}                                                   //BR
                }},
                {180, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx + page.W, Y = lly + page.H}, //BL
                    new AnchorOfset{Y = -page.H},                                                   //TL
                    new AnchorOfset{X = -page.W},                                                   //TR
                    new AnchorOfset{Y = +page.H}                                                   //BR
                }},
                {270, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx, Y = lly + page.W}, //BL
                    new AnchorOfset{X = +page.H},                                                  //TL
                    new AnchorOfset{Y = -page.W},                                                   //TR
                    new AnchorOfset{X = -page.H}                                                    //BR
                }},
            };

            return AnchorsFront[angle];
        }


        public AnchorOfset[] GetAnchorOfsetsBack(TemplatePage page, TemplateSheet sheet, double angle)
        {

            PageSide side = page.Back;

            double llx = ScreenDrawCommons.GetPageDrawXBack(sheet, page, side);
            double lly = ScreenDrawCommons.GetPageDrawYBack(sheet, page, side);

            CropMarksController crops = page.CropMarksController;
            double len = crops.Parameters.Len;
            double dist = crops.Parameters.Distance;


            Dictionary<double, AnchorOfset[]> AnchorsBack = new Dictionary<double, AnchorOfset[]>
            {
                {000, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx + page.W, Y = lly},        //BL
                    new AnchorOfset{Y = +page.H},                      //TL
                    new AnchorOfset{X = -page.W},                      //TR
                    new AnchorOfset{Y = -page.H}                       //BR
                }},
                {090, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx + page.H, Y = lly },       //BL
                    new AnchorOfset{X = -page.H},                      //TL
                    new AnchorOfset{Y = +page.W},                      //TR
                    new AnchorOfset{X = +page.H}                       //BR
                }},
                {180, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx + page.W, Y = lly+page.H}, //BL
                    new AnchorOfset{Y = -page.H},                      //TL
                    new AnchorOfset{X = -page.W},                      //TR
                    new AnchorOfset{Y = +page.H}                       //BR
                }},
                {270, new AnchorOfset[]
                {
                    new AnchorOfset{X = llx, Y = lly + page.W},        //BL
                    new AnchorOfset{X = +page.H},                      //TL
                    new AnchorOfset{Y = -page.W},                      //TR
                    new AnchorOfset{X = -page.H}                       //BR
                }},
            };

            return AnchorsBack[angle];
        }

        public AnchorOfset[] GetAnchorOfsetsWorkandTumbleBack(TemplatePage page, TemplateSheet sheet, double angle)
        {
            PageSide side = page.Back;
            ClipBox margins = page.Margins;

            CropMarksController crops = page.CropMarksController;
            double len = crops.Parameters.Len;
            double dist = crops.Parameters.Distance;


            Dictionary<double, AnchorOfset[]> AnchorsBack = new Dictionary<double, AnchorOfset[]>
            {
                {000, new AnchorOfset[]
                {
                    new AnchorOfset{X = side.X + margins.Left , Y = side.Y +margins.Bottom},       //BL
                    new AnchorOfset{Y = page.H},                                                   //TL
                    new AnchorOfset{X = page.W},                                                   //TR
                    new AnchorOfset{Y = -page.H}                                                   //BR
                }},
                {090, new AnchorOfset[]
                {
                    new AnchorOfset{X = side.X + margins.Top + page.H, Y = side.Y + margins.Left},          //BL
                    new AnchorOfset{X = -page.H},                                                   //TL
                    new AnchorOfset{Y = page.W},                                                   //TR
                    new AnchorOfset{X = page.H}                                                   //BR
                }},
                {180, new AnchorOfset[]
                {
                    new AnchorOfset{X = side.X + margins.Right + page.W, Y = side.Y + margins.Top + page.H}, //BL
                    new AnchorOfset{Y = -page.H},                                                   //TL
                    new AnchorOfset{X = -page.W},                                                   //TR
                    new AnchorOfset{Y = page.H}                                                   //BR
                }},
                {270, new AnchorOfset[]
                {
                    new AnchorOfset{X = side.X + margins.Bottom, Y = side.Y + margins.Right + page.W}, //BL
                    new AnchorOfset{X = page.H},                                                  //TL
                    new AnchorOfset{Y = -page.W},                                                   //TR
                    new AnchorOfset{X = -page.H}                                                    //BR
                }},
            };

            return AnchorsBack[angle];
        }
    }
}
