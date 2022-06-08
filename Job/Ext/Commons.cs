using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
            //job.IsCashe = updateJob.IsCashe;
            //job.IsCashePayed = updateJob.IsCashePayed;
            //job.CachePayedSum = updateJob.CachePayedSum;
        }

        //public static void SetPartList(this IJob job, object parts,Profile profile)
        //{

        //    if (parts == null)
        //    {
        //        profile.Jobs.GetJobParts(job).Clear();
        //    }

        //    else if (parts is IList p)
        //    {
        //        //Debug.WriteLine(parts.GetType().FullName);
        //        //job.Parts = (List<IJobPart>)p.Cast<Part>();
        //    }
        //}

        //public static void ChangeOrderPath(this Job job, string path,Profile profile)
        //{
        //    if (!job.DontCreateFolder)
        //    {
        //        var folder = profile.Jobs.GetFullPathToWorkFolder(job);

        //        if (job.UseCustomFolder)
        //        {
        //            folder = Path.GetFileName(folder);
        //        }
        //        else
        //        {
        //            job.UseCustomFolder = true;

        //        }
        //        job.SetFolder(Path.Combine(path, folder));

        //    }
        //}

        public static bool IsSignaJobExist(this IJob job, string signaFileShablon, string signaJobsPath, IUserProfile profile)
        {
            

            var category = profile.Categories.GetCategoryNameById(job.CategoryId);
            var fileName = string.IsNullOrEmpty(category) 
                ? string.Format(signaFileShablon, job.Customer, job.Number, job.Description) 
                : string.Format(signaFileShablon, job.Customer, job.Number, job.Description,category);

            var destFile = Path.Combine(signaJobsPath, $"{fileName}.sdf");

            return File.Exists(destFile);
        }

        public static bool IsSignaJobExist(this IJob job, IUserProfile profile)
        {
            var category = profile.Categories.GetCategoryNameById(job.CategoryId);
            var fileName = string.IsNullOrEmpty(category) 
                ? string.Format(profile.Jobs.Settings.SignaFileShablon, job.Customer, job.Number, job.Description) 
                : string.Format(profile.Jobs.Settings.SignaFileShablon, job.Customer, job.Number, job.Description,category);

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
