using System;
using System.Collections.Generic;
using System.Diagnostics;
using iText.Kernel.Pdf;

namespace JobSpace.Static.Pdf.ColorSpaces
{
    public class SignaColorExtractor
    {
        private HashSet<int> visited = new HashSet<int>();

        public List<string> Extract(string file)
        {
            visited.Clear();
            HashSet<string> colors = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            PdfDocument pdf = new PdfDocument(new PdfReader(file));

            int count = pdf.GetNumberOfPdfObjects();

            for (int i = 1; i <= count; i++)
            {
                PdfObject obj = pdf.GetPdfObject(i);
                ExtractFromObject(obj, colors);
            }

            pdf.Close();

            // Розкодування PANTONE (#20 → пробіл)
            List<string> result = new List<string>();
            foreach (var c in colors)
                result.Add(c.Replace("#20", " "));

            return result;
        }


        // ===================================================
        //     Універсальний рекурсивний аналіз PDF-об’єкта
        // ===================================================
        private void ExtractFromObject(PdfObject obj, HashSet<string> colors)
        {
            if (obj == null)
                return;

            // ---- Захист від зациклення ----
            int objNum = obj.GetIndirectReference() != null
                            ? obj.GetIndirectReference().GetObjNumber()
                            : -1;

            if (objNum > 0)
            {
                if (visited.Contains(objNum))
                    return;

                visited.Add(objNum);
            }

            // ---- Обробка за типом об’єкта ----
            switch (obj.GetObjectType())
            {
                case PdfObject.DICTIONARY:
                    ExtractFromDictionary((PdfDictionary)obj, colors);
                    break;

                case PdfObject.ARRAY:
                    ExtractFromArray((PdfArray)obj, colors);
                    break;

                case PdfObject.STREAM:
                    ExtractFromDictionary(((PdfStream)obj), colors);
                    break;
            }
        }


        private void ExtractFromDictionary(PdfDictionary dict, HashSet<string> colors)
        {
            foreach (PdfName key in dict.KeySet())
            {
                PdfObject value = dict.Get(key);
                string k = key.GetValue();

                // ======================================
                //     SIGNA: HDAG_ColorantNames
                // ======================================
                if (k == "HDAG_ColorantNames" && value is PdfArray names)
                {
                    for (int i = 0; i < names.Size(); i++)
                    {
                        PdfName nm = names.GetAsName(i);
                        if (nm != null)
                        {
                            string col = nm.GetValue().TrimStart('/');
                            colors.Add(col);
                        }
                    }
                }

                // ======================================
                //     ColorSpace та SignaMark-поля
                // ======================================
                if (k == "ColorSpace" || k.StartsWith("CS") || k.StartsWith("CSS"))
                {
                    ExtractColorFromColorSpaceObject(value, colors);
                }

                // Рекурсивний обхід
                ExtractFromObject(value, colors);
            }
        }


        private void ExtractFromArray(PdfArray arr, HashSet<string> colors)
        {
            foreach (PdfObject itm in arr)
            {
                ExtractFromObject(itm, colors);
            }
        }


        // ===================================================
        //    Розпізнавання Separation і DeviceN
        // ===================================================
        private void ExtractColorFromColorSpaceObject(PdfObject obj, HashSet<string> colors)
        {
            if (obj == null)
                return;

            if (obj.IsArray())
            {
                PdfArray arr = (PdfArray)obj;
                if (arr.Size() == 0) return;

                PdfObject first = arr.Get(0);

                // Separation
                if (first is PdfName sep && sep.Equals(PdfName.Separation))
                {
                    PdfName name = arr.GetAsName(1);
                    if (name != null)
                    {
                        string col = name.GetValue().TrimStart('/');
                        colors.Add(col);
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
                                string col = nm.GetValue().TrimStart('/');
                                colors.Add(col);
                            }
                        }
                    }
                }
            }
        }
    }
}
