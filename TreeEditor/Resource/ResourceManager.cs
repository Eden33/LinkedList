using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Model.Lock;
using Model.Message.Request;
using Model.Message.Response;
using Model.Message.Push;
using TreeEditor.UI.Proxy;

namespace TreeEditor.Resource
{
    // [CallbackBehavior(UseSynchronizationContext = false)]
    // if we use IsOneWay option this is not needed
    public class ResourceManager : ResourceService.IResourceServiceCallback
    {

        #region private members

        private ResourceService.ResourceServiceClient client = null;
        private ResourceCache cache = new ResourceCache();

        #endregion

        private ResourceManager()
        {
            InstanceContext context = new InstanceContext(this);
            client = new ResourceService.ResourceServiceClient(context);
            isConnected = true;
        }

        #region properties

        private static ResourceManager instance;

        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResourceManager();
                }
                return instance;
            }
        }

        private string loginName = null;

        public string LoginName 
        {
            set { loginName = value; }
            get { return this.loginName; }
        }

        private bool isConnected = false;

        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
            private set
            {
                isConnected = false;
            }
        }
        #endregion

        #region public methods to access remote service methods and cached resources

        public bool Login()
        {
            bool loginSuccess = false;
            try
            {
                loginSuccess = client.Login(loginName).Success;
            }
            catch(CommunicationException e)
            {
                IsConnected = false;
                Console.WriteLine(e.Message);
            }
            return loginSuccess;
        }

        public T GetSingleItem<T>(int id) where T : UIItem
        {
            ItemType itemType = ResourceMap.UITypeToItemType<T>();
            T item = cache.GetUIItem<T>(id);
            if(item == null)
            {
                try
                {
                    SingleItemResponse r = client.GetSingleItem(id, itemType);
                    if (r.Success)
                    {
                        Type modelType = ResourceMap.ItemTypeToModelType(itemType);
                        CacheItem(modelType, r.Item);
                        item = cache.GetUIItem<T>(id);
                    }
                    else
                    {
                        Console.WriteLine("GetSingleItem for type: {0} wasn't successfull.", typeof(T));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception cuaght in GetSingleItem: {0}", e.Message);
                }
            
            }
            return item;
        }

        public List<T> GetAllItems<T>() where T : UIItem
        {
            ItemType itemType = ResourceMap.UITypeToItemType<T>();
            try
            {
                AllItemsResponse r = client.GetAllItems(itemType);
                if (r.Success)
                {
                    Type modelType = ResourceMap.ItemTypeToModelType(itemType);
                    foreach(Item item in r.Items) 
                    {
                        CacheItem(modelType, item);
                    }
                }
                else
                {
                    Console.WriteLine("GetAllItems for Type: {0} wasn't successfull.", typeof(T));
                }
            } 
            catch(Exception e)
            {
                Console.WriteLine("Exception caught in GetAllItems: {0}", e.Message);
            }
            return cache.GetAllUIItems<T>();
        }

        public bool RequestLock<T>(int id) where T : UIItem 
        {
            ItemType itemType = ResourceMap.UITypeToItemType<T>();
            try
            {
                LockResponse r = client.TryLock(id, itemType);
                if(r.Success)
                {
                    cache.GetUIItem<T>(id).LockInfo = new UILockInfo(this.loginName, true);
                    return true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Excpetion caught in RequestLock: {0}", e.Message);
            }
            return false;
        }

        public bool Unlock<T>(int id) where T : UIItem
        {
            ItemType itemType = ResourceMap.UITypeToItemType<T>();
            try
            {
                UnlockResponse r = client.Unlock(id, itemType);
                if(r.Success)
                {
                    cache.GetUIItem<T>(id).LockInfo = null;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception caught in Unlock: {0}", e.Message);
            }
            return false;
        }

        #endregion

        #region private methods used for caching
        
        private void CacheItem(Type modelType, Item item)
        {
            if (modelType == typeof(CollectionPoint))
            {
                CollectionPoint cp = (CollectionPoint)item;
                cache.CacheItem(cp);

                UICollectionPoint cpProxy = new UICollectionPoint(cp);
                cache.CacheUIItem(cpProxy);

                foreach (Customer c in cp.Customers)
                {
                    cache.CacheItem(c);
                    UICustomer clientProxy = new UICustomer(c);
                    cache.CacheUIItem(clientProxy);
                }
            }
            else if (modelType == typeof(Customer))
            {
                Customer c = (Customer)item;
                cache.CacheItem(c);

                UICustomer cProxy = new UICustomer(c);
                cache.CacheUIItem(cProxy);
            }
        }

        #endregion

        #region service callbacks

        public void LockedNotification(LockMessage lockMsg)
        {
            String loginName = lockMsg.LoginName;
            bool isLocked = lockMsg.IsLocked;
            UILockInfo lockInfo = new UILockInfo(loginName, isLocked);
            
            //set it to null results in better overview in the GUI
            if(lockInfo.Locked == false)
            {
                lockInfo = null; 
            }

            foreach(LockItem l in lockMsg.LockBatch.ItemsToLock)
            {
                Type type = ResourceMap.ItemTypeToUIType(l.ItemType);
                if(type == typeof(UICollectionPoint))
                {
                    foreach(int id in l.IDsToLock)
                    {
                        cache.GetUIItem<UICollectionPoint>(id).LockInfo = lockInfo;
                    }
                }
                else if(type == typeof(UICustomer))
                {
                    foreach(int id in l.IDsToLock)
                    {
                        try
                        {
                            cache.GetUIItem<UICustomer>(id).LockInfo = lockInfo;
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("UICustomer: {0} not available.", id);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("This item cann't be marked as locked: {0}", l.ItemType);
                }
            }
        }

        public void UpdateNotification(UpdateMessage updateMsg)
        {
            throw new NotImplementedException();
        }

        public void DeleteNotification(DeleteMessage deleteMsg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }    
}
