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
            lock(locks)
            {
                foreach (LockItem item in batch.ItemsToLock)
                {
                    if (SetLocks(login, batch, mode, item) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Unlock the locks for given user and mode.
        /// </summary>
        /// <param name="login">The login name of the user the unlock should be processed for</param>
        /// <param name="batch">The items to unlock</param>
        /// <param name="mode">The lock mode to unlock</param>
        /// <returns>True on all items could be unlocked. False if unlocking of at least one item has failed.</returns>
        public bool Unlock(String login, LockBatch batch, LockMode mode)
        {
            lock(locks)
            {
                bool allLocksReleased = true;
                foreach (LockItem item in batch.ItemsToLock)
                {
                    if (ReleaseLocks(login, mode, item) == false)
                    {
                        allLocksReleased = false;
                    }
                }
                return allLocksReleased;
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

                // for future write-lock development 
                // we have to pass currentLocks as a whole to assure that ll
                // for this user are present in currentLocks
                // write locks can only be set if the ll for this user are set

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

        private bool ReleaseLocks(String login, LockMode mode, LockItem item)
        {
            bool allLocksReleased = true;
            foreach(int id in item.IDsToLock)
            {
                List<LockData> currentLocks = locks.GetLocksForItem(id, item.ItemType);
                LockData release = new LockData(login, mode);
                if(currentLocks.Contains(release))
                {
                    currentLocks.Remove(release);
                    locks.UpdateLocksForItem(id, item.ItemType, currentLocks);
                }
                else
                {
                    //this should never be the case!
                    allLocksReleased = false; 
                }
            }
            return allLocksReleased;
        }

        #endregion

        #region public methods to retrieve current locking information

        public LockBatch GetCurrentLocks(string loginName)
        {
            LockBatch batch = new LockBatch();

            lock(locks)
            {
                LockItem cpLockItem = new LockItem();
                cpLockItem.ItemType = ResourceMap.ModelTypeToItemType<CollectionPoint>();
                cpLockItem.IDsToLock = locks.GetLockedIdsForItemType(cpLockItem.ItemType, loginName, LockMode.Locked);

                LockItem cLocks = new LockItem();
                cLocks.ItemType = ResourceMap.ModelTypeToItemType<Customer>();
                cLocks.IDsToLock = locks.GetLockedIdsForItemType(cLocks.ItemType, loginName, LockMode.Locked);

                batch.ItemsToLock = new List<LockItem>(new LockItem[] { cpLockItem, cLocks });
            }

            return batch;
        }

        #endregion
    
    }

}
