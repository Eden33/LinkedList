﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Model.Data;

namespace TreeEditor.Resource
{
    // [CallbackBehavior(UseSynchronizationContext = false)]
    // if we use IsOneWay option this is not needed
    public class ResourceManager : ResourceService.IResourceServiceCallback
    {
        private static ResourceManager instance;

        private Dictionary<int, UICollectionVat> vats = new Dictionary<int, UICollectionVat>();
        private Dictionary<int, UICollectionPoint> points = new Dictionary<int, UICollectionPoint>();
        private ResourceService.ResourceServiceClient client = null;
        private string loginName = null;
        private bool isConnected = false;

        private ResourceManager() 
        {
            InstanceContext context = new InstanceContext(this);
            client = new ResourceService.ResourceServiceClient(context);
            isConnected = true;
        }

        public static ResourceManager Instance 
        {
            get 
            { 
                if(instance == null)
                {
                    instance = new ResourceManager();
                }
                return instance;
            } 
        }

        #region properties

        public string LoginName 
        {
            set { loginName = value; }
        }

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
        public UICollectionVat getCollectionVat(int id)
        {
            UICollectionVat flyweight = null;
            if (!vats.TryGetValue(id, out flyweight))
            {
                try
                {
                    CollectionVat vat = client.GetCollectionVat(id);
                    flyweight = new UICollectionVat(vat);
                    flyweight.PropertyChanged += UIObject_PropertyChanged;
                    vats.Add(id, flyweight);
                }
                catch(CommunicationException e)
                {
                    IsConnected = false;
                    Console.WriteLine(e.Message);

                }
            }
            return flyweight;
        }

        public UICollectionPoint getCollectionPoint(int id)
        {
            UICollectionPoint flyweight = null;
            if (!points.TryGetValue(id, out flyweight))
            {
                try
                {
                    CollectionPoint cp = client.GetCollectionPoint(id);
                    flyweight = new UICollectionPoint(cp);
                    flyweight.PropertyChanged += UIObject_PropertyChanged;
                    points.Add(id, flyweight);
                }
                catch(CommunicationException e)
                {
                    IsConnected = false;
                    Console.WriteLine(e.Message);
                }
            }
            return flyweight;
        }

        private void UIObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Deleted"))
            {
                if(sender is UICollectionVat)
                {
                    UICollectionVat removed = sender as UICollectionVat;
                    if (vats.Remove(removed.Id))
                    {
                        Console.WriteLine("Vat with id: " + removed.Id + " removed from ResourceManager.");
                    }
                }
                else if(sender is UICollectionPoint)
                {
                    UICollectionPoint removed = sender as UICollectionPoint;
                    if(points.Remove(removed.Id))
                    {
                        Console.WriteLine("CollectionPoint with id: " + removed.Id + " removed from ResourceManager.");
                    }
                }
            }
        }

        public void LockedNotification(string owner, LockBatch batch)
        {
            foreach(LockItem l in batch.ItemsToLock) 
            {
                Console.WriteLine("Lock item " + l.ItemTypeInfo);
                foreach(int id in l.IDsToLock)
                {
                    Console.WriteLine(id);
                }
            }
        }
    }
}
