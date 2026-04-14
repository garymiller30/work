using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives;
using Org.BouncyCastle.Crypto.Modes;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public class ColorControlBar
    {

        public static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
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
                            new JsonDerivedType(typeof(Group), "group"),
                        }
                    };
                }
            }
        }
            }
        };

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<string> RowElementsId { get; set; } = new List<string>();

        public double Width { get => Palette.GetWidth(RowElementsId); }
        public double Height { get => Palette.DefaultHeight; }

        public ColorPalette Palette { get; set; }

        public ColorControlBar()
        {
            Palette = new ColorPalette();
        }
        public void Add(Group group)
        {
            RowElementsId.Add(group.Id);
        }

        public void Add(Triangle triangle)
        {
            RowElementsId.Add(triangle.Id);
        }

        public void Add(string colorId)
        {
           
            var rect = Palette.CreateRectangle(colorId);

            RowElementsId.Add(rect.Id);
        }

        public void Add(string colorId, double tint)
        {
            var rect = Palette.CreateRectangle(colorId, tint);
            RowElementsId.Add(rect.Id);
        }

        public void Add(IPrimitive figure)
        {
            RowElementsId.Add(figure.Id);
        }

        private void Draw(PDFlib p, PointD startCoord)
        {
            double x = startCoord.X;
            double y = startCoord.Y;

            double w = Palette.DefaultWidth;
            double h = Palette.DefaultHeight;

            foreach (var item in RowElementsId)
            {
                IPrimitive prim = Palette.GetElementById(item);
                if (prim != null)
                {
                    x = prim.Draw(p, Palette, x, y, w, h);
                }
            }
        }


        public static ColorControlBar CreateTest()
        {
            var ccb = new ColorControlBar();

            double defW = ccb.Palette.DefaultWidth;
            double defH = ccb.Palette.DefaultHeight;

            var colorW = ccb.Palette.AddBaseColor(MarkColor.White);
            var colorC = ccb.Palette.AddBaseColor(MarkColor.Cyan);
            var colorM = ccb.Palette.AddBaseColor(MarkColor.Magenta);
            var colorY = ccb.Palette.AddBaseColor(MarkColor.Yellow);
            var colorK = ccb.Palette.AddBaseColor(MarkColor.Black);
            var colorReg = ccb.Palette.AddBaseColor(MarkColor.Registration);
            var proof = ccb.Palette.AddBaseColor(MarkColor.ProofColor);
            var colorDarkGrey = ccb.Palette.AddBaseColor(new MarkColor { C = 50, M = 40, Y = 40, K = 0 });
            var colorLightGrey = ccb.Palette.AddBaseColor(new MarkColor { C = 25, M = 19, Y = 19, K = 0 });


            var groupCMYK = ccb.Palette.GroupAdd();

            groupCMYK.Add(colorC, colorM, colorY, colorK);

            var groupC = ccb.Palette.GroupAdd();
            groupC.Add(colorC, 40);
            groupC.Add(colorC, 80);

            var groupM = ccb.Palette.GroupAdd();
            groupM.Add(colorM, 40);
            groupM.Add(colorM, 80);

            var groupY = ccb.Palette.GroupAdd();
            groupY.Add(colorY, 40);
            groupY.Add(colorY, 80);

            // намалюємо хрест
            Figure registration = ccb.Palette.CreateFigure();
            registration.Primitives = new List<IPrimitive>
                {
                    new Line()
                    {
                        From = new PointD { X = 0, Y = ccb.Palette.DefaultHeight / 2 },
                        To = new PointD { X = ccb.Palette.DefaultWidth, Y = ccb.Palette.DefaultHeight / 2 },
                        StrokeId = colorReg
                    },
                    new Line()
                    {
                        From = new PointD { X = ccb.Palette.DefaultWidth / 2, Y = 0 },
                        To = new PointD { X = ccb.Palette.DefaultWidth / 2, Y = ccb.Palette.DefaultHeight },
                        StrokeId = colorReg
                    },
                     new Circle
                     {
                         Center = new PointD { X = ccb.Palette.DefaultWidth / 2, Y = ccb.Palette.DefaultHeight / 2 },
                         Radius = 2,
                         StrokeId = colorReg
                     }
                };


            Figure darkGrey = ccb.Palette.CreateFigure(new Triangle()
            {
                FillId = colorDarkGrey,
                Point1 = new PointD { X = 0, Y = 0 },
                Point2 = new PointD { X = ccb.Palette.DefaultWidth, Y = 0 },
                Point3 = new PointD { X = 0, Y = ccb.Palette.DefaultHeight - 0.5 }
            },
                    new Triangle()
                    {
                        FillId = colorK,
                        Tint = 50,
                        Point1 = new PointD { X = 0, Y = ccb.Palette.DefaultHeight - 0.5 },
                        Point2 = new PointD { X = ccb.Palette.DefaultWidth, Y = 0 },
                        Point3 = new PointD { X = ccb.Palette.DefaultWidth, Y = ccb.Palette.DefaultHeight - 0.5 }
                    });

            Figure lightGrey = ccb.Palette.CreateFigure(
                new Triangle()
                {
                    FillId = colorLightGrey,
                    Point1 = new PointD { X = 0, Y = 0 },
                    Point2 = new PointD { X = ccb.Palette.DefaultWidth, Y = 0 },
                    Point3 = new PointD { X = ccb.Palette.DefaultWidth, Y = ccb.Palette.DefaultHeight - 0.5 }
                },
                new Triangle()
                {
                    FillId = colorK,
                    Tint = 25,
                    Point1 = new PointD { X = 0, Y = 0 },
                    Point2 = new PointD { X = 0, Y = ccb.Palette.DefaultHeight - 0.5 },
                    Point3 = new PointD { X = ccb.Palette.DefaultWidth, Y = ccb.Palette.DefaultHeight - 0.5 }
                }
            );
            Figure rozetkaC = ccb.Palette.CreateFigure(
                    new Triangle(0, 0, defW / 2, defW / 2, 0, 0.96) { FillId = colorC },
                    new Triangle(0, 1.65, defW / 2, defW / 2, 0, 2.2) { FillId = colorC },
                    new Triangle(0, 2.75, defW / 2, defW / 2, 0, 3.33) { FillId = colorC },
                    new Triangle(0, 3.93, defW / 2, defW / 2, 0, 4.58) { FillId = colorC },
                    new Triangle(0, 5.5, defW / 2, defW / 2, 0.96, 5.5) { FillId = colorC },
                    new Triangle(1.65, 5.5, defW / 2, defW / 2, 2.2, 5.5) { FillId = colorC },
                    new Triangle(2.75, 5.5, defW / 2, defW / 2, 3.33, 5.5) { FillId = colorC },
                    new Triangle(3.93, 5.5, defW / 2, defW / 2, 4.58, 5.5) { FillId = colorC },
                    new Triangle(5.5, 5.5, defW / 2, defW / 2, 5.5, 4.54) { FillId = colorC },
                    new Triangle(5.5, 3.84, defW / 2, defW / 2, 5.5, 3.3) { FillId = colorC },
                    new Triangle(5.5, 2.75, defW / 2, defW / 2, 5.5, 2.16) { FillId = colorC },
                    new Triangle(5.5, 1.56, defW / 2, defW / 2, 5.5, 0.91) { FillId = colorC },
                    new Triangle(5.5, 0, defW / 2, defW / 2, 4.54, 0) { FillId = colorC },
                    new Triangle(3.84, 0, defW / 2, defW / 2, 3.3, 0) { FillId = colorC },
                    new Triangle(2.75, 0, defW / 2, defW / 2, 2.16, 0) { FillId = colorC },
                    new Triangle(1.56, 0, defW / 2, defW / 2, 0.91, 0) { FillId = colorC }
            );
            Figure rozetkaM = ccb.Palette.CreateFigure(rozetkaC, colorM);
            Figure rozetkaY = ccb.Palette.CreateFigure(rozetkaC, colorY);
            Figure rozetkaK = ccb.Palette.CreateFigure(rozetkaC, colorK);

            var groupGrayControl = ccb.Palette.GroupAdd();
            groupGrayControl.Add(darkGrey, lightGrey);

            ccb.Add(registration);
            ccb.Add(colorW);
            ccb.Add(groupCMYK);
            ccb.Add(groupGrayControl);
            ccb.Add(groupCMYK);
            ccb.Add(groupC);
            ccb.Add(rozetkaC);
            ccb.Add(groupCMYK);
            ccb.Add(groupGrayControl);
            ccb.Add(groupCMYK);
            ccb.Add(groupM);
            ccb.Add(groupCMYK);
            ccb.Add(groupGrayControl);
            ccb.Add(groupCMYK);
            ccb.Add(groupGrayControl);
            ccb.Add(groupCMYK);
            ccb.Add(rozetkaM);
            ccb.Add(colorW);
            ccb.Add(groupCMYK);
            ccb.Add(groupGrayControl);
            ccb.Add(groupCMYK);
            ccb.Add(groupY);
            ccb.Add(registration);
            ccb.Add(colorK, 40);
            ccb.Add(darkGrey);
            ccb.Add(groupCMYK);
            ccb.Add(groupGrayControl);
            ccb.Add(groupCMYK);
            ccb.Add(colorK, 80);
            ccb.Add(rozetkaY);
            ccb.Add(groupCMYK);
            ccb.Add(groupGrayControl);
            ccb.Add(groupCMYK);
            ccb.Add(groupGrayControl);
            ccb.Add(groupCMYK);
            ccb.Add(groupC);
            ccb.Add(groupCMYK);
            ccb.Add(groupGrayControl);
            ccb.Add(groupCMYK);
            ccb.Add(rozetkaK);
            ccb.Add(groupM);
            ccb.Add(groupCMYK);
            ccb.Add(groupGrayControl);
            ccb.Add(groupCMYK);
            ccb.Add(colorW);
            ccb.Add(registration);

            ccb.Palette.ReplaceColor(colorC,MarkColor.ProofColor);

            ccb.SaveToJson("!test.json");
            var ccb_copy = ccb.LoadFromJson("!test.json");
            PointD startCoord = new PointD();

            using (PDFlib p = new PDFlib())
            {
                var doc = p.begin_document("!test.pdf", "");
                p.begin_page_ext(ccb.Width * PdfHelper.mn, ccb.Height * PdfHelper.mn, "");
                ccb.Draw(p, startCoord);
                p.end_page_ext("");
                p.end_document("");
            }


            return ccb;

        }

        public void ReplaceColor(MarkColor colorC, MarkColor proof)
        {

        }

        public void SaveToJson(string fileName)
        {
            var json = JsonSerializer.Serialize(this, SerializerOptions);
            File.WriteAllText(fileName, json);
        }
        public ColorControlBar LoadFromJson(string fileName)
        {
            var json = File.ReadAllText(fileName);

            var test = JsonSerializer.Deserialize<ColorControlBar>(json, SerializerOptions);
            return test;
        }
    }
}
