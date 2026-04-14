using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MailNotifier.Serialization
{
    [Serializable]
    internal class SerializeableCollection
    {
        Dictionary<String, string> Collection = new Dictionary<String, string>();
        internal SerializeableCollection()
        {

        }

        internal static SerializeableCollection GetSerializeableCollection(NameValueCollection col)
        {
            if (col == null)
                return null;

            SerializeableCollection scol = new SerializeableCollection();
            foreach (String key in col.Keys)
                scol.Collection.Add(key, col[key]);

            return scol;
        }

        internal static SerializeableCollection GetSerializeableCollection(StringDictionary col)
        {
            if (col == null)
                return null;

            SerializeableCollection scol = new SerializeableCollection();
            foreach (String key in col.Keys)
                scol.Collection.Add(key, col[key]);

            return scol;
        }

        internal void SetColletion(NameValueCollection scol)
        {

            foreach (String key in Collection.Keys)
            {
                scol.Add(key, this.Collection[key]);
            }

        }

        internal void SetColletion(StringDictionary scol)
        {

            foreach (String key in Collection.Keys)
            {
                if (scol.ContainsKey(key))
                    scol[key] = Collection[key];
                else
                    scol.Add(key, this.Collection[key]);
            }
        }
    }
}
