using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using Interfaces.Pdf.Imposition;


public class Brovapharma
{

    class Order
    {
        public string _id { get; set; }
        public int Number { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }
        public Ps[] sheets { get; set; }
    }

    class Ps
    {
        public double Id { get; set; }
        public int Number { get; set; }
        public bool Back { get; set; }
        public int Count { get; set; }
        public bool isFinished { get; set; }
        public PsItem[] pages { get; set; }
    }

    class PsItem
    {
        public int Id { get; set; }
        public int? Number { get; set; }
    }

    class file
    {
        public string path { get; set; }
        public int cntPages { get; set; }
        public int number { get; set; }
        public string preparatNumber { get; set; }
        public string preparatName { get; set; }


    }

    public async Task Run(dynamic values)
    {
        string description = (string)(values.Description);
        string fileName = (string)(values.FileName);

        IImpositionFactory imposFactory = (IImpositionFactory)(values.ImposFactory);

        string inputFiles = fileName + "\\files.json";

        var files = LoadFiles(inputFiles);

        if (files == null)
        {
            MessageBox.Show($"Failed to load files.json");
            return;
        }

        string orderNumber = description.Split('_').LastOrDefault();

        var order = await DownloadOrder(orderNumber);

        if (order == null)
        {
            MessageBox.Show($"Failed to download order {orderNumber}.");
            return;
        }


        double sheet_w = 430;
        double sheet_h = 305;

        imposFactory.AddProductPart().AddPrintSheet(sheet_w, sheet_h).AddMasterPage(210,297,2);//.AddPdfFile("").AddMarks().Impos().Draw();


        foreach (var sheet in order.sheets)
        {
           
        }

    }

    private file[] LoadFiles(string fileName)
    {
        try
        {
            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<file[]>(json);
            }
            else
            {
                MessageBox.Show($"File {fileName} does not exist.");
            }
        }
        catch (Exception ex)
        {

            MessageBox.Show($"{ex}");
        }
        return null;
    }

    private async Task<Order> DownloadOrder(string orderNumber)
    {
        string url = $"https://brovapharma.vercel.app/orders/{DateTime.Now.Year}/{orderNumber}";

        using (var httpClient = new System.Net.Http.HttpClient())
        {
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                Order order = null;
                try
                {
                    order = JsonSerializer.Deserialize<Order>(content);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deserializing order: {ex.Message}");
                }


                return order;
            }
            else
            {
                MessageBox.Show($"Failed to download order. Status code: {response.StatusCode}");
            }
        }
        return null;
    }
}
