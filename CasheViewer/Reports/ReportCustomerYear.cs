﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Interfaces;

namespace CasheViewer.Reports
{
    public class ReportCustomerYear : IReport
    {
        public IUserProfile UserProfile { get; set; }
        public List<INode> GetNodes()
        {
            Total = 0;
            var version1 = GetJobsByCustomers(false);
            Total = version1.Sum(x => x.Sum);
            return version1.Cast<INode>().ToList();
        }

        public decimal Total { get; set; }

        public List<JobNodeRoot> GetJobsByCustomers(bool isPayed)
        {

            var reportDate = new List<JobNodeRoot>();

            // тепер візьмемо інфу з плагінів
            var preportPlugins = GetJobsByCustomerRootByPlugin(isPayed);

            var comparer = new CustomerComparer();
            // об'єднаємо в один
            foreach (JobNodeRoot customer in reportDate)
            {
                if (preportPlugins.Contains(customer, comparer))
                {
                    // такий замовник є
                    var foundCustomer = preportPlugins.First(x => x.Name.Equals(customer.Name));
                    // шукаємо дати: 20.01...
                    foreach (INode date in customer.Children)
                    {
                        if (foundCustomer.Children.Contains(date,comparer))
                        {
                            var cc = foundCustomer.Children.First(x => x.Name.Equals(date.Name));
                            cc.Children.AddRange(date.Children);
                        }
                        else
                        {
                           foundCustomer.Children.Add(date);
                        }
                    }
                }
                else
                {
                    preportPlugins.Add(customer);
                }
            }

            return preportPlugins;
        }

        class CustomerComparer : IEqualityComparer<INode>
        {
            public bool Equals(INode x, INode y)
            {
                return x.Name.Equals(y.Name);
            }

            public int GetHashCode(INode obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        List<JobNodeRoot> GetJobsByCustomerRootByPlugin(bool isPayed)
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
                    Logger.Log.Error(null, $"Job not found",$"{pair.Key}");
                }
            }

            var jobs = rawJobs.GroupBy(c => c.Customer);
           //IEnumerable<IGrouping<string,Job.Job>> jobs = jobDictionary.Select(x => UserProfile.Base.GetById<Job.Job>("Jobs",x.Key)).GroupBy(c => c.Customer);

            foreach (var job in jobs)
            {
                var rd = new JobNodeRoot { Name = job.Key };

                rd.Children.AddRange(
                    job.GroupBy(x => x.Date.ToString("yy.MM"))
                        .Select(y => (INode)new JobNodeRoot()
                        {
                            Name = y.Key,
                            Children = y
                                .Select(u => (INode)new JobNode()
                                {
                                    Date = u.Date,
                                    Number = u.Number,
                                    Description = u.Description,
                                    Sum = jobDictionary[u.Id],
                                    Job = u,
                                    Category = UserProfile.Categories.GetCategoryNameById(u.CategoryId),
                                    ForegroundColor = Color.MediumBlue,
                                    ReportVersion = ReportVersionEnum.Version2
                                }).ToList()
                        }));

                rd.Children.Sort((x, y) => x.Date.CompareTo(y.Date));

                reportDate.Add(rd);
            }
            return reportDate;
        }
    }
}
