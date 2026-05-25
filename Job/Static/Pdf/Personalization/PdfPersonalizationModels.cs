using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace JobSpace.Static.Pdf.Personalization
{
    public enum PersonalizationLayerType
    {
        BasePdf,
        Pdf,
        Text,
        Code
    }

    public enum PersonalizationCodeType
    {
        Code128,
        Ean13,
        Ean8,
        Qr
    }

    public enum PersonalizationAnchorPoint
    {
        BottomLeft,
        LeftCenter,
        TopLeft,
        BottomCenter,
        Center,
        TopCenter,
        BottomRight,
        RightCenter,
        TopRight
    }

    public sealed class PdfPersonalizationSettings
    {
        public string BasePdfPath { get; set; }
        public string DataFilePath { get; set; }
        public string OutputFolder { get; set; }
        public List<PdfPersonalizationLayer> Layers { get; set; } = new List<PdfPersonalizationLayer>();
    }

    public sealed class PdfPersonalizationLayer
    {
        public bool Enabled { get; set; } = true;
        public PersonalizationLayerType Type { get; set; }
        public string Source { get; set; }
        public double Xmm { get; set; }
        public double Ymm { get; set; }
        public PersonalizationAnchorPoint BaseAnchor { get; set; } = PersonalizationAnchorPoint.BottomLeft;
        public PersonalizationAnchorPoint Anchor { get; set; } = PersonalizationAnchorPoint.BottomLeft;
        public double Rotation { get; set; }
        public double ScalePercent { get; set; } = 100;
        public PersonalizationCodeType CodeType { get; set; } = PersonalizationCodeType.Code128;
        public double TargetWidthMm { get; set; }
        public double TargetHeightMm { get; set; }
        public bool ShowHumanReadableText { get; set; } = true;
        public string FontName { get; set; } = "Arial";
        public double FontSize { get; set; } = 12;
        public double C { get; set; }
        public double M { get; set; }
        public double Y { get; set; }
        public double K { get; set; } = 100;
    }

    public sealed class PdfPersonalizationData
    {
        public List<string> Headers { get; } = new List<string>();
        public List<Dictionary<string, string>> Rows { get; } = new List<Dictionary<string, string>>();
        public string SourceFolder { get; private set; }

        public static PdfPersonalizationData Load(string filePath)
        {
            var data = new PdfPersonalizationData();
            data.SourceFolder = Path.GetDirectoryName(filePath) ?? string.Empty;

            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
                return data;

            data.Headers.AddRange(SplitTabLine(lines[0]));

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                var values = SplitTabLine(lines[i]);
                var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                for (int c = 0; c < data.Headers.Count; c++)
                {
                    string value = c < values.Count ? values[c] : string.Empty;
                    row[data.Headers[c]] = value;
                }

                data.Rows.Add(row);
            }

            return data;
        }

        public string GetValue(IReadOnlyDictionary<string, string> row, string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            return row.TryGetValue(source, out string value) ? value : source;
        }

        public string ResolveFile(IReadOnlyDictionary<string, string> row, string source)
        {
            string value = GetValue(row, source);
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.Trim().Trim('"');
            return Path.IsPathRooted(value) ? value : Path.Combine(SourceFolder, value);
        }

        public IReadOnlyList<string> Columns => Headers.ToList();

        private static List<string> SplitTabLine(string line)
        {
            var result = new List<string>();
            bool quoted = false;
            var value = new System.Text.StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];

                if (ch == '"')
                {
                    if (quoted && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        value.Append('"');
                        i++;
                    }
                    else
                    {
                        quoted = !quoted;
                    }
                    continue;
                }

                if (ch == '\t' && !quoted)
                {
                    result.Add(value.ToString());
                    value.Clear();
                }
                else
                {
                    value.Append(ch);
                }
            }

            result.Add(value.ToString());
            return result;
        }

        public static double ParseDouble(object value, double fallback)
        {
            if (value == null)
                return fallback;

            string text = System.Convert.ToString(value, CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(text))
                return fallback;

            text = text.Replace(',', '.');
            return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double result)
                ? result
                : fallback;
        }
    }
}
