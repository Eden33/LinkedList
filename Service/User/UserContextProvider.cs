using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Model.Message.Push;

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

        #region pseudo user authentication

        /// <summary>
        /// Pseudo authentication.
        /// Tries to add the given user.
        /// </summary>
        /// <param name="sessionId">Session id of user</param>
        /// <param name="loginName">The user name</param>
        /// <returns></returns>
        public bool AddUser(String sessionId, String loginName)
        {
            lock(currentUsers)
            {
               foreach(KeyValuePair<string, UserContext> entry in currentUsers)
               {
                   if(entry.Value.LoginName.Equals(loginName))
                   {
                       return false; //users must be named different
                   }
               }

               if (!currentUsers.ContainsKey(sessionId))
               {
                    UserContext c = new UserContext(loginName, OperationContext.Current);
                    currentUsers.Add(sessionId, c);
                    return true;
               }
            }
            return false;
        }

        #endregion

        #region access user data

        /// <summary>
        /// Returns the UserContext of this sessionId or null if no valid session exists.
        /// </summary>
        /// <param name="sessionId">The session id</param>
        /// <returns>The UserContext of this session</returns>
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

        /// <summary>
        /// Returns a list of the currently logged in users
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrentUsers()
        {
            List<string> users = new List<string>();
            lock(currentUsers)
            {
                foreach(KeyValuePair<string, UserContext> entry in currentUsers)
                {
                    users.Add(entry.Value.LoginName);
                }
            }
            return users;
        }

        #endregion

        #region messaging

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

        public void NotifyUser(String sessionId, NotificationMessage msg)
        {
            lock(currentUsers)
            {
                UserContext userContext = null;
                if(currentUsers.TryGetValue(sessionId, out userContext))
                {
                    userContext.AddMessage(msg);
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

        #endregion
    }

}
