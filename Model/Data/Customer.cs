using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    [DataContract(Namespace = "http://itm4.gopp/resources/customer")]
    public class Customer : Item
    {
        public Customer() : base() { }

        public Customer(int id) : base(id) { }

        private string firstName;

        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;

        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        private string address;

        [DataMember]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
    }
}
