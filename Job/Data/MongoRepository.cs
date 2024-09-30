// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using FtpClient;
using Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JobSpace.Data
{
    public sealed class MongoRepository : IRepository
    {
        private IMongoDatabase _mongoDatabase;

        public bool IsConnected { get; private set; }

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

        private void Cluster_DescriptionChanged(object sender, MongoDB.Driver.Core.Clusters.ClusterDescriptionChangedEventArgs e)
        {
            IsConnected = e.NewClusterDescription.State == MongoDB.Driver.Core.Clusters.ClusterState.Connected;
            Debug.WriteLine($"Mongodb description:{e.NewClusterDescription.State}");
        }

        public void CreateConnection(string connectingString,string databaseName,int timeout = 10)
        {
            var client = new MongoClient(connectingString);

            Debug.WriteLine($"Mongodb description:{client.Cluster.Description.State}");
            client.Cluster.DescriptionChanged += Cluster_DescriptionChanged;
            
            _mongoDatabase = client.GetDatabase(databaseName);

            IsConnected = _mongoDatabase.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(timeout * 1000);
        }

        public void Add<T>(T item) where T : class, new()
        {
            if (!IsConnected) throw new Exception("Can`t add - Mongodb is offline"); 

            var col = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            col.InsertOneAsync(item).Wait();
        }

        public void Delete<T>(T item) where T : IWithId
        {
            if (!IsConnected) throw new Exception("Can`t delete - Mongodb is offline"); 

            var filter = Builders<T>.Filter.Eq(x => x.Id, item.Id);
            var col = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            col.DeleteOneAsync(filter).Wait();
        }

        public List<T> All<T>() where T : class, new()
        {
            if (!IsConnected) throw new Exception("Can`t get all - Mongodb is offline");

            var col = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Empty;
            var j = col.FindAsync(filter);
            j.Wait();
            var res = j.Result.ToListAsync();
            res.Wait();
            return res.Result;
        }


        public void Update<T>(T item) where T : IWithId
        {
            if (!IsConnected) throw new Exception("Can`t update - Mongodb is offline");

            var filter = Builders<T>.Filter.Eq(x => x.Id, item.Id);
            var col = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            col.ReplaceOneAsync(filter, item).Wait();
        }

        public void Add<T>(string collection, T item) where T : class, new()
        {
            if (!IsConnected) throw new Exception("Can`t add - Mongodb is offline");

            var col = _mongoDatabase.GetCollection<T>(collection);
            col.InsertOneAsync(item).Wait();
        }

        public void Delete<T>(string collection, T item) where T : IWithId
        {
            if (!IsConnected) throw new Exception("Can`t delete - Mongodb is offline"); 

            var filter = Builders<T>.Filter.Eq(x => x.Id, item.Id);
            var col = _mongoDatabase.GetCollection<T>(collection);
            col.DeleteOneAsync(filter).Wait();
        }

        public List<T> All<T>(string collection) where T : class,  new()
        {
            if (!IsConnected) throw new Exception("Can`t get all - Mongodb is offline"); 

            var col = _mongoDatabase.GetCollection<T>(collection);
            var j = col.FindAsync(new BsonDocument());
            j.Wait();
            var res = j.Result.ToListAsync();
            res.Wait();
            return res.Result;
        }

        public object GetRawCollection<T>(string collection) where T : class, new()
        {
            if (!IsConnected) throw new Exception("Can`t GetRawCollection - Mongodb is offline");

            return _mongoDatabase.GetCollection<T>(collection);
        }

        public void Update<T>(string collection, T item) where T : IWithId
        {
            if (!IsConnected) throw new Exception("Can`t update - Mongodb is offline");

            var filter = Builders<T>.Filter.Eq(x => x.Id, item.Id);
            var col = _mongoDatabase.GetCollection<T>(collection);
            col.ReplaceOneAsync(filter, item).Wait();
        }

        public T GetById<T>(string collection, object id) where T : class, IWithId
        {

            if (!IsConnected) throw new Exception("Can`t get by id - Mongodb is offline");

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

            var filter = Builders<T>.Filter.Eq(t=>t.Id, objectId);

            var f = col.FindAsync(filter);
            f.Wait();
            var l = f.Result.ToListAsync(default);
            l.Wait();
            if (l.Result.Any())
            {
                return l.Result[0];
            }

            return null;
        }

        public object GetRawCollection<T>() where T : class, new()
        {
            if (!IsConnected) throw new Exception("Can`t GetRawCollection - Mongodb is offline");
            var name = typeof(T).Name;
            return _mongoDatabase.GetCollection<T>(name);
        }
    }
}
