using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Service.Resource
{
    public interface IData
    {
        T GetSingleItem<T>(int id) where T : Item;

        List<T> GetAllItems<T>() where T : Item;

        /// <summary>
        /// Get the monitor object and lock it if you want 
        /// to block updating the cache of this data source
        /// </summary>
        object CacheBlockUpdatesMonitor
        {
            get;
        }

        /// <summary>
        /// Get the cache of this data source
        /// </summary>
        ResourceCache Cache
        {
            get;
        }
    }
}
