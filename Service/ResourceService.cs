using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;


using Model.Data;
using Service.Message;
using Service.Transaction;
using Service.User;

namespace Service
{

    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ResourceService : IResourceService
    {
        private static readonly TransactionManager tm = RamTM.Instance;
        private static readonly UserContextProvider userContextProvider = UserContextProvider.Instance;
       
        public ResourceService() 
        {
            Console.WriteLine("ResourceService constructor...");
        }

        public bool TryLock(int id, ItemType type)
        {
            //retrieve user information
            UserContext userContext = null;
            lock(userContextProvider)
            {
                userContext = userContextProvider.getUserContext(OperationContext.Current.SessionId);
            }
            if(userContext == null)
            {
                return false;
            }

            //try lock
            LockBatch batch = null;
            bool lockSuccess = tm.TryLock(id, type, userContext.LoginName, out batch);

            //push lock information to all clients on lock success
            if(lockSuccess)
            {
                LockMessage lockMsg = new LockMessage(userContext.LoginName, batch);  
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
            bool added = false;
            lock(userContextProvider)
            {
                added = userContextProvider.AddUser(OperationContext.Current.SessionId, loginName);
            }
            if(added)
            {
                Console.WriteLine("New managed user {0} with session {1}", loginName, OperationContext.Current.SessionId);
            }
            return added;
        }
    }
}
