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
        public CollectionPoint(int id) : base(id) { }

        private string description;

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private List<Client> clients = new List<Client>();

        [DataMember]
        public List<Client> Clients
        {
            get
            {
                return clients;
            }
            set
            {
                clients = value;
            }
        }
    }
}
