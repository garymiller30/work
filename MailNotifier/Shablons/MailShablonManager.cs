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
        List<MailTemplate> _templates = new List<MailTemplate>();
        private const string TEMPLATE_FILE = "MailTemplates.json";
        private string _templatePath;

        public MailShablonManager(IMail mail)
        {
            _mail = mail;
            _templatePath = Path.Combine(_mail.Settings.SettingsFolder, TEMPLATE_FILE);

            _load();
        }

        public IEnumerable<MailTemplate> GetTemplates()
        {
            return _templates.ToArray();
        }

        private void _load()
        {
            if (File.Exists(_templatePath))
            {
                try
                {

                    string str = File.ReadAllText(_templatePath,Encoding.Unicode);
                    _templates = JsonSerializer.Deserialize<List<MailTemplate>>(str);
                }
                catch 
                {
                    _templates = new List<MailTemplate>();

                }
            }
        }

        public MailTemplate AddShablon(string name)
        {
            var first = _templates.FirstOrDefault(x =>
                x.ShablonName.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (first == null)
            {
                var s = new MailTemplate {ShablonName = name};
                _templates.Add(s);

                return s;
            }
            else
            {
                return first;
            }
            
        }

        public void DeleteShablon(MailTemplate shablon)
        {
            _templates.Remove(shablon);
        }

        public void SetMailTemplates(IEnumerable<MailTemplate> enumerable)
        {
            var list = enumerable.ToList();
            _templates = list;
            Save();
        }

        public void Save()
        {
            string str = JsonSerializer.Serialize(_templates);
            File.WriteAllText(_templatePath, str,Encoding.Unicode);
        }
    }
}
