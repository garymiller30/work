using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Perfecting;
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
        /// <summary>
        /// індекс групи
        /// </summary>
        public int Group { get; set; } = 0;

        public TemplatePage()
        {

        }

        public TemplatePage(double width, double height)
        {
            W = width;
            H = height;
        }

        //public TemplatePage(double width, double height, double bleeds) : this(width, height)
        //{
        //    Bleeds.SetDefault(bleeds);
        //    Margins.Set(bleeds);
        //}

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

        public void FlipAngle(TemplateSheetPlaceType sheetPlaceType)
        {
            Front.Angle = (Front.Angle + 180) % 360;
            
            switch (sheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    break;
                case TemplateSheetPlaceType.Sheetwise:
                case TemplateSheetPlaceType.WorkAndTurn:
                    Back.Angle = LooseBindingSheetwise.GetBackAngle(Front.Angle);
                    break;
                case TemplateSheetPlaceType.WorkAndTumble:
                    Back.Angle = LooseBindingWorkAndTumble.GetBackAngle(Front.Angle);
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
