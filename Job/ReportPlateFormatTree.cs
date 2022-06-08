using System.Collections.Generic;

namespace Job
{
    public class ReportPlateFormatTree
    {
        public PlateFormat Name { get; set; }
        public List<Job> MontagList { get; set; }
        public int Montag { get; set; }
        public List<Job> DienstagList { get; set; }
        public int Dienstag { get; set; }
        public List<Job> MittwochList { get; set; }
        public int Mittwoch { get; set; }
        public List<Job> DonnersTagList { get; set; }
        public int Donnerstag { get; set; }
        public List<Job> FreitagList { get; set; }
        public int Freitag { get; set; }
        public List<Job> SamstagList { get; set; }
        public int Samstag { get; set; }
        public List<Job> SonntagList { get; set; }
        public int Sonntag { get; set; }

        public int TotalSum()
        {
            return Montag + Dienstag + Mittwoch + Donnerstag + Freitag + Samstag + Sonntag;
        }
    }
}
