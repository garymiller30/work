using BrightIdeasSoftware;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CasheViewer
{
    public static class ConsumerPriceIndices
    {
        static string baseUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/inflation";
        static Dictionary<string, string> parameters = new Dictionary<string, string>() {
            {"id_api","prices_price_cpi_"},
            { "MCRD081","Total"},
            { "TZEP","PCPM_"},
            { "period","m"},
            {"date",""},
            {"json","" }
            };

        public static List<ConsumerPriceView> GetConsumerPrices(int year, int monthStart)
        {
            List<ConsumerPrice> res = new List<ConsumerPrice>();

            //завантажити індекси споживчих цін з сайту bank.gov.ua
            string url = $"{baseUrl}?";

            // дату потрібно брати на місяць вперед
            var date = new DateTime(year, monthStart, 1).AddMonths(1);
            parameters["date"] = date.ToString("yyyyMM");

            //parameters["start"] = $"{year}{monthStart:D2}01";
            url += string.Join("&", parameters.Select(kv => $"{kv.Key}={kv.Value}"));
            using (var client = new System.Net.WebClient())
            {
                client.Encoding = Encoding.UTF8;
                string json = client.DownloadString(url);
                if (string.IsNullOrEmpty(json))
                {
                    return new List<ConsumerPriceView>() { new ConsumerPriceView() };
                }

                res = JsonSerializer.Deserialize<List<ConsumerPrice>>(json);
            }


            var filtered = res.Where(x => x.ku == null);
            res = filtered.OrderBy(x => x.dt).ToList();

            if (res.Count == 0) return new List<ConsumerPriceView>() { new ConsumerPriceView() };

            return res.Select(x => new ConsumerPriceView(x)).ToList();

        }
    }
}
