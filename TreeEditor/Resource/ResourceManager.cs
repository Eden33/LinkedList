using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Model.Lock;
using TreeEditor.UI.Proxy;

namespace TreeEditor.Resource
{
    // [CallbackBehavior(UseSynchronizationContext = false)]
    // if we use IsOneWay option this is not needed
    public class ResourceManager : ResourceService.IResourceServiceCallback
    {

        #region private members

        // TODO: fix me, decide wisely

        //private Dictionary<int, UICollectionVat> vats = new Dictionary<int, UICollectionVat>();
        //private Dictionary<int, UICollectionPoint> points = new Dictionary<int, UICollectionPoint>();

        private ResourceService.ResourceServiceClient client = null;

        #endregion

        private ResourceManager() 
        {
            InstanceContext context = new InstanceContext(this);
            client = new ResourceService.ResourceServiceClient(context);
            isConnected = true;

            #region Test Client - Server Communication works as expected

            //get single
            Client aClient = (Client) client.GetSingleItem(1, ItemType.Client);
            Console.WriteLine("Client with name {0} retrieved.", aClient.FirstName);

            //get list
            IList<Item> list = client.GetAllItems(ItemType.Client);
            Client c = (Client)list.ElementAt(0);
            Console.WriteLine("Another client with name {0} retrieved.", c.FirstName);

            list = client.GetAllItems(ItemType.CollectionPoint);
            CollectionPoint cp = (CollectionPoint) list.ElementAt(0);
            Console.WriteLine("A CP with description: {0} retrieved.", cp.Description);

            c.FirstName = "New name";
            cp.Description = "New description";

            //update
            Console.WriteLine("Update a client value: {0}", client.UpdateItem(c, ItemType.Client));
            Console.WriteLine("Update a CP value: {0}", client.UpdateItem(cp, ItemType.CollectionPoint));
           
            //delete
            Console.WriteLine("Delete a CP: {0}", client.DeleteItem(3, ItemType.CollectionPoint));

            #endregion
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

        #region public methods to access remote service methods or cached resources

        public bool Login()
        {
            bool loginSuccess = false;
            try
            {
                loginSuccess = client.Login(loginName);
            }
            catch(CommunicationException e)
            {
                IsConnected = false;
                Console.WriteLine(e.Message);
            }
            return loginSuccess;
        }

        public T getSingleItem<T>(int id, T item) where T : UIItem
        {
            //TODO: implement me

            return default(T);
        }

        public bool RequestLock<T>(int id, T item) where T : UIItem 
        {
            //TODO: implement me

            return true;        
        }

        #endregion

        private void UIObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Deleted"))
            {
                // TODO: fix me, decide wisely

                //if(sender is UICollectionPoint)
                //{
                //    UICollectionPoint removed = sender as UICollectionPoint;
                //    if (vats.Remove(removed.Id))
                //    {
                //        Console.WriteLine("Vat with id: " + removed.Id + " removed from ResourceManager.");
                //    }
                //}
                //else if(sender is UICollectionPoint)
                //{
                //    UICollectionPoint removed = sender as UICollectionPoint;
                //    if(points.Remove(removed.Id))
                //    {
                //        Console.WriteLine("CollectionPoint with id: " + removed.Id + " removed from ResourceManager.");
                //    }
                //}
            }
        }

        #region service callbacks

        public void LockedNotification(string owner, LockBatch batch)
        {
            Console.WriteLine("LockedNotification fix me");

            //foreach(LockItem l in batch.ItemsToLock) 
            //{
            //    UILockInfo lockInfo = new UILockInfo(owner, true);
            //    if (l.ItemTypeInfo == ItemType.CollectionPoint)
            //    {
            //        foreach (int id in l.IDsToLock)
            //        {
            //            UICollectionPoint p = null;
            //            if(points.TryGetValue(id, out p))
            //            {
            //                p.LockInfo = lockInfo;
            //            }
            //        }
            //    }
            //    if(l.ItemTypeInfo == ItemType.CollectionVat)
            //    {
            //        foreach (int id in l.IDsToLock)
            //        {
            //            UICollectionVat v = null;
            //            if (vats.TryGetValue(id, out v))
            //            {
            //                v.LockInfo = lockInfo;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("Received lock notification for unknown type: {0}", l.ItemTypeInfo);
            //    }
            //}
        }

        #endregion
    }    
}
