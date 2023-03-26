using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ExtensionMethods;
using Interfaces;
using MongoDB.Bson;

namespace Job.Ext
{
    public static class Commons
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
        public static Part Duplicate(this Part part)
        {
            var p = DuplicateBase(part);
            p.Id = ObjectId.GenerateNewId();
            return p;
        }

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


        public static bool IsSignaJobExist(this IJob job, string signaFileShablon, string signaJobsPath, IUserProfile profile)
        {
            

            var category = profile.Categories.GetCategoryNameById(job.CategoryId);
            var fileName = string.IsNullOrEmpty(category)
                ? string.Format(CultureInfo.InvariantCulture, signaFileShablon, job.Customer.Transliteration(), job.Number, job.Description.Transliteration())
                : string.Format(CultureInfo.InvariantCulture, signaFileShablon, job.Customer.Transliteration(), job.Number, job.Description.Transliteration(), category.Transliteration());

            var destFile = Path.Combine(signaJobsPath, $"{fileName}.sdf");

            return File.Exists(destFile);
        }

        public static bool IsSignaJobExist(this IJob job, IUserProfile profile)
        {
            var category = profile.Categories.GetCategoryNameById(job.CategoryId);
            var fileName = string.IsNullOrEmpty(category) 
                ? string.Format(CultureInfo.InvariantCulture, profile.Jobs.Settings.SignaFileShablon, job.Customer.Transliteration(), job.Number, job.Description.Transliteration()) 
                : string.Format(CultureInfo.InvariantCulture, profile.Jobs.Settings.SignaFileShablon, job.Customer.Transliteration(), job.Number, job.Description.Transliteration(),category.Transliteration());

            var destFile = Path.Combine(profile.Jobs.Settings.SignaJobsPath, $"{fileName}.sdf");

            return File.Exists(destFile);
        }


/*
        public static string SendNotifyCovertString(this string str,Job job)
        {
            return str.Replace("$OrderNumber", job.Number)
                .Replace("$OrderDescription", job.Description);
        }
*/
        public static string SendNotifyCovertString(this string str,IJob job)
        {
            return str.Replace("$OrderNumber", job.Number)
                .Replace("$OrderDescription", job.Description);
        }
    }
}
