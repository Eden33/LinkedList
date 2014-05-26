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
        #region IData get available resources

        T GetSingleItem<T>(int id) where T : Item;

        List<T> GetAllItems<T>() where T : Item;

        #endregion

        #region get and block cached resources

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

        #endregion
    }
}
