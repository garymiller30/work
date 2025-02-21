using MongoDB.Bson.IO;
using Org.BouncyCastle.Crypto;
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
                case TemplateSheetPlaceType.WorkAndTumble:
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
                case TemplateSheetPlaceType.WorkAndTumble:
                    break;
                default:
                    throw new NotImplementedException();
            }
            return 0;
        }

        public bool GetMarkSideFront(TemplateSheetPlaceType placeType)
        {
            switch (placeType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    return Parameters.Front.SingleSide;
                case TemplateSheetPlaceType.Sheetwise:
                    return Parameters.Front.Sheetwise;
                case TemplateSheetPlaceType.WorkAndTurn:
                    return Parameters.Front.WorkAndTurn;
                case TemplateSheetPlaceType.WorkAndTumble:
                    return Parameters.Front.Perfecting;
                default:
                    throw new NotImplementedException();
            }
        }

        public bool GetMarkSideBack(TemplateSheetPlaceType placeType)
        {
            switch (placeType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    return Parameters.Back.SingleSide;
                case TemplateSheetPlaceType.Sheetwise:
                    return Parameters.Back.Sheetwise;
                case TemplateSheetPlaceType.WorkAndTurn:
                    return Parameters.Back.WorkAndTurn;
                case TemplateSheetPlaceType.WorkAndTumble:
                    return Parameters.Back.Perfecting;
                default:
                    throw new NotImplementedException();
            }
        }

        public double GetBackAngle(TemplateSheetPlaceType placeType)
        {
            switch (placeType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    return Angle;
                case TemplateSheetPlaceType.Sheetwise:
                case TemplateSheetPlaceType.WorkAndTurn:
                    if (Angle == 0 || Angle == 180) return Angle;
                    else if (Angle == 90) return 270;
                    else return 90;

                case TemplateSheetPlaceType.WorkAndTumble:

                    if (Angle == 0) return 180;
                    else if (Angle == 90 || Angle == 270) return Angle;
                    else return 0;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
