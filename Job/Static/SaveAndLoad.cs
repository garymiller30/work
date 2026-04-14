using Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Windows.Forms;

namespace JobSpace.Static
{
    public static class SaveAndLoad
    {
        /// <summary>
        /// Serialize to file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="data"></param>
        public static void Save(string file, object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, data);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, $@"save file {file}");
            }
        }

        


        /// <summary>
        /// deserialize from file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static T Load<T>(string file) where T: new ()
        {
            if (File.Exists(file))
            {
                try
                {
                    T obj;

                    using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        obj = (T)formatter.Deserialize(fs);

                    }
                    return obj;

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, $@"load file {file}");

                }
            }
         
            return default(T);
        }

        public static T LoadJson<T>(string file) where T : new()
        {
            if (File.Exists(file))
            {
                try
                {
                    var str = File.ReadAllText(file);
                    var obj = JsonSerializer.Deserialize<T>(str);
                    return obj;
                }
                catch (Exception e)
                {
                   Log.Error(null, "LoadJson", $"Error loading JSON from {file}: {e.Message}");
                }
            }
            return default(T);
        }

        public static Dictionary<string, decimal> LoadPaperThicknesses()
        {
            var file = Path.Combine(Application.StartupPath, "db\\paper_thickness.json");
            var thicknesses = new Dictionary<string, decimal>();

            if (File.Exists(file))
            {
                try
                {
                    thicknesses = LoadJson<Dictionary<string, decimal>>(file);
                    if (thicknesses != null)
                        return thicknesses;
                    else
                        thicknesses = new Dictionary<string, decimal>();
                }
                catch (Exception e)
                {
                    Log.Error(null, "LoadPaperThicknesses", $"Error loading paper thicknesses: {e.Message}");
                    thicknesses = new Dictionary<string, decimal>();
                }
            }
            return thicknesses;

        }
    }
}
