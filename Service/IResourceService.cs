using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Model.Data;
using Model.Lock;

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
        bool TryLock(int id, ItemType itemType);
        
        [OperationContract]
        Item GetSingleItem(int id, ItemType itemType);

        [OperationContract]
        List<Item> GetAllItems(ItemType itemType);

        [OperationContract]
        bool UpdateItem(Item theItem, ItemType itemType);

        [OperationContract]
        bool DeleteItem(int id, ItemType itemType);

    }

    public interface IResourceServiceNotifications
    {
        [OperationContract(IsOneWay = true)]
        void LockedNotification(String owner, LockBatch batch);

        //TODO: we need a callback for updates

        //TODO: probably we need another callback for delete?
    }
}
