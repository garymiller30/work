using System;
using System.Collections.Generic;
using iText.Kernel.Pdf;

namespace JobSpace.Static.Pdf.ColorSpaces
{
    public class SignaColorExtractor
    {
        public List<string> Extract(string filePath)
        {
            // Використання using гарантує звільнення ресурсів
            using (PdfReader reader = new PdfReader(filePath))
            using (PdfDocument pdf = new PdfDocument(reader))
            {
                var visited = new HashSet<int>();
                var colors = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                int count = pdf.GetNumberOfPdfObjects();

                for (int i = 1; i <= count; i++)
                {
                    PdfObject obj = pdf.GetPdfObject(i);
                    ExtractFromObject(obj, colors, visited);
                }

                // Видалення специфічного тегу PANTONE
                var result = new List<string>();
                foreach (var c in colors)
                {
                    result.Add(c.Replace("#20", " "));
                }

                return result;
            }
        }

        private void ExtractFromObject(PdfObject obj, HashSet<string> colors, HashSet<int> visited)
        {
            if (obj == null) return;

            // Перевірка на зациклення
            var refObj = obj.GetIndirectReference();
            if (refObj != null)
            {
                int objNum = refObj.GetObjNumber();
                if (visited.Contains(objNum)) return;
                visited.Add(objNum);
            }

            switch (obj.GetObjectType())
            {
                case PdfObject.DICTIONARY:
                    ExtractFromDictionary((PdfDictionary)obj, colors, visited);
                    break;

                case PdfObject.ARRAY:
                    ExtractFromArray((PdfArray)obj, colors, visited);
                    break;

                // Виправлено: Stream не є Dictionary, тому не кастимо його так
                case PdfObject.STREAM:
                    // Якщо потрібно обробляти контент потоку, це робиться інакше.
                    break;
            }
        }

        private void ExtractFromDictionary(PdfDictionary dict, HashSet<string> colors, HashSet<int> visited)
        {
            foreach (PdfName key in dict.KeySet())
            {
                PdfObject value = dict.Get(key);
                string keyName = key.GetValue();

                // HDAG_ColorantNames
                if (keyName == "HDAG_ColorantNames" && value is PdfArray names)
                {
                    for (int i = 0; i < names.Size(); i++)
                    {
                        PdfName nm = names.GetAsName(i);
                        if (nm != null)
                        {
                            colors.Add(nm.GetValue().TrimStart('/'));
                        }
                    }
                }

                // ColorSpace та суміжні поля
                if (keyName == "ColorSpace" || keyName.StartsWith("CS") || keyName.StartsWith("CSS"))
                {
                    ExtractColorFromColorSpaceObject(value, colors);
                }

                // Рекурсивний виклик
                ExtractFromObject(value, colors, visited);
            }
        }

        private void ExtractFromArray(PdfArray arr, HashSet<string> colors, HashSet<int> visited)
        {
            foreach (PdfObject item in arr)
            {
                ExtractFromObject(item, colors, visited);
            }
        }

        private void ExtractColorFromColorSpaceObject(PdfObject obj, HashSet<string> colors)
        {
            if (obj == null || !obj.IsArray()) return;

            PdfArray arr = (PdfArray)obj;
            if (arr.Size() == 0) return;

            PdfObject first = arr.Get(0);

            // Separation
            if (first is PdfName sep && sep.Equals(PdfName.Separation))
            {
                PdfName name = arr.GetAsName(1);
                if (name != null)
                {
                    colors.Add(name.GetValue().TrimStart('/'));
                }
            }

            // DeviceN
            if (first is PdfName dev && dev.Equals(PdfName.DeviceN))
            {
                PdfArray comps = arr.GetAsArray(1);
                if (comps != null)
                {
                    for (int i = 0; i < comps.Size(); i++)
                    {
                        PdfName nm = comps.GetAsName(i);
                        if (nm != null)
                        {
                            colors.Add(nm.GetValue().TrimStart('/'));
                        }
                    }
                }
            }
        }
    }
}
