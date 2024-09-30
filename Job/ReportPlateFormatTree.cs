using System.Collections.Generic;

namespace JobSpace
{
    public sealed class ReportPlateFormatTree
    {
        public PlateFormat Name { get; set; }
        public IEnumerable<Job> MontagList { get; set; }
        public int Montag { get; set; }
        public IEnumerable<Job> DienstagList { get; set; }
        public int Dienstag { get; set; }
        public IEnumerable<Job> MittwochList { get; set; }
        public int Mittwoch { get; set; }
        public IEnumerable<Job> DonnersTagList { get; set; }
        public int Donnerstag { get; set; }
        public IEnumerable<Job> FreitagList { get; set; }
        public int Freitag { get; set; }
        public IEnumerable<Job> SamstagList { get; set; }
        public int Samstag { get; set; }
        public IEnumerable<Job> SonntagList { get; set; }
        public int Sonntag { get; set; }

        public int TotalSum()
        {
            return Montag + Dienstag + Mittwoch + Donnerstag + Freitag + Samstag + Sonntag;
        }
    }
}
