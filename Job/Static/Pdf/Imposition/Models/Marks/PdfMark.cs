using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public class PdfMark : MarkAbstract
    {

        public PdfMarkParameters Parameters { get; set; } = new PdfMarkParameters();
        public PdfFile File { get; set; }

        public RectangleD ClipBoxFront { get; set; }
        public RectangleD ClipBoxBack { get; set; }

        public PdfMark()
        {
            ClipBoxFront = new RectangleD();
            ClipBoxBack = new RectangleD();
        }

        public PdfMark(PdfFile file)
        {
            File = file;
        }

        public PdfMark(string filePath)
        {
            File = new PdfFile(filePath);
        }

        public override double GetW()
        {
            return File?.Pages[0].Media.W ?? 0;
        }

        public override double GetH()
        {
            return File?.Pages[0].Media.H ?? 0;
        }

        public double GetClippedW()
        {
            double w = GetW();
            if (w == 0) return 0;

            return w - Parameters.ClipBox.Left - Parameters.ClipBox.Right;
        }

        public double GetClippedH()
        {
            double h = GetH();
            if (h == 0) return 0;
            return h - Parameters.ClipBox.Top - Parameters.ClipBox.Bottom;
        }

        public double GetClippedLeftByAngleFront()
        {
            switch (Angle)
            {
                case 0:
                    return Parameters.ClipBox.Left;
                case 90:
                    return Parameters.ClipBox.Top;
                case 180:
                    return Parameters.ClipBox.Right;
                case 270:
                    return Parameters.ClipBox.Bottom;
                default:
                    throw new NotImplementedException();
            }
        }

        public double GetClippedBottomByAngleFront()
        {
            switch (Angle)
            {
                case 0:
                    return Parameters.ClipBox.Bottom;
                case 90:
                    return Parameters.ClipBox.Right;
                case 180:
                    return Parameters.ClipBox.Top;
                case 270:
                    return Parameters.ClipBox.Left;
                default:
                    throw new NotImplementedException();
            }
        }

        public double GetClippedLeftByAngleBack(TemplateSheetPlaceType placeType)
        {
            switch (placeType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    break;
                case TemplateSheetPlaceType.Sheetwise:

                    switch (Angle)
                    {
                        case 0:
                            return Parameters.ClipBox.Right;
                        case 90:
                            return Parameters.ClipBox.Bottom;
                        case 180:
                            return Parameters.ClipBox.Left;
                        case 270:
                            return Parameters.ClipBox.Top;
                        default:
                            throw new NotImplementedException();
                    }

                    
                case TemplateSheetPlaceType.WorkAndTurn:
                    break;
                case TemplateSheetPlaceType.Perfecting:
                    break;
                default:
                    throw new NotImplementedException();
            }
            return 0;
        }

        public double GetClippedBottomByAngleBack(TemplateSheetPlaceType placeType)
        {
            switch (placeType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    break;
                case TemplateSheetPlaceType.Sheetwise:

                    switch (Angle)
                    {
                        case 0:
                            return Parameters.ClipBox.Bottom;
                        case 90:
                            return Parameters.ClipBox.Right;
                        case 180:
                            return Parameters.ClipBox.Top;
                        case 270:
                            return Parameters.ClipBox.Left;
                        default:
                            throw new NotImplementedException();
                    }


                case TemplateSheetPlaceType.WorkAndTurn:
                    break;
                case TemplateSheetPlaceType.Perfecting:
                    break;
                default:
                    throw new NotImplementedException();
            }
            return 0;
        }
    }
}
