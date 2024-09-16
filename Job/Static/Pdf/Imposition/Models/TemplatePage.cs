using Job.Static.Pdf.Imposition.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models
{
    public sealed class TemplatePage
    {
        public CropMarksController CropMarksController { get; } = new CropMarksController();
        public int FrontIdx { get; set; } = 1;
        public int BackIdx { get; set; } = 0;

        public double Bleeds { get; set; } = 0;

        public double W { get; set; } = 210;
        public double H { get; set; } = 297;
        public double X { get; set; } = 10;
        public double Y { get; set; } = 10;

        public double Angle { get; set; } = 0;

        public ClipBox Margins { get; set; } = new ClipBox();

        public double GetClippedW => W + Margins.Left + Margins.Right;

        public double GetClippedH => H + Margins.Top + Margins.Bottom;

        public TemplatePage()
        {

        }

        public TemplatePage(double width, double height)
        {
            W = width;
            H = height;
        }

        public TemplatePage(double width, double height, double bleeds):this(width,height)
        {
            Bleeds = bleeds;
        }

        public TemplatePage(double x, double y, double width, double height, double angle)
        {
            X = x;
            Y = y;
            W = width;
            H = height;
            Angle = angle;
        }

        public (double x, double y, double angle) GetPageStartCoordFront()
        {
            double llx = X;
            double lly = Y;

            if (Angle == 90)
            {
                llx = X + GetClippedH;
            }
            else if (Angle == 180)
            {
                llx = X + GetClippedW;
                lly = Y + GetClippedH;
            }
            else if (Angle == 270)
            {
                lly = Y + GetClippedW;
            }

            return (llx, lly, Angle);
        }




        public (double x, double y, double angle) GetPageStartCoordBack(TemplateSheet sheet)
        {
            double llx = X;
            double lly = Y;

            double correctedAngle = Angle;

            llx = sheet.W - X;
            lly = Y + GetClippedW;

            if (Angle == 0)
            {
                llx = sheet.W - (X + GetClippedW);
                lly = Y;
            }

            else if (Angle == 90)
            {
                correctedAngle = 270;
                llx = sheet.W - (X + GetClippedH);
            }
            else if (Angle == 180)
            {
                llx = sheet.W - X;
                lly = Y + GetClippedH;
            }
            else if (Angle == 270)
            {
                correctedAngle = 90;
                lly = Y;
            }

            return (llx, lly, correctedAngle);
        }

        public double GetClippedWByRotate()
        {
            if (Angle == 0 || Angle == 180) return GetClippedW;

            return GetClippedH;
        }

        public double GetClippedHByRotate()
        {

            if (Angle == 0 || Angle == 180) return GetClippedH;

            return GetClippedW;
        }

        public double GetPageDrawX()
        {
            switch (Angle)
            {
                case 0:
                    break;
                case 90:
                    return X + Margins.Top;
                case 180:
                    return X + Margins.Bottom;
                case 270:
                    return X + Margins.Right;
            }

            return X + Margins.Left;

        }

        public double GetPageDrawY()
        {

            switch (Angle)
            {
                case 0:
                    break;
                case 90:
                    return Y + Margins.Right;
                case 180:
                    return Y + Margins.Top;
                case 270:
                    return Y + Margins.Left;

            }
            return Y + Margins.Bottom;
        }

        public double GetPageDrawW()
        {
            if (Angle == 0 || Angle == 180) return W;
            return H;
        }

        public double GetPageDrawH()
        {
            if (Angle == 0 || Angle == 180) return H;
            return W;
        }
    }
}
