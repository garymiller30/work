using System;
using System.Globalization;
using System.IO;
using ExtensionMethods;
using Interfaces;
using Job.Static;
using Job.UC;
using Microsoft.VisualBasic.FileIO;

namespace Job
{
    public static class Factory
    {
        
        
        public static ICustomer CreateCustomer()
        {
            return new Customer {Name = string.Empty};
        }

        public static IPart CreatePart()
        {
            return new Part();
        }

        public static IFileBrowser CreateFileBrowser(IUserProfile profile)
        {
            return new FileBrowser(profile);
        }

        public static Forms CreateForms()
        {
            return new Forms();
        }

        public static IJob CreateJob(IUserProfile userProfile)
        {
            var job = new Job {StatusCode = userProfile.StatusManager.GetDefaultStatus()};
            return job;
        }

        public static void CreateJobFromFile(IUserProfile userProfile,IJob j, string filePath)
        {
            var job = CreateJob(userProfile);

            job.Customer = j.Customer;
            job.Number = $"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}";
            job.Description = Path.GetFileNameWithoutExtension(filePath);
            job.CategoryId = j.CategoryId;

            if (userProfile.Jobs.AddJob(job))
            {
                if (Directory.Exists(filePath))
                {
                    var target =  Path.Combine(userProfile.Jobs.GetFullPathToWorkFolder(job), Path.GetFileName(filePath));
                    FileSystem.CopyDirectory(filePath,target,UIOption.AllDialogs);
                }
                else
                {
                    var target =  userProfile.Jobs.GetFullPathToWorkFolder(job);
                    FileSystem.CopyFile(filePath, Path.Combine(target, Path.GetFileName(filePath) ),UIOption.AllDialogs);
                }
            }
        }
    }
}
