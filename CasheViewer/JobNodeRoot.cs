using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CasheViewer.Reports;

namespace CasheViewer
{
    public class JobNodeRoot : INode
    {
        public string Name { get; set; }

        public decimal Sum
        {
            get
            {
                return Children.Sum(x => x.Sum);
            }
            set
            {

            }
        }

        public List<INode> Children { get; set; } = new List<INode>();
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public object Job { get; set; }
        public Color ForegroundColor { get; set; }
        public ReportVersionEnum ReportVersion { get; set; }
        public decimal ConsumerPrice { get; set; }
        public decimal SumWithConsumerPrice { get; set; }
    }
}
