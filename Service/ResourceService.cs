using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;


using Model.Data;
using Model.Lock;
using Model.Message.Request;
using Model.Message.Response;
using Model.Message.Push;
using Service.Transaction;
using Service.User;

namespace Service
{

    public class ResourceService : IResourceService
    {
        private static readonly TransactionManager tm = RamTM.Instance;
        private static readonly UserContextProvider userContextProvider = UserContextProvider.Instance;
       
        public ResourceService() 
        {
            Console.WriteLine("ResourceService constructor...");
        }

        #region authentication

        public LoginResponse Login(string loginName)
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
            return new LoginResponse(added, added ? "" : "Login failed");
        }

        #endregion

        #region locking

        public LockResponse TryLock(int id, ItemType itemType)
        {
            #region retrive user information

            UserContext userContext = null;
            lock(userContextProvider)
            {
                userContext = userContextProvider.getUserContext(OperationContext.Current.SessionId);
            }
            if(userContext == null)
            {
                return new LockResponse(false, "No valid session.");
            }

            #endregion

            return new LockResponse(true);

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

        public SingleItemResponse GetSingleItem(int id, ItemType itemType)
        {
            // this method replaces the old WCF methods (GetCollectionPoint, GetCollectionVat)

            // TODO: fix me
            // return tm.GetCollectionPoint(id);

            SingleItemResponse r;
            if (id > 1 && id < 50)
            {
                r = new SingleItemResponse(true);
                Client c = new Client(id);
                c.FirstName = "First Name " + id;
                c.LastName = "Last Name " + id;
                c.Address = "Address " + id;
                r.Item = c;
            }
            else
            {
                r = new SingleItemResponse(false, "ID not found");
            }
            return r;
        }

        public AllItemsResponse GetAllItems(ItemType itemType)
        {
            //TODO: implement me

            Console.WriteLine("Incoming get all items for type: " + itemType);
            AllItemsResponse response = new AllItemsResponse(true, "");
            Random random = new Random();

            List<Item> list = new List<Item>();
            if(ItemType.Client.Equals(itemType))
            {
                for(int i = 1; i < 50; i++)
                {
                    Client c = new Client(i);
                    c.FirstName = "First Name " + i;
                    c.LastName = "Last Name " + i;
                    c.Address = "Address " + i;
                    list.Add((Item)c);
                }
            }
            else if(ItemType.CollectionPoint.Equals(itemType))
            {
                for(int i = 1; i < 50; i++)
                {
                    CollectionPoint cp = new CollectionPoint(i);
                    cp.Description = "This is CP " + i;
                    Client c = new Client(random.Next(1, 51));
                    c.FirstName = "First Name " + c.Id;
                    c.LastName = "Last Name " + c.Id;
                    c.Address = "Address " + c.Id;
                    cp.Clients.Add(c);
                    list.Add((Item)cp);
                }
            }
            response.Items = list;
            return response;
        }

        public UpdateResponse UpdateItem(UpdateRequest req)
        {
            // TODO: implement me

            return null;
        }

        public DeleteResponse DeleteItem(DeleteRequest req)
        {
            // TODO: implement me

            return null;
        }

        #endregion
    }
}
