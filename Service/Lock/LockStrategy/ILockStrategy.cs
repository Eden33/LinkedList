using Model.Lock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lock.LockStrategy
{
    public interface ILockStrategy
    {
        ///// <summary>
        ///// For each lock request a item-set the lock should be applied to has to be determined. 
        ///// The determined item-set depends on the underlying data model and dependencies in this model.
        ///// </summary>
        ///// <param name="id">The id of the item to be locked</param>
        ///// <returns>A LockBatch instance containing all information needed on the item-set during locking-process</returns>
        LockBatch GetItemsToLock(int id);
    }
}
