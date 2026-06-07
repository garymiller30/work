using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

public class Cf2Renderer
{
    // Клас для збереження ліній з файлу
    private class CadLine
    {
        public PointF Start;
        public PointF End;
        public int SubType; // 1 = різання, інші = згин
    }

    public static Bitmap RenderCf2ToBitmap(string filePath, int targetWidth, int targetHeight)
    {
        var lines = File.ReadAllLines(filePath);
        var cadLines = new List<CadLine>();

        // Змінні для меж креслення (Bounding Box)
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        // Крок 1: Парсинг файлу
        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;

            string[] parts = trimmed.Split(',');

            // Обробка лінії: L,тип,субтип,флаг,X1,Y1,X2,Y2
            if (parts[0] == "L" && parts.Length >= 8)
            {
                try
                {
                    int subType = int.Parse(parts[2]);
                    float x1 = float.Parse(parts[4], CultureInfo.InvariantCulture);
                    float y1 = float.Parse(parts[5], CultureInfo.InvariantCulture);
                    float x2 = float.Parse(parts[6], CultureInfo.InvariantCulture);
                    float y2 = float.Parse(parts[7], CultureInfo.InvariantCulture);

                    cadLines.Add(new CadLine
                    {
                        Start = new PointF(x1, y1),
                        End = new PointF(x2, y2),
                        SubType = subType
                    });

                    // Оновлення меж для автоматичного масштабування
                    minX = Math.Min(minX, Math.Min(x1, x2));
                    minY = Math.Min(minY, Math.Min(y1, y2));
                    maxX = Math.Max(maxX, Math.Max(x1, x2));
                    maxY = Math.Max(maxY, Math.Max(y1, y2));
                }
                catch { /* Пропускаємо рядки з помилками формату */ }
            }
            // ПРИМІТКА: Для спрощення дуги (A) тут пропущені, або їх можна малювати як лінії між X1,Y1 та X2,Y2
        }

        if (cadLines.Count == 0) return null;

        // Крок 2: Створення Bitmap та малювання
        Bitmap bmp = new Bitmap(targetWidth, targetHeight);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Створення інструментів малювання
            using (Pen cutPen = new Pen(Color.Red, 1.5f))
            using (Pen creasePen = new Pen(Color.Blue, 1.0f) { DashStyle = DashStyle.Dash })
            {
                // Обчислення коефіцієнта масштабування з урахуванням відступів (padding = 20px)
                int padding = 20;
                float cadWidth = maxX - minX;
                float cadHeight = maxY - minY;

                float scaleX = (targetWidth - padding * 2) / cadWidth;
                float scaleY = (targetHeight - padding * 2) / cadHeight;
                float scale = Math.Min(scaleX, scaleY); // Зберігаємо пропорції

                // Малюємо кожну лінію
                foreach (var line in cadLines)
                {
                    // Трансформація координат (CAD Y йде знизу вгору, а на екрані — зверху вниз)
                    float x1 = padding + (line.Start.X - minX) * scale;
                    float y1 = targetHeight - padding - (line.Start.Y - minY) * scale;
                    float x2 = padding + (line.End.X - minX) * scale;
                    float y2 = targetHeight - padding - (line.End.Y - minY) * scale;

                    // Вибір олівця залежно від субтипу лінії
                    Pen currentPen = (line.SubType == 1) ? cutPen : creasePen;

                    g.DrawLine(currentPen, x1, y1, x2, y2);
                }
            }
        }

        return bmp;
    }
}
