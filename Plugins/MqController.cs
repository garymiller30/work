using Interfaces;
using Interfaces.MQ;
using Interfaces.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins
{
    public class MqController : IMqController
    {
        readonly List<IMqPlugin> _plugins = new List<IMqPlugin>();

        #region [EVENTS]
        public event EventHandler<object> OnHello = delegate { };
        public event EventHandler<object> OnJobAdd = delegate { };
        public event EventHandler<object> OnJobBeginEdit = delegate { };
        public event EventHandler<object> OnJobFinishEdit = delegate { };
        public event EventHandler<object> OnJobChanged = delegate { };
        public event EventHandler<object> OnJobDeleted = delegate { };
        public event EventHandler<object> OnPlateChanged = delegate { };
        public event EventHandler<object> OnPlateAdd = delegate { };
        public event EventHandler<object> OnPlateRemove = delegate { };
        public event EventHandler<object> OnCustomerAdd = delegate { };
        public event EventHandler<object> OnCustomerRemove = delegate { };
        public event EventHandler<object> OnCustomerChanged = delegate { };
        public event EventHandler<object> OnPlateEventAdd = delegate { };
        public event EventHandler<object> OnPlateEventRemove = delegate { };
        public event EventHandler<object> OnPlateEventChange = delegate { };
        public event EventHandler<object> OnExit = delegate { };
        #endregion

        public MqController()
        {

        }
        public void Disconnect()
        {
            _plugins.ForEach(x => x.Disconnect());
        }

        public void PublishChanges(MessageEnum me, object id)
        {
            _plugins.ForEach(x => x.PublishChanges(me, id));
        }

        public void Add(IMqPlugin obj)
        {
            obj.Init(this);
            _plugins.Add(obj);
        }

        public IEnumerable<IPluginBase> GetPluginBase()
        {
            List<IPluginBase> l = new List<IPluginBase>();

            foreach (var plugin in _plugins)
            {
                if (plugin is IPluginBase pluginBase) l.Add(pluginBase);
            }

            return l.ToArray();
        }

        public void RiseEvent(MessageEnum me, object id)
        {
            switch (me)
            {
                case MessageEnum.Hello:
                    OnHello(this, id);
                    break;
                case MessageEnum.JobAdd:
                    OnJobAdd(this, id);
                    break;
                case MessageEnum.JobBeginEdit:
                    OnJobBeginEdit(this, id);
                    break;
                case MessageEnum.JobFinishEdit:
                    OnJobFinishEdit(this, id);
                    break;
                case MessageEnum.JobChanged:
                    OnJobChanged(this, id);
                    break;
                case MessageEnum.PlateAdd:
                    OnPlateAdd(this, id);
                    break;
                case MessageEnum.PlateChange:
                    OnPlateChanged(this, id);
                    break;
                case MessageEnum.PlateRemove:
                    OnPlateRemove(this, id);
                    break;
                case MessageEnum.CustomerAdd:
                    OnCustomerAdd(this, id);
                    break;
                case MessageEnum.CustomerRemove:
                    OnCustomerRemove(this, id);
                    break;
                case MessageEnum.CustomerChange:
                    OnCustomerChanged(this, id);
                    break;
                case MessageEnum.Exit:
                    OnExit(this, id);
                    break;
                case MessageEnum.FormEventAdd:
                    OnPlateEventAdd(this, id);
                    break;
                case MessageEnum.FormEventRemove:
                    OnPlateEventRemove(this, id);
                    break;
                case MessageEnum.FormEventChange:
                    OnPlateEventChange(this, id);
                    break;
                case MessageEnum.JobDelete:
                    OnJobDeleted(this, id);
                    break;
                default:
                    break;

            }
        }
    }
}