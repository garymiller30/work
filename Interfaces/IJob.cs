using System;

namespace Interfaces
{
    public interface IJob : IWithId
    {
        DateTime Date { get; set; }
        string Number { get; set; }
        string PreviousOrder { get; set; }
        string Customer { get; set; }
        string Description { get; set; }
        string Note { get; set; }
        [Obsolete]
        bool IsCashe { get; set; }
        [Obsolete]
        bool IsCashePayed { get; set; }
        [Obsolete]
        decimal CachePayedSum { get; set; }
        object CategoryId { get; set; }

        bool UseCustomFolder { get; set; }
        string Folder { get; set; }
        bool DontCreateFolder { get; set; }

        int StatusCode { get; set; }
        int ProgressValue { get; set; }
        bool IsJobInProcess { get; set; }
        

        object GetJob();
    }
}
