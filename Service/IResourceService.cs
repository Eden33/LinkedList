using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Model.Data;
using Model.Message.Request;
using Model.Message.Response;
using Model.Message.Push;

namespace Service
{
    [ServiceContract(CallbackContract = typeof(IResourceServiceNotifications))]
    public interface IResourceService
    {
        /// <summary>
        /// Pseudo Authentication. We don't use WCF authentication with intend.
        /// The passed loginName is mapped to the WCF session id.
        /// </summary>
        /// <param name="loginName">The user name of the client sending the message</param>
        /// <returns>True on login success, otherwise false</returns>
        [OperationContract]
        LoginResponse Login(String loginName);

        [OperationContract]
        LockResponse TryLock(int id, ItemType itemType);
        
        [OperationContract]
        SingleItemResponse GetSingleItem(int id, ItemType itemType);

        [OperationContract]
        AllItemsResponse GetAllItems(ItemType itemType);

        [OperationContract]
        UpdateResponse UpdateItem(UpdateRequest updateReq);

        [OperationContract]
        DeleteResponse DeleteItem(DeleteRequest deleteReq);

    }

    public interface IResourceServiceNotifications
    {
        [OperationContract(IsOneWay = true)]
        void LockedNotification(LockMessage lockMsg);

        [OperationContract(IsOneWay = true)]
        void UpdateNotification(UpdateMessage updateMsg);

        [OperationContract(IsOneWay = true)]
        void DeleteNotification(DeleteMessage deleteMsg);
    }
}
