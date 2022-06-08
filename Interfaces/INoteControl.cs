using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface INoteControl
    {
        void SetText(string text);
        string GetText();
        string GetRtf();
    }
}
