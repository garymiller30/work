using System;
using CasheViewer.Reports;
using Interfaces;
using Job.Profiles;

namespace CasheViewer.UC
{
    internal interface IReportControl
    {
        event EventHandler<decimal> OnChangeSelected;
        void ShowReport(IReport report);

        void PaySelected(IReport report);
    }
}