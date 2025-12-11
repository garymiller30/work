using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Colorspace; // КЛЮЧОВЕ: Додаємо для роботи з PdfName.ICCBased
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Remove
{
    public class PdfRemoveICCProfiles
    {
        public void Run(string filePath)
        {
            string outputPath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(filePath),
                System.IO.Path.GetFileNameWithoutExtension(filePath) + "_noICC.pdf");

            using (var reader = new PdfReader(filePath))
            using (var writer = new PdfWriter(outputPath))
            using (var pdfDoc = new PdfDocument(reader, writer))
            {
                int total = pdfDoc.GetNumberOfPdfObjects();

                for (int i = 1; i <= total; i++)
                {
                    PdfObject obj = pdfDoc.GetPdfObject(i);
                    if (obj == null || !obj.IsDictionary()) continue;

                    CleanDictionary((PdfDictionary)obj);
                }
            }
        }

        static void CleanDictionary(PdfDictionary dict)
        {
            // 0️⃣ Видалення XMP-Метаданих (видалено у фінальній версії) - це допомогло б, але зосередимося на ICC
            if (dict.ContainsKey(PdfName.Metadata))
                dict.Remove(PdfName.Metadata);

            // 1️⃣ OutputIntents (глобальні профілі, що використовуються для PDF/X)
            if (dict.ContainsKey(PdfName.OutputIntents))
                dict.Remove(PdfName.OutputIntents);

            // 2️⃣ ColorSpace (ПРЯМЕ ВИКОРИСТАННЯ: /ColorSpace та /CS для Transparency Group)
            // ДОДАНО: PdfName.CS
            foreach (var csKey in new[] { PdfName.ColorSpace, PdfName.CS })
            {
                if (dict.ContainsKey(csKey))
                {
                    PdfObject cs = dict.Get(csKey);
                    PdfObject newCs = CleanColorSpace(cs);
                    if (newCs != cs)
                        dict.Put(csKey, newCs);
                }
            }

            // 3️⃣ DefaultRGB/CMYK/Gray у ресурсах
            foreach (var defKey in new[] { PdfName.DefaultRGB, PdfName.DefaultCMYK, PdfName.DefaultGray })
            {
                if (dict.ContainsKey(defKey))
                {
                    PdfObject defCs = dict.Get(defKey);
                    PdfObject newDef = CleanColorSpace(defCs);
                    if (newDef != defCs)
                        dict.Put(defKey, newDef);
                }
            }

            // 4️⃣ ExtGState (графічний стан) - залишаємо без змін

            // 5️⃣ Shading/Pattern/SMask/Group/Resources рекурсивно
            foreach (PdfName key in new[] {
            PdfName.Shading, PdfName.Pattern, PdfName.SMask, PdfName.Group, PdfName.Resources })
            {
                if (dict.ContainsKey(key))
                {
                    PdfObject obj = dict.Get(key);
                    // Виправлено: обробка непрямих посилань
                    if (obj != null)
                    {
                        if (obj.IsDictionary())
                            CleanDictionary((PdfDictionary)obj);
                        else if (obj.IsIndirectReference())
                        {
                            PdfObject deref = ((PdfIndirectReference)obj).GetRefersTo();
                            if (deref != null && deref.IsDictionary())
                                CleanDictionary((PdfDictionary)deref);
                        }
                    }
                }
            }

            // 6️⃣ XObject у ресурсах
            if (dict.ContainsKey(PdfName.XObject))
            {
                PdfDictionary xobjs = dict.GetAsDictionary(PdfName.XObject);
                if (xobjs != null)
                {
                    foreach (PdfName name in xobjs.KeySet())
                    {
                        PdfObject xobj = xobjs.Get(name);
                        // Виправлено: обробка непрямих посилань на XObject
                        if (xobj != null)
                        {
                            if (xobj.IsStream())
                                CleanDictionary((PdfStream)xobj);
                            else if (xobj.IsIndirectReference())
                            {
                                PdfObject deref = ((PdfIndirectReference)xobj).GetRefersTo();
                                if (deref != null && deref.IsStream())
                                    CleanDictionary((PdfStream)deref);
                            }
                        }
                    }
                }
            }
        }

        static PdfObject CleanColorSpace(PdfObject csObj)
        {
            if (csObj == null)
                return null;

            // Обробка непрямого посилання
            if (csObj.IsIndirectReference())
            {
                PdfObject deref = ((PdfIndirectReference)csObj).GetRefersTo();
                PdfObject cleaned = CleanColorSpace(deref);
                if (cleaned != deref)
                    return cleaned;

                return csObj;
            }

            if (csObj.IsName())
                return csObj;

            if (csObj.IsArray())
            {
                PdfArray arr = (PdfArray)csObj;
                if (arr.Size() == 0) return csObj;
                PdfName type = arr.GetAsName(0);

                if (PdfName.ICCBased.Equals(type))
                {
                    PdfStream icc = arr.GetAsStream(1);

                    if (icc != null)
                    {
                        // 💥 КЛЮЧОВЕ ВИПРАВЛЕННЯ: Фізичне очищення ICC-потоку!
                        PdfObject iccObj = arr.Get(1);
                        PdfStream derefIcc = iccObj.IsIndirectReference()
                            ? (PdfStream)((PdfIndirectReference)iccObj).GetRefersTo()
                            : (PdfStream)iccObj;

                        if (derefIcc != null)
                        {
                            // Робимо потік недійсним: видаляємо дані та метадані стиснення
                            derefIcc.Remove(PdfName.Length);
                            derefIcc.Remove(PdfName.Filter);
                            derefIcc.Remove(PdfName.DecodeParms);
                            derefIcc.SetData(new byte[0]);
                            derefIcc.Put(PdfName.Length, new PdfNumber(0));
                        }

                        // Заміна на системний простір CMYK (так як Base Color Space був CMYK)
                        int n = icc.GetAsNumber(PdfName.N)?.IntValue() ?? 4;

                        // Якщо 4 канали, повертаємо DeviceCMYK
                        if (n == 4) return PdfName.DeviceCMYK;
                        if (n == 1) return PdfName.DeviceGray;
                        if (n == 3) return PdfName.DeviceRGB;
                    }

                    return PdfName.DeviceCMYK; // Запасний варіант
                }

                // ... (Логіка для Indexed, Separation, DeviceN, Pattern без змін)
                bool changed = false;
                PdfArray newArr = new PdfArray();
                newArr.Add(type);

                if (PdfName.Indexed.Equals(type))
                {
                    PdfObject baseCs = arr.Get(1);
                    PdfObject newBase = CleanColorSpace(baseCs);
                    if (newBase != baseCs) changed = true;
                    newArr.Add(newBase);
                    for (int i = 2; i < arr.Size(); i++)
                        newArr.Add(arr.Get(i));
                }
                else if (PdfName.Separation.Equals(type) || PdfName.DeviceN.Equals(type))
                {
                    newArr.Add(arr.Get(1));

                    PdfObject alt = arr.Get(2);
                    PdfObject newAlt = CleanColorSpace(alt);
                    if (newAlt != alt) changed = true;
                    newArr.Add(newAlt);

                    for (int i = 3; i < arr.Size(); i++)
                        newArr.Add(arr.Get(i));
                }
                else if ((PdfName.Pattern.Equals(type) || PdfName.Shading.Equals(type)) && arr.Size() > 1)
                {
                    PdfObject inner = arr.Get(1);
                    PdfObject newInner = CleanColorSpace(inner);
                    if (newInner != inner) changed = true;
                    newArr.Add(newInner);

                    for (int i = 2; i < arr.Size(); i++)
                        newArr.Add(arr.Get(i));
                }
                else
                {
                    for (int i = 1; i < arr.Size(); i++)
                        newArr.Add(arr.Get(i));
                }

                return changed ? newArr : csObj;
            }

            return csObj;
        }
    }
}