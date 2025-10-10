using ExtensionMethods;
using Interfaces;
using JobSpace.Controllers;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace JobSpace.Ext
{
    public static class Extensions
    {

        static T DuplicateBase<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);

            stream.Seek(0, SeekOrigin.Begin);
            var o = formatter.Deserialize(stream);
            return (T)o;
        }
        /// <summary>
        /// дублювати Part
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        //public static Part Duplicate(this Part part)
        //{
        //    var p = DuplicateBase(part);
        //    p.Id = ObjectId.GenerateNewId();
        //    return p;
        //}

        public static Job Duplicate(this Job job)
        {
            var j = DuplicateBase(job);
            j.Id = ObjectId.GenerateNewId();
            return j;
        }
        public static IJob Duplicate(this IJob job)
        {
            var j = DuplicateBase(job);
            j.Id = ObjectId.GenerateNewId();
            return j;
        }
        public static void Update(this IJob job, IJob updateJob)
        {
            job.Customer = updateJob.Customer;
            job.Description = updateJob.Description;
            job.Number = updateJob.Number;
            job.Note = updateJob.Note;
            job.Date = updateJob.Date;
            job.StatusCode = updateJob.StatusCode;
            job.PreviousOrder = updateJob.PreviousOrder;
            job.DontCreateFolder = updateJob.DontCreateFolder;
            job.CategoryId = updateJob.CategoryId;

        }


        //public static bool IsSignaJobExist(this IJob job, string signaFileShablon, string signaJobsPath, IUserProfile profile)
        //{
        //    var category = profile.Categories.GetCategoryNameById(job.CategoryId);
        //    var fileName = string.Format(CultureInfo.InvariantCulture, signaFileShablon, job.Customer.Transliteration(), job.Number, job.Description.Transliteration(), category.Transliteration());

        //    var destFile = Path.Combine(signaJobsPath, $"{fileName}.sdf");

        //    return File.Exists(destFile);
        //}

        public static bool IsSignaJobExist(this IJob job, IUserProfile profile)
        {
            return File.Exists(GetSignaFilePath(job, profile));
        }

        public static string[] GetSignaFileNames(this IJob job, IUserProfile profile)
        {
            if (profile.Settings.GetJobSettings().UseJobFolder)
            {
                var signaPath = Path.Combine(profile.Jobs.GetFullPathToWorkFolder(job), profile.Settings.GetJobSettings().SubFolderForSignaFile);
                if (Directory.Exists(signaPath))
                {
                    var files = Directory.GetFiles(signaPath, "*.sdf");
                    if (files.Length > 0)
                    {
                        return files.Select(x => Path.GetFileNameWithoutExtension(x)).ToArray();
                    }
                }
            }
            else
            {
                var file = Path.Combine(profile.Settings.GetJobSettings().SignaJobsPath, job.GetSignaFileName(profile));
                if (File.Exists($"{file}.sdf"))
                {
                    return new string[] { Path.GetFileNameWithoutExtension(file) };
                }

            }
            return Array.Empty<string>();
        }

        public static string GetSignaFileName(this IJob job, IUserProfile profile)
        {
            // потрібно розділити шаблон на частини типу: ["{1}","_","{0}","_","{2}","_","{3}"]
            var template = profile.Jobs.Settings.SignaFileShablon;
            var category = profile.Categories.GetCategoryNameById(job.CategoryId);

            string result = SignaFileNameBuilder(template, job.Number, job.Customer.Transliteration(), job.Description.Transliteration(), category.Transliteration());
            return result;

            //string fileName = string.Format(CultureInfo.InvariantCulture,
            //    profile.Jobs.Settings.SignaFileShablon,
            //    job.Customer.Transliteration(),
            //    job.Number,
            //    job.Description.Transliteration(),
            //    category.Transliteration());

            //return fileName;
        }

        static string SignaFileNameBuilder(string shablon, string jobNumber,string customer, string description, string category)
        {
            var template = shablon;
            char separator = DetectSeparator(template);

            if (string.IsNullOrEmpty(category))
            {
                // Екрануємо роздільник для використання в regex
                string sepEsc = Regex.Escape(separator.ToString());

                // Шукаємо optional left sep, literal {3}, optional right sep
                string pattern = "(?<left>" + sepEsc + @")?\s*\{3\}\s*(?<right>" + sepEsc + @")?";

                // Якщо є будь-який роздільник зліва або справа — замінюємо весь блок на один роздільник,
                // інакше — просто видаляємо блок (щоб не вставляти зайвий роздільник).
                template = Regex.Replace(template, pattern, m =>
                {
                    return (m.Groups["left"].Success || m.Groups["right"].Success)
                        ? separator.ToString()
                        : string.Empty;
                });
                // На всякий випадок прибираємо залишки {3}
                template = template.Replace("{3}", string.Empty);
            }

            string[] values = { customer ?? string.Empty, jobNumber ?? string.Empty, description ?? string.Empty, category ?? string.Empty };

            string result = template;
            for (int i = 0; i < values.Length; i++)
                result = result.Replace("{" + i + "}", values[i]);

            // Зводимо повтори роздільника до одного і обрізаємо з країв
            result = Regex.Replace(result, Regex.Escape(separator.ToString()) + "{2,}", separator.ToString());
            result = result.Trim(separator, ' ');

            return result;

        }

        private static char DetectSeparator(string template)
        {
            // Знаходимо перший символ між {n} — найімовірніше, це роздільник
            var match = Regex.Match(template, @"\}\s*([^\{\w\s])\s*\{");
            return match.Success ? match.Groups[1].Value.First() : '_'; // якщо не знайдено — "_"
        }


        public static string GetSignaFileName(this IJob job, IUserProfile profile, string oldNumber)
        {
            var category = profile.Categories.GetCategoryNameById(job.CategoryId);

            string result = SignaFileNameBuilder(profile.Jobs.Settings.SignaFileShablon, oldNumber, job.Customer.Transliteration(), job.Description.Transliteration(), category.Transliteration());

            return result;
            //string fileName = string.Format(CultureInfo.InvariantCulture,
            //    profile.Jobs.Settings.SignaFileShablon,
            //    job.Customer.Transliteration(),
            //    oldNumber,
            //    job.Description.Transliteration(),
            //    category.Transliteration());

            //return fileName;
        }

        public static string GetSignaFilePath(this IJob job, IUserProfile profile)
        {
            string fileName = job.GetSignaFileName(profile);

            string destFile;

            if (profile.Settings.GetJobSettings().UseJobFolder)
            {
                destFile = Path.Combine(profile.Jobs.GetFullPathToWorkFolder(job),
                    profile.Settings.GetJobSettings().SubFolderForSignaFile, $"{fileName}.sdf");
            }
            else
            {
                destFile = Path.Combine(profile.Jobs.Settings.SignaJobsPath, $"{fileName}.sdf");
            }
            return destFile;
        }

        public static string GetSignaFilePath(this IJob job, IUserProfile profile, string oldNumber)
        {
            string fileName = job.GetSignaFileName(profile, oldNumber);
            var destFile = Path.Combine(profile.Jobs.GetFullPathToWorkFolder(job),
                    profile.Settings.GetJobSettings().SubFolderForSignaFile, $"{fileName}.sdf");

            return destFile;
        }

        public static string SendNotifyConvertString(this string str, IJob job)
        {
            return str.Replace("$OrderNumber", job.Number)
                .Replace("$OrderDescription", job.Description);
        }

        public static string GetDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
                return enumValue.ToString();

            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }

            return enumValue.ToString();
        }

        public static IEnumerable<string> GetDescriptions(Type type)
        {
            var descs = new List<string>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds)
                {
                    descs.Add(fd.Description);
                }
            }
            return descs;

        }
    }
}
