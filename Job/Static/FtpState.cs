using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Ftp;

namespace Job.Static
{
    public class FtpState : IFtpState
    {
        private  readonly List<FtpStateItem> Items = new List<FtpStateItem>();
        private  bool _oldState;

        public event EventHandler<bool> OnChangeStatus = delegate { };

        public  bool IsEventAnyChanges { get; set; }

        public  void Add(object item, bool state = false)
        {
            Items.Add(new FtpStateItem{Item = item,IsHaveNewFiles = state});
        }

        private  bool IsHaveNewFiles()
        {
            return Items.Any(x => x.IsHaveNewFiles);
        }

        public  void ChangeStatus(object item, bool status)
        {

            var obj = Items.FirstOrDefault(x => x.Item.Equals(item));

            if (obj != null)
            {
                obj.IsHaveNewFiles = status;

                var curState = IsHaveNewFiles();

                if (IsEventAnyChanges || curState != _oldState)
                {
                    _oldState = curState;
                    OnChangeStatus(null, curState);
                }
            }
            else
            {
                throw new Exception("No object in list");
            }
        }

        public object[] GetObjectWithState()
        {
            return IsHaveNewFiles() ? Items.Where(x=>x.IsHaveNewFiles).Select(y=>y.Item).ToArray() : null;
        }
    }

}
