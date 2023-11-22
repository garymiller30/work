using System.Collections.Generic;
using Job.Profiles;

namespace Job.Fasades
{
    public sealed class ColorProofManager
    {
        public Profile UserProfile { get; set; }

        const string CollectionString = "ColorProof";


        List<ColorProof> _colorProofs = new List<ColorProof>();

        
        public ColorProofManager()
        {
       
        }


        public void Load()
        {

            var cltn = UserProfile.Base.GetCollection<ColorProof>(CollectionString);
            if (cltn != null) _colorProofs = cltn;

            //if (string.IsNullOrEmpty(MongoDbServer)) return;

            //try
            //{
            //    _client = new MongoClient("mongodb://" + MongoDbServer);
            //    _mongoDatabase = _client.GetDatabase(MongoDbBaseName);

            //    _collection = _mongoDatabase.GetCollection<ColorProof>("ColorProof");
            //    var j = _collection.FindAsync(job => true);
            //    j.Wait();
            //    var k = j.Result.ToListAsync();
            //    k.Wait();
            //    _colorProofs = k.Result;
            //}
            //catch 
            //{
            //}

        }

        public ColorProof Add()
        {
            var cp = new ColorProof();

            if (UserProfile.Base.Add(CollectionString, cp))
            {
                _colorProofs.Add(cp);
            }

            //_colorProofs.Add(cp);
            //_collection.InsertOneAsync(cp).Wait();
            //Managers.MQ.PublishChanges(MessageEnum.CustomerAdd, customer.Id);
            return cp;
        }

        public void Update(ColorProof cp)
        {
            UserProfile.Base.Update(CollectionString, cp);
            //var filter = Builders<ColorProof>.Filter.Eq(x => x.Id, cp.Id);
            //_collection.ReplaceOneAsync(filter, cp).Wait();
        }



        public System.Collections.ICollection Get()
        {
            return _colorProofs.ToArray();
        }

        public void Remove(ColorProof cp)
        {
            if (UserProfile.Base.Remove(CollectionString, cp))
            {
                _colorProofs.Remove(cp);
            }

            
            //var filter = Builders<ColorProof>.Filter.Eq(x => x.Id, cp.Id);
            //_collection.DeleteOneAsync(filter).Wait();
        }
    }
}
