using System;
using System.Collections;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IJobEvents
    {
        EventHandler<IJob> OnSetCurrentJob {get;set; }
        EventHandler<IJob> OnJobAdd { get;set; }
        EventHandler<ICollection> OnJobsAdd {get;set; }
        EventHandler<IJob> OnJobChange { get;set; }
        EventHandler<IJob> OnJobBeginEdit { get; set; }
        EventHandler<IJob> OnJobFinishEdit { get; set; }
        EventHandler<IJob> OnJobDelete { get; set; }
        EventHandler OnToolsMenuInitialized { get; set; }

        void RiseOnJobChange(IJob job);
        void Init(IUserProfile profile);
    }
}
