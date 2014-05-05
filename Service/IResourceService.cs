using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Model.Data;

namespace Service
{
    [ServiceContract(CallbackContract = typeof(IResourceServiceNotifications))]
    public interface IResourceService
    {
        [OperationContract]
        bool TryLock(int id, ItemType type);
        
        [OperationContract]
        CollectionPoint GetCollectionPoint(int id);
        
        [OperationContract]
        CollectionVat GetCollectionVat(int id);

        [OperationContract(IsOneWay = true)]
        void RegisterLockNotifications();
    }

    public interface IResourceServiceNotifications
    {
        [OperationContract(IsOneWay = true)]
        void LockedNotification(String owner, LockBatch batch);
    }
}
