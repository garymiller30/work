using PluginAddWorkFromPolymix.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace PluginAddWorkFromPolymix
{
    class PolymixController
    {
        private PluginAddWorkFromPolymixSettings _addWorkFromPolymixSettings;

        public PolymixController(PluginAddWorkFromPolymixSettings addWorkFromPolymixSettings)
        {
            _addWorkFromPolymixSettings = addWorkFromPolymixSettings;
        }

        /*
                public PolymixOrder[] GetOrders()
                {
                    List<PolymixOrder> orders = new List<PolymixOrder>();
                    var connectionString =
                        $"Server={_addWorkFromPolymixSettings.ServerAddress};Initial Catalog={_addWorkFromPolymixSettings.BaseName};User={_addWorkFromPolymixSettings.User};Password={_addWorkFromPolymixSettings.Password}";
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("SELECT ID_number,c.Name,Comment ");
                            sb.Append("FROM dbo.WorkOrder o JOIN dbo.CUSTOMER c ON o.Customer = c.N ");
                            sb.Append(
                                $"WHERE o.IsDraft!=1 and o.IsDeleted=0 and o.KindID={_addWorkFromPolymixSettings.KindId} and (o.OrderState=24 or o.OrderState=10 or o.OrderState=20 or o.OrderState=22)");

                            using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
                            {
                                conn.Open();
                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.HasRows)
                                {

                                    while (reader.Read()) // построчно считываем данные
                                    {
                                        var order = new PolymixOrder
                                        {
                                            Number = reader.GetInt32(0),
                                            Customer = reader.GetString(1),
                                            Description = reader.GetString(2)
                                        };

                                        orders.Add(order);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error(this, "PluginAddWorkFromPolymix", e.Message);
                    }


                    return orders.ToArray();
                }
        */

        public PolymixOrder[] GetOrders(IEnumerable<IFilter> filters)
        {
            List<PolymixOrder> orders = new List<PolymixOrder>();
            var connectionString =
                $"Server={_addWorkFromPolymixSettings.ServerAddress};Initial Catalog={_addWorkFromPolymixSettings.BaseName};User={_addWorkFromPolymixSettings.User};Password={_addWorkFromPolymixSettings.Password}";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT ID_number,c.Name,Comment ");
                    sb.Append("FROM dbo.WorkOrder o JOIN dbo.CUSTOMER c ON o.Customer = c.N ");

                    sb.Append("WHERE o.IsDraft!=1 and o.IsDeleted=0");

                    sb.Append(CreateQueryString(filters));

                    using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {

                            while (reader.Read()) // построчно считываем данные
                            {
                                var order = new PolymixOrder
                                {
                                    Number = reader.GetInt32(0),
                                    Customer = reader.GetString(1),
                                    Description = reader.GetString(2)
                                };

                                orders.Add(order);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(this, "PluginAddWorkFromPolymix", e.Message);
            }


            return orders.ToArray();

        }

        private string CreateQueryString(IEnumerable<IFilter> filters)
        {
            var sb = new StringBuilder();
            var kindId = filters.Where(x => x is KindIdFilter);


            if (kindId.Any())
            {

                var kinds = kindId.Cast<KindIdFilter>().ToArray();

                sb.Append(" and (");
                int idx = kindId.Count();
                do
                {

                    sb.Append($"o.KindID={kinds[idx - 1].KindID}");
                    idx--;

                    if (idx > 0)
                    {
                        sb.Append(" or ");
                    }

                } while (idx > 0);


                sb.Append(")");

                //sb.Append(
                //    $"WHERE o.IsDraft!=1 and o.IsDeleted=0 and o.KindID={_settings.KindId} and (o.OrderState=24 or o.OrderState=10 or o.OrderState=20 or o.OrderState=22)");
            }

            var statusesQuery = filters.Where(x => x is StatusFilter);

            if (statusesQuery.Any())
            {
                var statuses = statusesQuery.Cast<StatusFilter>().ToArray();

                sb.Append(" and (");
                int idx = statuses.Count();

                do
                {
                    sb.Append($"o.OrderState={statuses[idx - 1].Code}");
                    idx--;
                    if (idx > 0)
                    {
                        sb.Append(" or ");
                    }
                } while (idx > 0);
                sb.Append(")");
            }

            return sb.ToString();
        }


        public KindOrder[] GetKindId()
        {
            var kindList = new List<KindOrder>();
            var connectionString =
                $"Server={_addWorkFromPolymixSettings.ServerAddress};Initial Catalog={_addWorkFromPolymixSettings.BaseName};User={_addWorkFromPolymixSettings.User};Password={_addWorkFromPolymixSettings.Password}";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT KindID,KindDesc ");
                    sb.Append("FROM dbo.KindOrder ");


                    using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {

                            while (reader.Read()) // построчно считываем данные
                            {
                                var order = new KindOrder()
                                {
                                    KindID = reader.GetInt32(0),
                                    KindDesc = reader.GetString(1),
                                };

                                kindList.Add(order);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(this, "PluginAddWorkFromPolymix", e.Message);
            }

            return kindList.ToArray();
        }

        public OrderState[] GetStatuses()
        {
            var statusesList = new List<OrderState>();
            var connectionString =
                $"Server={_addWorkFromPolymixSettings.ServerAddress};Initial Catalog={_addWorkFromPolymixSettings.BaseName};User={_addWorkFromPolymixSettings.User};Password={_addWorkFromPolymixSettings.Password}";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT Code, Name, A2 ");
                    sb.Append("FROM dbo.Dic_OrderState ");


                    using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {

                            while (reader.Read()) // построчно считываем данные
                            {
                                var order = new OrderState()
                                {
                                    Code = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                };

                                order.Img = GetImageFromOrderStates((byte[])reader["A2"]);

                                statusesList.Add(order);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(this, "PluginAddWorkFromPolymix", e.Message);
            }

            return statusesList.ToArray();
        }
        private static Image GetImageFromOrderStates(byte[] imgBytes)
        {
            Image newImage = null;

            try
            {
                Color transparencyColor = Color.FromArgb(255, 128, 128, 0);

                //Read image data into a memory stream
                using (MemoryStream ms = new MemoryStream(imgBytes, 0, imgBytes.Length))
                {
                    ms.Write(imgBytes, 0, imgBytes.Length);
                    //Set image variable value using memory stream.
                    newImage = Image.FromStream(ms, true);
                    //Color backColor = ((Bitmap)newImage).GetPixel(4, 1);
                    ((Bitmap)newImage).MakeTransparent(transparencyColor);

                }
            }
            catch
            {


            }

            return newImage;
        }
    }
}
