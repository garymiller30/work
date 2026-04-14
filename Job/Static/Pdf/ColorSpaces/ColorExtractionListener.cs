using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Colorspace;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.Kernel.Pdf.Colorspace.PdfCieBasedCs;


namespace JobSpace.Static.Pdf.ColorSpaces
{
    public class ColorExtractionListener : IEventListener
    {
        // Використовуємо HashSet для зберігання унікальних кольорів
        // Ключ - це рядкове представлення кольору (включаючи простір)
        private readonly HashSet<string> _uniqueColors = new HashSet<string>();

        public ICollection<string> GetUniqueColors()
        {
            return _uniqueColors;
        }

        public void EventOccurred(IEventData data, EventType type)
        {
            // Нас цікавлять події, де колір може бути використаний (малювання, текст)
            // Перевірка графічного стану в цих подіях дає актуальні кольори.
            if (type == EventType.RENDER_PATH || type == EventType.RENDER_TEXT)
            {
                // Отримуємо поточний графічний стан
                var gs = data.GetGraphicsState();

                // Аналізуємо колір заповнення (Fill Color)
                ProcessColor(gs.GetFillColor(), "Fill");

                // Аналізуємо колір обведення (Stroke Color)
                ProcessColor(gs.GetStrokeColor(), "Stroke");
            }
            else if (type == EventType.RENDER_IMAGE)
            {
                var renderInfo = (ImageRenderInfo)data;
                var imageObject = renderInfo.GetImage();

                if (imageObject == null) return;

                var colorSpace = imageObject.GetPdfObject().Get(PdfName.ColorSpace);
                ProcessColorSpaceObject(colorSpace, "Image");
            }
        }

        private void ProcessColorSpaceObject(PdfObject colorSpace, string usageType)
        {
            if (colorSpace == null) return;

            if (colorSpace.IsIndirectReference())
            {
                ProcessColorSpaceObject(((PdfIndirectReference)colorSpace).GetRefersTo(), usageType);
                return;
            }

            if (colorSpace.IsName())
            {
                ProcessColorSpace((PdfName)colorSpace, usageType);
                return;
            }

            if (!colorSpace.IsArray())
            {
                return;
            }

            var array = (PdfArray)colorSpace;
            if (array.Size() == 0)
            {
                return;
            }

            var first = array.Get(0);
            if (first == null)
            {
                return;
            }

            if (first.IsIndirectReference())
            {
                first = ((PdfIndirectReference)first).GetRefersTo();
            }

            if (!(first is PdfName firstName))
            {
                return;
            }

            var colorSpaceName = firstName.GetValue();
            switch (colorSpaceName)
            {
                case "Separation":
                    var separationName = array.GetAsName(1)?.GetValue();
                    if (!string.IsNullOrWhiteSpace(separationName))
                    {
                        _uniqueColors.Add(separationName.TrimStart('/'));
                    }
                    break;
                case "DeviceN":
                    var names = array.GetAsArray(1);
                    if (names != null)
                    {
                        for (int i = 0; i < names.Size(); i++)
                        {
                            var colorName = names.GetAsName(i)?.GetValue();
                            if (!string.IsNullOrWhiteSpace(colorName))
                            {
                                _uniqueColors.Add(colorName.TrimStart('/'));
                            }
                        }
                    }
                    break;
                case "ICCBased":
                    var profile = array.GetAsStream(1);
                    var components = profile?.GetAsNumber(PdfName.N)?.IntValue();
                    var iccProfileLabel = GetIccProfileLabel(components);
                    if (!string.IsNullOrWhiteSpace(iccProfileLabel))
                    {
                        _uniqueColors.Add(iccProfileLabel);
                    }
                    if (components == 1)
                    {
                        _uniqueColors.Add("K");
                    }
                    break;
                default:
                    ProcessColorSpace(firstName, usageType);
                    break;
            }
        }

