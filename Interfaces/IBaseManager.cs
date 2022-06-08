using System;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IBaseManager : IRepository
    {
        IBaseSettings Settings { get; set; }

        bool Connect();
        bool IsConnected { get; }

        List<T> GetCollection<T>(string collection) where T : class, new();
        void Update<T>(string collection, List<T> list) where T : IWithId;
        bool Remove<T>(string collection, T obj) where T : IWithId;
        new bool Add<T>(string collection, T obj) where T : class, new();

        IJob GetByOrderNumber(string orderNumber);
        List<IJob> ApplyViewFilter(int[] statuses);
        List<IJob> Search(string text);
        List<IJob> SearchByDate(DateTime date);
    }
}
