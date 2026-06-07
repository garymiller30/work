using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;

public class Cf2UltraRenderer
{
    private class CadLine
    {
        public PointF Start;
        public PointF End;
        public int SubType;
    }

    private class CadSubBlock
    {
        public string Name;
        public List<CadLine> Lines = new List<CadLine>();
    }

    private class CadInstance
    {
        public string SubName;
        public float X;
        public float Y;
        public float Angle;
    }

    public static Bitmap RenderFullLayout(string filePath, float dpi)
    {
        var lines = File.ReadAllLines(filePath);

        var subBlocks = new Dictionary<string, CadSubBlock>();
        var instances = new List<CadInstance>();

        CadSubBlock currentSub = null;
        bool inMain = false;

        // Крок 1: Парсинг файлу
        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;

            string[] parts = trimmed.Split(',');
            if (parts.Length == 0) continue;
            string command = parts[0];

            if (command == "MAIN")
            {
                inMain = true;
                currentSub = null;
                continue;
            }
            if (command == "SUB" && parts.Length >= 2)
            {
                inMain = false;
                currentSub = new CadSubBlock { Name = parts[1] };
                subBlocks[parts[1]] = currentSub;
                continue;
            }
            if (command == "END")
            {
                inMain = false;
                currentSub = null;
                continue;
            }

            // Парсинг ЛІНІЙ: L,тип,субтип,флаг,X1,Y1,X2,Y2
            if (currentSub != null && command == "L" && parts.Length >= 8)
            {
                try
                {
                    int subType = int.Parse(parts[2]);
                    float x1 = float.Parse(parts[4], CultureInfo.InvariantCulture);
                    float y1 = float.Parse(parts[5], CultureInfo.InvariantCulture);
                    float x2 = float.Parse(parts[6], CultureInfo.InvariantCulture);
                    float y2 = float.Parse(parts[7], CultureInfo.InvariantCulture);

                    currentSub.Lines.Add(new CadLine
                    {
                        Start = new PointF(x1, y1),
                        End = new PointF(x2, y2),
                        SubType = subType
                    });
                }
                catch { }
            }

            // Парсинг ДУГ (A) та перетворення їх на дрібні лінії для точності
            if (currentSub != null && command == "A" && parts.Length >= 11)
            {
                try
                {
                    int subType = int.Parse(parts[2]);
                    float x1 = float.Parse(parts[4], CultureInfo.InvariantCulture);
                    float y1 = float.Parse(parts[5], CultureInfo.InvariantCulture);
                    float x2 = float.Parse(parts[6], CultureInfo.InvariantCulture);
                    float y2 = float.Parse(parts[7], CultureInfo.InvariantCulture);
                    float cx = float.Parse(parts[8], CultureInfo.InvariantCulture);
                    float cy = float.Parse(parts[9], CultureInfo.InvariantCulture);
                    int direction = int.Parse(parts[10]); // -1 = CW, 1 = CCW

                    // Обчислюємо радіус та початковий/кінцевий кути
                    double r = Math.Sqrt((x1 - cx) * (x1 - cx) + (y1 - cy) * (y1 - cy));
                    double startAngle = Math.Atan2(y1 - cy, x1 - cx);
                    double endAngle = Math.Atan2(y2 - cy, x2 - cx);

                    // Коригуємо кути залежно від напрямку дуги
                    if (direction == -1 && endAngle > startAngle) endAngle -= 2 * Math.PI;
                    if (direction == 1 && endAngle < startAngle) endAngle += 2 * Math.PI;

                    // Кількість кроків для апроксимації (30 кроків дають гладку дугу)
                    int segments = 30;
                    PointF prevPoint = new PointF(x1, y1);

                    for (int i = 1; i <= segments; i++)
                    {
                        double t = (double)i / segments;
                        double currentAngle = startAngle + (endAngle - startAngle) * t;

                        float currX = (float)(cx + r * Math.Cos(currentAngle));
                        float currY = (float)(cy + r * Math.Sin(currentAngle));

                        // Якщо це останній крок — примусово ставимо точну кінцеву точку X2, Y2
                        if (i == segments) { currX = x2; currY = y2; }

                        currentSub.Lines.Add(new CadLine
                        {
                            Start = prevPoint,
                            End = new PointF(currX, currY),
                            SubType = subType
                        });

                        prevPoint = new PointF(currX, currY);
                    }
                }
                catch { }
            }

            // Парсинг ЕКЗЕМПЛЯРІВ розкладки: C,назва,X,Y,Angle
            if (inMain && command == "C" && parts.Length >= 5)
            {
                try
                {
                    string subName = parts[1];
                    float instX = float.Parse(parts[2], CultureInfo.InvariantCulture);
                    float instY = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    float angle = float.Parse(parts[4], CultureInfo.InvariantCulture);

                    instances.Add(new CadInstance
                    {
                        SubName = subName,
                        X = instX,
                        Y = instY,
                        Angle = angle
                    });
                }
                catch { }
            }
        }

