using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Interfaces;
using Job.Profiles;
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

            var oldVariant = GetJobsByCustomers(false);

            var newVariant = GetJobsByCustomersByPlugins(false);

            var comparer = new CustomerComparer();

            foreach (JobNodeRoot customer in oldVariant)
            {
                if (newVariant.Contains(customer, comparer))
                {
                    var c = newVariant.First(x => x.Name.Equals(customer.Name));
                    c.Children.AddRange(customer.Children);
                }
                else
                {
                    newVariant.Add(customer);
                }
            }
            return newVariant.Cast<INode>().ToList();
        }

        public List<JobNodeRoot> GetJobsByCustomers(bool isPayed)
        {
#pragma warning disable CS0612 // 'IJob.IsCashe' is obsolete
#pragma warning disable CS0612 // 'IJob.IsCashePayed' is obsolete
            var jobs = UserProfile.Jobs.GetJobs().Where(x => x.IsCashe && x.IsCashePayed == isPayed);
#pragma warning restore CS0612 // 'IJob.IsCashePayed' is obsolete
#pragma warning restore CS0612 // 'IJob.IsCashe' is obsolete
            var list = new List<JobNodeRoot>();
            var customers = jobs.GroupBy(x => x.Customer);

            //Total = 0;

            foreach (var customer in customers)
            {
                var c = new JobNodeRoot
                {
                    Name = customer.Key,
                    Children =
                        customer.Select(
                            x =>

                                new JobNode
                                {
                                    Date = x.Date,
                                    Number = x.Number,
                                    Description = x.Description,
#pragma warning disable CS0612 // 'IJob.CachePayedSum' is obsolete
                                    Sum = x.CachePayedSum,
#pragma warning restore CS0612 // 'IJob.CachePayedSum' is obsolete
                                    Job = x,
                                    Category = UserProfile.Categories.GetCategoryNameById(x.CategoryId),
                                    ForegroundColor = Color.Black,
                                    ReportVersion = ReportVersionEnum.Version1
                                })

                .Cast<INode>().ToList()

                };



                list.Add(c);
            }

#pragma warning disable CS0612 // 'IJob.CachePayedSum' is obsolete
            Total += jobs.Sum(x => x.CachePayedSum);
#pragma warning restore CS0612 // 'IJob.CachePayedSum' is obsolete

            return list;
        }
        public List<JobNodeRoot> GetJobsByCustomersByPlugins(bool isPayed)
        {
            Dictionary<ObjectId, decimal> jobsDictionary = new Dictionary<ObjectId, decimal>();
            // отримаємо всі плагіни
            foreach (var pluginFormAddWork in UserProfile.Plugins.GetPluginFormAddWorks())
            {
                // отримаємо всі колекції з неоплаченими рахунками
                var collection = pluginFormAddWork.GetCollection(UserProfile)
                    .Where(y => y.Price - y.Pay > 0)
                    .GroupBy(x => x.ParentId);

                foreach (IGrouping<ObjectId, IProcess> processes in collection)
                {
                    if (!jobsDictionary.ContainsKey(processes.Key))
                    {
                        jobsDictionary.Add(processes.Key, 0);
                    }
                    jobsDictionary[processes.Key] += processes.Sum(x => x.Price - x.Pay);
                }
            }
            var list = new List<JobNodeRoot>();
            var listJob = jobsDictionary
                .Select(pair => UserProfile.Jobs.GetJobs().FirstOrDefault(x => x.Id == pair.Key))
                .Where(job => job != null)
                .GroupBy(c => c.Customer);

            foreach (IGrouping<string, IJob> grouping in listJob)
            {
                var c = new JobNodeRoot
                {
                    Name = grouping.Key,
                    Children =
                        grouping.Select(
                                x =>
                                    new JobNode
                                    {
                                        Date = x.Date,
                                        Number = x.Number,
                                        Description = x.Description,
                                        Sum = jobsDictionary[x.Id],
                                        Job = x,
                                        Category = UserProfile.Categories.GetCategoryNameById(x.CategoryId),
                                        ForegroundColor = Color.MediumBlue,
                                        ReportVersion = ReportVersionEnum.Version2
                                    })
                            .Cast<INode>().ToList()
                };



                list.Add(c);
            }

            Total += list.Sum(x => x.Sum);

            return list;


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
