using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace FtpClient
{
    public sealed partial class FormSelectFolder : Form
    {
        private readonly List<object> _ftpParams;
        private readonly Client _client;

        public string Server { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }
        public string RootFolder { get; private set; }
        public bool IsActive { get; private set; }
        public int Encode { get; private set; }

        public TreeNode SelectedTreeNode;


        public FormSelectFolder(IEnumerable<object> param)
        {
            _ftpParams = param.ToList();

            InitializeComponent();
            _client = new Client();

            if (_ftpParams.Any())
            {
                toolStripComboBoxFtps.Items.AddRange(_ftpParams.Select(x => x.GetType().GetProperty("Name").GetValue(x, null)).ToArray());
                toolStripComboBoxFtps.SelectedIndex = 0;
            }

            olvColumn1.ImageGetter += rowObject => 0;
            treeListView1.AddObjects(ConnectToFtp());

            toolStripComboBoxFtps.SelectedIndexChanged += ToolStripComboBoxFtps_SelectedIndexChanged;
            treeListView1.CanExpandGetter += model =>
            {
                var node = model as TreeNode;
                if (node?.ChildFolders != null)
                    return true;

                return false;
            };
            treeListView1.ChildrenGetter += model => ((TreeNode) model).ChildFolders;

        }

        private void ToolStripComboBoxFtps_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeListView1.AddObjects(ConnectToFtp());
        }

        private List<TreeNode> ConnectToFtp()
        {
            treeListView1.ClearObjects();

            var serverParam = _ftpParams[toolStripComboBoxFtps.SelectedIndex];

            Server = serverParam.GetObjectValue<string>("Server");
            User = serverParam.GetObjectValue<string>("User");
            Password = serverParam.GetObjectValue<string>("Password");
            RootFolder = serverParam.GetObjectValue<string>("RootFolder");
            IsActive = serverParam.GetObjectValue<bool>("IsActive");
            Encode = serverParam.GetObjectValue<int>("Encode");


            _client.CreateConnection(Server, User, Password, RootFolder, IsActive, Encode);

            // ftp
            var dirs = _client.GetDirectories();
            var nodes = dirs.Select(x => new TreeNode {Folder = x}).ToList();

            return nodes;



        }

        private void treeListView1_DoubleClick(object sender, EventArgs e)
        {
            // ftp
            var node = treeListView1.SelectedObject as TreeNode;

            if (node != null)
            {
                _client.ChangeDirectory(node.Folder);
                var dirs = _client.GetDirectories();
                if (dirs.Any())
                {
                    node.ChildFolders = dirs.Select(x => new TreeNode {Folder = x}).ToList();

                    treeListView1.RefreshObject(node);
                    if (node.ChildFolders.Any())
                    {
                        treeListView1.Expand(node);
                    }

                }
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            SelectedTreeNode = treeListView1.SelectedObject as TreeNode;
            Close();
        }

        private void toolStripTextBoxFilter_TextChanged(object sender, EventArgs e)
        {
            if (toolStripTextBoxFilter.Text.Length > 2)
            {
                treeListView1.ModelFilter = TextMatchFilter.Contains(treeListView1, toolStripTextBoxFilter.Text);
            }
            else if (string.IsNullOrEmpty(toolStripTextBoxFilter.Text))
            {
                ClearFilter();
            }
        }

        private void toolStripButtonResetFilter_Click(object sender, EventArgs e)
        {

            ClearFilter();
        }


        private void ClearFilter()
        {
            toolStripTextBoxFilter.Text = String.Empty;
            treeListView1.ModelFilter = null;
        }
    }
}
