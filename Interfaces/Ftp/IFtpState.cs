using System;

namespace Interfaces.Ftp
{
    public interface IFtpState
    {
        bool IsEventAnyChanges { get; set; }
        //int OnChangeStatus { get; set; }
        void ChangeStatus(object item, bool status);
        void Add(object item, bool state = false);
        object[] GetObjectWithState();
        event EventHandler<bool> OnChangeStatus;
    }
}
