using Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
    public static class FileNameFilter
    {
        /***    private static readonly Dictionary<string, string> Gost = new Dictionary<string, string>(){
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
                {"\\", ""},
                {" ", "_"},
                {"–","-"},


                };\
        ***/

        private static readonly Dictionary<char, string> Gost_char = new Dictionary<char, string>(){
            {' ', "_"},
            {':', "_"},
            {'Ї', "Y"},
            {'Є', "EH"},
            {'І', "I"},
            {'і', "i"},
            {'№', "#"},
            {'є', "eh"},
            {'А', "A"},
            {'Б', "B"},
            {'В', "V"},
            {'Г', "G"},
            {'Д', "D"},
            {'Е', "E"},
            {'Ё', "JO"},
            {'Ж', "ZH"},
            {'З', "Z"},
            {'И', "I"},
            {'Й', "Y"},
            {'К', "K"},
            {'Л', "L"},
            {'М', "M"},
            {'Н', "N"},
            {'О', "O"},
            {'П', "P"},
            {'Р', "R"},
            {'С', "S"},
            {'Т', "T"},
            {'У', "U"},
            {'Ф', "F"},
            {'Х', "KH"},
            {'Ц', "C"},
            {'Ч', "CH"},
            {'Ш', "SH"},
            {'Щ', "SHH"},
            {'Ъ', "'"},
            {'Ы', "Y"},
            {'Ь', ""},
            {'Э', "EH"},
            {'Ю', "YU"},
            {'Я', "YA"},
            {'а', "a"},
            {'б', "b"},
            {'в', "v"},
            {'г', "g"},
            {'д', "d"},
            {'е', "e"},
            {'ї', "y"},
            {'ё', "jo"},
            {'ж', "zh"},
            {'з', "z"},
            {'и', "i"},
            {'й', "y"},
            {'к', "k"},
            {'л', "l"},
            {'м', "m"},
            {'н', "n"},
            {'о', "o"},
            {'п', "p"},
            {'р', "r"},
            {'с', "s"},
            {'т', "t"},
            {'у', "u"},
            {'ф', "f"},
            {'х', "kh"},
            {'ц', "c"},
            {'ч', "ch"},
            {'ш', "sh"},
            {'щ', "shh"},
            {'ъ', ""},
            {'ы', "y"},
            {'ь', ""},
            {'э', "eh"},
            {'ю', "yu"},
            {'я', "ya"},
            {'«', ""},
            {'»', ""},
            {'—', "-"},
            {'*', "x"},
            {'\\', ""},
            {' ', "_"},
            {'–',"-"},
            {'(',"(" },
            {')',")" },
            {'+',"+" },
            {'.',"." },
            {',',"," },
            {'-',"-" },
            {'_',"_" },
            {'"',"\"" },
            {'\'',"\'" },
            {(char)(774),"_" },

            };


        static FileNameFilter()
        {

        }

        //заміна кирилиці
        public static string Transliteration(this string str)
        {

            if (string.IsNullOrEmpty(str)) return string.Empty;

            var sb_char = new StringBuilder();


            foreach (var _char in str)
            {
                if (IsLetterOrDigit(_char))
                {
                    sb_char.Append(_char);
                }
                else if (Gost_char.ContainsKey(_char))
                {
                    sb_char.Append(Gost_char[_char]);
                }
                else
                {
                    sb_char.Append(_char);
                    Log.Error(null,"transliteration",$"Key '{_char}' {(int)_char} not found in transliteration dictionary. Added as is.");
                    //throw new KeyNotFoundException($"Key '{_char}' {(int)_char} not found in transliteration dictionary.");
                }
            }
            return CleanFileName(sb_char.ToString());

        }

        static bool IsLetterOrDigit(char c)
        {
            if (char.IsDigit(c)) return true;

            if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')) return true;

            return false;
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
