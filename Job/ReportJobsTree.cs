using System.Collections.Generic;

namespace Job
{
    public sealed class ReportJobsTree
    {
        public IEnumerable<Job> Jobs { get; set; } = new List<Job>();
    }
}
