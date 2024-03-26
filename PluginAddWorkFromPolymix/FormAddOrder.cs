using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Krypton.Toolkit;
using PluginAddWorkFromPolymix.Model;

namespace PluginAddWorkFromPolymix
{
    public partial class FormAddOrder : KryptonForm
    {
        private readonly PluginAddWorkFromPolymixSettings _addWorkFromPolymixSettings;
        private readonly IUserProfile _profile;
        private List<OrderState> _orderStateList = new List<OrderState>();
        
        public List<PolymixOrder> OrderList { get; set; } = new List<PolymixOrder>();

       
        public FormAddOrder()
        {
            InitializeComponent();
            olvColumnName.GroupKeyGetter += rowObject => ((IFilter) rowObject).TypeName;
            olvColumnName.ImageGetter += rowObject => ((IFilter) rowObject).Img;
            olvColumnNumber.ImageGetter += rowObject => _orderStateList.FirstOrDefault(x=>x.Code == ((PolymixOrder)rowObject).OrderState)?.Img;
        }

        private void CreateFilterGroups()
        {
            CreateKindIdFilter();
            CreateStatusesFilter();
        }

        private void CreateStatusesFilter()
        {
            

            _orderStateList = new PolymixController(_addWorkFromPolymixSettings).GetStatuses().ToList();
            if (_orderStateList.Any())
            {

                foreach (var state in _orderStateList)
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

        public FormAddOrder(PluginAddWorkFromPolymixSettings addWorkFromPolymixSettings, IUserProfile profile) : this()
        {
            _addWorkFromPolymixSettings = addWorkFromPolymixSettings;
            _profile = profile;

            CreateFilterGroups();

            ApplyFilter();

            objectListViewFilter.ItemChecked += ObjectListViewFilter_ItemChecked;
        }

        private void ObjectListViewFilter_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ApplyFilter();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            OrderList = objectListViewOrderList.SelectedObjects.Cast<PolymixOrder>().ToList();
            if (OrderList.Any())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Потрібно вибрати хоча б одне замовлення");
            }
        }

     


        private void buttonApplyFilter_Click(object sender, EventArgs e)
        {
            
            ApplyFilter();
            
        }

        private void ApplyFilter()
        {
            Cursor.Current = Cursors.WaitCursor;
            
            objectListViewOrderList.ClearObjects();

            if (objectListViewFilter.CheckedObjects.Count>0)
            {
                var filters = objectListViewFilter.CheckedObjects.Cast<IFilter>().ToArray();
                if (filters.Any())
                {
                    var orders = new PolymixController(_addWorkFromPolymixSettings).GetOrders(filters).ToArray();
                    
                    if (orders.Any())
                    {
                        objectListViewOrderList.AddObjects(orders);
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
                    
                    _profile.Plugins.SaveSettings(_addWorkFromPolymixSettings);
                }
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
