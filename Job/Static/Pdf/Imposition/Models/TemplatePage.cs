using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public sealed class TemplatePage
    {
        public CropMarksController CropMarksController { get; set; } = new CropMarksController();

        public int MasterFrontIdx { get; set; } = 1;
        public int MasterBackIdx { get; set; } = 0;
        public int PrintFrontIdx { get; set; } = 0;
        public int PrintBackIdx { get; set; } = 0;
        [JsonIgnore]
        public ImposRunPage AssignedRunPageFront { get;set; }
        [JsonIgnore]
        public ImposRunPage AssignedRunPageBack { get; set; }
        public ClipBox Bleeds { get; set; } = new ClipBox();
        public double W { get; set; } = 210;
        public double H { get; set; } = 297;
        public double X { get; set; } = 10;
        public double Y { get; set; } = 10;
        public double Angle { get; set; } = 0;
        public ClipBox Margins { get; set; } = new ClipBox();
        public double GetClippedW => W + Margins.Left + Margins.Right;
        public double GetClippedH => H + Margins.Top + Margins.Bottom;

        public double GetPageWidthWithBleeds => W + Bleeds.Left + Bleeds.Right;
        public double GetPageHeightWithBleeds => H + Bleeds.Top + Bleeds.Bottom;

        public TemplatePage()
        {

        }

        public TemplatePage(double width, double height)
        {
            W = width;
            H = height;
        }

        public TemplatePage(double width, double height, double bleeds) : this(width, height)
        {
            Bleeds.SetDefault(bleeds);
            Margins.Set(bleeds);
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
            (double xOfs, double yOfs) = FixPositionFront(this);

            double llx = X + xOfs;
            double lly = Y + yOfs;

            if (Angle == 90)
            {
                llx = X + xOfs + GetPageHeightWithBleeds;
            }
            else if (Angle == 180)
            {
                llx = X + xOfs + GetPageWidthWithBleeds;
                lly = Y + yOfs + GetPageHeightWithBleeds;
            }
            else if (Angle == 270)
            {
                lly = Y + yOfs + GetPageWidthWithBleeds;
            }

            return (llx, lly, Angle);
        }

        (double xOfs, double yOfs) FixPositionFront(TemplatePage page)
        {
            double left = page.Bleeds.Left;
            double bottom = page.Bleeds.Bottom;
            double top = page.Bleeds.Top;
            double right = page.Bleeds.Right;

            double xOfs = 0;
            double yOfs = 0;


            switch (page.Angle)
            {
                case 0:
                    if (left > page.Margins.Left) xOfs = -left;
                    if (bottom > page.Margins.Bottom) yOfs = -bottom;
                    break;

                case 90:
                    if (top > page.Margins.Top) xOfs = -top;
                    if (left > page.Margins.Left) yOfs = -left;
                    break;

                case 180:
                    if (right > page.Margins.Right) xOfs = -right;
                    if (top > page.Margins.Top) yOfs = -top;
                    break;

                case 270:
                    if (bottom > page.Margins.Bottom) xOfs = -bottom;
                    if (right > page.Margins.Right) yOfs = -right;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return (xOfs, yOfs);

        }

        (double xOfs, double yOfs) FixPositionBack(TemplatePage page)
        {
            double left = page.Bleeds.Left;
            double bottom = page.Bleeds.Bottom;
            double top = page.Bleeds.Top;
            double right = page.Bleeds.Right;

            double xOfs = 0;
            double yOfs = 0;


            switch (page.Angle)
            {
                case 0:
                    if (left > page.Margins.Left) xOfs = -left;
                    if (bottom > page.Margins.Bottom) yOfs = -bottom;
                    break;

                case 180:
                    if (right > page.Margins.Right) xOfs = -right;
                    if (top > page.Margins.Top) yOfs = -top;
                    break;

                case 90:
                    if (top > page.Margins.Top) xOfs = -top;
                    if (left > page.Margins.Left) yOfs = -left;
                    break;

                case 270:
                    if (bottom > page.Margins.Bottom) xOfs = -bottom;
                    if (right > page.Margins.Right) yOfs = -right;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return (xOfs, yOfs);
        }


        public (double x, double y, double angle) GetPageStartCoordBack(TemplateSheet sheet)
        {
            (double xOfs, double yOfs) = FixPositionBack(this);


            double correctedAngle = Angle;

            double llx = 0;// sheet.W - (X + xOfs);
            double lly = 0;//Y + GetPageHeightWithBleeds + yOfs;

            if (Angle == 0)
            {
                llx = sheet.W - (X + xOfs + GetPageWidthWithBleeds);
                lly = Y + yOfs;
            }

            else if (Angle == 90)
            {
                correctedAngle = 270;
                llx = sheet.W - (X + xOfs + GetPageHeightWithBleeds);
                lly = Y + GetPageWidthWithBleeds + yOfs;
            }
            else if (Angle == 180)
            {
                llx = sheet.W - (X + xOfs);
                lly = Y + yOfs;
            }
            else if (Angle == 270)
            {
                correctedAngle = 90;
                llx = sheet.W - (X + xOfs);
                lly = Y + yOfs;
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

        public void FlipAngle() => Angle = (Angle + 180) % 360;

        public void SetMarginsLikeBleed() => Margins.Set(Bleeds);

        public TemplatePage Copy()
        {
            var str = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<TemplatePage>(str);
        }

        public RectangleD GetDrawBleedLeft()
        {
            switch (Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = X - Bleeds.Left,
                        Y1 = Y,
                        X2 = X,
                        Y2 = Y + GetPageDrawH(),
                    }
                        ;
                case 90:
                    return new RectangleD()
                    {
                        X1 = X,
                        Y1 = Y - Bleeds.Left,
                        X2 = X + GetPageDrawW(),
                        Y2 = Y
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = X + GetPageDrawW(),
                        Y1 = Y,
                        X2 = X + GetPageDrawW() + Bleeds.Left,
                        Y2 = Y + GetPageDrawH(),
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = X,
                        Y1 = Y + GetPageDrawH(),
                        X2 = X + GetPageDrawW(),
                        Y2 = Y + GetPageDrawH() + Bleeds.Left,
                    };
                default:
                    throw new NotImplementedException();
            }
        }
        public RectangleD GetDrawBleedRight()
        {
            switch (Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = X + GetPageDrawW(),
                        Y1 = Y,
                        X2 = X + GetPageDrawW() + Bleeds.Right,
                        Y2 = Y + GetPageDrawH(),
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = X,
                        Y1 = Y + GetPageDrawH(),
                        X2 = X + GetPageDrawW(),
                        Y2 = Y + GetPageDrawH() + Bleeds.Right
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = X - Bleeds.Right,
                        Y1 = Y,
                        X2 = X,
                        Y2 = Y + GetPageDrawH()
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = X,
                        Y1 = Y - Bleeds.Right,
                        X2 = X + GetPageDrawW(),
                        Y2 = Y
                    };
                default:
                    throw new NotImplementedException();
            }
        }
        public RectangleD GetDrawBleedBottom()
        {
            switch (Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = X,
                        Y1 = Y - Bleeds.Bottom,
                        X2 = X + GetPageDrawW(),
                        Y2 = Y
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = X + GetPageDrawW(),
                        Y1 = Y,
                        X2 = X + GetPageDrawW() + Bleeds.Bottom,
                        Y2 = Y + GetPageDrawH()
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = X,
                        Y1 = Y + GetPageDrawH(),
                        X2 = X + GetPageDrawW(),
                        Y2 = Y + GetPageDrawH() + Bleeds.Bottom,
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = X - Bleeds.Bottom,
                        Y1 = Y,
                        X2 = X,
                        Y2 = Y + GetPageDrawH()
                    };
                default:
                    throw new NotImplementedException();
            }
        }
        public RectangleD GetDrawBleedTop()
        {
            switch (Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = X,
                        Y1 = Y + GetPageDrawH(),
                        X2 = X + GetPageDrawW(),
                        Y2 = Y + GetPageDrawH() + Bleeds.Top
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = X - Bleeds.Top,
                        Y1 = Y,
                        X2 = X,
                        Y2 = Y + GetPageDrawH()
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = X,
                        Y1 = Y - Bleeds.Top,
                        X2 = X + GetPageDrawW(),
                        Y2 = Y
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = X + GetPageDrawW(),
                        Y1 = Y,
                        X2 = X + GetPageDrawW() + Bleeds.Top,
                        Y2 = Y + GetPageDrawH()
                    };
                default:
                    throw new NotImplementedException();
            }
        }

        public void RotateRight()
        {
            Angle = (Angle + 270) % 360;
        }

        public void RotateLeft()
        {
            Angle = (Angle + 90) % 360; 
        }

        public void SwitchWH()
        {
            // switch W and H
            double temp = W;
            W = H;
            H = temp;
            Angle = (Angle + 270) % 360;
        }
    }
}
