using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SpeedDevelopingPlate
{
    public static class PlateFormatsManager
    {
        const string FileName = "PlateFormats.json";
        static readonly List<PlateFormat> _plateFormats = new List<PlateFormat>();

        public static event EventHandler<PlateFormat> OnAdd = delegate { };
        public static event EventHandler<PlateFormat> OnChange = delegate { };
        public static event EventHandler<PlateFormat> Ondelete = delegate { };

        static PlateFormatsManager()
        {
            Load();
        }

        private static void Load()
        {
            _plateFormats.AddRange(jsonLoader(FileName));
        }

        private static IEnumerable<PlateFormat> jsonLoader(string fileName)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<List<PlateFormat>>(File.ReadAllText(fileName), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                });
                return (IEnumerable<PlateFormat>)obj;
            }
            catch (Exception)
            {
                // ignored
            }
            return new List<PlateFormat>();

        }

        public static PlateFormat[] GetPlateFormats => _plateFormats.ToArray();


        public static void Add(string manufacturer, string format)
        {
            var q =
                _plateFormats.Where(
                    x =>
                        x.Manufacturer.ToUpper().Equals(manufacturer.ToUpper()) &&
                        x.Format.ToUpper().Equals(format.ToUpper()));

            if (!q.Any())
            {
                var pf = new PlateFormat(manufacturer,format);
                _plateFormats.Add(pf);
                Save(_plateFormats, FileName);
                OnAdd(null, pf);
            }
        }

        public static void Delete(object plateFormat)
        {
            var pf = plateFormat as PlateFormat;
            if (pf != null)
            {
                _plateFormats.Remove(pf);
                Save(_plateFormats, FileName);
                Ondelete(null, pf);
            }
        }

        public static void Save()
        {
            Save(_plateFormats,FileName);
        }
        private static void Save(List<PlateFormat> plateformats, string fileName)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Formatting.Indented;
            serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            using (var sw = new StreamWriter(fileName, false, Encoding.UTF8))
            using (var writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, plateformats, typeof(List<PlateFormat>));
            }

        }

        public static void ChangePlateSpeed(PlateFormat plateFormat, decimal temperature, int speed)
        {
            plateFormat.Add(temperature,speed);
            Save(_plateFormats,FileName);
            OnChange(null, plateFormat);
        }

      
    }
}
