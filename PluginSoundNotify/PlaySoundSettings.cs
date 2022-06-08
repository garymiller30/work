using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSoundNotify
{
    public class PlaySoundSettings
    {
        
        public PlayEnum PlayType { get; set; } = PlayEnum.None;
        public string FileName { get; set; } = String.Empty;
        public string SpeechText { get; set; } = String.Empty;
        public string SpeechVoiceName { get; set; } = String.Empty;
        public string Emails { get; set; } = String.Empty;
    }
}
