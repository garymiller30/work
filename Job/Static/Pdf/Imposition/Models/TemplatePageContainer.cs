﻿using Job.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models
{
    public sealed class TemplatePageContainer
    {
        /// <summary>
        /// Сторінки шаблону
        /// </summary>
        public List<TemplatePage> TemplatePages { get; set; } = new List<TemplatePage>();

        /// <summary>
        /// PDF мітки сюжету
        /// </summary>

        public MarksContainer Marks { get; set; } = new MarksContainer();
        public int GetMaxIdx()
        {
            int frontIdx = TemplatePages.Max(x => x.FrontIdx);

            if (HasBack())
            {

                int backIdx = TemplatePages.Max(y => y.BackIdx);
                return frontIdx > backIdx ? frontIdx : backIdx;
            }
            else
            {
                return frontIdx;
            }
        }

        public void AddPage(TemplatePage page)
        {
            TemplatePages.Add(page);
        }

        public bool HasBack()
        {
            return TemplatePages.Any(x => x.BackIdx > 0);
        }

        public void SetCropMarksLen(double len)
        {
            TemplatePages.ForEach(x => x.CropMarksController.Parameters.Len = len);
        }

        public RectangleD GetSubjectRectFront()
        {
            double x1 = TemplatePages.Min(x => x.X);
            double y1 = TemplatePages.Min(x => x.Y);

            double x2 = TemplatePages.Max(x => x.X + x.GetClippedWByRotate());
            double y2 = TemplatePages.Max(x => x.Y + x.GetClippedHByRotate());

            return new RectangleD { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };
        }

        public RectangleD GetSubjectRectBack(TemplateSheet sheet)
        {
            var rect = GetSubjectRectFront();

            return new RectangleD
            {
                X1 = sheet.W - rect.X2,
                Y1 = rect.Y1,
                X2 = sheet.W - rect.X1,
                Y2 = rect.Y2
            };
        }

        public static void Save(TemplatePageContainer templatePageContainer, string filePath)
        {
            string jsonStr = JsonSerializer.Serialize(templatePageContainer);
            File.WriteAllText(filePath, jsonStr);
        }

        public static TemplatePageContainer Load(string filePath)
        {
            string jsonStr = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<TemplatePageContainer>(jsonStr);
        }

        public void UpdateCropMarkParameters(CropMarksParam param)
        {
            foreach (var page in TemplatePages)
            {
                page.CropMarksController.Parameters = param;
            }
        }
    }
}