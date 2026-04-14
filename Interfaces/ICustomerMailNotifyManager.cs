using System.Collections.Generic;

namespace Interfaces
{
    public interface ICustomerMailNotifyManager
    {
        void Notify(IJob job);
        List<ICustomerMailNotify> GetByCusomerName(string customer);
        void Save();
        ICustomerMailNotify Add(ICustomer customer, int code);
        void Remove(ICustomerMailNotify cmn);
    }
}
