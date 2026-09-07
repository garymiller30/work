using JobSpace.Static;
using JobSpace.Static.Pdf.Imposition.Models;
using SharpCompress;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class ImposColorsControl : UserControl
    {
        public ImposColorsControl()
        {
            InitializeComponent();
            olvColumn_Name.AspectGetter += (r) => ((ImposColor)r).Name;
            olvColumn_Front.AspectGetter += (r) => ((ImposColor)r).IsFront;
            olvColumn_Back.AspectGetter += (r) => ((ImposColor)r).IsBack;

            olvColumn_Front.AspectPutter += (r,v) => ((ImposColor)r).IsFront = (bool)v;

            olvColumn_Back.AspectPutter += (r,v) => ((ImposColor)r).IsBack = (bool)v;
        }

        private void tsb_addCMYK_Click(object sender, EventArgs e)
        {
            CreateCmykColors();
        }

        private void CreateCmykColors()
        {
            var c = new ImposColor()
            {
                Name = "Cyan",
                MarkColor = MarkColor.Cyan,
            };

            var m = new ImposColor
            {
                Name = "Magenta",
                MarkColor = MarkColor.Magenta,
            };

            var y = new ImposColor
            {
                Name = "Yellow",
                MarkColor = MarkColor.Yellow,
            };

            var k = new ImposColor
            {
                Name = "Black",
                MarkColor = MarkColor.Black,
            };

            objectListView1.AddObjects(new[] { c, m, y, k });

        }

        private void tsb_addPantone_Click(object sender, EventArgs e)
        {
            using (var form = new FormSelectSpotColor())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var spot = form.SelectedSpotColor;

                    var s = new ImposColor
                    {
                        Name = spot.Name,
                        MarkColor = new MarkColor
                        {
                            IsSpot = true,
                            Name = spot.Name,
                            C = (double)spot.C,
                            M = (double)spot.M,
                            Y = (double)spot.Y,
                            K = (double)spot.K,
                        }
                    };

                    
                    objectListView1.AddObject(s);
                }
            }
        }

        public ImposColors GetUsedColors()
        {
            var colors = new ImposColors();

            if (objectListView1.Objects != null)
            {
                colors.Colors = objectListView1.Objects.Cast<ImposColor>().ToList();
            }

                return colors;
        }

        public void SetUsedColors(ImposColors colors)
        {
            objectListView1.ClearObjects();
            if (colors?.Colors == null) return;

            objectListView1.AddObjects(colors.Colors);
        }


        public void AddColorsFromFiles(IEnumerable<string> filePaths)
        {
            var allExtractedColors = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var file in filePaths)
            {
                if (!File.Exists(file)) continue;

                var fileInfoExt = new FileInfo(file).ToFileSystemInfoExt();
                PdfUtils.GetColorspaces(fileInfoExt);

                if (fileInfoExt.UsedColors != null)
                {
                    foreach (var col in fileInfoExt.UsedColors)
                    {
                        allExtractedColors.Add(col);
                    }
                }
            }

            var existingColors = objectListView1.Objects?.Cast<ImposColor>().ToList() ?? new List<ImposColor>();
            var colorsToAdd = new List<ImposColor>();

            bool hasC = allExtractedColors.Any(c => c.Equals("CMYK", StringComparison.OrdinalIgnoreCase) || c.Contains("C"));
            bool hasM = allExtractedColors.Any(c => c.Equals("CMYK", StringComparison.OrdinalIgnoreCase) || c.Contains("M"));
            bool hasY = allExtractedColors.Any(c => c.Equals("CMYK", StringComparison.OrdinalIgnoreCase) || c.Contains("Y"));
            bool hasK = allExtractedColors.Any(c => c.Equals("CMYK", StringComparison.OrdinalIgnoreCase) || c.Contains("K") || c.Equals("Gray", StringComparison.OrdinalIgnoreCase));

            // Додаємо CMYK-кольори (якщо ще не додані)
            void TryAddProcess(string name, MarkColor markColor)
            {
                if (!existingColors.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    colorsToAdd.Add(new ImposColor { Name = name, MarkColor = markColor, IsFront = true, IsBack = true });
                }
            }

            if (hasC) TryAddProcess("Cyan", MarkColor.Cyan);
            if (hasM) TryAddProcess("Magenta", MarkColor.Magenta);
            if (hasY) TryAddProcess("Yellow", MarkColor.Yellow);
            if (hasK) TryAddProcess("Black", MarkColor.Black);

            // Додаємо Spot/Pantone кольори
            var ignoredNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "CMYK", "RGB", "Lab", "Gray", "Indexed", "ICCBased", "All", "None", "Pattern"
    };

            foreach (var colorName in allExtractedColors)
            {
                if (ignoredNames.Contains(colorName) || IsProcessColorGroup(colorName))
                    continue;

                if (!existingColors.Any(x => x.Name.Equals(colorName, StringComparison.OrdinalIgnoreCase)))
                {
                    colorsToAdd.Add(new ImposColor
                    {
                        Name = colorName,
                        IsFront = true,
                        IsBack = true,
                        MarkColor = new MarkColor
                        {
                            IsSpot = true,
                            Name = colorName
                        }
                    });
                }
            }

            if (colorsToAdd.Count > 0)
            {
                objectListView1.AddObjects(colorsToAdd);
            }
        }

        private static bool IsProcessColorGroup(string name)
        {
            return !string.IsNullOrEmpty(name) && name.All(ch => ch == 'C' || ch == 'M' || ch == 'Y' || ch == 'K');
        }

    }
}
