using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace UpdateHub
{
    public static class UpdateManifestSerializer
    {
        public static UpdateManifest Deserialize(string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return Deserialize(stream);
            }
        }

        public static UpdateManifest Deserialize(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(UpdateManifest));
            return (UpdateManifest)serializer.ReadObject(stream);
        }

        public static string Serialize(UpdateManifest manifest)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(UpdateManifest));
                serializer.WriteObject(stream, manifest);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static void SaveToFile(UpdateManifest manifest, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, Serialize(manifest), Encoding.UTF8);
        }
    }
}
