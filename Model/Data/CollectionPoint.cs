using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
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
}
