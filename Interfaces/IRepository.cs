using System.Collections.Generic;


namespace Interfaces
{
    public interface IRepository
    {
        bool IsConnected { get; }
        void CreateConnection(string connectionString,  string databaseName);
        void Add<T>(T item) where T : class,  new();
        void Add<T>(string collection,T item) where T : class, new();

        void Delete<T>(T item) where T : IWithId;
        void Delete<T>(string collection,T item) where T : IWithId;

        List<T> All<T>() where T : class, new();
        List<T> All<T>(string collection) where T : class, new();

        void Update<T>(T item) where T : IWithId;
        void Update<T>(string collection,T item) where T : IWithId;

        T GetById<T>(string collection, object id) where T : class,IWithId;

        object GetRawCollection<T>(string collection) where T : class, new();
    }
}
