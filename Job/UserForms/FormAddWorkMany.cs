using Interfaces;
using Interfaces.Profile;
using JobSpace.Models;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms
{
    public partial class FormAddWorkMany : KryptonForm
    {
        IUserProfile _profile;
        public List<Job> CreatedJobs = new List<Job>();
        public FormAddWorkMany(IUserProfile profile)
        {
            InitializeComponent();
            _profile = profile;
            LoadCustomers();
            DialogResult = DialogResult.Cancel;
        }

        private void LoadCustomers()
        {
            var customers = _profile.Customers.Where(x => x.Show).ToList();
            cb_customers.DataSource = customers;
            cb_customers.DisplayMember = "Name";
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {

            if (cb_customers.SelectedItem == null)
            {
                MessageBox.Show("Оберіть замовника");
                return;
            }

            var customer = cb_customers.SelectedItem as ICustomer;

            if (objectListView1.Objects == null || objectListView1.Objects.Cast<object>().Count() == 0)
            {
                MessageBox.Show("Немає замовлень для додавання");
                return;
            }

            CreatedJobs = objectListView1.Objects.Cast<Job>().ToList();
            bool allValid = true;
            foreach (var job in CreatedJobs)
            {
                if (string.IsNullOrEmpty(job.Number)) {
                    allValid = false;
                }
                job.Customer = customer.Name;
            }
            if (!allValid) {
                MessageBox.Show("У всіх замовленнях має бути заповнений номер");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btn_add_order_Click(object sender, EventArgs e)
        {
            var job = Factory.CreateJob(_profile);
            objectListView1.AddObject(job);
        }
    }
}
