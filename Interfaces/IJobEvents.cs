using System;
using System.Collections;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IJobEvents
    {
        event EventHandler<IJob> OnSetCurrentJob;
        event EventHandler<IJob> OnJobAdd;
        event EventHandler<ICollection> OnJobsAdd;
        event EventHandler<IJob> OnJobChange;
        event EventHandler<IJob> OnJobBeginEdit;
        event EventHandler<IJob> OnJobFinishEdit;
        event EventHandler<IJob> OnJobDelete;

        void RiseOnJobChange(IJob job);
        void Init(IUserProfile profile);
    }
}
