using System.Collections.Generic;

namespace Interfaces
{
    public interface ICustomerManager : IEnumerable<ICustomer>
    {
        //IUserProfile UserProfile { get; set; }
        ICustomer CurrentCustomer { get; set; }
        ICustomer FindCustomer(string p);
        //void SetCurrentCustomer(string customer);
        bool CheckCustomerPresent(string text,bool addSilence);
        void Connect(bool reconnect);
        //void Load();
        ICustomer Add();
        void Add(ICustomer customer);
        void Remove(ICustomer customer);
        List<ICustomer> GetCustomersWithFtp();
        //void AddCustomerPlateFormat(ICustomer customer, IPlateFormat plateFormat);
        void Refresh(ICustomer customer);
        string GetCustomerWorkFolder(ICustomer cust);
        //bool RemoveCustomerPlate(ICustomer customer, IPlateFormat pf);
    }
}
