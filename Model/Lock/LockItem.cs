using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Model.Lock
{

    public class LockItem
    {
        private Item typeToLock;

        public Item ItemTypeInfo
        {
            get { return typeToLock; }
            set { typeToLock = value; }
        }
        private List<int> idsToLock;

        public List<int> IDsToLock
        {
            get { return idsToLock; }
            set { idsToLock = value; }
        }

    }
}
