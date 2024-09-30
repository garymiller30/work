using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;

namespace JobSpace.Profiles.ProfileEvents
{
    public sealed class JobEvents : AbstractEvents, IJobEvents
    {
        public event EventHandler<IJob> OnSetCurrentJob = delegate { };
        public event EventHandler<IJob> OnJobAdd = delegate { };
        public event EventHandler<ICollection> OnJobsAdd = delegate { };
        public event EventHandler<IJob> OnJobChange = delegate { };
        public event EventHandler<IJob> OnJobBeginEdit = delegate { };
        public event EventHandler<IJob> OnJobFinishEdit = delegate { };
        public event EventHandler<IJob> OnJobDelete = delegate { };

        public override void Init(IUserProfile profile)
        {
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
