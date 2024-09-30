using System.Collections.Generic;

namespace JobSpace
{
    public sealed class ReportJobsTree
    {
        public IEnumerable<Job> Jobs { get; set; } = new List<Job>();
    }
}
