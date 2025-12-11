using JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    /// <summary>
    /// зберігаються елементи палітри з яких складається контрольна шкала кольорів
    /// </summary>
    public class ColorPalette
    {
        /// <summary>
        /// ширина елемента палітри за замовчуванням
        /// </summary>
        public double DefaultWidth { get; set; }
        /// <summary>
        /// висота елемента палітри за замовчуванням
        /// </summary>
        public double DefaultHeight { get; set; }

        public List<MarkColor> BaseColors { get; set; } = new List<MarkColor>();

        public List<IPrimitive> Elements { get; set; } = new List<IPrimitive>();

        public ColorPalette(double w, double h)
        {
            DefaultHeight = h;
            DefaultWidth = w;
        }

        public ColorPalette()
        {
            DefaultHeight = 6;
            DefaultWidth = 5.5;
        }

        public string AddBaseColor(MarkColor color)
        {
            BaseColors.Add(color);
            return color.Id;
        }

        public Group GroupAdd()
        {
            var group = new Group(DefaultWidth, DefaultHeight);
            Elements.Add(group);
            return group;
        }

        public MarkColor GetBaseColorById(string id)
        {
            var color = BaseColors.FirstOrDefault(x => x.Id == id);
            return color;
        }

        public IPrimitive CreateRectangle(string colorId)
        {
            var rect = FindRectangleByColorId(colorId);
            if (rect == null)
            {
                rect = new Rectangle { FillId = colorId, W = DefaultWidth, H = DefaultHeight };
                Elements.Add(rect);
            }
            return rect;
        }

        public IPrimitive CreateRectangle(string colorId, double tint)
        {
            var rect = FindRectangleByColorId(colorId, tint);
            if (rect == null)
            {
                rect = new Rectangle { FillId = colorId, Tint = tint, W = DefaultWidth, H = DefaultHeight };
                Elements.Add(rect);
            }

            return rect;
        }

        public IPrimitive GetElementById(string item)
        {
            return Elements.FirstOrDefault(x => x.Id == item);
        }

        public Figure CreateFigure()
        {
            var fig = new Figure();
            Elements.Add(fig);
            return fig;

        }

        public Figure CreateFigure(params IPrimitive[] elements)
        {
            var fig = new Figure();
            fig.Primitives.AddRange(elements);
            Elements.Add(fig);
            return fig;
        }

        public Figure CreateFigure(Figure template, string colorId)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers =
                    {
                        ti =>
                        {
                            if (ti.Type == typeof(IPrimitive))
                            {
                                ti.PolymorphismOptions = new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "$type",
                                    IgnoreUnrecognizedTypeDiscriminators = true,
                                    DerivedTypes =
                                    {
                                        new JsonDerivedType(typeof(Triangle), "triangle"),
                                        new JsonDerivedType(typeof(Rectangle), "rectangle"),
                                        new JsonDerivedType(typeof(Line), "line"),
                                        new JsonDerivedType(typeof(Circle), "circle"),
                                        new JsonDerivedType(typeof(Figure), "figure"),

                                    }
                                };
                            }
                        }
                    }
                }
            };

            var fig = new Figure();

            var json = JsonSerializer.Serialize(template.Primitives, options);
            fig.Primitives = JsonSerializer.Deserialize<List<IPrimitive>>(json, options);

            foreach (PrimitiveAbstract primitive in fig.Primitives)
            {
                primitive.FillId = colorId;
            }
            Elements.Add(fig);
            return fig;
        }

        public double GetWidth(List<string> rowElements)
        {
            double w = 0;

            double GetGroupWidth(Group group, double defW)
            {
                double gw = 0;
                foreach (var item in group.Items)
                {
                    if (item is Group g2)
                    {
                        gw += GetGroupWidth(g2, defW);
                    }
                    else
                    {
                        gw += defW;
                    }
                }
                return gw;
            }


            foreach (string rowElement in rowElements)
            {
                var item = GetElementById(rowElement);

                if (item is Group group)
                {
                    w += GetGroupWidth(group, DefaultWidth);
                }
                else
                {
                    w += DefaultWidth;
                }

            }

            return w;
        }

        public IPrimitive FindRectangleByColorId(string colorId)
        {
            return Elements.FirstOrDefault(x => (x is Rectangle r) && r.FillId == colorId && r.Tint == 100);
        }

        public IPrimitive FindRectangleByColorId(string colorId, double tint)
        {
            return Elements.FirstOrDefault(x => (x is Rectangle r) && r.FillId == colorId && r.Tint == tint);
        }

        public void ReplaceColor(string colorId, MarkColor color)
        {
            var old_color = GetBaseColorById(colorId);
            if (old_color != null)
            {
                BaseColors.Remove(old_color);
                color.Id = colorId;
                BaseColors.Add(color);
            }
        }
    }
}