
using System.Collections.Generic;
using System.ComponentModel;

namespace Interfaces
{
    public interface IPluginFormAddWork :  IContextMenu, INotifyPropertyChanged
    {
        string Name { get; set; }
        IUserProfile UserProfile { get; set; }
        IEnumerable<IProcess> GetProcesses();
        void SetJob(IUserProfile userProfile, IJob job);
        IProcess AddProcess();
        void RemoveProcess(IProcess process);
        void Update(IProcess process);
        void PayProcess(IProcess process, decimal sum);
        IEnumerable<IProcess> GetCollection(IUserProfile userProfile);

        decimal Price { get; }
        decimal Pay { get; }

        void RemoveProcessByJobId(object id);
        string GetValue(IJob job, string param);
    }
}
