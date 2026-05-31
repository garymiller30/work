using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IFormProfile
    {
        bool IsInitialized { get; }
        void SaveLayout();
        void InitProfile();
        void ResetLayout();
    }
}
