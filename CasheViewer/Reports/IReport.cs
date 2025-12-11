using System;
using System.Collections.Generic;
using Interfaces;
using JobSpace.Profiles;

namespace CasheViewer.Reports
{
    public interface IReport
    {
        IUserProfile UserProfile { get; set; }
        List<INode> GetNodes();
        DateTime DateMin { get; set; }
        decimal Total { get; set; }
        decimal TotalWithConsumerPrice { get; set; }
    }
}
