using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Interfaces.Plugins;
using Job.Static;

namespace PluginColorJobListRow
{
    public static class Settings
    {
        private static ColorJobListRowSettings _settings;

        private static Dictionary<string, int> _jobs = new Dictionary<string, int>();

        public static ColorJobListRowSettings Get(IUserProfile profile)
        {
            return _settings ?? (_settings = profile.Plugins.LoadSettings<ColorJobListRowSettings>());
        }

        public static void Save(IUserProfile profile)
        {
            if (_settings != null)
            {
                profile.Plugins.SaveSettings(_settings);
            }
        }

        public static void SetJob(string str, int code)
        {
            if (_jobs.ContainsKey(str))
            {
                _jobs[str] = code;
            }
            else
            {
                _jobs.Add(str,code);
            }
        }

        public static IRowColor GetJobColor(string job)
        {
            IRowColor color = null;

            foreach (var i in _jobs)
            {
                    if (job.StartsWith(i.Key,StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (_settings != null)
                        {
                            color = _settings.GetColor(i.Value);
                            break;
                        }
                            
                    }
            }

            return color;
        }
    }
}
