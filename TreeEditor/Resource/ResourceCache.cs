using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using TreeEditor.UI.Proxy;

namespace TreeEditor.Resource
{
    class ResourceCache
    {
        private Dictionary<Type, Dictionary<int, Item>> modelFlyweight = new Dictionary<Type, Dictionary<int, Item>>();
        private Dictionary<Type, Dictionary<int, UIItem>> proxyFlyweight = new Dictionary<Type, Dictionary<int, UIItem>>();
        
        #region public methods to cache and retrieve model data 

        /// <summary>
        /// Returns the cached model item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns>The cached item or null if nothing in cache</returns>
        public Item GetItem<T>(T item) where T : Item
        {
            Dictionary<int, Item> theDict = GetDict<T>();
            Item cached = null;
            theDict.TryGetValue(item.Id, out cached);
            return cached;

        }

        /// <summary>
        /// Caches the model item if not allready cached 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void CacheItem<T>(T item) where T : Item
        {
            Dictionary<int, Item> theDict = GetDict<T>();
            Item cached = null;
            if(!theDict.TryGetValue(item.Id, out cached))
            {
                theDict.Add(item.Id, item);
            }
        }

        #endregion

        #region public methods to cache and retrieve ui proxy data

        /// <summary>
        /// Returns the cached ui proxy item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns>The cached item or null if nothing in cache</returns>
        public T GetUIItem<T>(int id) where T : UIItem
        {
            Dictionary<int, UIItem> theDict = GetProxyDict<T>();
            UIItem cached = null;
            theDict.TryGetValue(id, out cached);
            return (T) cached;

        }

        /// <summary>
        /// Caches the ui proxy item if not allready cached
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void CacheUIItem<T>(T item) where T : UIItem
        {
            Dictionary<int, UIItem> theDict = GetProxyDict<T>();
            UIItem cached = null;
            if(!theDict.TryGetValue(item.Id, out cached))
            {
                theDict.Add(item.Id, item);
            }
        }

        public List<T> GetAllUIItems<T>() where T : UIItem
        {
            Dictionary<int, UIItem> theDict = GetProxyDict<T>();
            List<KeyValuePair<int, UIItem>> l = theDict.ToList();
            List<T> theList = new List<T>();
            foreach(KeyValuePair<int, UIItem> p in l)
            {
                theList.Add((T) p.Value);
            }
            return theList;
        }
        
        #endregion

        #region private methods

        private Dictionary<int, Item> GetDict<T>() where T : Item
        {
            Dictionary<int, Item> theDict = null;
            if (!modelFlyweight.TryGetValue(typeof(T), out theDict))
            {
                theDict = new Dictionary<int, Item>();
                modelFlyweight.Add(typeof(T), theDict);
            }
            return theDict;
        }

        private Dictionary<int, UIItem> GetProxyDict<T>() where T : UIItem
        {
            Dictionary<int, UIItem> theDict = null;
            if (!proxyFlyweight.TryGetValue(typeof(T), out theDict))
            {
                theDict = new Dictionary<int, UIItem>();
                proxyFlyweight.Add(typeof(T), theDict);
            }
            return theDict;
        }

        #endregion
    }
}
