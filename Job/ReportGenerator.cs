using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Job.Profiles;

namespace Job
{
    public sealed class ReportGenerator
    {
        public Profile UserProfile { get; set; }
        public IEnumerable<CustomerReport> GetByYearMonth(string[] customers,int year, int month, bool includeCashe=false)
        {
            var list = new List<CustomerReport>();


            //var q = UserProfile.Jobs.GetJobsByDate(year, month).Where(x => customers.Contains(x.Customer));

            //foreach (Job job in q)
            //{
            //    var g = job.Parts.GroupBy(x => x.Form.Format);

            //    foreach (IGrouping<PlateFormat, Part> parts in g)
            //    {

            //        var owners = parts.ToList().GroupBy(x => x.Form.Format.OwnerId);

            //        foreach (IGrouping<ObjectId, Part> owner in owners)
            //        {
            //            var dates = owner.ToList().GroupBy(x => x.FinishTime.Date);

            //            foreach (IGrouping<DateTime, Part> date in dates)
            //            {
            //                if (date.Key.Month == month && date.Key.Year == year)
            //                {
            //                    if (includeCashe || !job.IsCashe)
            //                    {
            //                        var cr = new CustomerReport
            //                        {
            //                            Name = job.Customer,
            //                            Number = job.Number,
            //                            Description = job.Description,
            //                            PlateFormats = parts.Key,
            //                            PlateCount = job.GetPlottedForms(year, date.Key, parts.Key, owner.Key),
            //                            Date = date.Key.ToString("d"),
            //                            FormOwner = owner.Key,
            //                            AssignedJob = job
            //                        };

            //                        var p = date.ToArray();
            //                        cr.Brak = p.Sum(x => x.Form.Brak);
            //                        cr.PaperProofs = p.Sum(x => x.PaperProof ? 1 : 0);

            //                        list.Add(cr);
            //                    }
            //                }
            //            }

                   
            //        }



            //    }

            //}

            return list;
        }

        public  IEnumerable<ReportCustomerTree> GetReportCustomerTreeByCurrentWeek(DateTime baseDate)
        {
            var l = new List<ReportCustomerTree>();


            //foreach (var customer in UserProfile.Customers)
            //{
            //    var c = new ReportCustomerTree {Name = customer.Name};
            //    c.SetPlateFormats(baseDate,customer);
            //    l.Add(c);
            //}

            return l;
        }

        public  string GetPlatesCountString(IEnumerable<Job> j)
        {
#pragma warning disable CS0612 // 'Job.Parts' is obsolete
#pragma warning disable CS0612 // 'Job.Parts' is obsolete
            var parts = j.Where(x=>x.Parts!=null).SelectMany(x => x.Parts).ToList();
#pragma warning restore CS0612 // 'Job.Parts' is obsolete
#pragma warning restore CS0612 // 'Job.Parts' is obsolete
            var forms = parts.GroupBy(x=>x.Form);

            var str = new StringBuilder();

            foreach (IGrouping<Forms, Part> form in forms)
            {
                str.Append($"[{form.Key} : {form.Sum(x => x.GetCountUnplottedForms(form.Key))}] ");
            }
            return str.ToString();
        }

        public IEnumerable<CustomerReport> GetByDate(string[] customers, DateTime dateTime, bool includeCashe)
        {
            var q = GetByYearMonth(customers, dateTime.Year, dateTime.Month, includeCashe);

            var s = q.Where(x => x.Date.Equals(dateTime.Date.ToString("d")));


            return s.ToList();
        }

        public  string GetPrintTimeString(IEnumerable<Job> j, int i)
        {
            

#pragma warning disable CS0612 // 'Job.Parts' is obsolete
#pragma warning disable CS0612 // 'Job.Parts' is obsolete
            var parts = j.Where(x => x.Parts != null).SelectMany(x => x.Parts).ToList();
#pragma warning restore CS0612 // 'Job.Parts' is obsolete
#pragma warning restore CS0612 // 'Job.Parts' is obsolete
            double forms = parts.Sum(x => x.Form.Unplotted);

            if (i != 0)
            {
                double time = forms/i;

                var ts = TimeSpan.FromMilliseconds(1000*60*60*time);

                return ts.ToString("g");
            }
            return "0:0.0";
        }
    }
}
