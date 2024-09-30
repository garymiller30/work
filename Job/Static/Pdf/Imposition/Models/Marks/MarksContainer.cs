using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public class MarksContainer
    {
        public string Name { get; set; } = "Marks";
        public List<MarksContainer> Containers { get; set; } = new List<MarksContainer>();
        public List<PdfMark> Pdf { get; set; } = new List<PdfMark>();
        public List<TextMark> Text { get; set; } = new List<TextMark>();

        public void Add(params PdfMark[] marks)
        {
            Pdf.AddRange(marks);
        }
        public void Add(params TextMark[] marks)
        {
            Text.AddRange(marks);
        }

        public static void Save(MarksContainer group, string fileName)
        {
            string jsonStr = JsonSerializer.Serialize(group);
            File.WriteAllText(fileName, jsonStr);
        }

        public void Save (string fileName)
        {
            Save(this,fileName);
        }

        public static MarksContainer Load(string fileName)
        {
            string jsonStr = File.ReadAllText(fileName);
            MarksContainer group = JsonSerializer.Deserialize<MarksContainer>(jsonStr);
            return group;
        }
    }
}
