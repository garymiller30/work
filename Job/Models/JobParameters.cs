// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 


using System;
using Interfaces;
using MongoDB.Bson;

namespace JobSpace.Models
{
    public sealed class JobParameters : IJob
    {
        public object Id { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string PreviousOrder { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool DontCreateFolder { get; set; }
        public bool IsCashe { get; set; }
        public bool IsCashePayed { get; set; }
        public bool UseCustomFolder { get; set; }

        public decimal CachePayedSum { get; set; }
        public object CategoryId { get; set; }
        public string Folder { get; set; }

        public int StatusCode { get; set; }

        public int ProgressValue { get; set; }

        public bool IsJobInProcess { get; set; }

        public object GetJob()
        {
            return _job;
        }
        //public List<IJobPart> Parts { get; set; }

        private readonly Job _job;

        public JobParameters(IJob job)
        {
            _job = (Job)job.GetJob();
            Id = _job.Id;
            Number = job.Number;
            PreviousOrder = job.PreviousOrder;
            Customer = job.Customer;
            Description = job.Description;
            Note = job.Note;
            DontCreateFolder = job.DontCreateFolder;
#pragma warning disable CS0612 // 'IJob.IsCashe' is obsolete
            IsCashe = job.IsCashe;
#pragma warning restore CS0612 // 'IJob.IsCashe' is obsolete
#pragma warning disable CS0612 // 'IJob.IsCashePayed' is obsolete
            IsCashePayed = job.IsCashePayed;
#pragma warning restore CS0612 // 'IJob.IsCashePayed' is obsolete
            UseCustomFolder = job.UseCustomFolder;
#pragma warning disable CS0612 // 'IJob.CachePayedSum' is obsolete
            CachePayedSum = job.CachePayedSum;
#pragma warning restore CS0612 // 'IJob.CachePayedSum' is obsolete
            CategoryId = job.CategoryId;
            Date = job.Date;
            Folder = job.Folder;
            //Parts = job.Parts.ToList();
        }

        public void ApplyToJob()
        {
            _job.Number = Number;
            _job.PreviousOrder = PreviousOrder;
            _job.Customer = Customer;
            _job.Description = Description;
            _job.Note = Note;
            _job.DontCreateFolder = DontCreateFolder;
#pragma warning disable CS0612 // 'Job.IsCashe' is obsolete
            _job.IsCashe = IsCashe;
#pragma warning restore CS0612 // 'Job.IsCashe' is obsolete
#pragma warning disable CS0612 // 'Job.IsCashePayed' is obsolete
            _job.IsCashePayed = IsCashePayed;
#pragma warning restore CS0612 // 'Job.IsCashePayed' is obsolete
            _job.UseCustomFolder = UseCustomFolder;
#pragma warning disable CS0612 // 'Job.CachePayedSum' is obsolete
            _job.CachePayedSum = CachePayedSum;
#pragma warning restore CS0612 // 'Job.CachePayedSum' is obsolete
            _job.CategoryId = CategoryId;
            _job.Date = Date;
            _job.Folder = Folder;
            //_job.Parts = Parts.ToList();   
        }
    }
}
