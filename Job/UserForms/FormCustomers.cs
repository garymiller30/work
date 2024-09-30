using System;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Krypton.Toolkit;
using Interfaces;
using JobSpace.ModelView;
using JobSpace.Fasades;
using System.Diagnostics;

namespace JobSpace.UserForms
{
    public sealed partial class FormCustomers : KryptonForm
    {
        private IUserProfile UserProfile { get; set; }

        public FormCustomers(IUserProfile _profile)
        {
            InitializeComponent();
            UserProfile = _profile;

            InitLVFtpServers();
            objectListView_Customers.AddObjects(UserProfile.Customers.ToList());

            InitCategories();
        }

        private void InitCategories()
        {
            var categories = UserProfile.Categories.GetAll().Select(x=> new CategoryToCustomerView((Category)x));
            objectListView_Categories.AddObjects(categories.ToArray());
        }

        private void InitLVFtpServers()
        {
            objectListView_FtpServers.Objects = UserProfile.Ftp.GetCollection();
            objectListView_FtpServers.CheckStateGetter += FtpCheckStateGetter;
        }

        private CheckState FtpCheckStateGetter(object r)
        {
            if (objectListView_FtpServers.Objects == null) return CheckState.Indeterminate;
            if (objectListView_Customers.SelectedObject == null) return CheckState.Indeterminate;

            if (((Customer)objectListView_Customers.SelectedObject).FtpServers.Contains(((FtpSettings)r).Name, StringComparer.OrdinalIgnoreCase))
            {
                return CheckState.Checked;
            }

            return CheckState.Unchecked;

        }



        private void objectListView_Customers_CellEditFinishing(object sender, CellEditEventArgs e)
        {

            if (e.Column == olvColumn_Customer)
            {
                if (!e.NewValue.ToString().Equals(e.Value.ToString()))
                {
                    ((Customer) e.RowObject).Name = e.NewValue.ToString();

                    UserProfile.Customers.Refresh((Customer) e.RowObject);
                }
            }
        }

        private void добавиьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objectListView_Customers.AddObject(UserProfile.Customers.Add());
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView_Customers.SelectedObject is Customer c)
            {
                objectListView_Customers.RemoveObject(c);
                UserProfile.Customers.Remove(c);
            }
        }

        private void objectListView_Customers_CellEditStarting(object sender, CellEditEventArgs e)
        {
            //Debug.WriteLine("begin edit");
        }
        /// <summary>
        /// сменился заказчик
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objectListView_Customers_Click(object sender, EventArgs e)
        {
            if (objectListView_Customers.SelectedObject is Customer customer)
            {
                 kryptonGroupBoxParameters.Enabled = true;

                objectListView_FtpServers.RefreshObjects(UserProfile.Ftp.GetCollection().ToArray());
                
                kryptonCheckBoxUseFtp.Checked = customer.IsFtpEnable;
                SetFtpParam(customer);
                objectListView_Categories.ItemChecked -= objectListView_Categories_ItemChecked;
                SetCategoryToCustomer(customer);
                objectListView_Categories.ItemChecked += objectListView_Categories_ItemChecked;
            }
            else
            {
                kryptonGroupBoxParameters.Enabled = false;
            }
        }

        private void SetCategoryToCustomer(Customer customer)
        {
           

            objectListView_Categories.UncheckAll();

            var categories = CategoryToCustomerAsignManager.GetCustomerCategories(UserProfile,customer.Id);
            
            if (!categories.Any()) return;

            var catIds = categories.Select(c => c.Id);
            
            var catToCus = objectListView_Categories.Objects.Cast<CategoryToCustomerView>().Where(x=> catIds.Contains(x.Id)).ToArray();

            objectListView_Categories.CheckObjects(catToCus);

            

        }

        private void SetFtpParam(Customer customer)
        {
            foreach (FtpSettings settings in objectListView_FtpServers.Objects)
            {
                if (customer.FtpServers.Contains(settings.Name))
                {
                    objectListView_FtpServers.CheckObject(settings);
                }
                else
                {
                    objectListView_FtpServers.UncheckObject(settings);
                }
            }
        }

      
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (objectListView_Customers.SelectedObject is Customer customer)
            {
                customer.IsFtpEnable = kryptonCheckBoxUseFtp.Checked;
                kryptonGroupBoxFTP.Enabled = kryptonCheckBoxUseFtp.Checked;
                UserProfile.Customers.Refresh(customer);
            }
        }

        private void objectListView_FtpServers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
            var customer = objectListView_Customers.SelectedObject as Customer;
            if (customer != null)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    customer.FtpServers.Add(UserProfile.Ftp[e.Index].Name);
                }
                else
                {
                    customer.FtpServers.Remove(UserProfile.Ftp[e.Index].Name);
                }
                UserProfile.Customers.Refresh(customer);
            }
        }

        private void buttonNotify_Click(object sender, EventArgs e)
        {

            var customer = (Customer)objectListView_Customers.SelectedObject;

            if (customer != null)
            {
                using (var fcn = new FormMailInformerSettings(customer,UserProfile))
                {
                    
                    fcn.ShowDialog();
                }
            }
          
        }

        private void objectListView_Customers_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var customer = (Customer)((OLVListItem)e.Item).RowObject;

            if (customer != null)
                UserProfile.Customers.Refresh(customer);
        }
 

        private void textBoxCustomerFilter_TextChanged(object sender, EventArgs e)
        {
            if (kryptonTextBoxCustomerFilter.Text.Length > 1)
            {
                objectListView_Customers.ModelFilter = TextMatchFilter.Contains(objectListView_Customers, kryptonTextBoxCustomerFilter.Text);
            }
            else if (string.IsNullOrEmpty(kryptonTextBoxCustomerFilter.Text))
            {
                objectListView_Customers.ModelFilter = null;
            }
        }

        private void objectListView_Categories_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var category = (CategoryToCustomerView)((OLVListItem)e.Item).RowObject;
            // add or change in database

            var customer = (Customer)objectListView_Customers.SelectedObject;

            CategoryToCustomerAsignManager.SetCategory(UserProfile,customer.Id,category.Id,e.Item.Checked);
        }
    }
}
