using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq;
using System.IO;
using ProofFolder.TetrisTableAdapters;
using Microsoft.VisualBasic.FileIO;


namespace ProofFolder
{
    public partial class WindowOut : UserControl,Interfaces.IPluginInfo
    {
        readonly DataContext _tetris;

        private string conString =
            "Data Source=server;Initial Catalog=TetrisNew;Persist Security Info=True;User ID=Evgeniy;Password=12351";

        private Table<OrderState> _orderStates;

        public WindowOut()
        {
            InitializeComponent();
            _tetris = new DataContext(conString);
            _orderStates = _tetris.GetTable<OrderState>();

            CreateStateMenu();

            olvColumnOrderState.AspectGetter += rowObject => _orderStates.FirstOrDefault(x=> ((Order)rowObject).OrderState == x.Code)?.Name ;


        }

        private void CreateStateMenu()
        {
            var l = new List<ToolStripMenuItem>();

            foreach (var item in _orderStates)
            {
                    var ttm = new ToolStripMenuItem { Text = item.Name, Tag = item };

                    ttm.Click += ttmClick;
                    l.Add(ttm);
            }

            toolStripSplitButtonStatus.DropDownItems.AddRange(l.ToArray());
          
        }

        private void ttmClick(object sender, EventArgs e)
        {
            if (objectListViewOrders.SelectedObject is Order order)
            {
                var status = ((ToolStripMenuItem) sender).Tag as OrderState;

                if (status != null)
                {
                    order.OrderState = status.Code;

                    _tetris.ExecuteCommand($"UPDATE dbo.WorkOrder SET OrderState = {status.Code} WHERE N = {order.N}");
                }
            }
        }

        public string GetPluginName()
        {
            return "Proof Folder";
        }

        public UserControl GetUserControl()
        {
            return this;
        }

        public void SetCurJobCallBack(object curJob)
        {
           
            //throw new NotImplementedException();
        }

        public void SetCurJobPathCallBack(object curJobPath)
        {
            //throw new NotImplementedException();
        }

        public void SetCurJob(object curJob)
        {
            Debug.WriteLine("curjob: " + curJob);

            objectListView_OrderAttachment.ClearObjects();
            objectListViewOrders.ClearObjects();

            if (curJob is Job.Job job)
            {
                var num = int.TryParse(job.Number, out int eResult);

                var order = _tetris.GetTable<Order>().Where(x => x.ID_number == eResult && x.OrderState !=80);
                if (order.Any())
                {
                    objectListViewOrders.AddObjects(order.ToArray());

                    if (order.Count() == 1)
                    {
                        GetAttachedFilesFromBase(order.First());
                    }
                }

            }

        }

        private void GetAttachedFilesFromBase(Order o)
        {
            objectListView_OrderAttachment.ClearObjects();

            var files = _tetris.GetTable<OrderAttachedFile>().Where(x => x.OrderID == o.N);
            if (files.Any())
            {
                objectListView_OrderAttachment.AddObjects(files.ToList());
            }
        }

        public void Start()
        {
            //throw new NotImplementedException();
        }

        private void objectListViewOrders_Click(object sender, EventArgs e)
        {
            if (objectListViewOrders.SelectedObject is Order job)
            {
                GetAttachedFilesFromBase(job);
            }
        }

        private void objectListView_OrderAttachment_DragDrop(object sender, DragEventArgs e)
        {
            if (objectListViewOrders.SelectedObject is Order job)
            {

                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
                    if (files != null)
                    {

                        var adapter = new QueriesTableAdapter();

                        try
                        {
                            foreach (var file in files)
                            {
                                adapter.up_NewOrderAttachedFile(job.N, Path.GetFileName(file), null);

                                //\\server\Polimix_file\2017
                                CopyFileToServer(file, job.ID_number);

                            }
                            objectListViewOrders.RefreshObject(job);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, "Proof folder");
                            throw;
                        }
                    }
                }
            }
        }

        private void CopyFileToServer(string file, int jobN)
        {

            

            var targetPath = Path.Combine(@"\\server\Polimix_file\", DateTime.Now.Year.ToString("D4"));

            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

            targetPath = Path.Combine(targetPath, jobN.ToString());

            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

            targetPath = Path.Combine(targetPath, Path.GetFileName(file));
            

            FileSystem.CopyFile(file, targetPath, UIOption.AllDialogs);
        }

        private void objectListView_OrderAttachment_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {

                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }
    }
}
