using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krypton.Toolkit;
using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using JobSpace.Static.Pdf.Common;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public class CropMarksController
    {
        public CropMarksParam Parameters { get; set; } = new CropMarksParam();
        public List<CropMark> CropMarks { get; set; } = new List<CropMark>();

        public CropMarksController()
        {
            
        }

        public CropMarksController(CropMarksController source)
        {
            Parameters = new CropMarksParam(source.Parameters);

            foreach (CropMark mark in source.CropMarks)
            {
                CropMarks.Add(new CropMark(mark));
            }
        }

        static class CropDirectionsDataFront
        {
            // Виносимо масиви структур у static readonly
            public static readonly CropDirection[] Angle0 = [
                new() { X = -1 }, new() { Y = -1 }, // BL
                new() { X = -1 }, new() { Y =  1 }, // TL
                new() { X =  1 }, new() { Y =  1 }, // TR
                new() { X =  1 }, new() { Y = -1 }  // BR
            ];

            public static readonly CropDirection[] Angle90 = [
                new() { Y = -1 }, new() { X =  1 }, // BL
                new() { Y = -1 }, new() { X = -1 }, // TL
                new() { Y =  1 }, new() { X = -1 }, // TR
                new() { Y =  1 }, new() { X =  1 }];  // BR

            public static readonly CropDirection[] Angle180 = [
                new() { X =  1 }, new() { Y =  1 }, // BL
                new() { X =  1 }, new() { Y = -1 }, // TL
                new() { X = -1 }, new() { Y = -1 }, // TR
                new() { X = -1 }, new() { Y =  1 }  // BR
            ];

            public static readonly CropDirection[] Angle270 = [
                new() { Y =  1 }, new() { X = -1 }, // BL
                new() { Y =  1 }, new() { X =  1 }, // TL
                new() { Y = -1 }, new() { X =  1 }, // TR
                new() { Y = -1 }, new() { X = -1 }  // BR
            ];
        }

        static class CropDirectionsDataBack
        {
            public static readonly CropDirection[] Angle0 = [
                new (){X=1}, new (){Y=-1}, //BL
                new (){X=1}, new (){Y= 1}, //Tl
                new (){X=-1},new (){Y= 1}, //TR
                new (){X=-1},new (){Y=-1}, //BR
            ];
            public static readonly CropDirection[] Angle90 = [
                new (){Y= 1},new (){X=-1}, //BL
                new (){Y= 1},new (){X= 1}, //Tl
                new (){Y=-1},new (){X= 1}, //TR
                new (){Y=-1},new (){X=-1}, //BR
                ];
            public static readonly CropDirection[] Angle180 = [
                new (){X= 1},new (){Y= 1}, //BL
                new (){X= 1},new (){Y=-1}, //Tl
                new (){X=-1},new (){Y=-1}, //TR
                new (){X=-1},new (){Y= 1}, //BR
            ];
            public static readonly CropDirection[] Angle270 = [
                new (){Y=-1},new (){X= 1}, //BL
                new (){Y=-1},new (){X=-1}, //Tl
                new (){Y= 1},new (){X=-1}, //TR
                new (){Y= 1},new (){X= 1}, //BR
                
                ];
        }

        static class CropDirectionsDataWorkAndTumbleBack
        {
            public static readonly CropDirection[] Angle0 = [
                new(){X=-1}, new(){Y=-1}, //BL
                    new(){X=-1}, new(){Y= 1}, //Tl
                    new(){X= 1}, new(){Y= 1}, //TR
                    new(){X= 1}, new(){Y=-1}, //BR
                ];
            public static readonly CropDirection[] Angle90 = [
                new (){Y=-1},new (){X= 1}, //BL
                    new (){Y=-1},new (){X=-1}, //Tl
                    new (){Y= 1},new (){X=-1}, //TR
                    new (){Y= 1},new (){X= 1}, //BR
                ];
            public static readonly CropDirection[] Angle180 = [
                 new (){X= 1},new (){Y= 1}, //BL
                    new (){X= 1},new (){Y=-1}, //Tl
                    new (){X=-1},new (){Y=-1}, //TR
                    new (){X=-1},new (){Y= 1}, //BR
                ];
            public static readonly CropDirection[] Angle270 = [
                 new (){Y= 1},new (){X=-1}, //BL
                    new (){Y= 1},new (){X= 1}, //Tl
                    new (){Y=-1},new (){X= 1}, //TR
                    new (){Y=-1},new (){X=-1}, //BR
                ];

        }

        public ReadOnlySpan<CropDirection> GetDrawDirectionFront(double angle)
        {
            int roundedAngle = (int)Math.Round(angle);

            return roundedAngle switch
            {
                Constants.ANGLE_0 => CropDirectionsDataFront.Angle0,
                Constants.ANGLE_90 => CropDirectionsDataFront.Angle90,
                Constants.ANGLE_180 => CropDirectionsDataFront.Angle180,
                Constants.ANGLE_270 => CropDirectionsDataFront.Angle270,
                _ => throw new ArgumentException($"Непідтримуваний кут: {angle}", nameof(angle))
            };
        }
        public ReadOnlySpan<CropDirection> GetDrawDirectionBack(double angle)
        {
            int roundedAngle = (int)Math.Round(angle);

            return roundedAngle switch
            {
                Constants.ANGLE_0 => CropDirectionsDataBack.Angle0,
                Constants.ANGLE_90 => CropDirectionsDataBack.Angle90,
                Constants.ANGLE_180 => CropDirectionsDataBack.Angle180,
                Constants.ANGLE_270 => CropDirectionsDataBack.Angle270,
                _ => throw new ArgumentException($"Непідтримуваний кут: {angle}", nameof(angle))
            };
        }

        public ReadOnlySpan<CropDirection> GetDrawDirectionWorkandTumbleBack(double angle)
        {
            int roundedAngle = (int)Math.Round(angle);

            return roundedAngle switch
            {
                Constants.ANGLE_0 =>CropDirectionsDataWorkAndTumbleBack.Angle0,
                Constants.ANGLE_90 => CropDirectionsDataWorkAndTumbleBack.Angle90,
                Constants.ANGLE_180 => CropDirectionsDataWorkAndTumbleBack.Angle180,
                Constants.ANGLE_270 => CropDirectionsDataWorkAndTumbleBack.Angle270,
                _ => throw new ArgumentException($"Непідтримуваний кут: {angle}", nameof(angle))
            };
        }



        public AnchorOfset[] GetAnchorOfsetsFront(TemplatePage page, double angle)
        {
            PageSide side = page.Front;

            double llx = ScreenDrawCommons.GetPageDrawX(page, side);
            double lly = ScreenDrawCommons.GetPageDrawY(page, side);

            // Округляємо до ближчого цілого, щоб уникнути проблем із точністю double
            int roundedAngle = (int)Math.Round(angle);

            return roundedAngle switch
            {
                Constants.ANGLE_0 => [
            new () { X = llx, Y = lly },
            new () { Y = page.H },
            new () { X = page.W },
            new () { Y = -page.H }
        ],
                Constants.ANGLE_90 => [
            new () { X = llx + page.H, Y = lly },
            new () { X = -page.H },
            new () { Y = page.W },
            new () { X = page.H }
        ],
                Constants.ANGLE_180 => [
            new () { X = llx + page.W, Y = lly + page.H },
            new () { Y = -page.H },
            new () { X = -page.W },
            new () { Y = page.H }
        ],
                Constants.ANGLE_270 => [
            new () { X = llx, Y = lly + page.W },
            new () { X = page.H },
            new () { Y = -page.W },
            new () { X = -page.H }
        ],
                _ => throw new ArgumentException($"Unsupported angle: {angle}", nameof(angle))
            };
        }


        public AnchorOfset[] GetAnchorOfsetsBack(TemplatePage page, TemplateSheet sheet, double angle)
        {

            PageSide side = page.Back;

            double llx = ScreenDrawCommons.GetPageDrawXBack(sheet, page, side);
            double lly = ScreenDrawCommons.GetPageDrawYBack(sheet, page, side);

            // Округляємо до ближчого цілого, щоб уникнути проблем із точністю double
            int roundedAngle = (int)Math.Round(angle);

            return roundedAngle switch
            {
                Constants.ANGLE_0 => [

                    new (){X = llx + page.W, Y = lly},        //BL
                    new (){Y = +page.H},                      //TL
                    new (){X = -page.W},                      //TR
                    new (){Y = -page.H}                       //BR
                ],
                Constants.ANGLE_90 => [

                    new (){X = llx + page.H, Y = lly },       //BL
                    new (){X = -page.H},                      //TL
                    new (){Y = +page.W},                      //TR
                    new (){X = +page.H}                       //BR
                ],
                Constants.ANGLE_180 => [

                    new (){X = llx + page.W, Y = lly+page.H}, //BL
                    new (){Y = -page.H},                      //TL
                    new (){X = -page.W},                      //TR
                    new (){Y = +page.H}                       //BR
                ],
                Constants.ANGLE_270 => [

                    new (){X = llx, Y = lly + page.W},        //BL
                    new (){X = +page.H},                      //TL
                    new (){Y = -page.W},                      //TR
                    new (){X = -page.H}                       //BR
                ],
                _ => throw new ArgumentException($"Unsupported angle: {angle}", nameof(angle))
            };
        }

        public AnchorOfset[] GetAnchorOfsetsWorkandTumbleBack(TemplatePage page, TemplateSheet sheet, double angle)
        {
            PageSide side = page.Back;
            ClipBox margins = page.Margins;

            int roundedAngle = (int)Math.Round(angle);

            return roundedAngle switch
            {
                Constants.ANGLE_0 => [

                    new (){X = side.X + margins.Left , Y = side.Y +margins.Bottom},       //BL
                    new (){Y = page.H},                                                   //TL
                    new (){X = page.W},                                                   //TR
                    new (){Y = -page.H}                                                   //BR
                ],
                Constants.ANGLE_90 => [

                    new (){X = side.X + margins.Top + page.H, Y = side.Y + margins.Left},          //BL
                    new (){X = -page.H},                                                   //TL
                    new (){Y = page.W},                                                   //TR
                    new (){X = page.H}                                                   //BR
                ],
                Constants.ANGLE_180 => [

                    new (){X = side.X + margins.Right + page.W, Y = side.Y + margins.Top + page.H}, //BL
                    new (){Y = -page.H},                                                   //TL
                    new (){X = -page.W},                                                   //TR
                    new (){Y = page.H}                                                   //BR
                ],
                Constants.ANGLE_270 => [

                    new (){X = side.X + margins.Bottom, Y = side.Y + margins.Right + page.W}, //BL
                    new (){X = page.H},                                                  //TL
                    new (){Y = -page.W},                                                   //TR
                    new (){X = -page.H}                                                    //BR
                ],
                _ => throw new ArgumentException($"Unsupported angle: {angle}", nameof(angle))
            };
        }
    }
}
