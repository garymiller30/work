// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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


    }
}
