using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Model.Lock;
using Service.Lock;
using Service.Lock.LockStrategy;

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

        #region synchronization

        private object currentItemsMonitor = new object();
        public override object GetCurrentItemsMonitorObject()
        {
            // TODO: move it to data source?

            return currentItemsMonitor;
        }

        #endregion

        #region public and protected methods for locking

        public override bool TryLock<T>(int id, string login, out LockBatch batch)
        {
            batch = GetItemsToLock<T>(id);                       
            return true;
        }

        protected override LockBatch GetItemsToLock<T>(int id)
        {
            LockStrategy<T> strategy = new LockStrategy<T>(this);
            return strategy.GetItemsToLock(id);
        }


        #endregion

        #region public methods to access cached and non cached ressources

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
