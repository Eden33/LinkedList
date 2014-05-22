using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Model.Lock;
using Service.Lock;

namespace Service.Transaction
{
    class TMImplementation : TransactionManager
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

        protected override void Initialize()
        {
            Console.WriteLine("Initialize RamTransactionManager.");
        }

        #region public and protected methods for locking

        public override bool TryLock<T>(int id, T item, string login, out LockBatch batch)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// For each lock request a item-set the lock should be applied to has to be determined by the TransactionManger. 
        ///// The determined item-set depends on the underlying data model and dependencies in this model.
        ///// This implementation only takes loaded items into account.
        ///// From current point of view this seems a good choice and should work even with a database in background.
        ///// </summary>
        ///// <param name="id">The id of the item to be locked</param>
        ///// <param name="item">The item type to be locked</param>
        ///// <returns>A LockBatch instance containing all information needed on the item-set during locking-process</returns>
        protected override LockBatch GetItemsToLock<T>(int id, T item)
        {
            LockBatch batch = new LockBatch();
            return batch;
            
            //TODO: fix me

            //if (ItemType.CollectionPoint.Equals(item))
            //{
            //    LockItem pointLock = new LockItem();
            //    pointLock.ItemTypeInfo = ItemType.CollectionPoint;
            //    pointLock.IDsToLock = new List<int>();

            //    LockItem vatLock = new LockItem();
            //    vatLock.ItemTypeInfo = ItemType.CollectionVat;
            //    vatLock.IDsToLock = new List<int>();

            //    //we know from our data model that deleting a CollectionPoint can affect CollectionVats
            //    //so we have to lock other CollectionPoints containing the same CollectionVats
            //    lock (currentItems)
            //    {
            //        CollectionPoint targetItem = null;
            //        if (currentItems.points.TryGetValue(id, out targetItem))
            //        {
            //            pointLock.IDsToLock.Add(id); // root id of request

            //            //get the vats
            //            IList<CollectionVat> targetVats = targetItem.Vats;

            //            //add the vats first
            //            foreach (CollectionVat vat in targetVats)
            //            {
            //                vatLock.IDsToLock.Add(vat.Id);
            //            }

            //            //go over other CollectionPoints
            //            int oldVatCount = -1;
            //            do
            //            {
            //                oldVatCount = vatLock.IDsToLock.Count;
            //                foreach (KeyValuePair<int, CollectionPoint> e in currentItems.points)
            //                {
            //                    if (e.Value.Id == targetItem.Id)
            //                    {
            //                        continue;
            //                    }
            //                    List<int> idsOfItem = new List<int>();
            //                    bool addIds = false;
            //                    foreach (CollectionVat v in e.Value.Vats)
            //                    {
            //                        idsOfItem.Add(v.Id);

            //                        if (vatLock.IDsToLock.Contains(v.Id))
            //                        {
            //                            addIds = true;
            //                        }
            //                    }
            //                    if (addIds)
            //                    {
            //                        foreach (int currentId in idsOfItem)
            //                        {
            //                            if (vatLock.IDsToLock.Contains(currentId) == false)
            //                            {
            //                                vatLock.IDsToLock.Add(currentId);
            //                            }
            //                        }
            //                        if (pointLock.IDsToLock.Contains(e.Value.Id) == false)
            //                        {
            //                            pointLock.IDsToLock.Add(e.Value.Id);
            //                        }
            //                    }
            //                }
            //            } while (oldVatCount.Equals(vatLock.IDsToLock.Count) == false);

            //        }
            //    }
            //    batch.ItemsToLock.Add(pointLock);
            //    batch.ItemsToLock.Add(vatLock);

            //    foreach (int debugId in pointLock.IDsToLock)
            //    {
            //        Console.WriteLine("Lock CollectionPoint with id: {0}", debugId);
            //    }
            //    foreach (int debugId in vatLock.IDsToLock)
            //    {
            //        Console.WriteLine("Lock CollectionVat with id: {0}", debugId);
            //    }
            //}
            //else if (ItemType.CollectionVat.Equals(item))
            //{
            //    Console.WriteLine("Type {0} currently not implemented.", item);
            //}
            //else
            //{
            //    Console.WriteLine("Type {0} not expected in GetItemsToLock.", item);
            //}
            //return batch;
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
