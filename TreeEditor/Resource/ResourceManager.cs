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
                loginSuccess = client.Login(loginName).Success;
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

        #region service callbacks

        public void LockedNotification(LockMessage lockMsg)
        {
            throw new NotImplementedException();
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
