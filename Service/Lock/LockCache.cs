using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Service.Lock
{
    class LockCache
    {
        private Dictionary<ItemType, Dictionary<int, List<LockData>>> locks = new Dictionary<ItemType, Dictionary<int, List<LockData>>>();

        /// <summary>
        /// Returns the current locks for the item of the corresponding item type.
        /// </summary>
        /// <param name="id">The id of the item</param>
        /// <param name="itemType">The item type of the item</param>
        /// <returns>A list containing all current locks for the item of this item type. 
        /// If currently no locks set the return value is an empty list.</returns>
        public List<LockData> GetLocksForItem(int id, ItemType itemType)
        {
            List<LockData> currentLocks = new List<LockData>();
            Dictionary<int, List<LockData>> itemLocks = null;
            if(locks.TryGetValue(itemType, out itemLocks))
            {
                if(itemLocks.ContainsKey(id))
                {
                    currentLocks = itemLocks[id];
                }
            }
            else
            {
                itemLocks = new Dictionary<int, List<LockData>>();
                locks.Add(itemType, itemLocks);
            }
            return currentLocks;
        }

        /// <summary>
        /// Updates the current locks for the item of the corresponding item type.
        /// Retrieve the locks first before calling this method.
        /// </summary>
        /// <param name="id">The id of the item</param>
        /// <param name="itemType">The item type of item</param>
        /// <param name="locks">The locks for the item of this item type</param>
        public void UpdateLocksForItem(int id, ItemType itemType, List<LockData> currentLocks)
        {
            Dictionary<int, List<LockData>> itemLocks = null;
            if(locks.TryGetValue(itemType, out itemLocks))
            {
                itemLocks.Remove(id);
                itemLocks.Add(id, currentLocks);
            }
        }
    }
}
