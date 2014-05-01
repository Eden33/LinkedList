using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Model.Data;

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

        public bool TryLock(int id, Type type)
        {
            return true;
        }

        public Model.Data.CollectionPoint GetCollectionPoint(int id)
        {
            return ResourceManager.GetCollectionPoint(id);
        }

        public Model.Data.CollectionVat GetCollectionVat(int id)
        {
            return ResourceManager.GetCollectionVat(id);
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
                                callback.LockedNotification("new owner: " + ctr);
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
