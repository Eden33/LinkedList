using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public enum ItemType
    {
        CollectionPoint,
        CollectionVat
    }

    [DataContract]
    public class LockBatch
    {
        private List<LockItem> itemsToLock;
        [DataMember]
        public List<LockItem> ItemsToLock 
        {
            get { return itemsToLock; }
            set { itemsToLock = value; }
        }
    }

    [DataContract]
    public class LockItem
    {
        private ItemType typeToLock;
        [DataMember]
        public ItemType ItemTypeInfo
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
