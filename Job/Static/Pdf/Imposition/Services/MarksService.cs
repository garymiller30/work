using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Statuses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

        private static MarksContainer FindParent(List<MarksContainer> marks, string parentId)
        {
            foreach (MarksContainer mark in marks)
            {
                if (mark.Id  == parentId) return mark;

                var parent = FindParent(mark.Containers, parentId);
                if (parent != null) return parent;
            }

            return null;
        }

        public static void AddPdfMark(MarksContainer container, PdfMark mark)
        {
            container.Pdf.Add(mark);
            SaveResourceMarks();
        }

        public static void DeleteMark(PdfMark pdfMark)
        {
            MarksContainer parent = FindPdfMarkParent(Marks,pdfMark.Id);
            if (parent == null) return;

            parent.Pdf.Remove(pdfMark);
            SaveResourceMarks();
        }

        private static MarksContainer FindPdfMarkParent(List<MarksContainer> marks, string id)
        {
            foreach(MarksContainer mark in marks)
            {
                foreach (var pdfMark in mark.Pdf)
                {
                    if (pdfMark.Id == id) return mark;
                }

                var parent = FindPdfMarkParent(mark.Containers,id);
                if (parent != null) return parent;
            }

            return null;
        }
    }
}
