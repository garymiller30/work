using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using ComponentFactory.Krypton.Toolkit;
using ExtensionMethods;
using PluginAddWorkFromPolymix.Model;

namespace PluginAddWorkFromPolymix
{
    public partial class FormAddOrder : KryptonForm
    {
        private readonly PluginAddWorkFromPolymixSettings _addWorkFromPolymixSettings;
        private readonly IUserProfile _profile;
        private readonly IJob _job;

       
        public FormAddOrder()
        {
            InitializeComponent();
            olvColumnName.GroupKeyGetter += rowObject => ((IFilter) rowObject).TypeName;
            olvColumnName.ImageGetter += rowObject => ((IFilter) rowObject).Img;
        }

        private void CreateFilterGroups()
        {
            CreateKindIdFilter();
            CreateStatusesFilter();
        }

        private void CreateStatusesFilter()
        {
            var statusesList = new PolymixController(_addWorkFromPolymixSettings).GetStatuses();
            if (statusesList.Any())
            {
                foreach (var state in statusesList)
                {
                    var filter = new StatusFilter()
                    {
                        Code = state.Code,
                        Name = state.Name,
                        Img = state.Img
                    };
                    objectListViewFilter.AddObject(filter);
                    if (_addWorkFromPolymixSettings.StatusFilter!= null)
                    {
                       
                        if (_addWorkFromPolymixSettings.StatusFilter.Contains(filter.Code))
                        {
                            objectListViewFilter.CheckObject(filter);
                        }
                    }
                }

            }
        }

        private void CreateKindIdFilter()
        {
            var orderKindList = new PolymixController(_addWorkFromPolymixSettings).GetKindId();
            if (orderKindList.Any())
            {
                foreach (var kindOrder in orderKindList)
                {
                    var filter = new KindIdFilter()
                    {
                        Name = kindOrder.KindDesc,
                        KindID = kindOrder.KindID
                    };
                    objectListViewFilter.AddObject(filter);

                    if (_addWorkFromPolymixSettings.KindFilter != null)
                    {
                        if (_addWorkFromPolymixSettings.KindFilter.Contains(filter.KindID))
                        {
                            objectListViewFilter.CheckObject(filter);
                        }
                    }

                }
            }
        }

        public FormAddOrder(PluginAddWorkFromPolymixSettings addWorkFromPolymixSettings, IUserProfile profile, IJob job) : this()
        {
            _addWorkFromPolymixSettings = addWorkFromPolymixSettings;
            _profile = profile;
            _job = job;

            CreateFilterGroups();
            
            var customers = _profile.Customers.Where(x => x.Show).ToList();
            comboBoxCustomer.DataSource = customers;
            comboBoxCustomer.DisplayMember = "Name";

            ApplyFilter();

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (_profile.Customers.CheckCustomerPresent(comboBoxCustomer.Text))
            {

                UnBind();
                CheckCustomerName();

                Close();
                DialogResult = DialogResult.OK;
                return;
            }

            DialogResult = DialogResult.Cancel;
        }

        private void UnBind()
        {

            _job.Number = textBoxNumber.Text;
            _job.Customer =  comboBoxCustomer.Text;
            _job.Description = ucTexBoxDescripion.Text;
            _job.Note = ucNote1.GetRtf();
        }

        private void CheckCustomerName()
        {
            var polyMixCustomer = ((PolymixOrder)comboBoxPolymix.SelectedItem).Customer;
            var jobCustomer = comboBoxCustomer.Text;

            if (_addWorkFromPolymixSettings.CusomerNames.ContainsKey(polyMixCustomer))
            {
                // update
                _addWorkFromPolymixSettings.CusomerNames[polyMixCustomer] = jobCustomer;
            }
            else
            {
                _addWorkFromPolymixSettings.CusomerNames.Add(polyMixCustomer,jobCustomer);
            }

            
        }

        private void comboBoxPolymix_SelectedIndexChanged(object sender, EventArgs e)
        {
            var order = (PolymixOrder) comboBoxPolymix.SelectedItem;
            textBoxNumber.Text = order.Number.ToString("D5");
            ucTexBoxDescripion.Text = order.Description.Split(',').First().Transliteration();

            ucNote1.SetText(order.Description);

            if (_addWorkFromPolymixSettings.CusomerNames.ContainsKey(order.Customer))
            {
                var customer = _addWorkFromPolymixSettings.CusomerNames[order.Customer];

                var search = comboBoxCustomer.Items.Cast<ICustomer>().FirstOrDefault(x => x.Name.Equals(customer));
                if (search != null)
                {
                    comboBoxCustomer.SelectedItem = search;
                }
            }
        }

        private void buttonApplyFilter_Click(object sender, EventArgs e)
        {
            
            ApplyFilter();
            
        }

        private void ApplyFilter()
        {
            Cursor.Current = Cursors.WaitCursor;

            comboBoxPolymix.Items.Clear();

            if (objectListViewFilter.CheckedObjects.Count>0)
            {
                var filters = objectListViewFilter.CheckedObjects.Cast<IFilter>().ToArray();
                if (filters.Any())
                {
                    var orders = new PolymixController(_addWorkFromPolymixSettings).GetOrders(filters).ToArray();
                    //var orders = await Task.Factory.StartNew(()=>new PolymixController(_addWorkFromPolymixSettings).GetOrders(filters).ToArray());
                    if (orders.Any())
                    {
                        comboBoxPolymix.Items.AddRange(orders);
                    }
                    
                    _addWorkFromPolymixSettings.StatusFilter.Clear();
                    _addWorkFromPolymixSettings.KindFilter.Clear();

                    foreach (var filter in filters)
                    {
                        if (filter is KindIdFilter kind)
                            _addWorkFromPolymixSettings.KindFilter.Add(kind.KindID);
                        else if (filter is StatusFilter status)
                            _addWorkFromPolymixSettings.StatusFilter.Add(status.Code);
                    }

                    //_addWorkFromPolymixSettings.Filters.AddRange(filters);
                    
                    _profile.Plugins.SaveSettings(_addWorkFromPolymixSettings);
                }


            }

            Cursor.Current = Cursors.Default;
            
           
        }
    }
}
