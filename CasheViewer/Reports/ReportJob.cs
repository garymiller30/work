using System;
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
        public decimal TotalWithConsumerPrice { get;set;}
        public DateTime DateMin { get; set; } = DateTime.MaxValue;

        List<INode> nodes;

        public List<INode> GetNodes()
        {
            if (nodes != null) return nodes;

            nodes = GetJobsByCustomerRootByPlugin(false).Cast<INode>().ToList();

            Total = nodes.Sum(x => x.Children.Sum(y => y.Sum));
            //TotalWithConsumerPrice = newVariant.Sum(x => x.SumWithConsumerPrice);
            return nodes;
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

            var jobs = rawJobs.GroupBy(x => x.Date.ToString("yyyy.MM"));

            

            foreach (var job in jobs)
            {
                var rd = new JobNodeRoot { Name = job.Key };
                // потрібно з рядка "job.key" витягнути рік і місяць
                var parts = job.Key.Split('.');

                int year = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);

                rd.Date = new DateTime(year, month, 1);


                //rd.ConsumerPrice = ConsumerPriceIndices.GetConsumerPrices(year, month)[0].ValueTask;

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

            // потрібно вирахувати суму з урахуванням індексу споживчих цін
            // недолік - місяці мають бути послідовні інакше буде неправильно

            for (int i = 0; i < reportDate.Count; i++)
            {
                var r = reportDate[i];
                r.SumWithConsumerPrice = r.Sum;

                for (int j = i; j < reportDate.Count; j++)
                {
                    var r2 = reportDate[j];
                    r.SumWithConsumerPrice = r.SumWithConsumerPrice * r2.ConsumerPrice / 100;
                }
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
