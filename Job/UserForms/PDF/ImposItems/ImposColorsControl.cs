using JobSpace.Static.Pdf.Imposition.Models;
using SharpCompress;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                Name = "cyan",
                MarkColor = MarkColor.Cyan,
            };

            var m = new ImposColor
            {
                Name = "magenta",
                MarkColor = MarkColor.Magenta,
            };

            var y = new ImposColor
            {
                Name = "yellow",
                MarkColor = MarkColor.Yellow,
            };

            var k = new ImposColor
            {
                Name = "black",
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
            objectListView1.AddObjects(colors.Colors);
        }
    }
}
