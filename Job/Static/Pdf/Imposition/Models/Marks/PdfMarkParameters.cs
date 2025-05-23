﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public class PdfMarkParameters 
    {
        public double Xofs { get; set; } = 0;
        public double Yofs { get; set; } = 0;

        public AnchorPoint MarkAnchorPoint { get; set; } = AnchorPoint.Center;
        public AnchorPoint ParentAnchorPoint { get; set; } = AnchorPoint.Center;
        public ClipBox ClipBox { get; set; } = new ClipBox();

        public bool IsAutoClipX { get; set; } = false;
        public bool IsAutoClipY { get; set; } = false;

        public AutoClipMarkEnum AutoClipRelativeX { get; set; } = AutoClipMarkEnum.Sheet;
        public AutoClipMarkEnum AutoClipRelativeY { get; set; } = AutoClipMarkEnum.Sheet;

        public MarkSide Front { get; set; } = new MarkSide();
        public MarkSide Back { get; set; } = new MarkSide();

        public bool IsBackMirrored { get; set; } = true;

        public PdfMarkParameters()
        {
            ClipBox = new ClipBox();
            Front = new MarkSide();
            Back = new MarkSide();
        }
    }
}
