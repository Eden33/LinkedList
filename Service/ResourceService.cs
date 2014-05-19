using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;


using Model.Data;
using Model.Lock;
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

        #region authentication

        public bool Login(string loginName)
        {
            bool added = false;
            lock (userContextProvider)
            {
                added = userContextProvider.AddUser(OperationContext.Current.SessionId, loginName);
            }
            if (added)
            {
                Console.WriteLine("New user {0} logged in successfully. Session-Id: {1} ", loginName, OperationContext.Current.SessionId);
            }
            return added;
        }

        #endregion

        #region locking

        public bool TryLock(int id, ItemType itemType)
        {
            #region retrive user information

            UserContext userContext = null;
            lock(userContextProvider)
            {
                userContext = userContextProvider.getUserContext(OperationContext.Current.SessionId);
            }
            if(userContext == null)
            {
                return false;
            }

            #endregion

            return true;

            // this WCF method has not changed - do we really have to fix it?

            //TODO: fix me

            ////try lock
            //LockBatch batch = null;
            //bool lockSuccess = tm.TryLock(id, type, userContext.LoginName, out batch);

            ////push lock information to all clients on lock success
            //if(lockSuccess)
            //{
            //    LockMessage lockMsg = new LockMessage(userContext.LoginName, batch);  
            //    lock(userContextProvider)
            //    {
            //        userContextProvider.NotifyAll(lockMsg);
            //    }
            //}
            //return lockSuccess;
        }

        #endregion

        #region data retrieval and manipulation

        public Item GetSingleItem(int id, ItemType itemType)
        {
            // this method replaces the old WCF methods (GetCollectionPoint, GetCollectionVat)

            // TODO: fix me
            // return tm.GetCollectionPoint(id);

            Client c = new Client(10);
            c.FirstName = "Foo";
            c.LastName = "Bar";
            return c;
        }

        public List<Item> GetAllItems(ItemType itemType)
        {
            //TODO: implement me

            Console.WriteLine("Incoming get all items for type: " + itemType);
            List<Item> list = new List<Item>();
            if(ItemType.Client.Equals(itemType))
            {
                Client c1 = new Client(1);
                c1.FirstName = "Fred";
                c1.LastName = "Feuerstein";
                list.Add((Item) c1);

                Client c2 = new Client(2);
                c2.FirstName = "Bam";
                c2.LastName = "Bam";
                list.Add((Item)c2);
            }
            else if(ItemType.CollectionPoint.Equals(itemType))
            {
                CollectionPoint cp = new CollectionPoint(1);
                cp.Description = "Description about what is being collected.";
                list.Add((Item)cp);
            }
            return list;
        }

        public bool UpdateItem(Item theItem, ItemType itemType)
        {
            // TODO: implement me

            if (ItemType.CollectionPoint.Equals(itemType))
            {
                CollectionPoint p = (CollectionPoint)theItem;
                Console.WriteLine("Update CP Description: {0}", p.Description);
            }
            else if(ItemType.Client.Equals(itemType))
            {
                Client c = (Client)theItem;
                Console.WriteLine("Update Client FirstName: {0}", c.FirstName);
            }

            return true;
        }

        public bool DeleteItem(int id, ItemType itemType)
        {
            // TODO: implement me

            if(ItemType.Client.Equals(itemType))
            {
                Console.WriteLine("Delete Client with id: {0}", id);
            }
            else if(ItemType.CollectionPoint.Equals(itemType))
            {
                Console.WriteLine("Delete CP with id: {0}", id);
            }

            return true;
        }

        #endregion
    }
}
