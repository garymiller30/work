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
        public static Dictionary<string, string> Values = new Dictionary<string, string>();

        static TextVariableAbstract textVariables;

        public static TextVariableAbstract TextVariableCommand
        {
            get
            {
                if (textVariables == null) InitTextCommand();
                return textVariables;
            }
        }

        private static void InitTextCommand()
        {
            var c_simple = new TextVariableSimpleText(null);
            var c_fromService = new TextVariableFromService(c_simple);
            var c_datetime = new TextVariableDatetime(c_fromService);
            var c_cmyk = new TextVariableCMYK(c_datetime);
            var c_usedColors = new TextVariableUsedColor(c_cmyk);

            textVariables = c_usedColors;

        }

        public static void SetValue(string key, object value)
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


        public static string ReplaceToRealValues(string str)
        {
            string output = str;

            foreach (var key in Values)
            {
                output = output.Replace(key.Key, key.Value);
            }

            return output;
        }
    }

    public static class ValueList
    {
        public const string SheetIdx    = "$[sheetIdx]";
        public const string SheetSide   = "$[sheetSide]";
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
