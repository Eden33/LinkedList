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
    class RamTM : TransactionManager
    {
        private static RamTM instance;

        private RamTM() : base()
        {
            // TODO: fix me
            //for (int i = 1; i <= 6; i++)
            //{
            //    currentItems.vats.Add(i, new CollectionVat(i));
            //}

            //CollectionPoint p1 = new CollectionPoint(1);
            //p1.Vats.Add(currentItems.vats.ElementAt(0).Value);
            //p1.Vats.Add(currentItems.vats.ElementAt(1).Value);
            //p1.Vats.Add(currentItems.vats.ElementAt(2).Value);

            //CollectionPoint p2 = new CollectionPoint(2);
            //p2.Vats.Add(currentItems.vats.ElementAt(2).Value);
            //p2.Vats.Add(currentItems.vats.ElementAt(3).Value);
            //p2.Vats.Add(currentItems.vats.ElementAt(4).Value);
            //currentItems.points.Add(1, p1);
            //currentItems.points.Add(2, p2);

            //CollectionPoint p3 = new CollectionPoint(3);
            //p3.Vats.Add(currentItems.vats.ElementAt(4).Value);
            //currentItems.points.Add(3, p3);

            //CollectionPoint p4 = new CollectionPoint(4);
            //p4.Vats.Add(currentItems.vats.ElementAt(5).Value);
            //currentItems.points.Add(4, p4);
        }

        public static TransactionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RamTM();
                }
                return instance;
            }
        }

        protected override void Initialize()
        {
            Console.WriteLine("Initialize RamTransactionManager.");
        }

        #region abstract base class get item overrides



        #endregion

        #region base class lock overrides


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


        public override T GetSingleItem<T>(int id, T item)
        {
            throw new NotImplementedException();
        }

        public override bool TryLock<T>(int id, T item, string login, out LockBatch batch)
        {
            throw new NotImplementedException();
        }
    }
}
