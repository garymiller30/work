using System.Collections.Generic;


namespace Interfaces
{
    public interface IProcess : IContextMenu, IWithId
    {
        //ObjectId Id { get; set; }
        object ParentId { get; set; }
        string Name { get; set; }
        decimal Price { get; set; }
        IEnumerable<IPay> GetAllPayed();
        decimal Pay { get; }
        void AddPay(decimal sum);
        bool EditProcess();
        bool EditProcess(IUserProfile profile);

        void SetParent(IPluginFormAddWork abstractPluginAddWork);


    }
}
