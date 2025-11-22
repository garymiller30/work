using Interfaces.Pdf.Imposition;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Processes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
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

        public int GetMaxIdx()
        {
            int frontIdx = TemplatePages.Max(x => x.Front.MasterIdx);

            if (HasBack())
            {
                int backIdx = TemplatePages.Max(y => y.Back.MasterIdx);
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
            return TemplatePages.Any(x => x.Back.MasterIdx > 0);
        }

        public void SetCropMarksLen(double len)
        {
            TemplatePages.ForEach(x => x.CropMarksController.Parameters.Len = len);
        }

        public void SetCropMarksDistance(double distance)
        {
            TemplatePages.ForEach(x => x.CropMarksController.Parameters.Distance = distance);
        }

     

        public RectangleD GetSubjectRectBack(TemplateSheet sheet)
        {
            var rect = ProcessSubject.GetSubjectRect(sheet,this);//  GetSubjectRectFront();

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


        public void FlipPagesAngle(TemplatePage page, TemplateSheetPlaceType placeType)
        {
            if (page.Front.Angle == 0 || page.Front.Angle == 180)
            {
                // міняємо кут у всіх з однаковим Y
                foreach (var item in TemplatePages)
                {
                    if (item.Front.Y == page.Front.Y)
                    {
                        item.FlipAngle(placeType);
                    }
                }
            }
            else
            {
                // міняємо кут у всіх з однаковим X
                foreach (var item in TemplatePages)
                {
                    if (item.Front.X == page.Front.X)
                    {
                        item.FlipAngle(placeType);
                    }
                }
            }
        }

        public void DeletePage(TemplateSheet sheet, TemplatePage page)
        {
            TemplatePages.Remove(page);
            // перерахувати мітки різу
            CropMarksService.FixCropMarks(sheet);
        }

        public TemplatePageContainer Copy()
        {
            return JsonSerializer.Deserialize<TemplatePageContainer>(JsonSerializer.Serialize(this));
        }

        public void SetTemplatePages(List<TemplatePage> templatePages)
        {
            TemplatePages.Clear();
            TemplatePages.AddRange(JsonSerializer.Deserialize<List<TemplatePage>>(JsonSerializer.Serialize(templatePages)));
        }
    }
}
