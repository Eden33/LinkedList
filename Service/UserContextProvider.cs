using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.User
{
    /// <summary>
    /// This class manages all user sessions and provides pseudo authentication.
    /// </summary>
    class UserContextProvider
    {
        private static UserContextProvider instance;
        private Dictionary<string, UserContext> currentUsers = new Dictionary<string, UserContext>();
        private Thread notificationThread = null;
        private static readonly int pushInterval = 5000;

        private UserContextProvider()
        {
            notificationThread = new Thread(PushNotifications);
            notificationThread.Name = "Push Service";
            notificationThread.Start();
        }

        public static UserContextProvider Instance 
        {
            get 
            {  
                if(instance == null)
                {
                    instance = new UserContextProvider();
                }
                return instance;
            }
        }

        /// <summary>
        /// Pseudo authentication.
        /// Tries to add the given user.
        /// </summary>
        /// <param name="sessionId">Session id of user</param>
        /// <param name="loginName">The user name</param>
        /// <returns></returns>
        public bool AddUser(String sessionId, String loginName)
        {
            UserContext context = null;
            lock(currentUsers)
            {
                if (!currentUsers.TryGetValue(sessionId, out context))
                {
                    UserContext c = new UserContext(loginName, OperationContext.Current);
                    currentUsers.Add(sessionId, c);
                    return true;
                }
            }
            return false;
        }

        public UserContext getUserContext(String sessionId)
        {
            UserContext context = null;
            lock(currentUsers)
            {
                if(!currentUsers.TryGetValue(sessionId, out context))
                {
                    Console.WriteLine("UserContext for sessionId: {0} cannot be retrieved.", sessionId);
                }
            }
            return context;
        }

        public void NotifyAll(NotificationMessage msg)
        {
            lock(currentUsers)
            {
                foreach(KeyValuePair<string, UserContext> e in currentUsers)
                {
                    e.Value.AddMessage(msg);
                }
            }
        }

        /// <summary>
        /// Push NotificationMessages to clients on an interval basis
        /// </summary>
        private void PushNotifications()
        {
            while(true)
            {
                lock (currentUsers)
                {
                    foreach (KeyValuePair<string, UserContext> e in currentUsers)
                    {
                        e.Value.PushNextMessage();
                    }
                }
                Thread.Sleep(pushInterval);
            }
        }
    }

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

        public string LoginName {
            get { return loginName; }
        }

        public void AddMessage(NotificationMessage msg)
        {
            if(msg.GetType() == typeof(LockMessage))
            {
                Console.WriteLine("User {0} queue new LockMessage.", loginName);
            }
            notificationQueue.Enqueue(msg);
        }

        public void PushNextMessage()
        {
            if(notificationQueue.Count > 0)
            {
                NotificationMessage nextMsg = notificationQueue.Peek();
                if(nextMsg.GetType() == typeof(LockMessage))
                {
                    LockMessage msg = (LockMessage)notificationQueue.Dequeue();
                    IResourceServiceNotifications callback = operationContext.GetCallbackChannel<IResourceServiceNotifications>();

                    try
                    {
                        CommunicationState state = ((ICommunicationObject)callback).State;
                        if(state == CommunicationState.Opened)
                        {
                            Console.WriteLine("Send lock notification message to user: {0}", loginName);
                            callback.LockedNotification(msg.LoginName, msg.LockBatch);
                        }
                        else
                        {
                            Console.WriteLine("TCP connection for user {0} is in state: {1}", loginName, state);
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Exception caught in PushNextMessage to user {0}\nException: {1}", loginName, e.Message);
                    }
                }
            }
        }
    }

    abstract class NotificationMessage
    {
        protected MessageType msgType;

        public MessageType MsgType 
        {
            get { return msgType; }
        }

        public enum MessageType
        {
            LockMessage
        }
    }

    class LockMessage : NotificationMessage
    {
        #region members

        private LockBatch lockBatch;
        private string loginName;

        #endregion

        public LockMessage(String loginName, LockBatch batch)
        {
            msgType = NotificationMessage.MessageType.LockMessage;
            lockBatch = batch;
            this.loginName = loginName;
        }

        #region properties

        public LockBatch LockBatch 
        {
            get { return lockBatch; } 
        }

        public string LoginName
        {
            get { return loginName; }
        }

        #endregion
    }

}
