// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 


using System;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Interfaces;
using Job.CustomerNotify;
using Job.Statuses;

namespace Job.UserForms
{
    public sealed partial class FormMailInformerSettings : KryptonForm
    {
        public IUserProfile UserProfile { get; set; }

        private readonly global::Job.Customer _customer;

        private CustomerMailNotify _curNotify;

        public FormMailInformerSettings(Customer customer,IUserProfile profile)
        {
            InitializeComponent();
            _customer = customer;

            olvColumn1.AspectGetter+= AspectGetter;

            UserProfile = profile;

            InitComboBoxStatuses();

            LoadCustomerSettings();
        }

        private object AspectGetter(object r)
        {
            return UserProfile?.StatusManager.GetJobStatusByCode(((CustomerMailNotify) r).StatusCode)?.Name;
        }

        private void InitComboBoxStatuses()
        {
            comboBoxStatuses.DisplayMember = "Name";
            comboBoxStatuses.Items.AddRange(UserProfile.StatusManager.GetJobStatuses());

        }

        private void LoadCustomerSettings()
        {
            objectListViewStatuses.AddObjects(UserProfile.CustomersNotifyManager.GetByCusomerName(_customer.Name));
        }

        private void objectListViewStatuses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objectListViewStatuses.SelectedObject !=null)
                Bind();
        }

        private void Bind()
        {
            _curNotify = (global::Job.CustomerNotify.CustomerMailNotify)objectListViewStatuses.SelectedObject;

            if (_curNotify != null)
            {
                textBoxBody.Text = _curNotify.Body;
                textBoxEmail.Text = _curNotify.Email;
                textBoxTema.Text = _curNotify.Tema;
            }
        }

        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {
            SetProperty(sender, "Email");
        }

        private void SetProperty(object sender, string property)
        {
            _curNotify?.GetType().GetProperty(property).SetValue(_curNotify, ((TextBox)sender).Text, null);
        }

        private void textBoxTema_TextChanged(object sender, EventArgs e)
        {
            SetProperty(sender, "Tema");
        }

        private void textBoxBody_TextChanged(object sender, EventArgs e)
        {
            SetProperty(sender, "Body");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserProfile.CustomersNotifyManager.Save();
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxStatuses.SelectedItem is JobStatus status)
            {
                var mailNotify = UserProfile.CustomersNotifyManager.Add(_customer, status.Code);
                objectListViewStatuses.AddObject(mailNotify);
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListViewStatuses.SelectedObject is CustomerMailNotify cmn)
            {
                UserProfile.CustomersNotifyManager.Remove(cmn);
                objectListViewStatuses.RemoveObject(cmn);
            }
        }
    }
}
