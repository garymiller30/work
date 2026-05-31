using Interfaces.Profile;
using System;
using System.Collections;

namespace Interfaces
{
    public interface IJobEvents
    {
        EventHandler<IJob> OnSetCurrentJob { get; set; }
        EventHandler<IJob> OnJobAdd { get; set; }
        EventHandler<ICollection> OnJobsAdd { get; set; }
        EventHandler<IJob> OnJobChange { get; set; }
        EventHandler<IJob> OnJobBeginEdit { get; set; }
        EventHandler<IJob> OnJobFinishEdit { get; set; }
        EventHandler<IJob> OnJobDelete { get; set; }
        EventHandler OnToolsMenuInitialized { get; set; }

        void RaiseOnSetCurrentJob(IJob job);
        void RaiseJobsAdd(object sender, ICollection jobs);
        void RaiseToolsMenuInitialized(object sender);
        void Init(IUserProfile profile);
    }
}
