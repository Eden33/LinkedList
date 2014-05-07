using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Model.Data
{
    [DataContract]
    public abstract class Item
    {
        public Item(int id)
        {
            Id = id;
        }

        [DataMember]
        private int id;
        public int Id {
            get 
            { 
                return id; 
            }
            set 
            { 
                id = value; 
            }
        }

        [DataMember]
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
    }

    [DataContract]
    public class CollectionPoint : Item
    {
        public CollectionPoint(int id) : base(id) { }

        private IList<CollectionVat> vats = new List<CollectionVat>();

        [DataMember]
        public IList<CollectionVat> Vats
        {
            get
            {
                return vats;
            }
            set
            {
                vats = value;
            }
        }
    }

    [DataContract]
    public class CollectionVat : Item
    {
        public CollectionVat(int id) : base(id) { }
    }
}
