using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Service.Resource
{
    // TODO: check back with client ResourceCache implementation
    // if not both caches needed on client side create ResourceCache<BaseItem> in separate library

    public class ResourceCache : IIdCache<Item>
    {
        private Dictionary<Type, Dictionary<int, Item>> cache = new Dictionary<Type, Dictionary<int, Item>>();

        public T GetItem<T>(int id) where T : Item
        {
            Dictionary<int, Item> theDict = GetDict<T>();
            Item cached = null;
            theDict.TryGetValue(id, out cached);
            return (T) cached;
        }

        public void CacheItem<T>(T item) where T : Item
        {
            Dictionary<int, Item> theDict = GetDict<T>();
            Item cached = null;
            if(!theDict.TryGetValue(item.Id, out cached))
            {
                theDict.Add(item.Id, item);
            }
        }

        public IList<T> GetAllItems<T>() where T : Item
        {
            Dictionary<int, Item> theDict = GetDict<T>();
            List<KeyValuePair<int, Item>> l = theDict.ToList();
            IList<T> theList = new List<T>();
            foreach(KeyValuePair<int, Item> p in l)
            {
                theList.Add((T) p.Value);
            }
            return theList;
        }
        
        #region private methods

        private Dictionary<int, Item> GetDict<T>() where T : Item
        {
            Dictionary<int, Item> theDict = null;
            if(!cache.TryGetValue(typeof(T), out theDict))
            {
                theDict = new Dictionary<int,Item>();
                cache.Add(typeof(T), theDict);
            }
            return theDict;
        }

        #endregion

    }
}
