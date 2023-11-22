using System;
using System.Collections.Generic;
using System.Linq;
using Job.Profiles;

namespace Job
{
    public sealed class ReportCustomerTree
    {
        public Profile UserProfile { get; set; }

        public string Name { get; set; }

        public IEnumerable<ReportPlateFormatTree> PlateFormat { get; private set; } = new List<ReportPlateFormatTree>();


        public int TotalPlateCount()
        {
            return PlateFormat.Sum(x => x.TotalSum());
        }

        internal void SetPlateFormats(DateTime baseDate,  Customer customer)
        {
            //var plateFormats = customer.GetUsedPlateFormats();
            //foreach (var plateFormat in plateFormats)
            //{
            //    var rpf = new ReportPlateFormatTree
            //    {
            //        Name = plateFormat
            //    };

            //    var montag = UserProfile.Jobs.GetPlottedPlates(Fasades.JobManager.GetWeekDate(baseDate, DayOfWeek.Monday), customer,
            //        plateFormat);
            //    rpf.Montag = montag.Item1;
            //    rpf.MontagList = montag.Item2;

            //    var dienstag =
            //        UserProfile.Jobs.GetPlottedPlates(Fasades.JobManager.GetWeekDate(baseDate, DayOfWeek.Tuesday), customer, plateFormat);

            //    rpf.Dienstag = dienstag.Item1;
            //    rpf.DienstagList = dienstag.Item2;

            //    var mittwoch = UserProfile.Jobs.GetPlottedPlates(Fasades.JobManager.GetWeekDate(baseDate, DayOfWeek.Wednesday), customer,
            //        plateFormat);

            //    rpf.Mittwoch = mittwoch.Item1;
            //    rpf.MittwochList = mittwoch.Item2;

            //    var donnerstag = UserProfile.Jobs.GetPlottedPlates(Fasades.JobManager.GetWeekDate(baseDate, DayOfWeek.Thursday), customer,
            //        plateFormat);

            //    rpf.Donnerstag = donnerstag.Item1;
            //    rpf.DonnersTagList = donnerstag.Item2;

            //    var freitag = UserProfile.Jobs.GetPlottedPlates(Fasades.JobManager.GetWeekDate(baseDate, DayOfWeek.Friday), customer,
            //        plateFormat);

            //    rpf.Freitag = freitag.Item1;
            //    rpf.FreitagList = freitag.Item2;

            //    var samstag = UserProfile.Jobs.GetPlottedPlates(Fasades.JobManager.GetWeekDate(baseDate, DayOfWeek.Saturday), customer,
            //        plateFormat);

            //    rpf.Samstag = samstag.Item1;
            //    rpf.SamstagList = samstag.Item2;

            //    var sonntag = UserProfile.Jobs.GetPlottedPlates(Fasades.JobManager.GetWeekDate(baseDate, DayOfWeek.Sunday), customer,
            //        plateFormat);

            //    rpf.Sonntag = sonntag.Item1;
            //    rpf.SonntagList = sonntag.Item2;
            //    PlateFormat.Add(rpf);
            //}
        }
    }
}
