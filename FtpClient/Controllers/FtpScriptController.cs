using System.Collections.Generic;
using System.IO;
using System.Linq;
using Interfaces;

namespace FtpClient.Controllers
{
    public class FtpScriptController : IFtpScriptController
    {
        private const string FtpScriptsFile = "FtpScripts.settings";
        private string _profilePath;
        private List<FtpScript> _ftpScripts = new List<FtpScript>();

        public IFtpScript Create(string path = null)
        {
            if (string.IsNullOrEmpty(path)) return new FtpScript() {Name = "none", Enable = false};

            var script = new FtpScript
            {
                ScriptPath = path,
                Name = Path.GetFileNameWithoutExtension(path)
            };
            return script;
        }

        public void Add(IFtpScript script)
        {
            _ftpScripts.Add((FtpScript)script);
            Save();
        }

        public void Remove(IFtpScript script)
        {
            _ftpScripts.Remove((FtpScript)script);
            Save();
        }

        public List<IFtpScript> All()
        {
            return _ftpScripts.Cast<IFtpScript>().ToList();
        }

        public void Load(string profilePath)
        {
            _profilePath = profilePath;
            var file = Path.Combine(profilePath, FtpScriptsFile);
            if (File.Exists(file))
            {
                _ftpScripts = Commons.DeserializeXML<List<FtpScript>>(file) ?? new List<FtpScript>();
            }
        }

        void Save()
        {
            Commons.SerializeXML(_ftpScripts, Path.Combine(_profilePath, FtpScriptsFile));
        }

        public void SetList(IEnumerable<IFtpScript> list)
        {
            
            _ftpScripts.Clear();
            _ftpScripts.AddRange(list.Cast<FtpScript>());
            Save();
        }
    }
}
