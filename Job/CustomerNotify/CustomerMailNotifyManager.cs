using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Job.Ext;
using MailNotifier;

namespace Job.CustomerNotify
{
    /// <summary>
    /// уведомление заказчика по почте о смене статуса заказа
    /// </summary>
    public sealed class CustomerMailNotifyManager : ICustomerMailNotifyManager
    {

        //public Profile UserProfile { get; set; }
        private readonly IUserProfile _profile;

        const string CollectionString = "CustomerMailNotify";

        List<CustomerMailNotify> _customerNotifyList = new List<CustomerMailNotify>();

        public CustomerMailNotifyManager(IUserProfile profile)
        {
            _profile = profile;
            Load();
        }

        public void Load()
        {
            if (_profile.Base.IsConnected)
            {
                var result = _profile.Base.GetCollection<CustomerMailNotify>(CollectionString);
                if (result != null && result.Count>0)
                {
                    _customerNotifyList = result;
                }

            }
        }

        /// <summary>
        /// заполнить для всех заказчиков и всех статусов
        /// </summary>


        public List<ICustomerMailNotify> GetByCusomerName(string customer)
        {
            var id = _profile.Customers.FindCustomer(customer);

            if (id != null)
            {

                var q = (from o in _customerNotifyList
                         where string.Equals(o.CustomerId.ToString(), id.Id.ToString()
, System.StringComparison.OrdinalIgnoreCase)
                         select o).Cast<ICustomerMailNotify>().ToList();

                return q;

            }

            return new List<ICustomerMailNotify>();

        }
        /// <summary>
        /// сохранить изменения в базе
        /// </summary>
        public void Save()
        {
            _profile.Base.Update(CollectionString, _customerNotifyList);
        }

        private bool IsNeedNotify(ICustomer customer, int statusCode)
        {
            return GetNotify(customer,statusCode) !=null;
        }

        public bool IsNeedNotify(string customer, int statusCode)
        {
            var cust = _profile.Customers.FindCustomer(customer);
            if (cust!= null)
            {
                return IsNeedNotify(cust, statusCode);
            }
            return false;
        }

        public bool IsNeedNotify(IJob job)
        {
            return IsNeedNotify(job.Customer, job.StatusCode);
        }

        public void SendNotify(IJob job)
        {
            var cust = _profile.Customers.FindCustomer(job.Customer);
            if (cust != null)
            {
                var notify = GetNotify(cust, job.StatusCode);
                if (notify !=null)
                {
                    if (notify.Enabled)
                    {
                        var header = notify.Tema.SendNotifyConvertString(job);
                        var body = notify.Body.SendNotifyConvertString(job);

                        _profile.MailNotifier.OnError += MailNotifier_OnError;

                        _profile.MailNotifier.ShowSendMailDialog(notify.Email, header, body);

                        _profile.MailNotifier.OnError -= MailNotifier_OnError;
                        
                    }
                }
            }
        }

        private void MailNotifier_OnError(object sender, System.Exception e)
        {
            Logger.Log.Error(_profile, "CustomerNotifyManager", e.Message);
        }

        public ICustomerMailNotify Add(ICustomer customer, int code)
        {
            var cmn = new CustomerMailNotify {StatusCode = code, CustomerId = customer.Id};

            _profile.Base.Add(CollectionString, cmn);
            _customerNotifyList.Add(cmn);

            return cmn;


        }

        public void Remove(ICustomerMailNotify cmn)
        {
            _profile.Base.Remove(CollectionString, (CustomerMailNotify)cmn);
        }

        private CustomerMailNotify GetNotify(ICustomer customer, int statusCode)
        {
            return _customerNotifyList.FirstOrDefault(x => string.Equals(x.CustomerId.ToString(), customer.Id.ToString(), System.StringComparison.OrdinalIgnoreCase) && x.StatusCode == statusCode && x.Enabled);
            
        }

        /// <summary>
        /// відправити поштою повідомлення про зміну замовлення
        /// </summary>
        /// <param name="job"></param>
        public void Notify(IJob job)
        {
            if (IsNeedNotify(job))
            {
                SendNotify(job);
            }
        }
    }
}
