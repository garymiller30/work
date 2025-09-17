// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace JobSpace
{
    public static class Commons
    {
        public static void Serialize(object x, String fn)
        {
            //откроем поток для записи в файл
            try
            {
                using (var fs = new FileStream(fn, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(fs, x);

                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        ///     десериалезировать из файла
        /// </summary>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static object Deserialize(String fn)
        {
            object x = null;

            try
            {
                if (File.Exists(fn))
                {
                    using (var fs = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var bf = new BinaryFormatter();
                        x = bf.Deserialize(fs);
                    }
                }
            }
            catch (Exception)
            {

            }
            return x;
        }

        public static void SerializeXML<T>(T x, string fn)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(fn))
            {
                serializer.Serialize(writer, x);
            }

        }

        public static T DeserializeXML<T>(string fn)
        {
            var a = default(T);

            if (File.Exists(fn))
            {
                try
                {
                    var deserializer = new XmlSerializer(typeof(T));
                    var reader = new StreamReader(fn);
                    var obj = deserializer.Deserialize(reader);
                    a = (T)obj;
                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.InnerException?.Message);
                }
            }

            return a;
        }

        public static string FixWrongKeyboardLayout(string input)
        {
            Dictionary<char, char> EnToUk = new Dictionary<char, char>
            {
                ['q'] = 'й',
                ['w'] = 'ц',
                ['e'] = 'у',
                ['r'] = 'к',
                ['t'] = 'е',
                ['y'] = 'н',
                ['u'] = 'г',
                ['i'] = 'ш',
                ['o'] = 'щ',
                ['p'] = 'з',
                ['['] = 'х',
                [']'] = 'ї',

                ['a'] = 'ф',
                ['s'] = 'і',
                ['d'] = 'в',
                ['f'] = 'а',
                ['g'] = 'п',
                ['h'] = 'р',
                ['j'] = 'о',
                ['k'] = 'л',
                ['l'] = 'д',
                [';'] = 'ж',
                ['\''] = 'є',

                ['z'] = 'я',
                ['x'] = 'ч',
                ['c'] = 'с',
                ['v'] = 'м',
                ['b'] = 'и',
                ['n'] = 'т',
                ['m'] = 'ь',
                [','] = 'б',
                ['.'] = 'ю'
            };

            Dictionary<char, char> UkToEn = EnToUk.ToDictionary(kv => kv.Value, kv => kv.Key);

            int latinCount = input.Count(c => c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z');
            int cyrillicCount = input.Count(c => c >= 'а' && c <= 'я' || c >= 'А' && c <= 'Я' || "іїєґІЇЄҐ".Contains(c));

            // вибираємо напрямок
            var map = latinCount >= cyrillicCount ? EnToUk : UkToEn;

            var sb = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                char lower = char.ToLower(c);
                if (map.TryGetValue(lower, out char mapped))
                {
                    sb.Append(char.IsUpper(c) ? char.ToUpper(mapped) : mapped);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
