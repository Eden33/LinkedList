using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Model.Lock;
using Service.Resource;

namespace Service.Lock
{
    public class LockManager
    {
        private LockCache locks = new LockCache();

        #region public methods to set/release locks on resources

        /// <summary>
        /// Tries to lock the items passed in LockBatch.
        /// </summary>
        /// <param name="login">The login of the user is our transaction id</param>
        /// <param name="batch">The items to lock</param>
        /// <param name="mode">The lock mode</param>
        /// <returns>True on lock success, false on rollback</returns>
        public bool Lock(String login, LockBatch batch, LockMode mode)
        {
            foreach (LockItem item in batch.ItemsToLock)
            {
                if(SetLocks(login, batch, mode, item) == false)
                {
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
            foreach (LockItem item in batch.ItemsToLock)
            {
                ReleaseLocks(login, mode, item);
            }
        }

        #endregion

        #region private helper methods

        private bool SetLocks(String login, LockBatch batch, LockMode mode, LockItem item)
        {
            foreach (int id in item.IDsToLock)
            {
                List<LockData> currentLocks = locks.GetLocksForItem(id, item.ItemType);
                LockData newLock = new LockData(login, mode);

                foreach(LockData lockData in currentLocks)
                {
                    if(newLock.Equals(lockData) == false                          
                        && !LockMatrix.IsModeCompatible(lockData.mode, mode))
                    {
                        Console.WriteLine("Lock conflict, rollback locks for user: {0}", login);
                        Unlock(login, batch, mode);
                        return false;
                    }
                }
                if(currentLocks.Contains(newLock) == false)
                {
                    currentLocks.Add(newLock);
                    locks.UpdateLocksForItem(id, item.ItemType, currentLocks);
                }
            }
            return true;
        }

        private void ReleaseLocks(String login, LockMode mode, LockItem item)
        {
            foreach(int id in item.IDsToLock)
            {
                List<LockData> currentLocks = locks.GetLocksForItem(id, item.ItemType);
                LockData release = new LockData(login, mode);
                if(currentLocks.Contains(release))
                {
                    currentLocks.Remove(release);
                    locks.UpdateLocksForItem(id, item.ItemType, currentLocks);
                }
            }
        }

        #endregion
    }

}
