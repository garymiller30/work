using System;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using ComponentFactory.Krypton.Toolkit;
using Interfaces;

namespace Job.UserForms
{
    public partial class FormCustomers : KryptonForm
    {
        private IUserProfile UserProfile { get; set; }

        public FormCustomers(IUserProfile _profile)
        {
            InitializeComponent();
            UserProfile = _profile;

            InitLVFtpServers();
            objectListView_Customers.AddObjects(UserProfile.Customers.ToList());

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

            if (((Customer)objectListView_Customers.SelectedObject).FtpServers.Contains(((FtpSettings)r).Name))
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

            }
            else
            {
                kryptonGroupBoxParameters.Enabled = false;
            }
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
        /// <summary>
        /// добавить заказчику формат пластин по умолчанию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
  

/*
        /// <summary>
        /// змінити володаря пластин
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objectListView_PlatesDefault_CellEditStarting(object sender, CellEditEventArgs e)
        {
            if (e.Column == olvColumnOwnerPlate)
            {
                var cb = new ComboBox
                {
                    Bounds = e.CellBounds,
                    Font = ((ObjectListView)sender).Font,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    DisplayMember = "Name"
                };
                cb.Items.AddRange(UserProfile.PlateOwners.GetCollection().ToArray());

                var sel = ((PlateFormat) e.RowObject).OwnerId;

                cb.SelectedItem = cb.Items.Cast<PlateOwner>().FirstOrDefault(x => x.Id == sel);

                e.Control = cb;

            }
        }
*/

/*
        private void objectListView_PlatesDefault_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.Column == olvColumnOwnerPlate)
            {
                if (e.Control is ComboBox box)
                {
                    if (box.SelectedItem != null)
                    {
                        var customer = objectListView_Customers.SelectedObject as Job.Customer;
                        if (customer != null)
                        {
                            ((PlateFormat)e.RowObject).OwnerId = ((PlateOwner)box.SelectedItem).Id;
                            UserProfile.Customers.Refresh(customer);

                            objectListView_PlatesDefault.RefreshObject(e.RowObject);

                        }
                    }
                }
            }
        }
*/

/*
        private void удалитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var pf = objectListView_PlatesDefault.SelectedObject as PlateFormat;

            if (objectListView_Customers.SelectedObject is Job.Customer customer && pf != null)
            {
                if (UserProfile.Customers.RemoveCustomerPlate(customer, pf))
                {
                    objectListView_PlatesDefault.RemoveObject(pf);
                }

            }
        }
*/

/*
        private void нетВладельцаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pf = objectListView_PlatesDefault.SelectedObject as PlateFormat;

            if (objectListView_Customers.SelectedObject is Job.Customer customer && pf != null)
            {
                pf.OwnerId = ObjectId.Empty;
                UserProfile.Customers.Refresh(customer);
                
                objectListView_PlatesDefault.RefreshObject(pf);
            }
        }
*/

        //private void textBoxDefaulEmail_Leave(object sender, EventArgs e)
        //{
        //    if (objectListView_Customers.SelectedObject is Job.Customer customer)
        //    {
        //        customer.DefaultEmail = textBoxDefaulEmail.Text;
        //        UserProfile.Customers.Refresh(customer);
        //    }
        //}

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
    }
}
