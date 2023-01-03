// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using Krypton.Toolkit;
using FtpClient;
using Interfaces;
using Interfaces.Ftp;
using Job.Static;

namespace Job.UC
{
    public sealed partial class UcFtpBrowser : UserControl, IUcFtpBrowser
    {
        public IFtpState FtpStates { get; set; } = new FtpState();
        public IUserProfile UserProfile { get; set; }

        private TabControl _tabControlCustomers;
        public event EventHandler<IDownloadTicket> OnCreateOrder = delegate { };
        public event EventHandler<IDownloadTicket> OnAddFilesToOrder = delegate { };
        public event EventHandler<IDownloadTicket> OnCreateOrderFromDir = delegate { };
        public event EventHandler<IDownloadTicket> OnCreateOrderFromDirLikeDescription =
            delegate { };//Tuple<string, IDownloadFileParam>



        public UcFtpBrowser()
        {
            
            InitializeComponent();
        }

        /// <summary>
        /// створити закладки з користувачами у яких є ftp
        /// </summary>
        public void Init()
        {
            var customersWithFtp = UserProfile.Customers.GetCustomersWithFtp();

            if (customersWithFtp.Any())
            {
                _tabControlCustomers = new TabControl
                {
                    Dock = DockStyle.Fill,
                    ImageList = imageList1
                };
                // створюємо закладки з замовниками
                foreach (var customer in customersWithFtp)
                {
                    // tab with customer name
                    var tabPageCustomer = new TabPage(customer.Name)
                    {
                        Tag = customer,
                        ImageIndex = 0
                    };

                    tabPageCustomer.Controls.Add(new KryptonPanel{Dock = DockStyle.Fill});

                    // add ftp parameters
                    var tabControlParams = new TabControl
                    {
                        Dock = DockStyle.Fill,
                        Appearance = TabAppearance.FlatButtons,
                        ImageList = imageList1
                        
                    };
                    //tabControlParams.Controls.Add(new KryptonPanel{Dock = DockStyle.Fill});
                    // створюємо таби з фтп
                    foreach (var ftpName in customer.FtpServers)
                    {
                        var ftpParam = UserProfile.Ftp.GetParamByName(ftpName);

                        if (ftpParam != null)
                        {
                            var tabPageParam = new TabPage(ftpParam.Name)
                            {
                                Tag = ftpParam,
                                ImageIndex = 1
                            };

                            tabPageParam.Controls.Add(new KryptonPanel{Dock = DockStyle.Fill});

                            // добавить проводник
                            var explorer =
                                new UcFtpExplorer(ftpParam)
                                {
                                    FriendlyName = customer.Name,
                                    Dock = DockStyle.Fill,
                                    Tag = customer,
                                    UserProfile = UserProfile
                                    
                                };
                            explorer.LoadScripts(UserProfile.Ftp.FtpScriptController.All());
                            explorer.OnCreateNewOrder += ExplorerOnOnCreateNewOrder;
                            explorer.OnCreateOrderFromFolderLikeDescription += ExplorerOnOnCreateOrderFromFolderLikeDescription;
                            explorer.OnAddFilesToOrder += ExplorerOnOnAddFilesToOrder;
                            explorer.OnCreateOrderFromFolder += ExplorerOnOnCreateOrderFromFolder;
                            explorer.OnNewFiles += ExplorerOnOnNewFiles;

                            FtpStates.Add(explorer);

                            tabPageParam.Controls[0].Controls.Add(explorer);

                            tabControlParams.TabPages.Add(tabPageParam);
                        }
                    }

                    tabPageCustomer.Controls[0].Controls.Add(tabControlParams);

                    _tabControlCustomers.TabPages.Add(tabPageCustomer);
                }

                kryptonPanel1.Controls.Add(_tabControlCustomers);

            }
        }

