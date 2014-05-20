using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model.Lock
{

    public class LockBatch
    {
        private List<LockItem> itemsToLock = new List<LockItem>();

        public List<LockItem> ItemsToLock
        {
            get { return itemsToLock; }
            set { itemsToLock = value; }
        }
    }
}
