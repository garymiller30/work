using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CSScriptLibrary.Sandbox;


    public class FinishOrder
    {
        string PolymixFilesRoot = "\\\\192.168.25.102\\Polimix_file\\";
        string connectionStr = "Data Source=192.168.25.102;Initial Catalog=TetrisNew;Persist Security Info=True;User ID=oksana;Password=zaza";
        //KindId: [{"id":33,"name":"Офсет"},{"id":36,"name":"Отделка"},{"id":37,"name":"Широкоформатка"},{"id":38,"name":"Цифра"}]
        int Kind = 33;
        int Status = 28;


        public void Run(dynamic values)
        {
            string fileName = (string)(values.FileName);

            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string orderNumber = filenameWithoutExtension.Split('_').FirstOrDefault();
            
        if (string.IsNullOrEmpty(orderNumber))
            {
                MessageBox.Show("Order number not found in the file name.");
                return;
            }
            
            

            int orderId = GetOrderIdByOrderNumber(orderNumber, Kind);

            if (orderId == 0)
            {
                return;
            }

            bool result = AttachToPolymix(orderId, orderNumber, fileName);

            if (!result)
            {
                return;
            }

            result = ChangeOrderStatus(orderId, Status);
            if (!result)
            {
                return;
            }


            bool deleted = false;

            while (!deleted)
            {
                try
                {
                    File.Delete(fileName);
                    deleted = true;
                }
                catch (Exception ex)
                {
                    var res = MessageBox.Show($"Error deleting file: {ex.Message}","Увага!",MessageBoxButtons.RetryCancel,MessageBoxIcon.Warning);
                    if (res == DialogResult.Cancel)
                    {
                     break;
                    }
                }
            }
           

        }

        private bool ChangeOrderStatus(int id, int status)
        {
            try
            {
                var command = "up_ChangeOrderState";
                var TheConnection = new System.Data.SqlClient.SqlConnection(connectionStr);
                var MyAction = new System.Data.SqlClient.SqlCommand(command, TheConnection);
                MyAction.CommandType = CommandType.StoredProcedure;
                MyAction.Parameters.AddWithValue("@OrderID", id);
                MyAction.Parameters.AddWithValue("@NewOrderState", status);
                TheConnection.Open();
                MyAction.ExecuteNonQuery();
                TheConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing order status: {ex.Message}");
                return false;
            }
        }

        private bool AttachToPolymix(int id, string number, string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            var res = int.TryParse(number,out int number_int);

            var targetDir = Path.Combine(PolymixFilesRoot, DateTime.Now.Year.ToString("D4"), number_int.ToString());
            Directory.CreateDirectory(targetDir);
            var targetFile = Path.Combine(targetDir, fileName);

            if (File.Exists(targetFile))
            {
                MessageBox.Show("File already exists in Polymix.");
                return false;
            }

            try
            {
                File.Copy(filePath, targetFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying file: {ex.Message}");
                return false;
            }

            var command = "up_NewOrderAttachedFile";
            try
            {
                var TheConnection = new System.Data.SqlClient.SqlConnection(connectionStr);
                var MyAction = new System.Data.SqlClient.SqlCommand(command, TheConnection);
                MyAction.CommandType = CommandType.StoredProcedure;
                MyAction.Parameters.AddWithValue("@OrderID", id);
                MyAction.Parameters.AddWithValue("@FileName", fileName);
                MyAction.Parameters.AddWithValue("@FileDesc", "");
                TheConnection.Open();
                MyAction.ExecuteNonQuery();
                TheConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error attaching file to order: {ex.Message}");
                return false;
            }
            return true;
        }

        private int GetOrderIdByOrderNumber(string orderNumber, int kind)
        {
            var command = String.Format("SELECT N FROM dbo.WorkOrder WHERE ID_number={0} AND KindID={1} AND OrderState!=80 AND OrderState !=70 AND OrderState!=90 AND OrderState!=110 AND IsDraft=0 AND IsDeleted=0", orderNumber, kind);
            try
            {
                var TheConnection = new System.Data.SqlClient.SqlConnection(connectionStr);
                TheConnection.Open();
                var MyAction = new System.Data.SqlClient.SqlCommand(command, TheConnection);
                var MyReader = MyAction.ExecuteReader();
                MyReader.Read();
                var id = (int)MyReader[0];
                MyReader.Close();
                TheConnection.Close();
                return id;

            }
            catch (Exception)
            {
                return 0;
            }

        }
    }

