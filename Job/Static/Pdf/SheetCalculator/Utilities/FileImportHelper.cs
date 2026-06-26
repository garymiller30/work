using System;
using System.IO;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;

namespace JobSpace.Static.Pdf.SheetCalculator.Utilities
{
    /// <summary>
    /// Helpers for importing products from files (features 11 &amp; 12).
    /// </summary>
    public static class FileImportHelper
    {
        private const int DefaultCirculation = 1000;

        // Matches:  "Name#1000"  or  "Name #1000"  at end of string
        private static readonly Regex CirculationPattern =
            new Regex(@"^(.+?)\s*#(\d+)$", RegexOptions.Compiled);

        /// <summary>
        /// Parses a file name (without extension) to extract the product name
        /// and, optionally, the required circulation encoded as <c>#&lt;number&gt;</c>
        /// at the end of the name.
        /// </summary>
        /// <param name="fileNameWithoutExt">File base name without extension.</param>
        /// <returns>
        /// Tuple of (productName, circulation).
        /// If no circulation pattern is found, circulation defaults to
        /// <see cref="DefaultCirculation"/>.
        /// </returns>
        public static (string Name, int Circulation) ParseFileName(string fileNameWithoutExt)
        {
            if (string.IsNullOrWhiteSpace(fileNameWithoutExt))
                return ("Новий виріб", DefaultCirculation);

            var m = CirculationPattern.Match(fileNameWithoutExt.Trim());
            if (m.Success && int.TryParse(m.Groups[2].Value, out int circ) && circ > 0)
                return (m.Groups[1].Value.Trim(), circ);

            return (fileNameWithoutExt.Trim(), DefaultCirculation);
        }

        /// <summary>
        /// Tries to read the physical dimensions of the first page of a PDF file.
        /// Converts from PDF points (1 pt = 1/72 inch) to millimetres.
        /// </summary>
        /// <param name="filePath">Absolute path to the PDF file.</param>
        /// <param name="widthMm">Page width in millimetres.</param>
        /// <param name="heightMm">Page height in millimetres.</param>
        /// <returns><c>true</c> on success; <c>false</c> on any error.</returns>
        public static bool TryReadPdfDimensions(string filePath,
                                                out double widthMm,
                                                out double heightMm)
        {
            widthMm = 0;
            heightMm = 0;

            try
            {
                if (!File.Exists(filePath)) return false;

                using (var reader = new PdfReader(filePath))
                {
                    // GetPageSizeWithRotation returns the effective media box
                    // after applying any /Rotate attribute.
                    var size = reader.GetPageSizeWithRotation(1);
                    const double ptToMm = 25.4 / 72.0;
                    widthMm  = Math.Round(size.Width  * ptToMm, 1);
                    heightMm = Math.Round(size.Height * ptToMm, 1);
                    return widthMm > 0 && heightMm > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the file extension (lower-case, without the dot) for quick format checks.
        /// </summary>
        public static string GetExtension(string filePath)
            => Path.GetExtension(filePath)?.TrimStart('.').ToLowerInvariant() ?? string.Empty;
    }
}
