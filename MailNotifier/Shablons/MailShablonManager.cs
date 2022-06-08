using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Interfaces;

namespace MailNotifier.Shablons
{
    public class MailShablonManager
    {
        private IMail _mail;
        List<MailShablon> _shablons = new List<MailShablon>();
        private const string ShablonFile = "Shablons.mail";
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
                    

                    using (FileStream fs = new FileStream(_shablonsPath, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        _shablons = (List<MailShablon>)formatter.Deserialize(fs);

                    }


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
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(_shablonsPath, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, _shablons);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, $@"save file {_shablonsPath}");
            }
        }
    }
}
