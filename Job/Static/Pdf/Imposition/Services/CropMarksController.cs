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

        public CropDirection[] GetDrawDirectionFront(double angle) => angle switch
        {
            000 => [
                new() { X = -1 }, new() { Y = -1 }, // BL
                new() { X = -1 }, new() { Y =  1 }, // TL
                new() { X =  1 }, new() { Y =  1 }, // TR
                new() { X =  1 }, new() { Y = -1 }  // BR
            ],
            090 => [
                new() { Y = -1 }, new() { X =  1 }, // BL
                new() { Y = -1 }, new() { X = -1 }, // TL
                new() { Y =  1 }, new() { X = -1 }, // TR
                new() { Y =  1 }, new() { X =  1 }  // BR
            ],
            180 => [
                new() { X =  1 }, new() { Y =  1 }, // BL
                new() { X =  1 }, new() { Y = -1 }, // TL
                new() { X = -1 }, new() { Y = -1 }, // TR
                new() { X = -1 }, new() { Y =  1 }  // BR
            ],
            270 => [
                new() { Y =  1 }, new() { X = -1 }, // BL
                new() { Y =  1 }, new() { X =  1 }, // TL
                new() { Y = -1 }, new() { X =  1 }, // TR
                new() { Y = -1 }, new() { X = -1 }  // BR
            ],
            _ => throw new ArgumentException($"Непідтримуваний кут: {angle}", nameof(angle))
        };

        public CropDirection[] GetDrawDirectionBack(double angle) => angle switch
        {

            000 => [
                    new (){X=1}, new (){Y=-1}, //BL
                    new (){X=1}, new (){Y= 1}, //Tl
                    new (){X=-1},new (){Y= 1}, //TR
                    new (){X=-1},new (){Y=-1}, //BR
                ],
            090 => [
                    new (){Y= 1},new (){X=-1}, //BL
                    new (){Y= 1},new (){X= 1}, //Tl
                    new (){Y=-1},new (){X= 1}, //TR
                    new (){Y=-1},new (){X=-1}, //BR
            ],
            180 => [
                    new (){X= 1},new (){Y= 1}, //BL
                    new (){X= 1},new (){Y=-1}, //Tl
                    new (){X=-1},new (){Y=-1}, //TR
                    new (){X=-1},new (){Y= 1}, //BR
                ],
            270 => [

                    new (){Y=-1},new (){X= 1}, //BL
                    new (){Y=-1},new (){X=-1}, //Tl
                    new (){Y= 1},new (){X=-1}, //TR
                    new (){Y= 1},new (){X= 1}, //BR
           ],
            _ => throw new ArgumentException($"Непідтримуваний кут: {angle}", nameof(angle))


        };

        public CropDirection[] GetDrawDirectionWorkandTumbleBack(double angle) => angle switch
        {

            0 => [
                    new(){X=-1}, new(){Y=-1}, //BL
                    new(){X=-1}, new(){Y= 1}, //Tl
                    new(){X= 1}, new(){Y= 1}, //TR
                    new(){X= 1}, new(){Y=-1}, //BR
                ],
            090 => [
                    new (){Y=-1},new (){X= 1}, //BL
                    new (){Y=-1},new (){X=-1}, //Tl
                    new (){Y= 1},new (){X=-1}, //TR
                    new (){Y= 1},new (){X= 1}, //BR
            ],
            180 => [
                    new (){X= 1},new (){Y= 1}, //BL
                    new (){X= 1},new (){Y=-1}, //Tl
                    new (){X=-1},new (){Y=-1}, //TR
                    new (){X=-1},new (){Y= 1}, //BR
                ],
            270 => [
           
                    new (){Y= 1},new (){X=-1}, //BL
                    new (){Y= 1},new (){X= 1}, //Tl
                    new (){Y=-1},new (){X= 1}, //TR
                    new (){Y=-1},new (){X=-1}, //BR
           ],
            _ => throw new ArgumentException($"Непідтримуваний кут: {angle}", nameof(angle))
        };





        public AnchorOfset[] GetAnchorOfsetsFront(TemplatePage page, double angle)
        {

            PageSide side = page.Front;

            double llx = ScreenDrawCommons.GetPageDrawX(page, side);
            double lly = ScreenDrawCommons.GetPageDrawY(page, side);

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
