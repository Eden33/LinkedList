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
using Service.Resource;

namespace Service
{

    public class ResourceService : IResourceService
    {
        private static readonly TransactionManager tm = TMImplementation.Instance;
        private static readonly UserContextProvider userContextProvider = UserContextProvider.Instance;
       
        public ResourceService() 
        {
            Console.WriteLine("ResourceService constructor...");
        }

        #region authentication

        public LoginResponse Login(string loginName)
        {
            LoginResponse r = new LoginResponse();
            userContextProvider.AddUser(OperationContext.Current.SessionId, loginName);
            UserContext context = null;
            if (ValidSession(out context))
            {
                r.Success = true;
                List<string> currentUser = userContextProvider.GetCurrentUsers();
                foreach(string userLogin in currentUser)
                {
                    LockBatch currentLocks = tm.GetCurrentLocks(userLogin);
                    LockMessage msg = new LockMessage(userLogin, true, currentLocks);
                    userContextProvider.NotifyUser(OperationContext.Current.SessionId, msg);
                }
            } 
            else
            {
                r.Success = false;
                r.ErrorDesc = "Login failed";
            }
            return r;
        }

        #endregion

        #region locking

        public LockResponse TryLock(int id, ItemType itemType)
        {
            Console.WriteLine("Lock request for item {0} with id: {1}", itemType, id);

            LockResponse r = new LockResponse();

            UserContext userContext = null;
            if(!ValidSession(out userContext))
            {
                r.Success = false;
                r.ErrorDesc = "No valid session";
                return r;
            }
            else
            {
                LockBatch batch = null;
                Type type = ResourceMap.ItemTypeToModelType(itemType);
                if (type == typeof(CollectionPoint))
                {
                    r.Success = tm.TryLock<CollectionPoint>(id, userContext.LoginName, out batch);

                }
                else if (type == typeof(Customer))
                {
                    r.Success = tm.TryLock<Customer>(id, userContext.LoginName, out batch);
                }
                else
                {
                    r.Success = false;
                    r.ErrorDesc = "You can't lock this item.";
                }

                //notify all clients on lock success
                if (r.Success)
                {
                    LockMessage lockMsg = new LockMessage(userContext.LoginName, true, batch);
                    userContextProvider.NotifyAll(lockMsg);
                }
            }
            return r;
        }

        public UnlockResponse Unlock(int id, ItemType itemType)
        {
            UnlockResponse r = new UnlockResponse();

            UserContext userContext = null;
            if(!ValidSession(out userContext))
            {
                r.Success = false;
                r.ErrorDesc = "No valid session";
            }
            else
            {
                LockBatch batch = null;
                Type type = ResourceMap.ItemTypeToModelType(itemType);

                if(type == typeof(CollectionPoint))
                {
                    r.Success = tm.Unlock<CollectionPoint>(id, userContext.LoginName, out batch);
                }
                else if (type == typeof(Customer))
                {
                    r.Success = tm.Unlock<Customer>(id, userContext.LoginName, out batch);
                }
                else
                {
                    r.Success = false;
                    r.ErrorDesc = "You can't unlock this item.";
                }

                //notify all clients on unlock success
                if(r.Success)
                {
                    LockMessage lockMsg = new LockMessage(userContext.LoginName, false, batch);
                    userContextProvider.NotifyAll(lockMsg);
                }
            }
            return r;
        }

        #endregion

        #region data retrieval and manipulation

        public SingleItemResponse GetSingleItem(int id, ItemType itemType)
        {
            Console.WriteLine("Incoming get single item for type : " + itemType);

            UserContext userContext = null;
            SingleItemResponse r = new SingleItemResponse();
            if (!ValidSession(out userContext))
            {
                r.Success = false;
                r.ErrorDesc = "No valid session.";
            }
            else
            {
                Type returnType = ResourceMap.ItemTypeToModelType(itemType);
                Item item = GetSingleItemFromType(id, returnType);
            }
            return r;
        }

        public AllItemsResponse GetAllItems(ItemType itemType)
        {
            Console.WriteLine("Incoming get all items for type: " + itemType);

            UserContext userContext = null;
            AllItemsResponse r = new AllItemsResponse();
            if(!ValidSession(out userContext))
            {
                r.Success = false;
                r.ErrorDesc = "No valid session.";
            }
            else
            {
                r.Success = true;
                Type type = ResourceMap.ItemTypeToModelType(itemType);
                r.Items = GetAllItemsFromType(type);
            }
            return r;
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

        #region private helper methods
 
        /// <summary>
        /// Helper method to check if client done the request have a valid user session.
        /// </summary>
        /// <param name="userContext">Handle in an UserContext to get the data back on success</param>
        /// <returns>The UserContext of this user or null if no valid session.</returns>
        private bool ValidSession(out UserContext userContext)
        {
            userContext = userContextProvider.getUserContext(OperationContext.Current.SessionId);
            if(userContext == null)
            {
                return false;
            }
            return true;
        }

        private Item GetSingleItemFromType(int id, Type theType)
        {
            Item theItem = null;
            if(theType == typeof(CollectionPoint))
            {
                theItem = tm.GetSingleItem<CollectionPoint>(id);
            }
            else if(theType == typeof(Customer))
            {
                theItem = tm.GetSingleItem<Customer>(id);
            }
            return theItem;
        }
        
        private List<Item> GetAllItemsFromType(Type theType)
        {
            List<Item> items = null;
            if(theType == typeof(CollectionPoint))
            {
                items = tm.GetAllItems<CollectionPoint>().Cast<Item>().ToList();
            }
            else if(theType == typeof(Customer))
            {
                items = tm.GetAllItems<Customer>().Cast<Item>().ToList();
            }
            return items;
        }

        #endregion
    }
}
