using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models
{
    public class ImposToolsParameters
    {
        int _frontNum=1;
        int _backNum;

        public bool IsFlipAngle { get; set; }
        public bool IsNumering { get; set; }
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
    }
}
