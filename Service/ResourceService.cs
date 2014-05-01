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
    public class ResourceService : IResourceService, IResourceServiceNotifications
    {
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

        private static int ctr = 0;
        public void RegisterLockNotifications()
        {
            while(true)
            {
                Thread.Sleep(5000);
                ctr++;
                OperationContext.Current.GetCallbackChannel<IResourceServiceNotifications>().LockedNotification("foo "+ctr);
            }
        }

        public void LockedNotification(string owner)
        {
            Console.WriteLine("Locked Notification: " + owner);
        }
    }
}
