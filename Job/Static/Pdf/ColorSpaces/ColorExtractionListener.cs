using iText.Kernel.Colors;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Colorspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.Kernel.Pdf.Colorspace.PdfSpecialCs;
using Microsoft.Extensions.Primitives;
using iText.Kernel.Pdf;

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


                // With the following code:
                var colorSpace = imageObject.GetPdfObject().GetAsName(PdfName.ColorSpace);
                if (colorSpace != null)
                {
                    ProcessColorSpace(colorSpace, "Image");
                }
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
                        _uniqueColors.Add("RGB");
                        break;
                    case "/DeviceCMYK":
                        _uniqueColors.Add("CMYK");
                        break;
                    default:
                        // Можна додати інші колірні простори, якщо потрібно
                        _uniqueColors.Add(colorSpaceRepresentation);
                        break;
                }
                
            }
        }

        private void ProcessColor(iText.Kernel.Colors.Color color, string usageType) // usageType не використовується в прикладі, але може бути корисним
        {
            if (color == null) return; // Колір може бути не встановлений

            string colorRepresentation = GetColorRepresentation(color);
            if (colorRepresentation != null)
            {
                _uniqueColors.Add(colorRepresentation);
            }
        }
        private string GetColorRepresentation(iText.Kernel.Colors.Color color)
        {
            PdfColorSpace cs = color.GetColorSpace();
            float[] components = color.GetColorValue();
            StringBuilder sb = new StringBuilder();

            switch (cs.GetType().Name) // Перевіряємо тип колірного простору
            {
                case "Gray":
                case nameof(CalGray):
                case nameof(DeviceGray):
                    sb.Append("Grayscale");
                    //sb.Append($"Grayscale({components[0]:F3})"); // :F3 для форматування з 3 знаками після коми
                    break;
                case "Rgb":
                case nameof(CalRgb):
                case nameof(DeviceRgb):
                    sb.Append("RGB");
                    //sb.Append($"RGB({components[0]:F3}, {components[1]:F3}, {components[2]:F3})");
                    break;
                case "Cmyk":
                case nameof(DeviceCmyk):
                    sb.Append("CMYK");
                    //sb.Append($"CMYK({components[0]:F3}, {components[1]:F3}, {components[2]:F3}, {components[3]:F3})");
                    break;

                case nameof(PdfSpecialCs.Separation):
                    var separationCs = (PdfSpecialCs.Separation)cs;
                    // Replace the line causing the error with the correct method call
                    string colorantName = separationCs.GetName().GetValue(); // Отримуємо ім'я Spot кольору
                    sb.Append("Spot");                                                         // Компонент [0] - це зазвичай tint (відтінок) для Spot кольору (0.0 = alternate, 1.0 = full colorant)
                    //sb.Append($"Spot(Name: '{colorantName}', Tint: {components[0]:F3})");
                    // Можна також додати інформацію про альтернативний простір: separationCs.GetAlternateColorSpace().GetType().Name
                    break;

                case nameof(PdfSpecialCs.Indexed):
                    // Indexed кольори посилаються на палітру. Отримати конкретний колір з палітри складніше.
                    // Можна просто записати індекс.
                    sb.Append("Indexed");
                    //sb.Append($"Indexed(Index: {(int)components[0]})");
                    // Для повного аналізу потрібно було б знайти PdfArray палітри і отримати звідти базовий колір.
                    break;

                case nameof(Pattern):
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

                case nameof(Lab):
                    sb.Append("Lab");
                    //sb.Append($"Lab({components[0]:F3}, {components[1]:F3}, {components[2]:F3})");
                    break;
                case nameof(IccBased):
                    sb.Append("ICCBased");
                    //sb.Append($"ICCBased(");
                    //for (int i = 0; i < components.Length; ++i)
                    //{
                    //    sb.Append($"{components[i]:F3}");
                    //    if (i < components.Length - 1) sb.Append(", ");
                    //}
                    //sb.Append(")");
                    break;
                case nameof(PdfSpecialCs.DeviceN):
                default:
                    sb.Append("Unknown");
                    // Невідомий або непідтримуваний колірний простір
                    //sb.Append($"Unknown({cs.GetType().Name}, Components: {string.Join(", ", components.Select(c => c.ToString("F3")))})");
                    break;
            }

            return sb.Length > 0 ? sb.ToString() : null;
        }
        public ICollection<EventType> GetSupportedEvents()
        {
            // Повертаємо null, щоб отримувати всі типи подій, або вказуємо конкретні:
            // return new HashSet<EventType> { EventType.RENDER_PATH, EventType.RENDER_TEXT, EventType.RENDER_IMAGE };
            return null;
        }
    }
}
