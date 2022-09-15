using Interfaces;
using Logger;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Job.Fasades
{
    public sealed class BaseManager : IBaseManager
    {

        private readonly IRepository _repository;

        public IBaseSettings Settings { get; set; }

        private BaseManager(IRepository repository)
        {
            _repository = repository;
        }
        public BaseManager(IRepository repository, IBaseSettings settings) : this(repository)
        {
            Settings = settings;
        }
        public bool IsConnected
        {
            get
            {
                if (_repository == null) return false;
                //if (_mongoDatabase == null) return false;
                return true;// _mongoDatabase.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(BaseTimeOut*1000);

            }
        }



        /// <summary>
        /// підключення до сервера
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            if (string.IsNullOrEmpty(Settings.MongoDbServer) ||
                string.IsNullOrEmpty(Settings.MongoDbBaseName)
            ) return false;

            try
            {
                _repository.CreateConnection(Settings.MongoDbServer, Settings.MongoDbBaseName);
            }
            catch (Exception e)
            {
                Log.Error(this, $"({Settings.MongoDbServer})BaseManager", e.Message);
                MessageBox.Show(e.Message, "BaseManager.Connect()");
            }

            return IsConnected;
        }




        public void CreateConnection(string connectionString, string databaseName)
        {
            Settings.MongoDbServer = connectionString;
            Settings.MongoDbBaseName = databaseName;

            Connect();
        }

        public void Add<T>(T item) where T : class, new()
        {
            _repository.Add(item);
        }

        public void Delete<T>(T item) where T : IWithId
        {
            _repository.Delete(item);
        }

        public List<T> All<T>() where T : class, new()
        {
            return _repository.All<T>();
        }

        public void Update<T>(T item) where T : IWithId
        {
            try
            {
                _repository.Update(item);
            }
            catch (Exception e)
            {
                Log.Error(this, $"({Settings.MongoDbServer}) BaseManager", e.InnerException?.Message);
                throw;
            }
        }

        public object GetRawCollection<T>(string collection) where T : class, new()
        {
            return _repository.GetRawCollection<T>(collection);
        }

        void IRepository.Add<T>(string collection, T item)
        {
            _repository.Add<T>(collection, item);
        }

        public void Delete<T>(string collection, T item) where T : IWithId
        {
            _repository.Delete(collection, item);
        }

        public List<T> All<T>(string collection) where T : class, new()
        {
            return _repository.All<T>(collection);
        }

        public void Update<T>(string collection, T item) where T : IWithId
        {
            try
            {
                _repository.Update(collection, item);
            }
            catch (Exception e)
            {
                Log.Error(this, $"({Settings.MongoDbServer}) BaseManager", e.InnerException?.Message);
                throw;
            }

        }

        public T GetById<T>(string collection, object id) where T : class, IWithId
        {
            return _repository.GetById<T>(collection, id);
        }

        public bool Add<T>(string collection, T obj) where T : class, new()
        {
            try
            {
                _repository.Add<T>(collection, obj);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(this, $"({Settings.MongoDbServer}) BaseManager", e.InnerException?.Message);
                return false;
            }
        }


        public void Update<T>(string collection, List<T> list) where T : IWithId
        {
            try
            {
                foreach (var obj in list)
                {
                    _repository.Update(collection, obj);
                }
            }
            catch (Exception e)
            {
                Log.Error(this, $"({Settings.MongoDbServer}) BaseManager", e.InnerException?.Message);
            }

        }


        public bool Remove<T>(string collection, T obj) where T : IWithId
        {
            try
            {
                _repository.Delete(collection, obj);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(this, $"({Settings.MongoDbServer}) BaseManager", e.InnerException?.Message);
                return false;
            }
        }

        public List<T> GetCollection<T>(string collection) where T : class, new()
        {
            try
            {
                return _repository.All<T>(collection);
            }
            catch (Exception e)
            {
                Log.Error(this, $"({Settings.MongoDbServer}) BaseManager", e.InnerException?.Message);
            }
            return new List<T>();
        }

        public IJob GetByOrderNumber(string orderNumber)
        {
            try
            {
                var jobs = _repository.GetRawCollection<Job>("Jobs");
                var job = ((IMongoCollection<Job>)jobs).Find(x => x.Number.Equals(orderNumber));
                return job.ToList(default).LastOrDefault();

                //return _repository.All<Job>("Jobs").LastOrDefault(x=>x.Number.Equals(orderNumber));

            }
            catch (Exception e)
            {
                Log.Error(this, $"({Settings.MongoDbServer}) BaseManager", $"GetByOrderNumber: {e.Message}");
                return null;
            }
        }

        public List<IJob> ApplyViewFilter(int[] statuses)
        {
            try
            {
                if (statuses.Length > 0)
                {
                    var jobs = _repository.GetRawCollection<Job>("Jobs");
                    var jobFilter = ((IMongoCollection<Job>)jobs).Find(x => statuses.Contains(x.StatusCode));
                    return jobFilter.ToList(default).ToList<IJob>();

                }

                return new List<IJob>(0);
            }
            catch (Exception e)
            {
                Log.Error(this, "BaseManager: ApplyViewFilter", e.Message);

            }

            return new List<IJob>();
        }

        public List<IJob> Search(string text)
        {
            var searchString = text.ToLower(CultureInfo.InvariantCulture);

            try
            {
                var jobs = _repository.GetRawCollection<Job>("Jobs");
                var categories = _repository.GetRawCollection<Category>("Categories");

                var catFilter = from c in ((IMongoCollection<Category>)categories).AsQueryable()
                                where c.Name.ToLower(CultureInfo.InvariantCulture).Contains(searchString)
                                select c.Id;

                var catList = catFilter.ToList();

                var jobFilter = from j in ((IMongoCollection<Job>)jobs).AsQueryable()
                                where j.Customer.ToLower(CultureInfo.InvariantCulture).Contains(searchString) || j.Description.ToLower(CultureInfo.InvariantCulture).Contains(searchString)
                                                                                  || j.Note.ToLower(CultureInfo.InvariantCulture).Contains(searchString)
                                                                                  || j.Number.ToLower(CultureInfo.InvariantCulture).Contains(searchString)
                                                                                  || catList.Contains(j.CategoryId)

                                select j;
                return jobFilter.ToList().ToList<IJob>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<IJob> SearchByDate(DateTime date)
        {
            try
            {
                DateTime currentDayStart = date.Date;
                DateTime currentDayEnds = currentDayStart.AddDays(1);

                var jobs = _repository.GetRawCollection<Job>("Jobs");
                var jobFilter = from j in ((IMongoCollection<Job>)jobs).AsQueryable()
                                where j.Date >= currentDayStart && j.Date < currentDayEnds
                                select j;
                return jobFilter.ToList().ToList<IJob>();

            }
            catch (Exception e)
            {
                Log.Error(this, "BaseManager : SearchByDate", e.Message);
            }

            return new List<IJob>();
        }
    }
}
