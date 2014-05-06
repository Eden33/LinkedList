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
        /// <summary>
        /// Pseudo Authentication. We don't use WCF authentication with intend.
        /// You can not login with the same user twice.
        /// </summary>
        /// <param name="loginName">The user name of the client sending the message</param>
        /// <returns>True on login success, otherwise false</returns>
        [OperationContract]
        bool Login(String loginName);

        [OperationContract]
        bool TryLock(int id, ItemType type);
        
        [OperationContract]
        CollectionPoint GetCollectionPoint(int id);
        
        [OperationContract]
        CollectionVat GetCollectionVat(int id);
    }

    public interface IResourceServiceNotifications
    {
        [OperationContract(IsOneWay = true)]
        void LockedNotification(String owner, LockBatch batch);
    }
}
