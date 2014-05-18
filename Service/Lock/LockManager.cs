using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lock
{
    class LockManager
    {
        private Dictionary<int, List<LockData>> collectionPoints = new Dictionary<int, List<LockData>>();
        private Dictionary<int, List<LockData>> collectionVats = new Dictionary<int, List<LockData>>();

        /// <summary>
        /// Tries to lock the items passed in LockBatch.
        /// </summary>
        /// <param name="login">The login of the user is our transaction id</param>
        /// <param name="batch">The items to lock</param>
        /// <param name="mode">The lock mode</param>
        /// <returns>True on lock success, false on rollback</returns>
        public bool Lock(String login, LockBatch batch, LockMode mode)
        {
            foreach(LockItem item in batch.ItemsToLock)
            {
                if(item.ItemTypeInfo.Equals(ItemType.CollectionPoint))
                {
                    if(SetLocks(login, batch, mode, item, collectionPoints) == false)
                    {
                        return false;
                    }
                }
                else if(item.ItemTypeInfo.Equals(ItemType.CollectionVat))
                {
                    if(SetLocks(login, batch, mode, item, collectionVats) == false)
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Type not handled in Lock: " + item.ItemTypeInfo);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Unlock locks of this mode on given items.
        /// </summary>
        /// <param name="login">The login of the user is our transaction id</param>
        /// <param name="batch">The items to unlock</param>
        /// <param name="mode">The mode to be unlocked</param>
        public void Unlock(String login, LockBatch batch, LockMode mode)
        {
            foreach(LockItem item in batch.ItemsToLock)
            {
                if(item.ItemTypeInfo.Equals(ItemType.CollectionPoint))
                {
                    ReleaseLocks(login, mode, item, collectionPoints);
                }
                else if(item.ItemTypeInfo.Equals(ItemType.CollectionVat))
                {
                    ReleaseLocks(login, mode, item, collectionVats);
                }
                else
                {
                    Console.WriteLine("Type no handled in Unlock: " + item.ItemTypeInfo);
                }
            }
        }

        class LockData
        {
            public string login;
            public LockMode mode;

            public LockData(string login, LockMode mode)
            {
                this.login = login;
                this.mode = mode;
            }

            public override bool Equals(System.Object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                LockData d = obj as LockData;
                if ((System.Object)d == null)
                {
                    return false;
                }

                return (login.Equals(d.login)) && (mode.Equals(d.mode));
            }
            public bool Equals(LockData d)
            {
                if ((object)d == null)
                {
                    return false;
                }
                return (login.Equals(d.login)) && (mode.Equals(d.mode));
            }
        }

        /// <summary>
        /// Tries to set the locks for given target collection and performs rollback on lock can not be applied.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="batch"></param>
        /// <param name="mode"></param>
        /// <param name="item"></param>
        /// <param name="targetCollection"></param>
        /// <returns></returns>
        private bool SetLocks(String login, LockBatch batch, LockMode mode, LockItem item, Dictionary<int, List<LockData>> targetCollection)
        {
            foreach (int id in item.IDsToLock)
            {
                List<LockData> currentLocks = null;
                if (!targetCollection.TryGetValue(id, out currentLocks))
                {
                    currentLocks = new List<LockData>();
                    currentLocks.Add(new LockData(login, mode));
                    targetCollection.Add(id, currentLocks);
                }
                else
                {
                    foreach (LockData lockData in currentLocks)
                    {
                        if (!LockMatrix.IsModeCompatible(lockData.mode, mode))
                        {
                            Unlock(login, batch, mode);
                            return false;
                        }
                        else
                        {
                            currentLocks.Add(new LockData(login, mode));
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Releases all locks for given target collection
        /// </summary>
        /// <param name="login"></param>
        /// <param name="batch"></param>
        /// <param name="mode"></param>
        /// <param name="item"></param>
        /// <param name="targetCollection"></param>
        private void ReleaseLocks(String login, LockMode mode, LockItem item, Dictionary<int, List<LockData>> targetCollection)
        {
            foreach(int id in item.IDsToLock)
            {
                List<LockData> currentLocks = null;
                if(targetCollection.TryGetValue(id, out currentLocks))
                {
                    currentLocks.Remove(new LockData(login, mode));
                }
            }
        }
    }

}
