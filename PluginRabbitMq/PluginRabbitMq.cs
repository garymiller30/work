using EasyNetQ;
using EasyNetQ.SystemMessages;
using EasyNetQ.Topology;
using Interfaces;
using Interfaces.MQ;
using Interfaces.Plugins;
using Logger;
using System;
using System.Linq;

namespace PluginRabbitMq
{
    public class PluginRabbitMq : IMqPlugin, IPluginBase
    {
        private Settings _settings;

        private bool _isOnline;

        private string _channelNumber;
        private IBus _bus;
        private ISubscriptionResult _subscription;
        private IMqController _controller;

        public IUserProfile UserProfile { get; set; }
        public string PluginName => "Rabbit MQ plugin";
        public string PluginDescription => "Синхронізація дій у мережі";

        public void Disconnect()
        {
            try
            {
                _subscription?.Dispose();
                _bus?.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(this, $"({_settings?.RabbitUser}) MQManager", e.Message);
            }
        }

        public void Init(IMqController controller)
        {
            //load settings
            _settings = UserProfile.Plugins.LoadSettings<Settings>(this);
            _controller = controller;

            _channelNumber = $"{Environment.UserName}_{Environment.MachineName}"; // працює лише з однією копією запущеної програми
            Connect();
        }

        public void PublishChanges(MessageEnum me, object id)
        {
            if (!_isOnline) return;

            var message = new MessageMQ { Id = id.ToString(), QueryId = _channelNumber, Code = me };
            try
            {
                _bus.PubSub.Publish(message,default);
            }
            catch (Exception e)
            {
                Log.Error(this, $"({_settings?.RabbitUser}) MQManager", e.Message);
            };
        }


        public void ShowSettingsDlg()
        {
            using (var fs = new FormSettings(_settings))
            {
                if (fs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UserProfile.Plugins.SaveSettings(this, _settings);
                    Disconnect();
                    Connect();
                }
            }
        }

        private void Connect()
        {

            if (!string.IsNullOrEmpty(_settings.RabbitServer) &&
                !string.IsNullOrEmpty(_settings.RabbitUser) &&
                !string.IsNullOrEmpty(_settings.RabbitPassword))
            {
                CreateChanell();
                SubscribeToErrorQuery();
            }
            else
            {
                Log.Error(this, $"({_settings?.RabbitUser}) MQManager", "Server or Login or Password empty");

                _isOnline = false;
            }

            //return UseRabbitMQ;
        }

        private void CreateChanell()
        {
            try
            {
                _bus = RabbitHutch.CreateBus(
                    $"host={_settings.RabbitServer};virtualHost={_settings.RabbitVirtualHost};username={_settings.RabbitUser};password={_settings.RabbitPassword}");
                _subscription = _bus.PubSub.Subscribe<MessageMQ>(_channelNumber, GetMessage,default);
                _isOnline = true;
            }
            catch (Exception e)
            {
                Log.Error(this, $"({_settings?.RabbitUser}) MQManager", e.Message);
                _isOnline = false;
            }
        }
        private void SubscribeToErrorQuery()
        {

            if (!_isOnline) return;

            try
            {
                ITypeNameSerializer typeNameSerializer = new DefaultTypeNameSerializer();
                var conventions = new Conventions(typeNameSerializer);

                var errQueryName = conventions.ErrorQueueNamingConvention(null);

                Action<IMessage<Error>, MessageReceivedInfo> foo = HandleErrorMessage;
                IQueue queue = new Queue(errQueryName, false);
                _bus.Advanced.Consume<Error>(queue, foo);

            }
            catch (Exception e)
            {
                Log.Error(this, $"({_settings?.RabbitUser}) MQManager", e.Message);
            }

        }

        private void HandleErrorMessage(IMessage<Error> msg, MessageReceivedInfo info)
        {
            var error = msg.Body;
            Log.Error(this, $"({_settings?.RabbitUser}) MQManager Error Queue",
                $@"Виключення: {error.Exception}, Повідомлення: {error.Message}, RoutingKey: {error.RoutingKey}");
        }

        private void GetMessage(MessageMQ obj)
        {
            if (!_isOnline) return;
            if (string.Compare(obj.QueryId, _channelNumber, StringComparison.Ordinal) == 0) return;

            _controller.RiseEvent(obj.Code, obj.Id);
            Log.Info(this, $"({_settings?.RabbitUser}) MQManager", $"{obj.Code} : Id = {obj.Id}");
        }
    }
}
