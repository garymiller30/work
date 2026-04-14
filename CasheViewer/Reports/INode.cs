using System;
using System.Collections.Generic;
using System.Drawing;

namespace CasheViewer.Reports
{
    public interface INode
    {
        string Name { get; set; }
        decimal Sum { get; set; }
        decimal ConsumerPrice { get; set; }
        decimal SumWithConsumerPrice { get;set; }
        List<INode> Children { get; set; }

        DateTime Date { get; set; }
        string Number { get; set; }
        string Description { get; set; }
        string Category { get; set; }

        object Job { get; set; }

        Color ForegroundColor { get; set; }
        ReportVersionEnum ReportVersion { get; set; }
    }
}