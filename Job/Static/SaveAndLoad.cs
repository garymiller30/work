using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Job.Static
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

    }
}
