using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Model.Lock;
using Service.Lock;
using Service.Resource;

namespace Service.Transaction
{

    public abstract class TransactionManager : IData
    {
        protected LockManager lm = new LockManager();
        protected IData dataSource = new DBFacade();

        protected TransactionManager()
        {
            Initialize();
        }

        #region initialization

        /// <summary>
        /// Concrete implementation hast to:
        /// - retrieve current locking information during initialization
        /// </summary>
        protected abstract  void Initialize();

        #endregion

        #region public and protected methods for locking

        /// <summary>
        /// Depending on the "root" item to lock the lock granularity is determined.
        /// Beside sub-items all items that could be affected by root item changes 
        /// must be locked.<br/> 
        /// This method determines and returns needed information in a 
        /// LockBatch object.
        /// </summary>
        /// <typeparam name="T">The item Type to be locked which is a subclass of Model.Item</typeparam>
        /// <param name="id">The id of the item</param>
        /// <returns>A LockBatch object containing all information needed for locking</returns>
        protected abstract  LockBatch GetItemsToLock<T>(int id) where T : Item;

        public abstract bool TryLock<T>(int id, String login, out LockBatch batch) where T : Item;

        #endregion

        #region IData get available resources

        public abstract T GetSingleItem<T>(int id) where T : Item;

        public abstract List<T> GetAllItems<T>() where T : Item;

        #endregion

        #region IDate get and block cached resources

        public abstract object CacheBlockUpdatesMonitor
        {
            get;
        }

        public abstract ResourceCache Cache
        {
            get;
        }

        #endregion
    }

}