        private void ProcessColorSpace(PdfName colorSpace, string usageType)
        {
            if (colorSpace == null) return;

            string colorSpaceRepresentation = colorSpace.ToString();
            if (!string.IsNullOrEmpty(colorSpaceRepresentation))
            {
                switch (colorSpaceRepresentation)
                {
                    case "/DeviceRGB":
                        if (string.Equals(usageType, "Image", StringComparison.OrdinalIgnoreCase))
                        {
                            _uniqueColors.Add("RGB");
                        }
                        break;
                    case "/DeviceGray":
                        _uniqueColors.Add("K");
                        break;
                    default:
                        break;
                }

            }
        }

        private void ProcessColor(iText.Kernel.Colors.Color color, string usageType) // usageType не використовується в прикладі, але може бути корисним
        {
            if (color == null) return; // Колір може бути не встановлений

            if (TryAddProcessColorComponents(color))
            {
                return;
            }

            string colorRepresentation = GetColorRepresentation(color);
            if (colorRepresentation != null)
            {
                _uniqueColors.Add(colorRepresentation);
            }
        }

        private bool TryAddProcessColorComponents(iText.Kernel.Colors.Color color)
        {
            var cs = color.GetColorSpace();
            var components = color.GetColorValue();

            switch (cs.GetType().Name)
            {
                case "Gray":
                case nameof(iText.Kernel.Colors.CalGray):
                case nameof(DeviceGray):
                    _uniqueColors.Add("K");
                    return true;
                case "Rgb":
                case nameof(iText.Kernel.Colors.CalRgb):
                case nameof(DeviceRgb):
                    _uniqueColors.Add("RGB");
                    return true;
                case "Cmyk":
                case nameof(DeviceCmyk):
                    const float epsilon = 0.0001f;
                    if (components.Length > 0 && components[0] > epsilon) _uniqueColors.Add("Cyan");
                    if (components.Length > 1 && components[1] > epsilon) _uniqueColors.Add("Magenta");
                    if (components.Length > 2 && components[2] > epsilon) _uniqueColors.Add("Yellow");
                    if (components.Length > 3 && components[3] > epsilon) _uniqueColors.Add("K");
                    return true;
                default:
                    return false;
            }
        }

        private string GetColorRepresentation(iText.Kernel.Colors.Color color)
        {

            StringBuilder sb = new StringBuilder();
            PdfColorSpace cs = color.GetColorSpace();
            float[] components = color.GetColorValue();

            switch (cs.GetType().Name) // Перевіряємо тип колірного простору
            {
                case "Gray":
                case nameof(iText.Kernel.Colors.CalGray):
                case nameof(DeviceGray):
                    sb.Append("K");
                    //sb.Append($"Grayscale({components[0]:F3})"); // :F3 для форматування з 3 знаками після коми
                    break;
                case "Rgb":
                case nameof(iText.Kernel.Colors.CalRgb):
                case nameof(DeviceRgb):
                    break;
                case "Cmyk":
                case nameof(DeviceCmyk):
                    break;

                case nameof(PdfSpecialCs.Separation):
                    var separationCs = (PdfSpecialCs.Separation)cs;
                    // Replace the line causing the error with the correct method call
                    string colorantName = separationCs.GetName().GetValue(); // Отримуємо ім'я Spot кольору
                    sb.Append(colorantName);                                                         // Компонент [0] - це зазвичай tint (відтінок) для Spot кольору (0.0 = alternate, 1.0 = full colorant)
                                                                                               //sb.Append($"Spot(Name: '{colorantName}', Tint: {components[0]:F3})");
                                                                                               // Можна також додати інформацію про альтернативний простір: separationCs.GetAlternateColorSpace().GetType().Name
                    break;

                case nameof(PdfSpecialCs.Indexed):
                    break;

                case nameof(PdfSpecialCs.Pattern):
                    sb.Append("Pattern");
                    // Pattern кольори дуже складні (можуть бути зафарбовані або незафарбовані)
                    //if (color is PatternColor patternColor)
                    //{
                    //    if (patternColor.GetPattern() != null)
                    //        sb.Append($"Pattern(Name/Ref: {patternColor.GetPattern().GetPdfObject().GetIndirectReference()?.ToString() ?? "Inline"})");
                    //    else
                    //        sb.Append("Pattern(Unknown)");
                    //}
                    //else
                    //{
                    //    sb.Append("Pattern(Generic)");
                    //}
                    break;

                //case nameof(PdfSpecialCs.DeviceN):
                //    var deviceNCs = (PdfSpecialCs.DeviceN)cs;
                //    var names = deviceNCs.GetNames();
                //    sb.Append("DeviceN(");
                //    for (int i = 0; i < names.Count() && i < components.Length; ++i)
                //    {
                //        sb.Append($"{names.ToArray()[i]}={components[i]:F3}");
                //        if (i < components.Length - 1) sb.Append(", ");
                //    }
                //    sb.Append(")");
                //    break;

                case nameof(iText.Kernel.Colors.Lab):
                    sb.Append("Lab");
                    //sb.Append($"Lab({components[0]:F3}, {components[1]:F3}, {components[2]:F3})");
                    break;
                case nameof(iText.Kernel.Pdf.Colorspace.PdfCieBasedCs.IccBased):
                    var iccProfileColorLabel = GetIccProfileLabel(components.Length);
                    if (!string.IsNullOrWhiteSpace(iccProfileColorLabel))
                    {
                        sb.Append(iccProfileColorLabel);
                    }

                    if (components.Length == 1)
                    {
                        if (sb.Length > 0)
                        {
                            _uniqueColors.Add(sb.ToString());
                            sb.Clear();
                        }

                        sb.Append("K");
                    }
                    break;
                case nameof(PdfSpecialCs.DeviceN):
                default:
                    break;
            }




            return sb.Length > 0 ? sb.ToString() : null;
        }

