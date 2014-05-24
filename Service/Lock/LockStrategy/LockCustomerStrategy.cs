using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Model.Lock;
using Service.Transaction;

namespace Service.Lock.LockStrategy
{
    public class LockCustomerStrategy : ILockStrategy
    {
        private TransactionManager tm;
        
        public LockCustomerStrategy(TransactionManager tm)
        {
            this.tm = tm;
        }

        public LockBatch GetItemsToLock(int id)
        {
            Console.WriteLine("Customer Strategy called.");

            LockBatch batch = new LockBatch();
            LockItem lockItem = new LockItem();
            lockItem.ItemType = ItemType.CollectionPoint;
            lockItem.IDsToLock = new List<int>(new int[] { 1, 2, 3 });
            LockItem lockItem2 = new LockItem();
            lockItem2.ItemType = ItemType.Customer;
            List<int> aLotOfIds = new List<int>();
            for (int i = 1; i < 30; i++)
            {
                aLotOfIds.Add(i);
            }
            lockItem2.IDsToLock = aLotOfIds;
            batch.ItemsToLock = new List<LockItem>(new LockItem[] { lockItem, lockItem2 });
            return batch;
        }
    }
}
