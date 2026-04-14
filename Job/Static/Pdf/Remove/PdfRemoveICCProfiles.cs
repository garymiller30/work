using Interfaces.FileBrowser;
using Interfaces.Plugins;
using iText.Kernel.Pdf;
using System;
using System.IO;
using System.Linq;

namespace JobSpace.Static.Pdf.Remove
{
    [PdfTool("","Видалити ICC-профілі з PDF",Icon = "remove_icc_profile")]
    public class PdfRemoveICCProfiles : IPdfTool
    {
        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {

                RemoveICCProfiles(file.FullName);
            }
        }

        

        public bool Configure(PdfJobContext context)
        {
            return true;
        }


        private void RemoveICCProfiles(string filePath)
        {
            string outputPath = Path.Combine(
                Path.GetDirectoryName(filePath),
                Path.GetFileNameWithoutExtension(filePath) + "_no_icc.pdf");

            using (PdfReader reader = new PdfReader(filePath))
            using (PdfWriter writer = new PdfWriter(outputPath))
            using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
            {
                // 1. Очищення глобальних параметрів
                PdfDictionary catalog = pdfDoc.GetCatalog().GetPdfObject();
                catalog.Remove(PdfName.OutputIntents);
                catalog.Remove(PdfName.Metadata);

                // 2. Глобальний перебір об'єктів для пошуку та заміни ICCBased
                int total = pdfDoc.GetNumberOfPdfObjects();
                for (int i = 1; i <= total; i++)
                {
                    PdfObject obj = pdfDoc.GetPdfObject(i);
                    if (obj == null) continue;

                    // Обробка масивів типу [/ICCBased 86 0 R]
                    if (obj.IsArray())
                    {
                        PdfArray arr = (PdfArray)obj;
                        if (arr.Size() >= 2 && PdfName.ICCBased.Equals(arr.GetAsName(0)))
                        {
                            ReplaceWithDeviceSpace(pdfDoc, i, arr);
                        }
                        // Обробка Indexed простору: [/Indexed /ICCBased_Array ...]
                        else if (arr.Size() >= 2 && PdfName.Indexed.Equals(arr.GetAsName(0)))
                        {
                            ProcessIndexed(pdfDoc, arr);
                        }
                    }
                    // Обробка словників (Resource, Group, ColorSpace)
                    else if (obj.IsDictionary())
                    {
                        CleanDictionary((PdfDictionary)obj);
                    }
                }
            }
        }
        private void ReplaceWithDeviceSpace(PdfDocument pdfDoc, int objNumber, PdfArray iccArray)
        {
            // Визначаємо кількість компонентів (N) з потоку профілю
            PdfStream stream = iccArray.GetAsStream(1);
            int n = 4; // За замовчуванням CMYK
            if (stream != null)
            {
                PdfNumber nNum = stream.GetAsNumber(PdfName.N);
                if (nNum != null) n = nNum.IntValue();
            }

            PdfName deviceSpace = PdfName.DeviceCMYK;
            if (n == 1) deviceSpace = PdfName.DeviceGray;
            else if (n == 3) deviceSpace = PdfName.DeviceRGB;

            // ХІРУРГІЧНА ОПЕРАЦІЯ: замінюємо весь масив профілю на ім'я Device колірного простору
            // Це видаляє [/ICCBased 86 0 R] і ставить замість нього /DeviceCMYK
            pdfDoc.GetPdfObject(objNumber).SetModified(); // Позначаємо для запису

            // Ми не можемо просто замінити тип об'єкта в таблиці iText напряму через індекс у такий спосіб,
            // тому ми проходимо по всіх словниках і замінюємо посилання. 
            // Але простіший шлях - обнулити масив і зробити його невалідним для профілю.
            iccArray.Clear();
            iccArray.Add(deviceSpace);
        }

        private void ProcessIndexed(PdfDocument pdfDoc, PdfArray indexedArray)
        {
            // Indexed масив виглядає так: [/Indexed BaseCS Hival Lookup]
            // BaseCS (індекс 1) може бути посиланням на ICCBased
            PdfObject baseCs = indexedArray.Get(1);
            if (baseCs.IsIndirectReference())
            {
                PdfObject resolvedBase = ((PdfIndirectReference)baseCs).GetRefersTo();
                if (resolvedBase.IsArray())
                {
                    PdfArray baseArr = (PdfArray)resolvedBase;
                    if (baseArr.Size() >= 2 && PdfName.ICCBased.Equals(baseArr.GetAsName(0)))
                    {
                        // Замінюємо посилання на ICC на пряме ім'я Device-простору
                        indexedArray.Set(1, GetDeviceNameForIcc(baseArr));
                    }
                }
            }
        }

        private PdfName GetDeviceNameForIcc(PdfArray iccArray)
        {
            PdfStream stream = iccArray.GetAsStream(1);
            int n = 4;
            if (stream != null)
            {
                PdfNumber nNum = stream.GetAsNumber(PdfName.N);
                if (nNum != null) n = nNum.IntValue();
            }
            if (n == 1) return PdfName.DeviceGray;
            if (n == 3) return PdfName.DeviceRGB;
            return PdfName.DeviceCMYK;
        }

        private void CleanDictionary(PdfDictionary dict)
        {
            PdfName[] csKeys = { PdfName.ColorSpace, PdfName.CS, PdfName.DefaultCMYK, PdfName.DefaultRGB };
            var keys = dict.KeySet().ToList();

            foreach (var key in keys)
            {
                if (csKeys.Contains(key))
                {
                    PdfObject cs = dict.Get(key);
                    if (cs.IsIndirectReference())
                    {
                        PdfObject resolved = ((PdfIndirectReference)cs).GetRefersTo();
                        if (resolved.IsArray() && PdfName.ICCBased.Equals(((PdfArray)resolved).GetAsName(0)))
                        {
                            dict.Put(key, GetDeviceNameForIcc((PdfArray)resolved));
                        }
                    }
                }
            }
        }
    }
}