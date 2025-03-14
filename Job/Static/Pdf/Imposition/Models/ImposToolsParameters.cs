using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.UserForms.PDF.ImposItems;
using System;
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

        public int CurGroup { get; set; } = 0;

        public CropMarksParam CropMarksParameters { get; set; } = new CropMarksParam();

        public EventHandler<int> FrontNumChanged { get;set;} = delegate { };
        public EventHandler<int> BackNumChanged { get; set; } = delegate { };
        public EventHandler OnTheSameNumberClick { get; set; } = delegate { };
        public EventHandler OnListNumberClick { get; set; } = delegate { };
        public EventHandler OnClickCenterH { get; set; } = delegate { };
        public EventHandler OnClickCenterV { get; set; } = delegate { };
        public EventHandler OnCropMarksChanged { get; set; } = delegate { };
        public EventHandler<double> OnMoveLeftClick { get; set; } = delegate { };
        public EventHandler<double> OnMoveRightClick { get; set; } = delegate { };
        public EventHandler<double> OnMoveUpClick { get; set; } = delegate { };
        public EventHandler<double> OnMoveDownClick { get; set; } = delegate { };
        public EventHandler OnRotateLeft { get; set; } = delegate { };
        public EventHandler OnRotateRight { get; set; } = delegate { };
        public EventHandler OnSwitchWH { get; set; } = delegate { };
        public EventHandler<TemplatePage> OnAddPageToGroup { get; set; } = delegate { };
        public EventHandler<TemplatePage> OnFlipAngle { get; set; } = delegate { };
        public EventHandler<TemplatePage> OnFlipRowAngle { get; set; } = delegate { };
        public EventHandler<List<PageGroup>> OnPageGroupDistributeHor { get;set; } = delegate { };
    }
}
