using System.Collections.Generic;
using System.Linq;
using Job.Profiles;
using MongoDB.Bson;

namespace Job.Fasades
{
    public sealed class PlateOwnersManager
    {

        public Profile UserProfile { get; set; }

        const string CollectionString = "PlateOwners";
        List<PlateOwner> _owners = new List<PlateOwner>();
        public void Load()
        {
            var result = UserProfile.Base.GetCollection<PlateOwner>(CollectionString);
            if (result != null)
            {
                _owners = result;
            }
        }


        public PlateOwner Add(string name)
        {
            var po = new PlateOwner {Name = name};
            return Add(po) ? po : null;
        }

        public bool Add(PlateOwner fs)
        {

            if (UserProfile.Base.Add(CollectionString, fs))
            {
                _owners.Add(fs);
                return true;
            }
            return false;
        }

        public void Remove(object fs)
        {
            if (fs is PlateOwner po)
            {
                if (UserProfile.Base.Remove(CollectionString, po))
                {
                    _owners.Remove(po);
                }
            }
        }

        public void Update(PlateOwner fs)
        {
            UserProfile.Base.Update(CollectionString, fs);
        }

        public ICollection<PlateOwner> GetCollection()
        {
            return _owners;
        }

        public PlateOwner GetParamByName(string server)
        {
            return _owners.FirstOrDefault(x => x.Name.Equals(server));
        }

        public PlateOwner this[int index]
        {
            set { _owners[index] = value; }
            get { return _owners[index]; }
        }
        /// <summary>
        /// отримати ім'я володаря за його Id
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public string GetOwnerNameById(ObjectId ownerId)
        {
            var ret = _owners.FirstOrDefault(x => (ObjectId)x.Id == ownerId);

            return ret == null ? string.Empty : ret.Name;
        }
    }
}
