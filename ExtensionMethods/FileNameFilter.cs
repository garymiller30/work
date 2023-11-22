using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
    public static class FileNameFilter
    {
        private static readonly Dictionary<string, string> Gost = new Dictionary<string, string>(){
            {" ", "_"},
            {":", "_"},
            {"Ї", "Y"},
            {"Є", "EH"},
            {"І", "I"},
            {"і", "i"},
            {"№", "#"},
            {"є", "eh"},
            {"А", "A"},
            {"Б", "B"},
            {"В", "V"},
            {"Г", "G"},
            {"Д", "D"},
            {"Е", "E"},
            {"Ё", "JO"},
            {"Ж", "ZH"},
            {"З", "Z"},
            {"И", "I"},
            {"Й", "Y"},
            {"К", "K"},
            {"Л", "L"},
            {"М", "M"},
            {"Н", "N"},
            {"О", "O"},
            {"П", "P"},
            {"Р", "R"},
            {"С", "S"},
            {"Т", "T"},
            {"У", "U"},
            {"Ф", "F"},
            {"Х", "KH"},
            {"Ц", "C"},
            {"Ч", "CH"},
            {"Ш", "SH"},
            {"Щ", "SHH"},
            {"Ъ", "'"},
            {"Ы", "Y"},
            {"Ь", ""},
            {"Э", "EH"},
            {"Ю", "YU"},
            {"Я", "YA"},
            {"а", "a"},
            {"б", "b"},
            {"в", "v"},
            {"г", "g"},
            {"д", "d"},
            {"е", "e"},
            {"ї", "y"},
            {"ё", "jo"},
            {"ж", "zh"},
            {"з", "z"},
            {"и", "i"},
            {"й", "y"},
            {"к", "k"},
            {"л", "l"},
            {"м", "m"},
            {"н", "n"},
            {"о", "o"},
            {"п", "p"},
            {"р", "r"},
            {"с", "s"},
            {"т", "t"},
            {"у", "u"},
            {"ф", "f"},
            {"х", "kh"},
            {"ц", "c"},
            {"ч", "ch"},
            {"ш", "sh"},
            {"щ", "shh"},
            {"ъ", ""},
            {"ы", "y"},
            {"ь", ""},
            {"э", "eh"},
            {"ю", "yu"},
            {"я", "ya"},
            {"«", ""},
            {"»", ""},
            {"—", "-"},
            {"*", "x"},
            {"\"", ""},
            {" ", "_"},
            {"–","-"},
            

            };


        static FileNameFilter()
        {
            //Gost.Add(" ", "_");
            //Gost.Add(":", "_");
            //Gost.Add("Ї", "Y");
            //Gost.Add("Є", "EH");
            //Gost.Add("І", "I");
            //Gost.Add("і", "i");
            //Gost.Add("№", "#");
            //Gost.Add("є", "eh");
            //Gost.Add("А", "A");
            //Gost.Add("Б", "B");
            //Gost.Add("В", "V");
            //Gost.Add("Г", "G");
            //Gost.Add("Д", "D");
            //Gost.Add("Е", "E");
            //Gost.Add("Ё", "JO");
            //Gost.Add("Ж", "ZH");
            //Gost.Add("З", "Z");
            //Gost.Add("И", "I");
            //Gost.Add("Й", "Y");
            //Gost.Add("К", "K");
            //Gost.Add("Л", "L");
            //Gost.Add("М", "M");
            //Gost.Add("Н", "N");
            //Gost.Add("О", "O");
            //Gost.Add("П", "P");
            //Gost.Add("Р", "R");
            //Gost.Add("С", "S");
            //Gost.Add("Т", "T");
            //Gost.Add("У", "U");
            //Gost.Add("Ф", "F");
            //Gost.Add("Х", "KH");
            //Gost.Add("Ц", "C");
            //Gost.Add("Ч", "CH");
            //Gost.Add("Ш", "SH");
            //Gost.Add("Щ", "SHH");
            //Gost.Add("Ъ", "'");
            //Gost.Add("Ы", "Y");
            //Gost.Add("Ь", "");
            //Gost.Add("Э", "EH");
            //Gost.Add("Ю", "YU");
            //Gost.Add("Я", "YA");
            //Gost.Add("а", "a");
            //Gost.Add("б", "b");
            //Gost.Add("в", "v");
            //Gost.Add("г", "g");
            //Gost.Add("д", "d");
            //Gost.Add("е", "e");
            //Gost.Add("ї", "y");
            //Gost.Add("ё", "jo");
            //Gost.Add("ж", "zh");
            //Gost.Add("з", "z");
            //Gost.Add("и", "i");
            //Gost.Add("й", "y");
            //Gost.Add("к", "k");
            //Gost.Add("л", "l");
            //Gost.Add("м", "m");
            //Gost.Add("н", "n");
            //Gost.Add("о", "o");
            //Gost.Add("п", "p");
            //Gost.Add("р", "r");
            //Gost.Add("с", "s");
            //Gost.Add("т", "t");
            //Gost.Add("у", "u");
            //Gost.Add("ф", "f");
            //Gost.Add("х", "kh");
            //Gost.Add("ц", "c");
            //Gost.Add("ч", "ch");
            //Gost.Add("ш", "sh");
            //Gost.Add("щ", "shh");
            //Gost.Add("ъ", "");
            //Gost.Add("ы", "y");
            //Gost.Add("ь", "");
            //Gost.Add("э", "eh");
            //Gost.Add("ю", "yu");
            //Gost.Add("я", "ya");
            //Gost.Add("«", "");
            //Gost.Add("»", "");
            //Gost.Add("—", "-");
            //Gost.Add("*", "x");
            //Gost.Add("\"", "");
            //Gost.Add(" ", "_");
            //Gost.Add("–","-");
        }

        //заміна кирилиці
        public static string Transliteration(this string str)
        {

            if (string.IsNullOrEmpty(str)) return string.Empty;

            var sb = new StringBuilder(str);

            foreach (var key in Gost)
            {
                sb.Replace(key.Key, key.Value);
            }

            return CleanFileName(sb.ToString());
        }

        public static string TransliterationChar(this char chr)
        {
            var str = chr.ToString();

            if (Gost.ContainsKey(str))
            {
                return Gost[str];
            }

            return CleanFileName(str);
        }

        /// <summary>
        /// видалити заборонені символи
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }


    }

}