        // Крок 2: Застосування матричних трансформацій (Поворот + Зміщення)
        var finalLines = new List<CadLine>();
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        foreach (var inst in instances)
        {
            if (subBlocks.TryGetValue(inst.SubName, out var sub))
            {
                using (Matrix matrix = new Matrix())
                {
                    matrix.Rotate(inst.Angle);
                    matrix.Translate(inst.X, inst.Y, MatrixOrder.Append);

                    foreach (var baseLine in sub.Lines)
                    {
                        PointF[] pts = new PointF[] { baseLine.Start, baseLine.End };
                        matrix.TransformPoints(pts);

                        finalLines.Add(new CadLine
                        {
                            Start = pts[0],
                            End = pts[1],
                            SubType = baseLine.SubType
                        });

                        minX = Math.Min(minX, Math.Min(pts[0].X, pts[1].X));
                        minY = Math.Min(minY, Math.Min(pts[0].Y, pts[1].Y));
                        maxX = Math.Max(maxX, Math.Max(pts[0].X, pts[1].X));
                        maxY = Math.Max(maxY, Math.Max(pts[0].Y, pts[1].Y));
                    }
                }
            }
        }

        if (finalLines.Count == 0) return null;

        // Крок 3: Рендеринг у Bitmap з DPI
        float mmWidth = maxX - minX;
        float mmHeight = maxY - minY;
        float mmPadding = 10f; // 10 мм відступ по краях

        float scale = (dpi / 25.4f);

        int pixelWidth = (int)Math.Ceiling((mmWidth + mmPadding * 2) * scale);
        int pixelHeight = (int)Math.Ceiling((mmHeight + mmPadding * 2) * scale);

        Bitmap bmp = new Bitmap(pixelWidth, pixelHeight);
        bmp.SetResolution(dpi, dpi);

        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Товщина ліній масштабується під обраний DPI
            float cutThickness = (0.3f / 25.4f) * dpi;
            float creaseThickness = (0.2f / 25.4f) * dpi;

            using (Pen cutPen = new Pen(Color.Red, Math.Max(1f, cutThickness)))
            using (Pen creasePen = new Pen(Color.Blue, Math.Max(1f, creaseThickness)) { DashStyle = DashStyle.Dash })
            {
                foreach (var line in finalLines)
                {
                    float x1 = (mmPadding + (line.Start.X - minX)) * scale;
                    float y1 = pixelHeight - (mmPadding + (line.Start.Y - minY)) * scale;

                    float x2 = (mmPadding + (line.End.X - minX)) * scale;
                    float y2 = pixelHeight - (mmPadding + (line.End.Y - minY)) * scale;

                    // Тип лінії: субтип 1 = різ (червоний), інші (наприклад, 2 або 92) = згин (синій пунктир)
                    Pen currentPen = (line.SubType == 1) ? cutPen : creasePen;
                    g.DrawLine(currentPen, x1, y1, x2, y2);
                }
            }
        }

        return bmp;
    }
}
