using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;

namespace JobSpace.Profiles.ProfileEvents
{
    public sealed class JobEvents : AbstractEvents, IJobEvents
    {
        public EventHandler<IJob> OnSetCurrentJob { get;set;}  = delegate { };
        public EventHandler<IJob> OnJobAdd { get;set;} = delegate { };
        public EventHandler<ICollection> OnJobsAdd { get;set;} = delegate { };
        public EventHandler<IJob> OnJobChange { get; set; } = delegate { };
        public EventHandler<IJob> OnJobBeginEdit { get; set; } = delegate { };
        public EventHandler<IJob> OnJobFinishEdit { get; set; } = delegate { };
        public EventHandler<IJob> OnJobDelete { get; set; } = delegate { };
        public EventHandler OnToolsMenuInitialized { get ; set ; } = delegate { };

        public override void Init(IUserProfile profile)
        {
            if (profile == null || profile.Jobs == null) return; 

            profile.Jobs.OnSetCurrentJob += (sender,job) => OnSetCurrentJob(sender,job);
            profile.Jobs.OnJobAdd += (sender,job )=> OnJobAdd(sender,job);
            profile.Jobs.OnJobBeginEdit +=  (sender,job ) => OnJobBeginEdit(sender,job);
            profile.Jobs.OnJobsAdd +=  (sender,jobs ) => OnJobsAdd(sender,jobs);
            profile.Jobs.OnJobChange +=  (sender,job ) => OnJobChange(sender,job);
            profile.Jobs.OnJobFinishEdit +=  (sender,job ) => OnJobFinishEdit(sender,job);
            profile.Jobs.OnDeleteJob +=  (sender,job ) => OnJobDelete(sender,job);

        }

        public void RiseOnJobChange(IJob job)
        {
            OnSetCurrentJob(null,job);
        }
    }
}
