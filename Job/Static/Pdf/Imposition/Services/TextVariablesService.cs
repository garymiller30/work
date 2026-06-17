using JobSpace.Static.Pdf.Imposition.Services.TextVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public class TextVariablesService
    {
        public Dictionary<string, string> Values { get;set;} = new Dictionary<string, string>();

        TextVariableAbstract textVariables;

        public TextVariableAbstract TextVariableCommand
        {
            get
            {
                if (textVariables == null) InitTextCommand();
                return textVariables;
            }
        }

        private void InitTextCommand()
        {
            var c_simple = new TextVariableSimpleText(null);
            var c_fromService = new TextVariableFromService(c_simple);
            var c_datetime = new TextVariableDatetime(c_fromService);
            var c_cmyk = new TextVariableCMYK(c_datetime);
            var c_usedColors = new TextVariableUsedColor(c_cmyk);

            textVariables = c_usedColors;

        }

        public void SetValue(string key, object value)
        {
            if (Values.ContainsKey(key))
            {
                Values[key] = value.ToString();
            }
            else
            {
                Values.Add(key, value.ToString());
            }
        }


        public string ReplaceToRealValues(string? str)
        {
            if (string.IsNullOrEmpty( str)) return string.Empty;

            // Ініціалізуємо StringBuilder початковим рядком
            var sb = new StringBuilder(str);

            foreach (var (key, value) in Values)
            {
                sb.Replace(key, value);
            }

            return sb.ToString();
        }
    }

    public static class ValueList
    {
        public const string SheetIdx    = "$[sheetIdx]";
        public const string SheetSide   = "$[sheetSide]";
        public const string SheetDesc =   "$[sheetDesc]";
        public const string SheetFormat = "$[sheetFormat]";
        public const string SheetCount  = "$[sheetCount]";
        public const string OrderNo     = "$[orderNo]";
        public const string Customer    = "$[customer]";
        public const string OrderDesc   = "$[orderDesc]";
        public const string CurDate     = "$[dateTime]";
        public const string Cmyk        = "$[cmyk]";
        public const string UsedColor   = "$[usedColor]";
    }
}
