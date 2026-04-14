namespace ActiveWorks
{
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Список фирм, где выводим на стороне
    /// </summary>
    public static class AutoCompleteCtp
    {
        /// <summary>
        /// имя файла
        /// </summary>
        private const string FileName = "AutocompleteCTP.xml";

        /// <summary>
        /// список фирм, где выводим
        /// </summary>
        private static string[] _list;

        /// <summary>
        /// получить список фирм (загружает из файла AutocompleteCTP.xml)
        /// </summary>
        /// <returns>массив строк</returns>
        public static string[] GetList()
        {
            if (_list == null)
            {
                if (File.Exists(FileName))
                {
                    var deserializer = new XmlSerializer(typeof(string[]));
                    TextReader reader = new StreamReader(FileName);
                    var obj = deserializer.Deserialize(reader);
                    _list = (string[])obj;
                    reader.Close();
                }
            }

            return _list;
        }
    }
}
