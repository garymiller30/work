using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExtensionMethods;
using Interfaces;
using Interfaces.MQ;

namespace Job.Fasades
{
    public sealed class CustomerManager : ICustomerManager
    {
        //public IUserProfile UserProfile { get; set; }
        private readonly IUserProfile _profile;

        const string CollectionString = "Customers";

        public delegate void EventHandler(Customer form);

        public event EventHandler OnCustomerChange = delegate { };
        public event EventHandler OnCustomerAdd = delegate { };
        public event EventHandler OnCustomerRemove = delegate { };


        List<Customer> _customers = new List<Customer>();
        
        public ICustomer CurrentCustomer { get; set; }


        public CustomerManager(IUserProfile userProfile)
        {
            _profile = userProfile;
            _profile.Events.Jobs.OnSetCurrentJob += Jobs_OnSetCurrentJob;
            Connect(false);
            Load();
        }

        private void Jobs_OnSetCurrentJob(object sender, IJob e)
        {
            if (e == null) { CurrentCustomer = null;return;}

            SetCurrentCustomer(e.Customer);
        }

        public void Connect(bool reconnect)
        {
            CurrentCustomer = Factory.CreateCustomer();

            try
            {
                if (!reconnect && _profile.Plugins != null)
                {
                    //_profile.MQ.OnCustomerAdd += MQ_OnCustomerAdd;
                    //_profile.MQ.OnCustomerChanged += MQ_OnCustomerChanged;
                    //_profile.MQ.OnCustomerRemove += MQ_OnCustomerRemove;
                    _profile.Plugins.MqController.OnCustomerAdd += MQ_OnCustomerAdd;
                    _profile.Plugins.MqController.OnCustomerChanged += MQ_OnCustomerChanged;
                    _profile.Plugins.MqController.OnCustomerRemove += MQ_OnCustomerRemove;

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        void MQ_OnCustomerRemove(object sender, object id)
        {

            var o = _profile.Base.GetById<Customer>(CollectionString, id);
            if (o != null)
            {
                var ff = _customers.FirstOrDefault(x => x.Id.Equals(o.Id));
                if (ff != null)
                {
                    _customers.Remove(ff);

                    OnCustomerRemove(ff);
                }
            }

        }

        void MQ_OnCustomerChanged(object sender, object id)
        {
            var o = _profile.Base.GetById<Customer>(CollectionString, id);
            if (o != null)
            {
                var ff = _customers.FirstOrDefault(x => x.Id.Equals(o.Id));
                if (ff != null)
                {
                    ff.Update(o);

                    OnCustomerChange(ff);
                }
            }
        }

        void MQ_OnCustomerAdd(object sender, object id)
        {
            var o = _profile.Base.GetById<Customer>(CollectionString, id);
            if (o != null)
            {
                //if (o.UseCustomFolder)
                //{
                //    CustomerCustomFolderPath.ForceReload();
                //}
                _customers.Add(o);
                OnCustomerAdd(o);
            }
        }


        private void Load()
        {
            var result = _profile.Base.GetCollection<Customer>(CollectionString);
            if (result != null)
            {
                _customers = result;
            }

            _customers.Sort(Customer.SortByName);
        }

        public void Add(ICustomer customer)
        {
            if (_profile.Base.Add(CollectionString, (Customer)customer))
            {
                _customers.Add((Customer)customer);
                _profile.Plugins.MqController.PublishChanges(MessageEnum.CustomerAdd, customer.Id);
            }
            else
            {
                JobLogger.WriteLine($"Не вдалося додати замовника {customer.Name} до бази");
            }
        }

        public void Remove(ICustomer customer)
        {
            if (_profile.Base.Remove(CollectionString, (Customer)customer))
            {
                _customers.Remove((Customer)customer);
                _profile.Plugins.MqController.PublishChanges(MessageEnum.CustomerRemove, customer.Id);
            }
            else
            {
                JobLogger.WriteLine(@"Не удалось удалить заказчика из базы");
            }
        }

        public void Refresh(ICustomer customer)
        {
            try
            {
                _profile.Base.Update(CollectionString, (Customer)customer);
                _profile.Plugins.MqController.PublishChanges(MessageEnum.CustomerChange, customer.Id);
            }
            catch
            {
            }
        }

        public string GetCustomerWorkFolder(ICustomer cust)
        {
            return Path.Combine(_profile.Jobs.Settings.WorkPath, cust.Name.Transliteration());
        }

        public IEnumerator<ICustomer> GetEnumerator()
        {
            return _customers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_customers).GetEnumerator();
        }

        public ICustomer Add()
        {
            var c = Factory.CreateCustomer();
            Add((Customer)c);
            return (Customer)c;
        }

        public ICustomer FindCustomer(string p)
        {
            var f = _customers.FirstOrDefault(x => x.Name.Equals(p));

            return f ?? Factory.CreateCustomer();


        }

        private void SetCurrentCustomer(string customer)
        {
            CurrentCustomer = FindCustomer(customer);
        }

        /// <summary>
        /// получить заказчиков у которых есть фтп
        /// </summary>
        /// <returns></returns>
        public List<ICustomer> GetCustomersWithFtp()
        {
            var result = _customers.FindAll(x => x.Show && x.IsFtpEnable).Cast<ICustomer>().ToList();
            return result;
        }

        /// <summary>
        /// перевірити, чи існує такий замовник в базі. Якщо ні, то запропонувати додати і створити
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool CheckCustomerPresent(string text)
        {
            var customer = _profile.Customers.FindCustomer(text);

            if (string.IsNullOrEmpty(customer.Name))
            {
                if (MessageBox.Show("Такого замовника не існує. Додати до бази?", text,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    customer.Name = text;

                    string customerDir;

                    if (string.IsNullOrEmpty(_profile.Jobs.Settings.WorkPath))
                    {
                        using (var fd = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog())
                        {
                            if (fd.ShowDialog() == DialogResult.OK)
                            {
                                try
                                {
                                    customerDir = Path.Combine(fd.SelectedPath, text);


                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show(e.Message, Localize.FormAddWork_CheckCustomerPresent_Не_вдалося_створити_папку, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                    return false;
                                }
                                //customer.CustomFolder = fd.SelectedPath;
                            }
                            else
                            {
                                return false;
                            }

                        }
                    }
                    else
                    {
                        customerDir = Path.Combine(_profile.Jobs.Settings.WorkPath, text);
                    }

                    Directory.CreateDirectory(customerDir);
                    _profile.Customers.Add(customer);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
