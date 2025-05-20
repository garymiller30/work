using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Interfaces;
using JobSpace.Profiles;
using MongoDB.Bson;

namespace CasheViewer.Reports
{
    public class ReportJob : IReport
    {
        public IUserProfile UserProfile { get; set; }
        public decimal Total { get; set; }

        public List<INode> GetNodes()
        {
            Total = 0;

            //var oldVariant = GetJobsByCustomers(false);
            var newVariant = GetJobsByCustomerRootByPlugin(false);

            //var comparer = new CustomerComparer();

            //foreach (JobNodeRoot customer in newVariant)
            //{
            //    if (newVariant.Contains(customer, comparer))
            //    {
            //        var c = newVariant.First(x => x.Name.Equals(customer.Name));
            //        c.Children.AddRange(customer.Children);
            //    }
            //    else
            //    {
            //        newVariant.Add(customer);
            //    }
            //}
            return newVariant.Cast<INode>().ToList();
        }

        
        public List<JobNodeRoot> GetJobsByCustomerRootByPlugin(bool isPayed)
        {
            var reportDate = new List<JobNodeRoot>();

            Dictionary<object, decimal> jobDictionary = new Dictionary<object, decimal>();

            // отримати плагіни
            var plugins = UserProfile.Plugins.GetPluginFormAddWorks();

            foreach (IPluginFormAddWork plugin in plugins)
            {
                var collection = plugin.GetCollection(UserProfile)
                    .Where(x => isPayed ? x.Price - x.Pay == 0 : x.Price - x.Pay > 0)
                    .GroupBy(i => i.ParentId);

                foreach (IGrouping<object, IProcess> processes in collection)
                {
                    if (!jobDictionary.ContainsKey(processes.Key))
                    {
                        jobDictionary.Add(processes.Key, 0);
                    }
                    jobDictionary[processes.Key] += processes.Sum(x => isPayed ? x.Pay : x.Price - x.Pay);
                }
            }

            List<JobSpace.Job> rawJobs = new List<JobSpace.Job>();

            foreach (var pair in jobDictionary)
            {
                var j = UserProfile.Base.GetById<JobSpace.Job>("Jobs", pair.Key);
                if (j != null)
                {
                    rawJobs.Add(j);
                }
                else
                {
                    Logger.Log.Error(null, $"Job not found", $"{pair.Key}");
                }
            }

            var jobs = rawJobs.GroupBy(x => x.Date.ToString("yy.MM"));
            

            foreach (var job in jobs)
            {
                var rd = new JobNodeRoot { Name = job.Key };

                rd.Children.AddRange(
                    job.Select(u => (INode)new JobNode()
                    {
                        Name = u.Customer,
                        Date = u.Date,
                        Number = u.Number,
                        Description = u.Description,
                        Sum = jobDictionary[u.Id],
                        Job = u,
                        Category = UserProfile.Categories.GetCategoryNameById(u.CategoryId),
                        ForegroundColor = Color.MediumBlue,
                        ReportVersion = ReportVersionEnum.Version2
                    }).ToList());

                rd.Children.Sort((x, y) => x.Date.CompareTo(y.Date));

                reportDate.Add(rd);
            }
            return reportDate;
        }
        class CustomerComparer : IEqualityComparer<JobNodeRoot>
        {
            public bool Equals(JobNodeRoot x, JobNodeRoot y)
            {
                return y != null && x != null && x.Name.Equals(y.Name);
            }

            public int GetHashCode(JobNodeRoot obj)
            {
                return obj.Name.GetHashCode();
            }
        }
    }
}
