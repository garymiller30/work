using Interfaces.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Plugins
{
    public interface IMqController
    {
        event EventHandler<object> OnHello;
        event EventHandler<object> OnJobAdd;
        event EventHandler<object> OnJobBeginEdit;
        event EventHandler<object> OnJobFinishEdit;
        event EventHandler<object> OnJobChanged;
        event EventHandler<object> OnPlateChanged;
        event EventHandler<object> OnPlateAdd;
        event EventHandler<object> OnPlateRemove;
        event EventHandler<object> OnCustomerAdd;
        event EventHandler<object> OnCustomerRemove;
        event EventHandler<object> OnCustomerChanged;
        event EventHandler<object> OnPlateEventAdd;
        event EventHandler<object> OnPlateEventRemove;
        event EventHandler<object> OnPlateEventChange;
        event EventHandler<object> OnExit;

        void PublishChanges(MessageEnum me, object id);

        void Disconnect();

        //bool IsOnline();
        void Add(IMqPlugin obj);

        IEnumerable<IPluginBase> GetPluginBase();

        void RiseEvent(MessageEnum me, object id);
    }
}
