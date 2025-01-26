using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Sheetwise;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Browsable(false)]
        public CropMarksController CropMarksController { get; set; } = new CropMarksController();
        [Category("Сторона")]
        [DisplayName("Лице")]
        [Description("Параметри лицевої сторони")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PageSide Front { get; set; } = new PageSide();
        [Category("Сторона")]
        [DisplayName("Зворот")]
        [Description("Параметри зворотної сторони")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PageSide Back { get; set; } = new PageSide();
        [Browsable(false)]
        public ClipBox Bleeds { get; set; } = new ClipBox();
        public double W { get; set; } = 210;
        public double H { get; set; } = 297;
        [Browsable(false)]
        public ClipBox Margins { get; set; } = new ClipBox();
        [Browsable(false)]
        public double GetClippedW => W + Margins.Left + Margins.Right;
        [Browsable(false)]
        public double GetClippedH => H + Margins.Top + Margins.Bottom;
        [Browsable(false)]
        public double GetPageWidthWithBleeds => W + Bleeds.Left + Bleeds.Right;
        [Browsable(false)]
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
            Front.X = x;
            Front.Y = y;
            W = width;
            H = height;
            Front.Angle = angle;
        }



        public double GetClippedWByRotate()
        {
            if (Front.Angle == 0 || Front.Angle == 180) return GetClippedW;

            return GetClippedH;
        }

        public double GetClippedHByRotate()
        {

            if (Front.Angle == 0 || Front.Angle == 180) return GetClippedH;

            return GetClippedW;
        }

        public double GetPageDrawFrontX()
        {
            switch (Front.Angle)
            {
                case 0:
                    return Front.X + Margins.Left;
                case 90:
                    return Front.X + Margins.Top;
                case 180:
                    return Front.X + Margins.Bottom;
                case 270:
                    return Front.X + Margins.Right;
                default:
                    throw new NotImplementedException();
            }
        }



        public double GetPageDrawFrontY()
        {

            switch (Front.Angle)
            {
                case 0:
                    break;
                case 90:
                    return Front.Y + Margins.Right;
                case 180:
                    return Front.Y + Margins.Top;
                case 270:
                    return Front.Y + Margins.Left;

            }
            return Front.Y + Margins.Bottom;
        }

        public double GetPageDrawFrontW()
        {
            if (Front.Angle == 0 || Front.Angle == 180) return W;
            return H;
        }

        public double GetPageDrawFrontH()
        {
            if (Front.Angle == 0 || Front.Angle == 180) return H;
            return W;
        }

        public void FlipAngle(TemplateSheetPlaceType sheetPlaceType)
        {
            Front.Angle = (Front.Angle + 180) % 360;

            switch (sheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:

                    break;
                case TemplateSheetPlaceType.Sheetwise:

                    Back.Angle = LooseBindingSheetwise.GetBackAngle(Front.Angle);
                    break;
                case TemplateSheetPlaceType.WorkAndTurn:
                    //TODO: Add logic for WorkAndTurn if needed
                    break;
                case TemplateSheetPlaceType.Perfecting:
                    //TODO: Add logic for Perfecting if needed
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sheetPlaceType), sheetPlaceType, null);
            }
        }

        public void SetMarginsLikeBleed() => Margins.Set(Bleeds);

        public TemplatePage Copy()
        {
            var str = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<TemplatePage>(str);
        }

        public RectangleD GetDrawBleedFrontLeft()
        {
            switch (Front.Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = Front.X - Bleeds.Left,
                        Y1 = Front.Y,
                        X2 = Front.X,
                        Y2 = Front.Y + GetPageDrawFrontH(),
                    }
                        ;
                case 90:
                    return new RectangleD()
                    {
                        X1 = Front.X,
                        Y1 = Front.Y - Bleeds.Left,
                        X2 = Front.X + GetPageDrawFrontW(),
                        Y2 = Front.Y
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = Front.X + GetPageDrawFrontW(),
                        Y1 = Front.Y,
                        X2 = Front.X + GetPageDrawFrontW() + Bleeds.Left,
                        Y2 = Front.Y + GetPageDrawFrontH(),
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = Front.X,
                        Y1 = Front.Y + GetPageDrawFrontH(),
                        X2 = Front.X + GetPageDrawFrontW(),
                        Y2 = Front.Y + GetPageDrawFrontH() + Bleeds.Left,
                    };
                default:
                    throw new NotImplementedException();
            }
        }
        public RectangleD GetDrawBleedFrontRight()
        {
            switch (Front.Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = Front.X + GetPageDrawFrontW(),
                        Y1 = Front.Y,
                        X2 = Front.X + GetPageDrawFrontW() + Bleeds.Right,
                        Y2 = Front.Y + GetPageDrawFrontH(),
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = Front.X,
                        Y1 = Front.Y + GetPageDrawFrontH(),
                        X2 = Front.X + GetPageDrawFrontW(),
                        Y2 = Front.Y + GetPageDrawFrontH() + Bleeds.Right
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = Front.X - Bleeds.Right,
                        Y1 = Front.Y,
                        X2 = Front.X,
                        Y2 = Front.Y + GetPageDrawFrontH()
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = Front.X,
                        Y1 = Front.Y - Bleeds.Right,
                        X2 = Front.X + GetPageDrawFrontW(),
                        Y2 = Front.Y
                    };
                default:
                    throw new NotImplementedException();
            }
        }
        public RectangleD GetDrawBleedFrontBottom()
        {
            switch (Front.Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = Front.X,
                        Y1 = Front.Y - Bleeds.Bottom,
                        X2 = Front.X + GetPageDrawFrontW(),
                        Y2 = Front.Y
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = Front.X + GetPageDrawFrontW(),
                        Y1 = Front.Y,
                        X2 = Front.X + GetPageDrawFrontW() + Bleeds.Bottom,
                        Y2 = Front.Y + GetPageDrawFrontH()
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = Front.X,
                        Y1 = Front.Y + GetPageDrawFrontH(),
                        X2 = Front.X + GetPageDrawFrontW(),
                        Y2 = Front.Y + GetPageDrawFrontH() + Bleeds.Bottom,
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = Front.X - Bleeds.Bottom,
                        Y1 = Front.Y,
                        X2 = Front.X,
                        Y2 = Front.Y + GetPageDrawFrontH()
                    };
                default:
                    throw new NotImplementedException();
            }
        }
        public RectangleD GetDrawBleedFrontTop()
        {
            switch (Front.Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = Front.X,
                        Y1 = Front.Y + GetPageDrawFrontH(),
                        X2 = Front.X + GetPageDrawFrontW(),
                        Y2 = Front.Y + GetPageDrawFrontH() + Bleeds.Top
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = Front.X - Bleeds.Top,
                        Y1 = Front.Y,
                        X2 = Front.X,
                        Y2 = Front.Y + GetPageDrawFrontH()
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = Front.X,
                        Y1 = Front.Y - Bleeds.Top,
                        X2 = Front.X + GetPageDrawFrontW(),
                        Y2 = Front.Y
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = Front.X + GetPageDrawFrontW(),
                        Y1 = Front.Y,
                        X2 = Front.X + GetPageDrawFrontW() + Bleeds.Top,
                        Y2 = Front.Y + GetPageDrawFrontH()
                    };
                default:
                    throw new NotImplementedException();
            }
        }

        public void SwitchWH()
        {
            // switch W and H
            double temp = W;
            W = H;
            H = temp;
            Front.Angle = (Front.Angle + 270) % 360;
        }

        public double GetPageDrawBackX()
        {
            switch (Back.Angle)
            {
                case 0:
                    return Back.X + Margins.Left;
                case 90:
                    return Back.X + Margins.Top;
                case 180:
                    return Back.X + Margins.Bottom;
                case 270:
                    return Back.X + Margins.Right;
                default:
                    throw new NotImplementedException();
            }
        }

        public double GetPageDrawBackY()
        {
            switch (Back.Angle)
            {
                case 0:
                    return Back.Y + Margins.Bottom;
                case 90:
                    return Back.Y + Margins.Right;
                case 180:
                    return Back.Y + Margins.Top;
                case 270:
                    return Back.Y + Margins.Left;
                default:
                    throw new NotImplementedException();
            }

        }

        public double GetPageDrawBackH()
        {
            if (Back.Angle == 0 || Back.Angle == 180) return H;
            return W;
        }

        public double GetPageDrawBackW()
        {
            if (Back.Angle == 0 || Back.Angle == 180) return W;
            return H;
        }
    }
}
