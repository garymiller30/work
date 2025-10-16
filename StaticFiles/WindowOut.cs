using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Interfaces;
using JobSpace;
using JobSpace.Profiles;
using JobSpace.UC;

namespace StaticFiles
{
    public sealed partial class WindowOut: UserControl, IPluginInfo
    {
        private const string StaticFolderName = ".static";

        private UCFileBrowser _fileBrowser;
        private Profile _profile;

        public WindowOut()
        {
            InitializeComponent();
            Disposed += OnDisposed;
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            _fileBrowser.SaveSettings();
        }

        public void SetUserProfile(IUserProfile profile)
        {
            _profile = profile as Profile;
        }

        public IUserProfile UserProfile { get; set; }

        public UserControl GetUserControl()
        {
            return this;
        }

        public void Start()
        {
            _fileBrowser = new UCFileBrowser(_profile)
            {
                Location = new Point(0, toolStrip1.Height),
                Size = new Size(this.Width, this.Height - toolStrip1.Height),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                DefaultSettingsFolder = Path.Combine(_profile.ProfilePath, "Browsers", "staticFiles")
            };
            _fileBrowser.LoadSettings();
            _fileBrowser.InitToolStripUtils(-1);

            Controls.Add(_fileBrowser);

            var customers = _profile.Customers.Where(x => x.Show).ToList();
            toolStripComboBoxCustomers.ComboBox.DataSource = customers;
            toolStripComboBoxCustomers.ComboBox.DisplayMember = "Name";

        }
        public string GetPluginName()
        {
            return "Static Files";
        }
        public void SetCurJob(IJob curJob)
        {
            if (toolStripButtonSetByOrder.Checked)
                SetStaticFolder(curJob);
            
        }

        public void BeforeJobChange(IJob job)
        {
            
        }

        public void AfterJobChange(IJob job)
        {
            
        }

        private void SetStaticFolder(object curJob)
        {
            if (curJob is JobSpace.Job job)
            {
                var customer = job.Customer;

                var cust= (Customer)_profile.Customers.FindCustomer(customer);

                if (cust != null)
                {
                    toolStripComboBoxCustomers.SelectedItem = cust;
                    SetFolderByCustomer(cust);
                }

            }
        }

        private void SetFolderByCustomer(Customer cust)
        {
            //_fileBrowser.Enabled = cust.UseCustomFolder;

            //if (cust.UseCustomFolder)
            //{
                var custFolder = _profile.Customers.GetCustomerWorkFolder(cust);


                if (Directory.Exists(custFolder))
                {
                    _fileBrowser.Enabled = true;
                    var folder = Path.Combine(custFolder, StaticFolderName);

                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    _fileBrowser.SetRootFolder(folder);
                }
                else
                {
                    _fileBrowser.Enabled = false;
                }
           // }
        }

        private void toolStripComboBoxCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!toolStripButtonSetByOrder.Checked)
                if (toolStripComboBoxCustomers.SelectedItem is Customer cust)
                {
                    SetFolderByCustomer(cust);
                }
            
        }

        public string PluginName => GetPluginName();
        
        public string PluginDescription => "Папка, де можна зберігати файли, які періодично повторюються";
        public void ShowSettingsDlg()
        {
            MessageBox.Show("Налаштування відсутні");
        }
    }
}
