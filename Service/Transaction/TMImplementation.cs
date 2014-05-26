using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Model.Lock;
using Service.Lock;
using Service.Lock.LockStrategy;
using Service.Resource;

namespace Service.Transaction
{
    public class TMImplementation : TransactionManager
    {
        private static TMImplementation instance;

        private TMImplementation() : base() {}

        public static TransactionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TMImplementation();
                }
                return instance;
            }
        }

        #region initialization

        protected override void Initialize()
        {
            Console.WriteLine("Initialize RamTransactionManager.");
        }

        #endregion

        #region IDate get and block cached resources

        public override ResourceCache Cache
        {
            get { return this.dataSource.Cache; }
        }

        public override object CacheBlockUpdatesMonitor
        {
            get
            {
                return this.dataSource.CacheBlockUpdatesMonitor;
            }
        }

        #endregion

        #region public and protected methods for locking


        public override bool TryLock<T>(int id, string login, out LockBatch batch)
        {
            bool lockSuccess = false;
            batch = GetItemsToLock<T>(id);
            if(batch.ItemsToLock != null)
            {
                lockSuccess = lm.Lock(login, batch, LockMode.Locked);
            }
            return lockSuccess;
        }

        public override bool Unlock<T>(int id, String login, out LockBatch batch)
        {
            bool unlockSuccess = false;
            batch = GetItemsToLock<T>(id);
            if(batch.ItemsToLock != null)
            {
                unlockSuccess = lm.Unlock(login, batch, LockMode.Locked);
            }
            return unlockSuccess;
        }

        protected override LockBatch GetItemsToLock<T>(int id)
        {
            LockStrategy<T> strategy = new LockStrategy<T>(this);
            return strategy.GetItemsToLock(id);
        }

        public override LockBatch GetCurrentLocks(string loginName)
        {
            return lm.GetCurrentLocks(loginName);
        }

        #endregion

        #region IDate  get available resources

        /// <summary>
        /// Get a single item of Type T.
        /// </summary>
        /// <typeparam name="T">The item type of the requested class which has to be a subclass of Model.Item</typeparam>
        /// <param name="id">The item id</param>
        /// <returns>The item or null if no such item exists.</returns>
        public override T GetSingleItem<T>(int id)
        {
            T item = dataSource.GetSingleItem<T>(id);
            return item;
        }

        /// <summary>
        /// Get all items available of Type T.
        /// </summary>
        /// <typeparam name="T">The item type of the requested class which has to be a subclass of Model.Item</typeparam>
        /// <returns>The list of items requested. Of no items available a empty list is returned.</returns>
        public override List<T> GetAllItems<T>()
        {
            List<T> items = dataSource.GetAllItems<T>();
            return items;
        }

        #endregion

    }
}
