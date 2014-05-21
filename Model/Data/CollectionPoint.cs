using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    [DataContract(Namespace = "http://itm4.gopp/resources/collectionPoint")]
    public class CollectionPoint : Item
    {
        public CollectionPoint() : base() { }

        public CollectionPoint(int id) : base(id) { }

        private string description;

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private List<Customer> customers = new List<Customer>();

        [DataMember]
        public List<Customer> Customers
        {
            get
            {
                return customers;
            }
            set
            {
                customers = value;
            }
        }
    }
}
