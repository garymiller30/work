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
        public string Id { get;set; } = Guid.NewGuid().ToString();
        public string ParentId { get;set; }
        public string Name { get; set; } = "Мітки";
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

        public void Add(params MarksContainer[] marks)
        {
            Containers.AddRange(marks);
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

        public bool Delete(TextMark textMark)
        {
            if (Text.Contains(textMark))
            {
                Text.Remove(textMark);
                return true;
            }
            else
            {
                foreach (MarksContainer container in Containers)
                {
                    if (container.Delete(textMark))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Delete(PdfMark pdfMark)
        {
            if (Pdf.Contains(pdfMark))
            {
                Pdf.Remove(pdfMark);
                return true;
            }
            else
            {
                foreach (MarksContainer container in Containers)
                {
                    if (container.Delete(pdfMark))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Delete(MarksContainer container)
        {
            if (Containers.Contains(container))
            {
                Containers.Remove(container);
                return true;
            }
            else
            {
                foreach (MarksContainer cont in Containers)
                {
                    if (cont.Delete(container))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
