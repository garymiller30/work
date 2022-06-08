using System.Collections.Generic;
using Interfaces;
using Job.Profiles;

namespace CasheViewer.Reports
{
    public interface IReport
    {
        IUserProfile UserProfile { get; set; }
        List<INode> GetNodes();

        decimal Total { get; set; }
    }
}
