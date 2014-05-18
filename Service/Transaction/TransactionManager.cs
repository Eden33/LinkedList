﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Service.Lock;

namespace Service.Transaction
{

    abstract class TransactionManager 
    {
        protected RamItems currentItems = new RamItems();
        protected LockManager lm = new LockManager();

        protected TransactionManager()
        {
            Initialize();
        }

        /// <summary>
        /// Concrete implementation hast to:
        /// - retrieve current locking information during initialization
        /// </summary>
        protected abstract  void Initialize();

        /// <summary>
        /// Depending on the "root" item to lock the lock granularity is determined.
        /// Beside sub-items all items that could be affected by root item changes 
        /// must be locked.<br/> 
        /// This method determines and returns needed information in a 
        /// LockBatch object.
        /// </summary>
        /// <param name="id">The item id of the item to be locked.</param>
        /// <param name="item">The item type of the item to be locked.</param>
        /// <returns>A LockBatch object containing all information needed for locking.</returns>
        protected abstract  LockBatch GetItemsToLock(int id, ItemType item);

        public abstract CollectionPoint GetCollectionPoint(int id);

        public abstract CollectionVat GetCollectionVat(int id);

        public abstract bool TryLock(int id, ItemType item, String login, out LockBatch batch);
    }

}