        private static string GetIccProfileLabel(int? components)
        {
            switch (components)
            {
                case 4:
                    return "ICC Profile (CMYK)";
                case 3:
                    return "ICC Profile (RGB)";
                case 1:
                    return "ICC Profile (Gray)";
                default:
                    return null;
            }
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            // Повертаємо null, щоб отримувати всі типи подій, або вказуємо конкретні:
            // return new HashSet<EventType> { EventType.RENDER_PATH, EventType.RENDER_TEXT, EventType.RENDER_IMAGE };
            return null;
        }


        public static (string Name, bool ProfileExists) CheckICCBasedProfile(PdfColorSpace cs)
        {
            // 1. Перевірка типу на ICCBased
            if (cs is iText.Kernel.Pdf.Colorspace.PdfCieBasedCs.IccBased iccBasedCs)
            {
                // 2. Отримуємо базовий об'єкт колірного простору
                PdfObject pdfObject = iccBasedCs.GetPdfObject();

                PdfDictionary profileDict = null;

                // ICCBased – це масив: [/ICCBased, Словник_Профілю]
                if (pdfObject is PdfArray array && array.Size() > 1)
                {
                    // iText 9.x часто автоматично розв'язує посилання, але безпечніше спробувати GetAsDictionary,
                    // який вміє працювати з непрямими посиланнями всередині масиву.
                    // Ми використовуємо Get(1) для отримання об'єкта та приводимо його до словника.
                    profileDict = array.GetAsDictionary(1); // Цей метод *може* автоматично розв'язати посилання
                }

                if (profileDict != null)
                {
                    // 3. Словник профілю має бути потоком (PdfStream)
                    if (profileDict is PdfStream iccProfileStream)
                    {
                        // Профіль знайдено (існує як потік даних)
                        PdfString description = iccProfileStream.GetAsString(PdfName.D);
                        string profileName = description != null
                            ? description.ToUnicodeString()
                            : "Unnamed Profile";

                        return ($"ICCBased: {profileName} (Components: {iccBasedCs.GetNumberOfComponents()})", true);
                    }
                    else
                    {
                        // Словник знайдено, але він не є потоком
                        return ("ICCBased: Dictionary Found, but Stream MISSING", false);
                    }
                }

                // 4. Невдала перевірка структури
                return ("ICCBased: Invalid Structure or Dictionary Missing", false);
            }

            // Для інших типів
            return (cs.GetType().Name, true);
        }
    }
}
