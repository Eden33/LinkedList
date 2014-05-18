using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using Service.Message;

namespace Service.User
{
    class UserContext
    {
        private string loginName;
        private OperationContext operationContext;
        private Queue<NotificationMessage> notificationQueue = new Queue<NotificationMessage>(20);

        public UserContext(string loginName, OperationContext context)
        {
            this.loginName = loginName;
            this.operationContext = context;

        }

        public string LoginName
        {
            get { return loginName; }
        }

        public void AddMessage(NotificationMessage msg)
        {
            if (msg.GetType() == typeof(LockMessage))
            {
                Console.WriteLine("User {0} queue new LockMessage.", loginName);
            }
            notificationQueue.Enqueue(msg);
        }

        public void PushNextMessage()
        {
            if (notificationQueue.Count > 0)
            {
                NotificationMessage nextMsg = notificationQueue.Peek();
                if (nextMsg.GetType() == typeof(LockMessage))
                {
                    LockMessage msg = (LockMessage)notificationQueue.Dequeue();
                    IResourceServiceNotifications callback = operationContext.GetCallbackChannel<IResourceServiceNotifications>();

                    try
                    {
                        CommunicationState state = ((ICommunicationObject)callback).State;
                        if (state == CommunicationState.Opened)
                        {
                            Console.WriteLine("Send lock notification message to user: {0}", loginName);
                            callback.LockedNotification(msg.LoginName, msg.LockBatch);
                        }
                        else
                        {
                            Console.WriteLine("TCP connection for user {0} is in state: {1}", loginName, state);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception caught in PushNextMessage to user {0}\nException: {1}", loginName, e.Message);
                    }
                }
            }
        }
    }
}
