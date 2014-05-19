using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Model.Lock
{
    [DataContract]
    public class LockItem
    {
        private Item typeToLock;
        [DataMember]
        public Item ItemTypeInfo
        {
            get { return typeToLock; }
            set { typeToLock = value; }
        }
        private List<int> idsToLock;
        [DataMember]
        public List<int> IDsToLock
        {
            get { return idsToLock; }
            set { idsToLock = value; }
        }

    }
}
