using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interfaces.Plugins
{
    public interface IPluginNewOrder : IPluginBase
    {
        IUserProfile UserProfile { get; set; }
        Image PluginImage { get; }

        DialogResult ShowDialogNewOrder(IUserProfile profile, IJob job);
    }
}
