using ImageMagick;
using Interfaces;
using JobSpace.Models;
using Logger;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.Fasades
{
    public sealed class BaseManager : IBaseManager
    {

        private JobListFilter jobListFilter = new JobListFilter();

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
                return _repository != null && _repository.IsConnected;

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
            ) { 
                Log.Error(this, $"({Settings.MongoDbServer})BaseManager", "Empty MongoDbServer or MongoDbBaseName");
                
                return false; 
                } //return false;

            try
            {
                _repository.CreateConnection(Settings.MongoDbServer, Settings.MongoDbBaseName, Settings.BaseTimeOut);
            }
            catch (Exception e)
            {
                Log.Error(this, $"({Settings.MongoDbServer})BaseManager", e.Message);
                MessageBox.Show(e.Message, "BaseManager.Connect()");
            }

            return IsConnected;
        }


        public void CreateConnection(string connectionString, string databaseName, int baseTimeOut)
        {
            Settings.MongoDbServer = connectionString;
            Settings.MongoDbBaseName = databaseName;
            Settings.BaseTimeOut = baseTimeOut;

            Connect();
        }

        public void Add<T>(T item) where T : class, new()
        {
            try
            {
                _repository.Add(item);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public void Delete<T>(T item) where T : IWithId
        {
            try
            {
                _repository.Delete(item);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public List<T> All<T>() where T : class, new()
        {
            try
            {
                var list = _repository.All<T>();
                return list;
            }
            catch (Exception)
            {
                throw;
            }
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

        public object GetRawCollection<T>() where T : class, new()
        {
            try
            {
                return _repository.GetRawCollection<T>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetRawCollection<T>(string collection) where T : class, new()
        {
            try
            {
                var obj = _repository.GetRawCollection<T>(collection);
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        void IRepository.Add<T>(string collection, T item)
        {
            try
            {
                _repository.Add<T>(collection, item);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public void Delete<T>(string collection, T item) where T : IWithId
        {
            try
            {
                _repository.Delete(collection, item);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public List<T> All<T>(string collection) where T : class, new()
        {
            try
            {
                return _repository.All<T>(collection);
            }
            catch (Exception)
            {
                throw;
            }
            
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
            try
            {
                return _repository.GetById<T>(collection, id);
            }
            catch (Exception)
            {

                throw;
            }
            
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

        public List<T> GetCollection<T>(string collection) where T : class, IWithId, new()
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
            }
            catch (Exception e)
            {
                Log.Error(this, $"({Settings.MongoDbServer}) BaseManager", $"GetByOrderNumber: {e.Message}");
                return null;
            }
        }

        public List<IJob> ApplyViewFilter(int[] statuses)
        {
            if (statuses.Length == 0) new List<IJob>(0);

            try
            {
                var filter = new BsonDocument("StatusCode", new BsonDocument("$in", new BsonArray(statuses)));

                var jobs = (IMongoCollection<Job>)_repository.GetRawCollection<Job>("Jobs");
                var filteredJobs = jobs.Find(filter).ToList(default);
                return filteredJobs.ToList<IJob>();
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
                                where c.Name.ToLower().Contains(searchString)
                                select c.Id;

                var catList = catFilter.ToList();

                var filter = Builders<Job>.Filter.Or(
                    Builders<Job>.Filter.Regex(j=>j.Number,new BsonRegularExpression($"/{text}/i")),
                    Builders<Job>.Filter.Regex(j => j.Customer, new BsonRegularExpression($"/{text}/i")),
                    Builders<Job>.Filter.Regex(j => j.Note, new BsonRegularExpression($"/{text}/i")),
                    Builders<Job>.Filter.In("CategoryId", catFilter),
                    Builders<Job>.Filter.Regex(j=>j.Description,new BsonRegularExpression($"/{text}/i")));

                var filtered = ((IMongoCollection<Job>)jobs).Find(filter).ToList(default);
                return filtered.ToList<IJob>();
                
            }
            catch (Exception e)
            {
                Log.Error(this, "BaseManager: Search", e.Message);
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

        public List<IJob> ApplyViewFilterStatuses(int[] statuses)
        {
            jobListFilter.Statuses = statuses ?? new int[0];
            return filterJobs();
        }

        public List<IJob> ApplyViewFilterCustomer(string customer)
        {
            jobListFilter.Customer = customer.ToLower();
            return filterJobs();
        }

        public List<IJob> ApplyViewFilterDate(DateTime date)
        {
            jobListFilter.Date = date;
            return filterJobs();
        }

        public List<IJob> ApplyViewFilterText(string text)
        {
            jobListFilter.Text = text.ToLower();
            return filterJobs();
        }

        private List<IJob> filterJobs()
        {

            BsonDocument f_statuses;

           
            f_statuses = new BsonDocument("StatusCode", new BsonDocument("$in", new BsonArray(jobListFilter.Statuses)));

            FilterDefinition<Job> f_customer = Builders<Job>.Filter.Regex(j => j.Customer, new BsonRegularExpression($"/{jobListFilter.Customer}/i"));

            var f_date = Builders<Job>.Filter.And(
                Builders<Job>.Filter.Gte(j => j.Date, jobListFilter.Date.Date),
                Builders<Job>.Filter.Lt(j => j.Date, jobListFilter.Date.AddDays(1).Date));


            var categories = _repository.GetRawCollection<Category>("Categories");

            var catFilter = from c in ((IMongoCollection<Category>)categories).AsQueryable()
                            where c.Name.ToLower().Contains(jobListFilter.Text)
                            select c.Id;

            var catList = catFilter.ToList();


            var f_text = Builders<Job>.Filter.Or(
                Builders<Job>.Filter.Regex(j => j.Number, new BsonRegularExpression($"/{jobListFilter.Text}/i")),
                Builders<Job>.Filter.Regex(j => j.Note, new BsonRegularExpression($"/{jobListFilter.Text}/i")),
                Builders<Job>.Filter.Regex(j => j.Description, new BsonRegularExpression($"/{jobListFilter.Text}/i")),
                Builders<Job>.Filter.In("CategoryId", catFilter)
            );

            var filter = Builders<Job>.Filter.And(
                f_statuses,
                f_customer,
                //f_date,
                f_text
            );
            var jobs = _repository.GetRawCollection<Job>("Jobs");
            var filteredJobs = ((IMongoCollection<Job>)jobs).Find(filter).ToList(default);
            return filteredJobs.ToList<IJob>();
        }
    }
}
