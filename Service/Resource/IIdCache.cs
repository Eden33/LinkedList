using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Service.Resource
{
    /// <summary>
    /// ResourceCache interface
    /// </summary>
    /// <typeparam name="BaseItem">The base item of items to cache</typeparam>
    interface IIdCache<BaseItem>
    {
        T GetItem<T>(int id) where T : BaseItem;

        void CacheItem<T>(T item) where T : BaseItem;

        IList<T> GetAllItems<T>() where T : BaseItem;
    }
}
