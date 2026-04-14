using Interfaces.FileBrowser;
using Interfaces.Plugins;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.XMP;
using JobSpace.UserForms.PDF;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace JobSpace.Static.Pdf.Change
{
    [PdfTool("", "Перейменувати Spot колір", Icon = "spot_color_change")]
    public class PdfSpotColorRenamer : IPdfTool
    {

        Dictionary<string, string> _colorRenames;

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
                        _colorRenames = form.ColorRenames;
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
                string output = Path.Combine(Path.GetDirectoryName(file.FullName), $"{Path.GetFileNameWithoutExtension(file.FullName)}_col_ren.pdf");

                using (var pdf = new PdfDocument(new PdfReader(file.FullName), new PdfWriter(output)))
                {
                    //var renamer = new PitStopSpotRenamer(_colorRenames);
                    var renamer = new SpotColorRenamer(_colorRenames);
                    renamer.ProcessDocument(pdf);

                }
            }
        }
    }
    #region PitStop-like approach (не працює з iText7, але залишу для історії)
    public class PitStopSpotRenamer
    {
        private readonly Dictionary<string, string> _renames;
        private readonly HashSet<PdfObject> _visited = new HashSet<PdfObject>();

        public PitStopSpotRenamer(Dictionary<string, string> renames)
        {
            _renames = renames;
        }

        public void ProcessDocument(PdfDocument pdf)
        {
            _visited.Clear();

            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
            {
                var page = pdf.GetPage(i);

                ProcessObject(page.GetPdfObject());
                RewritePageContent(page);
            }
            //RemoveMetadata(pdf);
            //RewriteMetadata(pdf);
        }
        public void RemoveMetadata(PdfDocument pdf)
        {
            // Глобальні заходи
            pdf.GetCatalog().GetPdfObject().Remove(PdfName.Metadata);
            // Спробуйте це як "контрольний постріл"
            pdf.SetXmpMetadata(XMPMetaFactory.Create());
        }


        private void RewriteMetadata(PdfDocument pdf)
        {
            RewriteMetadataStream(pdf.GetCatalog().GetPdfObject());

            // Якщо раптом metadata є на сторінках
            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
            {
                RewriteMetadataStream(pdf.GetPage(i).GetPdfObject());
            }
        }

        private void RewriteMetadataStream(PdfDictionary dict)
        {
            if (dict == null) return;

            var metadata = dict.GetAsStream(PdfName.Metadata);
            if (metadata == null) return;

            string xml = Encoding.UTF8.GetString(metadata.GetBytes());

            // Проста, але ефективна заміна по всьому XML рядку.
            // Увага: щоб не замінити зайвого, шукаємо назви в лапках або між тегами.
            foreach (var pair in _renames)
            {
                // Заміна в текстових блоках: >cut< -> >cutting<
                xml = xml.Replace($">{pair.Key}<", $">{pair.Value}<");
                // Заміна в атрибутах: "cut" -> "cutting"
                xml = xml.Replace($"\"{pair.Key}\"", $"\"{pair.Value}\"");
            }

            metadata.SetData(Encoding.UTF8.GetBytes(xml));
        }
        // ================= CONTENT REWRITE =================

        private void RewritePageContent(PdfPage page)
        {
            var contents = page.GetPdfObject().Get(PdfName.Contents);

            if (contents == null)
                return;

            if (contents is PdfStream singleStream)
            {
                RewriteStream(singleStream);
            }
            else if (contents is PdfArray array)
            {
                foreach (var item in array)
                {
                    if (item is PdfStream stream)
                        RewriteStream(stream);
                }
            }
        }

        private void RewriteStream(PdfStream stream)
        {
            byte[] bytes = stream.GetBytes();
            string content = Encoding.ASCII.GetString(bytes);

            foreach (var pair in _renames)
            {
                // Додаємо варіанти з різними закінченнями, які зустрічаються в PDF-синтаксисі
                string[] delimiters = { " ", "/", ")", ">", "\n", "\r", "\t" };
                foreach (var d in delimiters)
                {
                    content = content.Replace("/" + pair.Key + d, "/" + pair.Value + d);
                }
            }
            stream.SetData(Encoding.ASCII.GetBytes(content));
        }

        // ================= OBJECT TREE =================

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
                    ProcessDictionary((PdfStream)obj);
                    break;
            }
        }

        private void ProcessDictionary(PdfDictionary dict)
        {
            ProcessColorSpace(dict);
            ProcessResources(dict.GetAsDictionary(PdfName.Resources));

            foreach (var key in dict.KeySet())
                ProcessObject(dict.Get(key));
        }

        private void ProcessArray(PdfArray array)
        {
            if (array.Size() == 0) return;

            var first = array.Get(0);

            if (PdfName.Separation.Equals(first) && array.Size() > 1)
                RenameArrayEntry(array, 1);

            if (PdfName.DeviceN.Equals(first) && array.Size() > 1)
            {
                var names = array.GetAsArray(1);
                if (names != null)
                    for (int i = 0; i < names.Size(); i++)
                        RenameArrayEntry(names, i);
            }

            foreach (var item in array)
                ProcessObject(item);
        }

        private void RenameArrayEntry(PdfArray array, int index)
        {
            if (array.Get(index) is PdfName name)
            {
                string oldName = name.GetValue();
                if (_renames.ContainsKey(oldName))
                    array.Set(index, new PdfName(_renames[oldName]));
            }
        }

        private void ProcessColorSpace(PdfDictionary dict)
        {
            var cs = dict.Get(PdfName.ColorSpace);
            if (cs != null)
                ProcessObject(cs);
        }

        private void ProcessResources(PdfDictionary resources)
        {
            if (resources == null) return;

            var colorSpaces = resources.GetAsDictionary(PdfName.ColorSpace);
            if (colorSpaces != null)
            {
                var keys = colorSpaces.KeySet().ToList();

                foreach (var key in keys)
                {
                    var value = colorSpaces.Get(key);
                    string oldName = key.GetValue();

                    if (_renames.ContainsKey(oldName))
                    {
                        var newKey = new PdfName(_renames[oldName]);
                        colorSpaces.Remove(key);
                        colorSpaces.Put(newKey, value);
                    }

                    ProcessObject(value);
                }
            }

            var xObjects = resources.GetAsDictionary(PdfName.XObject);
            if (xObjects != null)
            {
                foreach (var key in xObjects.KeySet())
                {
                    if (xObjects.Get(key) is PdfStream stream)
                        RewriteStream(stream);
                }
            }

            ProcessObject(resources);
        }

    }
    #endregion
    class SpotColorRenamer
    {
        private readonly Dictionary<string, string> _renames;
        private readonly HashSet<PdfObject> _visited = new HashSet<PdfObject>();

        public SpotColorRenamer(Dictionary<string, string> renames)
        {
            _renames = renames;
        }

        public void ProcessDocument(PdfDocument pdf)
        {
            _visited.Clear();
            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
            {
                var page = pdf.GetPage(i);
                ProcessObject(page.GetPdfObject());
            }
            RemoveMetadata(pdf);
        }

        public void RemoveMetadata(PdfDocument pdf)
        {
            // Глобальні заходи
            pdf.GetCatalog().GetPdfObject().Remove(PdfName.Metadata);
            // Спробуйте це як "контрольний постріл"
            pdf.SetXmpMetadata(XMPMetaFactory.Create());
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

            if (nameObj == null)
                return;
            string originalName = nameObj.GetValue();

            if (_renames.ContainsKey(originalName))
            {
                array.Set(index, new PdfName(_renames[originalName]));
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
