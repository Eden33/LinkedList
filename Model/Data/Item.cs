using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    //TODO: probably we can make it abstract, but we should be very careful

    [KnownType(typeof(CollectionPoint))]
    [KnownType(typeof(Client))]
    [DataContract(Namespace="http://itm4.gopp/resources/item")]
    public class Item
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
    }
}
