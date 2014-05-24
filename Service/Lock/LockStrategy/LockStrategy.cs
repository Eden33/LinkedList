using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Service.Transaction;
using Model.Lock;
using Model.Data;

namespace Service.Lock.LockStrategy
{
    public class LockStrategy<T> : ILockStrategy
    {
        private TransactionManager tm;
        private Dictionary<Type, ILockStrategy> strategies = new Dictionary<Type, ILockStrategy>();

        public LockStrategy(TransactionManager tm)
        {
            this.tm = tm;
            strategies.Add(typeof(Customer), new LockCustomerStrategy(tm));
            strategies.Add(typeof(CollectionPoint), new LockCollectionPointStrategy(tm));
        }

        public LockBatch GetItemsToLock(int id)
        {
            LockBatch batch = new LockBatch();
            ILockStrategy s = null;
            if(strategies.TryGetValue(typeof(T), out s))
            {
                object monitor = tm.GetCurrentItemsMonitorObject();
                lock(monitor)
                {
                    batch = s.GetItemsToLock(id);
                }
            }
            else
            {
                Console.WriteLine("No strategy to determine needed locks for type: {0}", typeof(T));
            }
            return batch;
        }
    }
}
