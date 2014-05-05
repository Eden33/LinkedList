using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Model.Data;
using Service.Data;

namespace Service
{
    // [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    // if we use IsOneWay option this is not needed
    public class ResourceService : IResourceService
    {
        private static Thread testThread;
        private static readonly List<IResourceServiceNotifications> subscribers = new List<IResourceServiceNotifications>();
        
        public ResourceService()
        {
            testThread = new Thread(TestThreadMethod);
            testThread.IsBackground = true;
            testThread.Start();
        }

        public bool TryLock(int id, ItemType type)
        {
            throw new NotImplementedException();
        }

        public Model.Data.CollectionPoint GetCollectionPoint(int id)
        {
            return TransactionManager.GetCollectionPoint(id);
        }

        public Model.Data.CollectionVat GetCollectionVat(int id)
        {
            return TransactionManager.GetCollectionVat(id);
        }

        private void TestThreadMethod()
        {
            while(true)
            {
                lock (subscribers)
                {
                    ctr++;
                    try
                    {
                        subscribers.ForEach(delegate(IResourceServiceNotifications callback)
                        {
                            if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                            {
                                LockBatch batch = new LockBatch();
                                LockItem i1 = new LockItem();
                                i1.ItemTypeInfo = ItemType.CollectionPoint;
                                i1.IDsToLock = new List<int> { 1, 2, 3 };
                                LockItem i2 = new LockItem();
                                i2.ItemTypeInfo = ItemType.CollectionVat;
                                i2.IDsToLock = new List<int> { 5, 6 };
                                batch.ItemsToLock = new List<LockItem> { i1, i2 };

                                callback.LockedNotification("test", batch);
                            }
                            else
                            {
                                subscribers.Remove(callback);
                            }
                        });
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Exception caught in TestThreadMethod: " + e.Message);
                    }
                }
                Thread.Sleep(5000);
            }
        }

        private static int ctr = 0;
        public void RegisterLockNotifications()
        {
            IResourceServiceNotifications callback = OperationContext.Current.GetCallbackChannel<IResourceServiceNotifications>();
            lock(subscribers)
            {
                try
                {
                    if (!subscribers.Contains(callback))
                    {
                        subscribers.Add(callback);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Exception caught in RegisterLockNotifications(): " + e.Message);
                }
            }
        }
    }
}
