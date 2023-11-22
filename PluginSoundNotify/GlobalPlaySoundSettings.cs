using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Plugins;

namespace PluginSoundNotify
{
    public sealed class  GlobalPlaySoundSettings
    {
        public Dictionary<AvailableSound, PlaySoundSettings> _settinges { get; set; } = new Dictionary<AvailableSound, PlaySoundSettings>();

        public PlaySoundSettings GetPlaySoundSettings(AvailableSound sound)
        {
            if (_settinges.ContainsKey(sound))
                return _settinges[sound];

            var newSet = new PlaySoundSettings();
            _settinges.Add(sound,newSet);

            return newSet;
        }


    }
}
