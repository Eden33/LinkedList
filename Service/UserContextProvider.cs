using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// This class manages all user sessions and provides pseudo authentication.
    /// </summary>
    class UserContextProvider
    {
        private static UserContextProvider instance;
        private Dictionary<string, UserContext> currentUsers = new Dictionary<string, UserContext>();

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
        /// <param name="loginName">The user name</param>
        /// <returns>True if user has been added successfully to this class, otherwise false</returns>
        public bool AddUser(String loginName)
        {
            UserContext context = null;
            lock(currentUsers)
            {
                if (!currentUsers.TryGetValue(loginName, out context))
                {
                    UserContext c = new UserContext(loginName, OperationContext.Current);
                    currentUsers.Add(loginName, c);
                    return true;
                }
            }
            return false;
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
            notificationQueue.Enqueue(msg);
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
