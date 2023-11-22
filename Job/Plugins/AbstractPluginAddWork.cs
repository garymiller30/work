using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System;

namespace Interfaces.Plugins
{
    public abstract class AbstractPluginAddWork<T> : IPluginFormAddWork where T :  class, IProcess, new()
    {
        protected IJob _job;

        public virtual string Name { get; set; }
        public IUserProfile UserProfile { get; set; }

        protected readonly ContextMenuStrip _menuPlugin = new ContextMenuStrip();
        protected readonly List<T> _processes = new List<T>();

        protected AbstractPluginAddWork()
        {
            CreateContextMenu();
        }

        protected abstract void CreateContextMenu();


        public ContextMenuStrip GetContextMenu()
        {
            return _menuPlugin;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public IEnumerable<IProcess> GetProcesses() => (IEnumerable<IProcess>)_processes;

        public void SetJob(IUserProfile userProfile, IJob job)
        {
            //UserProfile = userProfile;
            _job = job;

            LoadJobParameters();

        }

        /// <summary>
        /// завантажити процеси роботи
        /// </summary>
        protected virtual void LoadJobParameters()
        {
            _processes.Clear();

            var allProcesses = UserProfile.Base.All<T>();

            if (allProcesses.Any())
            {
                var processes = allProcesses.Where(x => string.Compare(((IProcess)x).ParentId.ToString(), _job.Id.ToString(), StringComparison.Ordinal) == 0);

                foreach (IProcess process in processes)
                {
                    // підключити контекстне меню
                    process.SetParent(this);
                }

                _processes.AddRange(processes);

            }
        }
        /// <summary>
        /// додати процес
        /// </summary>
        /// <returns></returns>
        public IProcess AddProcess()
        {
            var process = (IProcess)new T();
            process.ParentId = _job.Id;
            process.SetParent(this);

            UserProfile.Base.Add((T)process);

            _processes.Add((T)process);
            OnPropertyChanged();
            return process;
        }
        /// <summary>
        /// видалити процес
        /// </summary>
        /// <param name="process"></param>
        public void RemoveProcess(IProcess process)
        {
            _processes.Remove((T)process);
            UserProfile.Base.Delete((T)process);
            OnPropertyChanged();
        }
        /// <summary>
        /// оновити процес
        /// </summary>
        /// <param name="process"></param>
        public void Update(IProcess process)
        {
            UserProfile.Base.Update((T)process);
            OnPropertyChanged();
        }
        /// <summary>
        /// додати платіж до процесу
        /// </summary>
        /// <param name="process"></param>
        /// <param name="sum"></param>
        public void PayProcess(IProcess process, decimal sum)
        {
            process.AddPay(sum);
            Update(process);
        }

        /// <summary>
        /// вертає загальну суму цін всіх процесів
        /// </summary>
        public decimal Price
        {
            get
            {
                if (_processes.Any())
                {
                    return ((IEnumerable<IProcess>)_processes).Sum(x => x.Price);
                }
                return 0;
            }
        }
        /// <summary>
        /// вертає загальну сплачену суму всіх процесів
        /// </summary>
        public decimal Pay
        {
            get
            {
                if (_processes.Any())
                {
                    return ((IEnumerable<IProcess>)_processes).Sum(x => x.Pay);
                }

                return 0;
            }
        }

        public void RemoveProcessByJobId(object id)
        {
            if (UserProfile != null)
            {
                var processes = UserProfile.Base.All<T>().Where(x=>x.ParentId == id);
                var enumerable = processes as T[] ?? processes.ToArray();
                if (enumerable.Any())
                {
                    foreach (T process in enumerable)
                    {
                        UserProfile.Base.Delete(process);
                    }
                }
            }
            
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<IProcess> GetCollection(IUserProfile userProfile)
        {
            return userProfile.Base.All<T>();
        }
    }
}
