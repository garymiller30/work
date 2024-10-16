using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Interfaces.Plugins
{
    public abstract class AbstractProcess<T> : IProcess where T : class, IPay, new()
    {
        public object Id { get; set; } = ObjectId.GenerateNewId();
        public object ParentId { get; set; }
        public abstract decimal Price { get; set; }

        protected readonly ContextMenuStrip _contextMenu = new ContextMenuStrip();
        public List<T> Pays = new List<T>();


        public ContextMenuStrip GetContextMenu()
        {
            return _contextMenu;
        }

        public abstract string Name { get; set; }

        public IEnumerable<IPay> GetAllPayed()
        {
            return Pays;
        }

        public decimal Pay
        {
            get
            {
                if (Pays.Any())
                {
                    return Pays.Sum(x => x.Sum);
                }
                return 0;
            }
        }

        public void AddPay(decimal sum)
        {
            var pay = new T(){Sum = sum};
            Pays.Add(pay);
        }

        public abstract bool EditProcess();
        public abstract bool EditProcess(IUserProfile profile);


        public void SetParent(IPluginFormAddWork abstractPluginAddWork)
        {
            CreateContextMenu(abstractPluginAddWork);
        }

        protected abstract void CreateContextMenu(IPluginFormAddWork abstractPluginAddWork);
    }
}
