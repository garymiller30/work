using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Interfaces;
using System.Text;
using System.Text.Json;

namespace MailNotifier.Shablons
{
    public class MailShablonManager : IMailShablonManager
    {
        private IMail _mail;
        List<MailShablon> _shablons = new List<MailShablon>();
        private const string ShablonFile = "MailTemplates.json";
        private string _shablonsPath;

        public MailShablonManager(IMail mail)
        {
            _mail = mail;
            _shablonsPath = Path.Combine(_mail.Settings.SettingsFolder, ShablonFile);

            _load();
        }

        public IEnumerable<MailShablon> GetShablons()
        {
            return _shablons.ToArray();
        }

        private void _load()
        {
            if (File.Exists(_shablonsPath))
            {
                try
                {

                    string str = File.ReadAllText(_shablonsPath,Encoding.Unicode);
                    _shablons = JsonSerializer.Deserialize<List<MailShablon>>(str);
                }
                catch 
                {
                    _shablons = new List<MailShablon>();

                }
            }
        }

        public MailShablon AddShablon(string name)
        {
            var first = _shablons.FirstOrDefault(x =>
                x.ShablonName.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (first == null)
            {
                var s = new MailShablon {ShablonName = name};
                _shablons.Add(s);

                return s;
            }
            else
            {
                return first;
            }
            
        }

        public void DeleteShablon(MailShablon shablon)
        {
            _shablons.Remove(shablon);
        }

        public void Save()
        {
            string str = JsonSerializer.Serialize(_shablons);
            File.WriteAllText(_shablonsPath, str,Encoding.Unicode);
        }
    }
}
