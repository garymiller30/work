using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * Щоб розмістити всі візитки на одному друкарському листі, потрібно дізнатись, скільки кожного типу візиток можна розмістити на листі так, щоб задовольнити весь тираж кожного виду візиток.

На одному друкарському листі вміщується 8 візиток. Маємо три види візиток з наступними тиражами:
- Перша візитка: 500 шт
- Друга візитка: 1500 шт
- Третя візитка: 2000 шт

Давайте поділимо доступні місця на друкарському листі відповідно до тиражів:

1. Порахуймо загальний тираж усіх візиток:
   \(500 + 1500 + 2000 = 4000\) шт

2. Визначимо частки кожного типу візиток в загальному тиражі:
   - Перша візитка: \(\frac{500}{4000} = 0.125\) або 12.5%
   - Друга візитка: \(\frac{1500}{4000} = 0.375\) або 37.5%
   - Третя візитка: \(\frac{2000}{4000} = 0.5\) або 50%

3. Визначимо кількість місць на друкарському листі для кожного типу візиток:
   - Перша візитка: \(8 \times 0.125 = 1\) місце
   - Друга візитка: \(8 \times 0.375 = 3\) місця
   - Третя візитка: \(8 \times 0.5 = 4\) місця

Отже, на одному друкарському листі потрібно розмістити:
- 1 візитку першого типу
- 3 візитки другого типу
- 4 візитки третього типу

Це забезпечить оптимальний розподіл візиток на друкарському листі відповідно до заданих тиражів.
 */
namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class BindingSimpleOneFormatDifferentCount : BindingSimpleControl, IBindControl
    {
        CalcResult _result;
        public BindingSimpleOneFormatDifferentCount() : base()
        {
            InitializeComponent();
            btn_custom.Visible = true;
            btn_custom.Click += Btn_custom_Click;

        }

        private void Btn_custom_Click(object sender, EventArgs e)
        {
            if (_result == null) return;
            using (var form = new FormSameFormatResult())
            {
                form.SetResult(_result);
                form.ShowDialog();
            }
        }


        new public void RearangePages(List<PrintSheet> sheets, List<ImposRunPage> pages)
        {
            if (sheets.Count ==0) return;

            // скинути 
            pages.ForEach(p => p.IsAssumed = false);

            _result = new CalcResult(parameters.PdfFileList.Objects.Cast<PdfFile>().ToList());
            _result.SetCountOnSheet(sheets[0].TemplatePageContainer.TemplatePages.Count);
            _result.Calc();

            int tp_idx = _result.CountOnSheet - 1;
            var tp = sheets[0].TemplatePageContainer.TemplatePages;

            int pageIdx = 1;

            foreach (FileResult file in _result.Files)
            {
                for (int i = 0; i < file.PagesOnSheet; i++)
                {
                    var rp = pages[pageIdx - 1];

                    tp[tp_idx].Front.PrintIdx = pageIdx;
                    rp.IsAssumed = true;
                    rp.IsValidFormat = ValidateFormat(rp, tp[tp_idx]);
                    tp[tp_idx].Front.AssignedRunPage = rp;

                    if (file.Pages > 1)
                    {
                        tp[tp_idx].Back.PrintIdx = pageIdx + 1;

                        
                        rp = pages[pageIdx];
                        rp.IsAssumed = true;
                        rp.IsValidFormat = ValidateFormat(rp, tp[tp_idx]);
                        tp[tp_idx].Back.AssignedRunPage = rp;

                    }
                    tp_idx--;
                }

                if (sheets[0].SheetPlaceType == TemplateSheetPlaceType.SingleSide || file.Pages == 1)
                {
                    pageIdx++;
                }
                else
                {
                    pageIdx += 2;
                }
                
               
            }
            sheets[0].Count = _result.SheetCount;
           
        }

        public class CalcResult
        {
            public List<FileResult> Files { get; set; } = new List<FileResult>();
            public int CountOnSheet { get; set; }
            public int TotalCount { get; set; }
            public int SheetCount { get; set; }
            public CalcResult(List<PdfFile> files)
            {
                Files.AddRange(files.Select(f => new FileResult(f)));
            }

            public void SetCountOnSheet(int count)
            {
                CountOnSheet = count;
            }

            public void Calc()
            {
                TotalCount = Files.Sum(f => f.Count);

                var freePlace = CountOnSheet;

                foreach (var file in Files)
                {
                    var count = (int)(CountOnSheet * (double)file.Count / TotalCount);
                    file.PagesOnSheet = count;
                    
                    freePlace -= count;

                    if (file.PagesOnSheet == 0)
                    {
                        file.PagesOnSheet = 1;
                    }
                    file.SheetCount = (int)Math.Ceiling((double)file.Count / file.PagesOnSheet);
                }

                if (freePlace > 0) // є вільні місця, візьмемо з найбільшим тиражем і розподілимо
                {
                    var maxFile = Files.OrderByDescending(f => f.Count).First();
                    maxFile.PagesOnSheet += freePlace;
                    maxFile.SheetCount = (int)Math.Ceiling((double)maxFile.Count / maxFile.PagesOnSheet);
                }


                SheetCount = Files.Max(f => f.SheetCount);
                Files.ForEach(f=>f.Wasted = f.PagesOnSheet*SheetCount - f.Count);
            }
        }

        public class FileResult
        {
            public PdfFile File { get; set; }
            public int Count { get; set; }
            public int Pages { get; set; }
            public int PagesOnSheet { get; set; }
            public int Wasted { get; set; }
            public int SheetCount { get; set; }
            public FileResult(PdfFile file)
            {
                File = file;
                Count = file.Count;
                Pages = file.Pages.Length;
            }
        }
    }
}
