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
    public class PluginTelegramNotify : IPluginInfo
    {

        TelegramBotClient _botClient;
        
        private TelegramNotifySettings _settings = new TelegramNotifySettings();
        public IUserProfile UserProfile { get ; set; }

        IJob _before_job_change;
        int _before_status_code;
        IJob _after_job_change;


        public string PluginName => "Telegram Notify v1.0";

        public string PluginDescription => "сповіщає про зміну статусу замовлення";

        public void AfterJobChange(IJob job)
        {
            _after_job_change = job;

            if(_before_job_change == null || _after_job_change == null) return;

            if (_before_job_change != _after_job_change) return;

            if (_before_status_code != _after_job_change.StatusCode)
            {
                if (_botClient == null) return;

                var res = long.TryParse(_settings.ChatId, out long chatId);

                if (!res) return;

                // Notify about status change
                var status_desc = UserProfile.StatusManager.GetJobStatusDescriptionByCode(_after_job_change.StatusCode);
                var t = _botClient.SendMessage(chatId: chatId, text: $"AW: статус => '{status_desc}' #{job.Number}_{job.Customer}_{job.Description}", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                t.Wait();
            }

        }

        public void BeforeJobChange(IJob job)
        {
            _before_job_change = job;
            _before_status_code = job.StatusCode;
        }

      

        public string GetPluginName()
        {
            return PluginName;
        }

        public System.Windows.Forms.UserControl GetUserControl()
        {
            return null; // No user control provided in this plugin
        }

        public void SetCurJob(IJob curJob)
        {
            
        }

        public void ShowSettingsDlg()
        {
            using (var form = new FormSettings(_settings))
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UserProfile.Plugins.SaveSettings(_settings);
                    Start(); // Restart the plugin with new settings
                }
            }
        }

        public void Start()
        {
            _settings = UserProfile.Plugins.LoadSettings<TelegramNotifySettings>(this);
            if (string.IsNullOrEmpty(_settings.BotId) || string.IsNullOrEmpty(_settings.ChatId)) return;

            // Initialize the Telegram Bot Client with the provided Bot ID
            if (_botClient != null) _botClient.Close().Wait(); // Already initialized
            _botClient = new TelegramBotClient(_settings.BotId);
        }
    }
}