        private void ExplorerOnOnNewFiles(object sender, int e)
        {

            FtpStates.ChangeStatus(sender, e!=0);

            // прилітає повідомлення від UcFtpExplorer
            // потрібно знайти таб, з якого отримали повідомлення "замовник-параметри"
            var customer = (Customer)(sender as UcFtpExplorer)?.Tag;
            // знайдемо таб з замовником
            var customerTab = _tabControlCustomers.TabPages.Cast<TabPage>().FirstOrDefault(x => Equals(x.Tag, customer));

            // знайдемо таб з контролом, який відправив повідомлення
            //var tabs = ((TabControl) customerTab?.Controls[0]?.Controls[0])?.TabPages.Cast<TabPage>();
            //var tab = tabs.First(x => Equals(x.Controls[0].Controls[0], sender));
            var paramsTab = ((TabControl)customerTab?.Controls[0]?.Controls[0])?.TabPages.Cast<TabPage>()
                .FirstOrDefault(x => Equals(x.Controls[0]?.Controls[0], sender));

            // напишемо на табі скільки нових файлів з'явилося
            if (paramsTab != null)
            {
                if (e == 0)
                {
                    paramsTab.Text = $"{((FtpSettings) paramsTab.Tag).Name}";
                    customerTab.Text = customer.Name;
                    customerTab.ImageIndex = 0;
                }
                else
                {
                    paramsTab.Text = $"{((FtpSettings) paramsTab.Tag).Name} ({e})";
                    customerTab.Text = $"{customer.Name}";
                    customerTab.ImageIndex = 2;
                }
            }

        }

        private void ExplorerOnOnCreateOrderFromFolderLikeDescription(object sender, IDownloadTicket downloadProperties)//Tuple<string, IDownloadFileParam> tuple
        {
            downloadProperties.Customer = (ICustomer)((UcFtpExplorer)sender).Tag;

            OnCreateOrderFromDirLikeDescription(this,downloadProperties);
        }

        private void ExplorerOnOnCreateOrderFromFolder(object sender,IDownloadTicket downloadProperties)
        {
            downloadProperties.Customer = (ICustomer)((UcFtpExplorer)sender).Tag;

            OnCreateOrderFromDir(this, downloadProperties);
        }

        private void ExplorerOnOnAddFilesToOrder(object sender, IDownloadTicket downloadProperties)
        {
            downloadProperties.Customer = (ICustomer)((UcFtpExplorer) sender).Tag;

            OnAddFilesToOrder(this,downloadProperties);
        }

        private void ExplorerOnOnCreateNewOrder(object sender, IDownloadTicket downloadProperties)//IDownloadFileParam downloadFileParams
        {
            downloadProperties.Customer = (ICustomer)((UcFtpExplorer) sender).Tag;
            OnCreateOrder(this, downloadProperties);
        }

        public string GetClientNamesWithNewFiles()
        {
            var ftps = FtpStates.GetObjectWithState().Cast<UcFtpExplorer>();

            return (ftps.Select(x => x.FriendlyName)).Aggregate((a, n) => $"{a},{n}");
            
        }

        //public void DownloadFiles(IList modelObjects, string targetPath)
        //{
        //    if (_tabControlCustomers.TabPages.Count > 0)
        //    {
        //        if (modelObjects is FtpFileExt[] files)
        //        {
        //            var explorer = (UcFtpExplorer)((TabControl) _tabControlCustomers.SelectedTab.Controls[0]).SelectedTab.Controls[0];

        //            OnAddFilesToOrder(explorer.Tag,explorer.CreateDownloadFileList(files.ToList()));
        //        }
                
        //    }
        //}

        public IDownloadFileParam GetDownloadFileParam(IList modelObjects)
        {
            if (_tabControlCustomers.TabPages.Count > 0)
            {

                var files = modelObjects.Cast<IFtpFileExt>().ToList();
                
                var explorer = (UcFtpExplorer)((TabControl) _tabControlCustomers.SelectedTab.Controls[0].Controls[0]).SelectedTab.Controls[0].Controls[0];

                return explorer.CreateDownloadFileList(files.ToList());

                
            }

            return null;
        }
    }
}
