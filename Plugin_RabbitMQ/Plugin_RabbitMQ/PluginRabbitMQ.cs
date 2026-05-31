using EasyNetQ;
using Interfaces;
using Interfaces.MQ;
using Interfaces.Plugins;
using Interfaces.Profile;
using System;
using System.Windows.Forms;

namespace Plugin_RabbitMQ
{
    public sealed class PluginRabbitMQ : IMqPlugin, IPluginBase
    {
        public IUserProfile UserProfile { get; set; }

        public string PluginName => "Rabbit MQ plugin";

        public string PluginDescription => "Синхронізація дій у мережі";

        IServiceState serviceState;

        private IBus _bus;
        private IMqController _controller;
        private PluginRabbitMQSettings _settings;
        private string _queueName;

        public void Disconnect()
        {
            _bus?.Dispose();

            ServiceStateOff();
        }

        public void Init(IMqController controller)
        {
            _controller = controller;
            _settings = UserProfile.Plugins.LoadSettings<PluginRabbitMQSettings>(this);

            _queueName = $"{Environment.UserName}_{Environment.MachineName}"; // працює лише з однією копією запущеної програми

            ServiceStateCreate();

            Connect();

        }

        private void Connect()
        {
            if (_settings.isValid())
            {
                _bus = RabbitHutch.CreateBus($"host={_settings.Server};virtualHost={_settings.VirtualHost};username={_settings.User};password={_settings.Password}");
               
                _bus.Advanced.Connected += (s, e) => { ServiceStateOn(); };
                _bus.Advanced.Disconnected += (s, e) => { ServiceStateOff(); };

                _bus.PubSub.Subscribe<MessageMQ.MessageMQ>(_queueName, ProcessMessage);
            }
        }

        private void ProcessMessage(MessageMQ.MessageMQ message)
        {
            if (string.Compare(message.QueryId, _queueName, StringComparison.Ordinal) == 0) return;
            _controller.RiseEvent(message.Code, message.Id);
        }


        public void PublishChanges(MessageEnum me, object id)
        {
            if (_bus == null || !_bus.Advanced.IsConnected) return;

            var message = new MessageMQ.MessageMQ { Id = id.ToString(), QueryId = _queueName, Code = me };
            _bus.PubSub.Publish(message);
        }

        public void ShowSettingsDlg()
        {
            using (var fs = new FormSettings(_settings))
            {
                if (fs.ShowDialog() == DialogResult.OK)
                {
                    UserProfile.Plugins.SaveSettings(this, _settings);
                    Disconnect();
                    Connect();
                }
            }
        }

        void ServiceStateCreate()
        {
            serviceState = UserProfile.ServicesState.Create();
            serviceState.Name = "RabbitMQ";
            serviceState.Tooltip = "Стан підключення до RabbitMQ";
            serviceState.Description = "Стан підключення до RabbitMQ";
        }

        void ServiceStateOff()
        {
            serviceState.Description = "Відключено від RabbitMQ";
            serviceState.State = Interfaces.Enums.ServiceStateEnum.INACTIVE;
            UserProfile.Events.ServiceStateEvents.UpdateServiceState(this, serviceState);
        }

        void ServiceStateOn()
        {
            serviceState.Description = "Підключено до RabbitMQ";
            serviceState.State = Interfaces.Enums.ServiceStateEnum.ACTIVE;
            UserProfile.Events.ServiceStateEvents.UpdateServiceState(this, serviceState);
        }
    }
}
