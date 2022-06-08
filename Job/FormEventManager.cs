using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interfaces.MQ;
using Job.Profiles;
using MongoDB.Bson;

namespace Job
{
    public class FormEventManager : IEnumerable<FormEvent>
    {
        public Profile UserProfile { get; set; }

        const string CollectionString = "FormEvents";

        private List<FormEvent> _formEvents;

        //private MongoClient _client;
        //private IMongoDatabase _mongoDatabase;
        //private IMongoCollection<FormEvent> _collection;

        public delegate void EventHandler(FormEvent form);

        public EventHandler OnAdd = delegate { };
        public EventHandler OnRemove = delegate { };
        public EventHandler OnChange = delegate { };


        //public string MongoDbServer { get; set; }
       // public string MongoDbBaseName { get; set; }

        public void Connect(bool reconnect)
        {
            //if (string.IsNullOrEmpty(MongoDbServer)) return;

            try
            {
                if (!reconnect)
                {
                    UserProfile.Plugins.MqController.OnPlateEventAdd += MqOnOnPlateEventAdd;
                    UserProfile.Plugins.MqController.OnPlateEventRemove += MQ_OnPlateEventRemove;
                    UserProfile.Plugins.MqController.OnPlateEventChange += MQ_OnPlateEventChange;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void MQ_OnPlateEventChange(object sender,object id)
        {
            var o = (FormEvent)UserProfile.Base.GetById<FormEvent>(CollectionString, id);

            if (o != null)
            {
                var ff = _formEvents.FirstOrDefault(x => x.Id.Equals(o.Id));
                if (ff != null)
                {
                    ff.Update(o);

                    OnChange(ff);
                }
            }

            //var f = _collection.FindAsync(x => x.Id.Equals(id));
            //f.Wait();
            //var l = f.Result.ToListAsync();
            //l.Wait();

            //if (l.Result.Any())
            //{
            //    var ff = _formEvents.FirstOrDefault(x => x.Id.Equals(l.Result[0].Id));
            //    if (ff != null)
            //    {
            //        ff.Update(l.Result[0]);

            //        OnChange(ff);
            //    }

            //}
        }

        void MQ_OnPlateEventRemove(object sender,object id)
        {
            var ff = _formEvents.FirstOrDefault(x => x.Id.Equals(id));
            if (ff != null)
            {
                _formEvents.Remove(ff);

                OnRemove(ff);
            }
        }

        private void MqOnOnPlateEventAdd(object sender,object id)
        {
            var o = (FormEvent)UserProfile.Base.GetById<FormEvent>(CollectionString, id);
            if (o != null)
            {
                _formEvents.Add(o);
                OnAdd(o);
            }

            //var f = _collection.FindAsync(x => x.Id.Equals(id));
            //f.Wait();
            //var l = f.Result.ToListAsync();
            //l.Wait();
            //if (l.Result.Any())
            //{
            //    _formEvents.Add(l.Result[0]);
            //    OnAdd(l.Result[0]);
            //}
        }

        public void Load()
        {

            var col = UserProfile.Base.GetCollection<FormEvent>(CollectionString);
            if (col != null)
            {
                _formEvents = col;
            }

            //if (_mongoDatabase != null)
            //{
            //    _collection = _mongoDatabase.GetCollection<FormEvent>("FormEvents");
            //    var j = await _collection.FindAsync(job => true);
            //    _formEvents = await j.ToListAsync();
            
            //}
        }

        public void Add(FormEvent formEvent)
        {
            if (UserProfile.Base.Add(CollectionString, formEvent))
            {
                _formEvents.Add(formEvent);
                UserProfile.Plugins.MqController.PublishChanges(MessageEnum.FormEventAdd, formEvent.Id);
            }

            
            //await _collection.InsertOneAsync(formEvent);
           
        }
        public FormEvent Add()
        {
            var c = new FormEvent { Date = DateTime.Now };
            Add(c);
            return c;
        }

        public void RemoveAt(FormEvent formEvent)
        {

            if (UserProfile.Base.Remove(CollectionString, formEvent))
            {
                _formEvents.Remove(formEvent);
                UserProfile.Plugins.MqController.PublishChanges(MessageEnum.FormEventRemove, formEvent.Id);
            }

            //var filter = Builders<FormEvent>.Filter.Eq(x => x.Id, formEvent.Id);
            //await _collection.DeleteOneAsync(filter);

            
        }

        public void Remove(object formEvent)
        {
            RemoveAt((FormEvent)formEvent);
        }

        public void Refresh(FormEvent formEvent)
        {
            try
            {
                UserProfile.Base.Update(CollectionString, formEvent);
                UserProfile.Plugins.MqController.PublishChanges(MessageEnum.FormEventChange, formEvent.Id);
            }
            catch
            {
                
            }
            //var filter = Builders<FormEvent>.Filter.Eq(x => x.Id, formEvent.Id);
            //await _collection.ReplaceOneAsync(filter, formEvent);

            
        }

        public IEnumerator<FormEvent> GetEnumerator()
        {
            return _formEvents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_formEvents).GetEnumerator();
        }
    }
}
