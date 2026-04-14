using System;
using System.Collections.Generic;
using System.Drawing;
using CasheViewer.Reports;

namespace CasheViewer
{
    public class JobNode : INode
    {
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public object Job { get; set; }
        public Color ForegroundColor { get; set; }
        public ReportVersionEnum ReportVersion { get; set; }
        
        public string Name { get; set; }
        public decimal Sum { get; set; }
        public List<INode> Children { get; set; } = new List<INode>();
        public decimal ConsumerPrice { get; set; }
        public decimal SumWithConsumerPrice { get; set; }
        public int CompareTo(JobNode other)
        {
            if (other == null) return 1;

            return Date.CompareTo(other.Date);
        }
    }
}
