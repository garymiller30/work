using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ISearchManager
    {
        Dictionary<IJobStatus, bool> GetStatuses();
        void ChangeStatus(IJobStatus s, bool @checked);
        void RefreshStatuses();
        void Search(string customer, string text);
        void ClearFilters();
    }
}
