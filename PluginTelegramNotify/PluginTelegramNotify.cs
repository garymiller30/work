using Interfaces;
using Interfaces.MQ;
using Interfaces.Plugins;
using JobSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PluginTelegramNotify
{
    public class PluginTelegramNotify : IMqPlugin, IPluginBase
    {

        TelegramBotClient _botClient;
        private IMqController _controller;
        private TelegramNotifySettings _settings = new TelegramNotifySettings();
        public IUserProfile UserProfile { get ; set; }

        public string PluginName => "Telegram Notify v1.0";

        public string PluginDescription => "сповіщає про зміну статусу замовлення";

        public void Disconnect()
        {
            
        }

        public void Init(IMqController controller)
        {
            _controller = controller;
            _settings = UserProfile.Plugins.LoadSettings<TelegramNotifySettings>(this);

            if (string.IsNullOrEmpty(_settings.BotId) || string.IsNullOrEmpty(_settings.ChatId)) return;

            _botClient = new TelegramBotClient(_settings.BotId);

        }

        public void PublishChanges(MessageEnum me, object id)
        {
            if (_botClient == null) return;

            var res = long.TryParse(_settings.ChatId, out long chatId);

            if (!res) return;

            var job = UserProfile.Base.GetById<Job>("Jobs",id);
            if (job == null) return;


            switch (me)
            {
                case MessageEnum.JobAdd:
                   var task = _botClient.SendMessage(chatId: chatId, text: $"{job.Number}_{job.Customer}_{job.Description}", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html); 
                    task.Wait();
                    break;
                case MessageEnum.JobChanged:
                    var t =_botClient.SendMessage(chatId: chatId, text: $"зміни в роботі #{job.Number}_{job.Customer}_{job.Description}", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                    t.Wait();
                    break;
                default:
                    break;
            }
        }

        public void ShowSettingsDlg()
        {
            
        }
    }
}
