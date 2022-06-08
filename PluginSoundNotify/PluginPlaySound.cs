using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interfaces;
using Interfaces.Plugins;

namespace PluginSoundNotify
{
    public class PluginPlaySound : IPluginPlaySound
    {
        public IUserProfile UserProfile { get; set; }

        //private readonly WMPLib.WindowsMediaPlayer _wplayer = new WMPLib.WindowsMediaPlayer();
        private readonly SpeechSynthesizer _synthesizer = new SpeechSynthesizer();

        private GlobalPlaySoundSettings _playSoundSettings;

        public string PluginName { get; } = "SoundNotify";
        public string PluginDescription { get; } = "Програє мелодію, або ще щось в залежності від події";
        public void ShowSettingsDlg()
        {
            if (_playSoundSettings == null)
            {
                _playSoundSettings = UserProfile.Plugins.LoadSettings<GlobalPlaySoundSettings>();
            }

            using (var formSettings = new Forms.FormSettings(_playSoundSettings))
            {
                if (formSettings.ShowDialog() == DialogResult.OK)
                {
                    UserProfile.Plugins.SaveSettings(_playSoundSettings);
                }
            }
        }


        public void PlaySound(AvailableSound soundType, object param)
        {
            if (_playSoundSettings == null)
                _playSoundSettings = UserProfile.Plugins.LoadSettings<GlobalPlaySoundSettings>();

            var settings = _playSoundSettings.GetPlaySoundSettings(soundType);

            switch (settings.PlayType)
            {
                case PlayEnum.None:
                    break;
                case PlayEnum.PlayFile:

                    if (!string.IsNullOrEmpty(settings.FileName) && File.Exists(settings.FileName))
                    {
                        Task.Run(() =>
                        {
                            var player = new WMPLib.WindowsMediaPlayer();
                            player.URL = settings.FileName;
                            player.controls.play();
                        }).ConfigureAwait(false);
                    }
                    break;

                case PlayEnum.Speech:

                    if (!string.IsNullOrEmpty(settings.SpeechVoiceName) && !string.IsNullOrEmpty(settings.SpeechText))
                    {
                        var voice = _synthesizer.GetInstalledVoices().FirstOrDefault(x => x.VoiceInfo.Description.Equals(settings.SpeechVoiceName));
                        if (voice != null)
                        {
                            if (!string.IsNullOrEmpty(settings.SpeechText))
                            {
                                _synthesizer.SelectVoice(voice.VoiceInfo.Name);
                                _synthesizer.SpeakAsync(settings.SpeechText);
                            }
                        }
                    }

                    break;

                case PlayEnum.Mail:

                    var emails = settings.Emails;
                    string header = string.Empty;
                    string body = string.Empty;

                    if (param is MailMessage message)
                    {
                        header = $"Лист від {message.From.DisplayName}";
                        body = message.Body;

                    }
                    else if (param is IEnumerable<IFtpFileExt> files)
                    {
                        header = $"Нові файли на FTP";

                        foreach (var file in files)
                        {
                            body +=$"<p>{file.Name}</p>";
                        }

                        
                    }

                    Task.Run(()=>UserProfile.MailNotifier.SendToMany(emails,header,body,new string[0])).ConfigureAwait(false);


                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
