using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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

}
