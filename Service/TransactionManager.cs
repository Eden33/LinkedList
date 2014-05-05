using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Service.Data
{

    abstract class TransactionManager 
    {
        protected TransactionManager()
        {
            Initialize();
        }

        /// <summary>
        /// Concrete implementation hast to retrieve current
        /// locking information during initialization
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

        public abstract bool TryLock(int id, ItemType item, String login);
    }

    class RamTransactionManager : TransactionManager
    {
        private static RamTransactionManager instance;
        private Dictionary<int, CollectionVat> vats = new Dictionary<int, CollectionVat>();
        private Dictionary<int, CollectionPoint> points = new Dictionary<int, CollectionPoint>();

        private RamTransactionManager() : base() { }

        public static TransactionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RamTransactionManager();
                }
                return instance;
            }
        }

        protected override void Initialize()
        {
            Console.WriteLine("Initialize RamTransactionManager.");
        }

        #region abstract base class get item overrides

        public override CollectionVat GetCollectionVat(int id)
        {
            CollectionVat flyweight = null;
            if (!vats.TryGetValue(id, out flyweight))
            {
                flyweight = new CollectionVat(id);
                vats.Add(id, flyweight);
            }
            return flyweight;
        }

        public override CollectionPoint GetCollectionPoint(int id)
        {
            CollectionPoint flyweight = null;
            if (!points.TryGetValue(id, out flyweight))
            {
                flyweight = new CollectionPoint(id);
                points.Add(id, flyweight);
            }
            return flyweight;
        }
        
        #endregion

        #region base class lock overrides

        public override bool TryLock(int id, ItemType item, string login)
        {
            throw new NotImplementedException();
        }
        protected override LockBatch GetItemsToLock(int id, ItemType item)
        {
            LockBatch batch = new LockBatch();
            LockItem i1 = new LockItem();
            i1.ItemTypeInfo = ItemType.CollectionPoint;
            i1.IDsToLock = new List<int> { 1, 2, 3 };
            LockItem i2 = new LockItem();
            i2.ItemTypeInfo = ItemType.CollectionVat;
            i2.IDsToLock = new List<int> { 5, 6 };
            batch.ItemsToLock = new List<LockItem> { i1, i2 };
            return batch;
        }

        #endregion
    }
}