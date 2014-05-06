using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Model.Data;
using Service.Data;
using Service.User;

namespace Service
{

    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ResourceService : IResourceService
    {
        private static readonly TransactionManager tm = RamTransactionManager.Instance;
        private static readonly UserContextProvider userContextProvider = UserContextProvider.Instance;
       
        public ResourceService() 
        {
            Console.WriteLine("ResourceService constructor...");
        }

        public bool TryLock(int id, ItemType type)
        {
            LockBatch batch = null;
            bool lockSuccess = tm.TryLock(id, type, "foo", out batch);
            if(lockSuccess)
            {
                LockMessage lockMsg = new LockMessage("foo", batch);  
                lock(userContextProvider)
                {
                    userContextProvider.NotifyAll(lockMsg);
                }
            }
            return lockSuccess;
        }

        public Model.Data.CollectionPoint GetCollectionPoint(int id)
        {
            //Console.WriteLine("GetCollectionPoint: " + OperationContext.Current.SessionId);
            return tm.GetCollectionPoint(id);
        }

        public Model.Data.CollectionVat GetCollectionVat(int id)
        {
            //Console.WriteLine("GetCollectionVat: " + OperationContext.Current.SessionId);
            return tm.GetCollectionVat(id);
        }

        public bool Login(string loginName)
        {
            return userContextProvider.AddUser(loginName);
        }
    }
}
