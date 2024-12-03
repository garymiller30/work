using Ghostscript.NET.Rasterizer;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Statuses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class MarksService
    {
        public static List<MarksContainer> Marks { get; set; }

        public static MarksContainer CreateGroup(string name)
        {
            MarksContainer container = new MarksContainer() { Name = name };
            Marks.Add(container);
            SaveResourceMarks();
            return container;
        }

        public static MarksContainer CreateGroup(string name, MarksContainer container)
        {
            MarksContainer group = new MarksContainer() { Name = name };
            group.ParentId = container.Id;
            container.Containers.Add(group);
            SaveResourceMarks();
            return group;
        }

        public static List<MarksContainer> GetResourceMarks()
        {
            if (Marks == null) LoadResourceMarks();

            return Marks;
        }

        public static void LoadResourceMarks()
        {
            Marks = SaveLoadService.LoadResourceMarks();
        }

        public static void SaveResourceMarks()
        {
            SaveLoadService.SaveResourceMarks(Marks);
        }

        public static void DeleteGroup(MarksContainer group)
        {
            if (group.ParentId == null)
            {
                Marks.Remove(group);
                SaveResourceMarks();
            }
            else
            {
                var parent = FindParent(Marks, group.ParentId);

                if (parent != null)
                {
                    parent.Containers.Remove(group);
                    SaveResourceMarks();
                }
            }
        }

        public static bool DeleteGroup(List<MarksContainer> group, MarksContainer container)
        {
            if (container.ParentId == null)
            {
                group.Remove(container);
                return true;
            }
            else
            {
                var parent = FindParent(Marks, container.ParentId);

                if (parent != null)
                {
                    parent.Containers.Remove(container);
                    return true;
                }
            }

            return false;
        }

        private static MarksContainer FindParent(List<MarksContainer> marks, string parentId)
        {
            foreach (MarksContainer mark in marks)
            {
                if (mark.Id == parentId) return mark;

                var parent = FindParent(mark.Containers, parentId);
                if (parent != null) return parent;
            }

            return null;
        }

        public static void AddMark(MarksContainer container, PdfMark mark)
        {
            container.Pdf.Add(mark);
            SaveResourceMarks();
        }

        public static void AddMark(MarksContainer container, TextMark mark)
        {
            container.Text.Add(mark);
            SaveResourceMarks();
        }

        public static void DeleteMark(PdfMark pdfMark)
        {
            MarksContainer parent = FindPdfMarkParent(Marks, pdfMark.Id);
            if (parent == null) return;

            parent.Pdf.Remove(pdfMark);
            SaveResourceMarks();
        }

        public static void DeleteMark(TextMark mark)
        {
            MarksContainer parent = FindTextMarkParent(Marks, mark.Id);
            if (parent == null) return;

            parent.Text.Remove(mark);
            SaveResourceMarks();
        }

        private static MarksContainer FindTextMarkParent(List<MarksContainer> marks, string id)
        {

            foreach (MarksContainer mark in marks)
            {
                foreach (var txtMark in mark.Text)
                {
                    if (txtMark.Id == id) return mark;
                }

                var parent = FindTextMarkParent(mark.Containers, id);
                if (parent != null) return parent;
            }

            return null;
        }

        private static MarksContainer FindPdfMarkParent(List<MarksContainer> marks, string id)
        {
            foreach (MarksContainer mark in marks)
            {
                foreach (var pdfMark in mark.Pdf)
                {
                    if (pdfMark.Id == id) return mark;
                }

                var parent = FindPdfMarkParent(mark.Containers, id);
                if (parent != null) return parent;
            }

            return null;
        }

        public static T Duplicate<T>(T mark)
        {
            var str = JsonSerializer.Serialize(mark);
            return JsonSerializer.Deserialize<T>(str);
        }

        public static Image GetBitmap(PdfMark mark)
        {
            var png_path = Path.Combine(Path.GetDirectoryName(mark.File.FileName), Path.GetFileNameWithoutExtension(mark.File.FileName)) + ".png";

            if (!File.Exists(png_path))
            {
                Rasterize(mark.File.FileName);
            }

            if (File.Exists(png_path))
            {
                return Bitmap.FromFile(png_path);
            }

            return null;
        }

        public static void Rasterize(string pdfFile)
        {
            string outputPath = $"{Path.GetDirectoryName(pdfFile)}\\{Path.GetFileNameWithoutExtension(pdfFile)}.png";
            using (GhostscriptRasterizer rasterizer = new GhostscriptRasterizer())
            {
                byte[] buffer = File.ReadAllBytes(pdfFile);
                MemoryStream ms = new MemoryStream(buffer);
                rasterizer.Open(ms);

                var img = rasterizer.GetPage(96, 1);
                img.Save(outputPath, ImageFormat.Png);
            }
        }
    }
}
