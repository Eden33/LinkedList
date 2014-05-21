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
            #region retrive user information

            UserContext userContext = null;
            if(!ValidSession(out userContext))
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
            UserContext userContext = null;
            SingleItemResponse r = new SingleItemResponse();
            if (!ValidSession(out userContext))
            {
                r.Success = false;
                r.ErrorDesc = "No valid session.";
            }
            else
            {
                Type returnType = ResourceMap.getModelType(itemType);

                // this method replaces the old WCF methods (GetCollectionPoint, GetCollectionVat)

                // TODO: fix me
                // return tm.GetCollectionPoint(id);

                if (id > 1 && id < 50)
                {
                    r = new SingleItemResponse(true);
                    Customer c = new Customer(id);
                    c.FirstName = "First Name " + id;
                    c.LastName = "Last Name " + id;
                    c.Address = "Address " + id;
                    r.Item = c;
                }
                else
                {
                    r = new SingleItemResponse(false, "ID not found");
                }
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
                Random random = new Random();

                List<Item> list = new List<Item>();
                if (ItemType.Customer.Equals(itemType))
                {
                    for (int i = 1; i < 50; i++)
                    {
                        Customer c = new Customer(i);
                        c.FirstName = "First Name " + i;
                        c.LastName = "Last Name " + i;
                        c.Address = "Address " + i;
                        list.Add((Item)c);
                    }
                }
                else if (ItemType.CollectionPoint.Equals(itemType))
                {
                    for (int i = 1; i < 50; i++)
                    {
                        CollectionPoint cp = new CollectionPoint(i);
                        cp.Description = "This is CP " + i;
                        Customer c = new Customer(random.Next(1, 51));
                        c.FirstName = "First Name " + c.Id;
                        c.LastName = "Last Name " + c.Id;
                        c.Address = "Address " + c.Id;
                        cp.Customers.Add(c);
                        list.Add((Item)cp);
                    }
                }
                r.Items = list;
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

        #endregion
    }
}
