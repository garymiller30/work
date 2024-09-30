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
        public const string SheetIdx = "$[sheetIdx]";
        public const string SheetSide = "$[sheetSide]";
        public const string SheetFormat  = "$[sheetFormat]";
        public const string CurDate = "$[datetime]";
    }
}
