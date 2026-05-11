using System;
using System.Collections;
using Interfaces;
using Interfaces.Profile;

namespace JobSpace.Profiles.ProfileEvents
{
    public sealed class JobEvents : AbstractEvents, IJobEvents
    {
        private bool _isInitialized;

        public EventHandler<IJob> OnSetCurrentJob { get; set; } = delegate { };
        public EventHandler<IJob> OnJobAdd { get; set; } = delegate { };
        public EventHandler<ICollection> OnJobsAdd { get; set; } = delegate { };
        public EventHandler<IJob> OnJobChange { get; set; } = delegate { };
        public EventHandler<IJob> OnJobBeginEdit { get; set; } = delegate { };
        public EventHandler<IJob> OnJobFinishEdit { get; set; } = delegate { };
        public EventHandler<IJob> OnJobDelete { get; set; } = delegate { };
        public EventHandler OnToolsMenuInitialized { get; set; } = delegate { };

        public override void Init(IUserProfile profile)
        {
            if (_isInitialized || profile == null || profile.Jobs == null) return;

            _isInitialized = true;

            profile.Jobs.OnSetCurrentJob += (sender,job) => OnSetCurrentJob(sender,job);
            profile.Jobs.OnJobAdd += (sender,job )=> OnJobAdd(sender,job);
            profile.Jobs.OnJobBeginEdit +=  (sender,job ) => OnJobBeginEdit(sender,job);
            profile.Jobs.OnJobsAdd +=  (sender,jobs ) => OnJobsAdd(sender,jobs);
            profile.Jobs.OnJobChange +=  (sender,job ) => OnJobChange(sender,job);
            profile.Jobs.OnJobFinishEdit +=  (sender,job ) => OnJobFinishEdit(sender,job);
            profile.Jobs.OnDeleteJob +=  (sender,job ) => OnJobDelete(sender,job);

        }

        public void RaiseOnSetCurrentJob(IJob job)
        {
            OnSetCurrentJob(null, job);
        }

        public void RaiseJobsAdd(object sender, ICollection jobs)
        {
            OnJobsAdd(sender, jobs);
        }

        public void RaiseToolsMenuInitialized(object sender)
        {
            OnToolsMenuInitialized(sender, EventArgs.Empty);
        }
    }
}
