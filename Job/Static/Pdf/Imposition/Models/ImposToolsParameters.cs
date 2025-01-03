﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class ImposToolsParameters
    {
        int _frontNum=1;
        int _backNum;

        public ImposToolEnum CurTool {  get; set; } = ImposToolEnum.Select;

        //public bool IsFlipAngle { get; set; }
        //public bool IsNumering { get; set; }
        public int FrontNum
        {
            get => _frontNum;
            set
            {
                _frontNum = value;
                FrontNumChanged(this, _frontNum);
            }
        }
        public int BackNum
        {
            get => _backNum;
            set
            {
                _backNum = value;
                BackNumChanged(this, _backNum);
            }
        }

        public EventHandler<int> FrontNumChanged = delegate { };
        public EventHandler<int> BackNumChanged = delegate { };
        public EventHandler OnTheSameNumberClick = delegate { };
        public EventHandler OnListNumberClick = delegate { };
        public EventHandler OnClickCenterH = delegate { };
        public EventHandler OnClickCenterV = delegate { };
    }
}
