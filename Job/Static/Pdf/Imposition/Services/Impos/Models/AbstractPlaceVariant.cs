﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services.Impos.Models
{
    public abstract class AbstractPlaceVariant
    {
        public int CntX { get; set; }
        public int CntY { get; set; }

        public bool IsRotated { get; set; }

        public int Total => CntX * CntY;

        protected LooseBindingParameters Parameters { get; set; }

        public double BlockWidth { get; set; }
        public double BlockHeight { get; set; }

        public double FreeSpace => (Parameters.Sheet.W - BlockWidth) * (Parameters.Sheet.H - BlockHeight);

        public AbstractPlaceVariant(LooseBindingParameters bindingParameters)
        {
            Parameters = bindingParameters;
        }

        protected void Calc(double w, double h)
        {
            var _sheet = Parameters.Sheet;

            double sheetW = _sheet.W - _sheet.SafeFields.Left - _sheet.SafeFields.Right;
            double sheetH = _sheet.H - _sheet.SafeFields.Top - _sheet.SafeFields.Bottom;

            if (!Parameters.IsCenterHorizontal) sheetW += Parameters.Xofs;

            if (!Parameters.IsCenterVertical) sheetH += Parameters.Yofs;

            CntX = (int)(sheetW / w);
            CntY = (int)(sheetH / h);

            BlockWidth = CntX * w;
            BlockHeight = CntY * h;
        }

    }
}