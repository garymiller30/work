using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Processes
{
    public static class ProcessFixBleeds
    {
        // Допоміжний запис (Record) для кешування
        private record struct PageGeometry(
            TemplatePage Page,
            RectangleD PageRect,
            RectangleD BleedLeft,
            RectangleD BleedRight,
            RectangleD BleedTop,
            RectangleD BleedBottom
        );


        public static void Front(TemplatePageContainer templatePageContainer)
        {
            var pages = templatePageContainer.TemplatePages;
            if (pages.Count <= 1) return;

            // 1. Скидаємо бліди до дефолтних значень
            foreach (var page in pages)
            {
                page.Bleeds.Set(page.Bleeds.Default);
            }

            // 2. Кешуємо геометрію всіх сторінок за 1 прохід, щоб не обчислювати в O(N^2)
            var pageGeometries = new PageGeometry[pages.Count];
            for (int i = 0; i < pages.Count; i++)
            {
                var p = pages[i];
                (double x, double y, double w, double h) = ScreenDrawCommons.GetPageDraw(p, p.Front);

                pageGeometries[i] = new PageGeometry(
                    Page: p,
                    PageRect: new RectangleD(x, y, x + w, y + h),
                    BleedLeft: ScreenDrawCommons.GetDrawBleedLeftFront(p),
                    BleedRight: ScreenDrawCommons.GetDrawBleedRightFront(p),
                    BleedTop: ScreenDrawCommons.GetDrawBleedTopFront(p),
                    BleedBottom: ScreenDrawCommons.GetDrawBleedBottomFront(p)
                );
            }

            // 3. Обчислюємо перетини
            for (int i = 0; i < pageGeometries.Length; i++)
            {
                var current = pageGeometries[i];
                var page = current.Page;
                var m = page.Margins;

                // Отримуємо бліди поточної сторінки
                (RectangleD left, RectangleD right, RectangleD top, RectangleD bottom) = ScreenDrawCommons.GetDrawBleedsFront(page);

                // Прапори, щоб не перевизначати бліди кілька разів, якщо знайшли перетин
                bool hasLeft = false, hasRight = false, hasTop = false, hasBottom = false;

                for (int j = 0; j < pageGeometries.Length; j++)
                {
                    if (i == j) continue; // Пропускаємо саму себе

                    var target = pageGeometries[j];

                    // Перевірка для Left
                    if (!hasLeft && (left.IntersectsWith(target.PageRect) ||
                                     left.IntersectsWith(target.BleedLeft) ||
                                     left.IntersectsWith(target.BleedRight) ||
                                     left.IntersectsWith(target.BleedTop) ||
                                     left.IntersectsWith(target.BleedBottom)))
                    {
                        page.Bleeds.Left = m.Left;
                        hasLeft = true;
                    }

                    // Перевірка для Right
                    if (!hasRight && (right.IntersectsWith(target.PageRect) ||
                                      right.IntersectsWith(target.BleedLeft) ||
                                      right.IntersectsWith(target.BleedRight) ||
                                      right.IntersectsWith(target.BleedTop) ||
                                      right.IntersectsWith(target.BleedBottom)))
                    {
                        page.Bleeds.Right = m.Right;
                        hasRight = true;
                    }

                    // Перевірка для Top
                    if (!hasTop && (top.IntersectsWith(target.PageRect) ||
                                    top.IntersectsWith(target.BleedLeft) ||
                                    top.IntersectsWith(target.BleedRight) ||
                                    top.IntersectsWith(target.BleedTop) ||
                                    top.IntersectsWith(target.BleedBottom)))
                    {
                        page.Bleeds.Top = m.Top;
                        hasTop = true;
                    }

                    // Перевірка для Bottom
                    if (!hasBottom && (bottom.IntersectsWith(target.PageRect) ||
                                       bottom.IntersectsWith(target.BleedLeft) ||
                                       bottom.IntersectsWith(target.BleedRight) ||
                                       bottom.IntersectsWith(target.BleedTop) ||
                                       bottom.IntersectsWith(target.BleedBottom)))
                    {
                        page.Bleeds.Bottom = m.Bottom;
                        hasBottom = true;
                    }

                    // Якщо всі бліди вже змінено, далі target-сторінки можна не перевіряти для цієї сторінки
                    if (hasLeft && hasRight && hasTop && hasBottom)
                        break;
                }
            }

        }
    }
}
