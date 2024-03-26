// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 


using System;
using System.IO;
using System.Xml.Serialization;

namespace FtpClient
{
    public static class Commons
    {
        public static T GetObjectValue<T>(this object o, string property)
        {
            return (T)o.GetType().GetProperty(property)?.GetValue(o, null);
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
                    Logger.Log.Error(null, "DeserializeXML",$"{e.Message}");
                }
            }

            return a;
        }
    }
}
