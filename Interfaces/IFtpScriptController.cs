using System.Collections.Generic;

namespace Interfaces
{
    public interface IFtpScriptController
    {
        List<IFtpScript> All();
        void Load(string profilePath);
        IFtpScript Create(string path);
        void Add(IFtpScript script);
        void SetList(IEnumerable<IFtpScript> list);
    }
}
