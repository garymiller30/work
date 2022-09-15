// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Job.Data
{
    public sealed class MongoRepository : IRepository
    {
        private IMongoDatabase _mongoDatabase;

        public void CreateConnection(string server, int port, string user, string password, string databaseName)
        {
            var credential = MongoCredential.CreateCredential(databaseName, user, password);
            var setting = new MongoClientSettings
            {
                Credential = credential,
                Server = new MongoServerAddress(server, port)
            };

            var client = new MongoClient(setting);
            _mongoDatabase = client.GetDatabase(databaseName);
        }

        public void CreateConnection(string connectingString,string databaseName)
        {
            var client = new MongoClient(connectingString);
            _mongoDatabase = client.GetDatabase(databaseName);
        }

        public void Add<T>(T item) where T : class, new()
        {
            var col = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            col.InsertOneAsync(item).Wait();
        }

        public void Delete<T>(T item) where T : IWithId
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, item.Id);
            var col = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            col.DeleteOneAsync(filter).Wait();
        }

        public List<T> All<T>() where T : class, new()
        {
            var col = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            var j = col.FindAsync(o => true);
            j.Wait();
            var res = j.Result.ToListAsync();
            res.Wait();
            return res.Result;
        }


        public void Update<T>(T item) where T : IWithId
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, item.Id);
            var col = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            col.ReplaceOneAsync(filter, item).Wait();
        }

        public void Add<T>(string collection, T item) where T : class, new()
        {
            var col = _mongoDatabase.GetCollection<T>(collection);
            col.InsertOneAsync(item).Wait();
        }

        public void Delete<T>(string collection, T item) where T : IWithId
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, item.Id);
            var col = _mongoDatabase.GetCollection<T>(collection);
            col.DeleteOneAsync(filter).Wait();
        }

        public List<T> All<T>(string collection) where T : class, new()
        {
            //.Where(x => _profile.StatusManager.IsViewStatusChecked(x.StatusCode));

            var col = _mongoDatabase.GetCollection<T>(collection);
            //var j =  col.FindAsync(o => true);
            var j = col.FindAsync(o => true);

            j.Wait();
            var res = j.Result.ToListAsync();
            res.Wait();
            return res.Result;
        }

        public object GetRawCollection<T>(string collection) where T : class, new()
        {
            return _mongoDatabase.GetCollection<T>(collection);
        }

        public void Update<T>(string collection, T item) where T : IWithId
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, item.Id);
            var col = _mongoDatabase.GetCollection<T>(collection);
            col.ReplaceOneAsync(filter, item).Wait();
        }

        public T GetById<T>(string collection, object id) where T : class, IWithId
        {

            ObjectId objectId = ObjectId.Empty;

            if (id is string idStr)
            {
                var r = ObjectId.TryParse(idStr, out var ids);
                if (r)
                {
                    objectId = ids;
                }
            }
            else
            {
                objectId = (ObjectId)id;
            }

            var col = _mongoDatabase.GetCollection<T>(collection);
            var f = col.FindAsync(x => x.Id.Equals(objectId));
            f.Wait();
            var l = f.Result.ToListAsync(default);
            l.Wait();
            if (l.Result.Any())
            {
                return l.Result[0];
            }

            return null;
        }
    }
}
