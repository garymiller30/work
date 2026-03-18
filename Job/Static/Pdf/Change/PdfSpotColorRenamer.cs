using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.UserForms.PDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using System.IO;


namespace JobSpace.Static.Pdf.Change
{
    [PdfTool("", "Перейменувати Spot колір",Icon = "spot_color_change")]
    public class PdfSpotColorRenamer : IPdfTool
    {
        string originalName;
        string newName;
        private readonly HashSet<PdfObject> _visited = new HashSet<PdfObject>();

        public bool Configure(PdfJobContext context)
        {
            var file = context.InputFiles.FirstOrDefault();
            if (file != null)
            {
                using (var form = new FormSpotColorRenamer(file))
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        originalName = form.SelectedColor;
                        newName = form.NewColorName;
                        return true;
                    }
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                using (var pdf = new PdfDocument(new PdfReader(file.FullName), new PdfWriter(Path.Combine(Path.GetDirectoryName(file.FullName),$"{Path.GetFileNameWithoutExtension(file.FullName)}_modified.pdf"))))
                {
                    Process(pdf);
                }
            }
        }

        public void Process(PdfDocument pdf)
        {
            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
            {
                var page = pdf.GetPage(i);
                ProcessObject(page.GetPdfObject());
            }
        }

        private void ProcessObject(PdfObject obj)
        {
            if (obj == null || _visited.Contains(obj))
                return;

            _visited.Add(obj);

            switch (obj.GetObjectType())
            {
                case PdfObject.DICTIONARY:
                    ProcessDictionary((PdfDictionary)obj);
                    break;

                case PdfObject.ARRAY:
                    ProcessArray((PdfArray)obj);
                    break;

                case PdfObject.STREAM:
                    var dict = (PdfStream)obj;
                    ProcessDictionary(dict);
                    break;
            }
        }

        private void ProcessDictionary(PdfDictionary dict)
        {
            // 🔹 Обробка ColorSpace
            var cs = dict.Get(PdfName.ColorSpace);
            if (cs != null)
            {
                ProcessObject(cs);
            }

            // 🔹 Обробка Resources
            var resources = dict.GetAsDictionary(PdfName.Resources);
            if (resources != null)
            {
                ProcessResources(resources);
            }

            // 🔹 Рекурсія по всіх ключах
            foreach (var key in dict.KeySet())
            {
                var value = dict.Get(key);
                ProcessObject(value);
            }
        }

        private void ProcessArray(PdfArray array)
        {
            if (array.Size() == 0)
                return;

            var first = array.Get(0);

            // 🔥 Separation
            if (first.Equals(PdfName.Separation) && array.Size() >= 2)
            {
                RenameIfMatch(array, 1);
            }

            // 🔥 DeviceN
            if (first.Equals(PdfName.DeviceN) && array.Size() >= 2)
            {
                var namesArray = array.GetAsArray(1);
                if (namesArray != null)
                {
                    for (int i = 0; i < namesArray.Size(); i++)
                    {
                        RenameIfMatch(namesArray, i);
                    }
                }
            }

            // 🔁 Рекурсія
            for (int i = 0; i < array.Size(); i++)
            {
                ProcessObject(array.Get(i));
            }
        }

        private void RenameIfMatch(PdfArray array, int index)
        {
            var nameObj = array.Get(index) as PdfName;
            if (nameObj != null && nameObj.GetValue() == originalName)
            {
                array.Set(index, new PdfName(newName));
            }
        }

        private void ProcessResources(PdfDictionary resources)
        {
            // 🔹 ColorSpaces
            var colorSpaces = resources.GetAsDictionary(PdfName.ColorSpace);
            if (colorSpaces != null)
            {
                foreach (var key in colorSpaces.KeySet())
                {
                    ProcessObject(colorSpaces.Get(key));
                }
            }

            // 🔹 XObject (Form / Image)
            var xObjects = resources.GetAsDictionary(PdfName.XObject);
            if (xObjects != null)
            {
                foreach (var key in xObjects.KeySet())
                {
                    var xobj = xObjects.Get(key);

                    if (xobj is PdfStream stream)
                    {
                        var subtype = stream.GetAsName(PdfName.Subtype);

                        if (PdfName.Form.Equals(subtype))
                        {
                            var form = new PdfFormXObject(stream);
                            ProcessObject(form.GetPdfObject());
                        }
                        else if (PdfName.Image.Equals(subtype))
                        {
                            // У image теж може бути ColorSpace
                            ProcessObject(stream);
                        }
                    }
                }
            }

            // 🔁 Рекурсія
            foreach (var key in resources.KeySet())
            {
                ProcessObject(resources.Get(key));
            }
        }

    }
}
