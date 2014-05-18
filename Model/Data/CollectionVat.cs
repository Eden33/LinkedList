using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    [DataContract]
    public class CollectionVat : Item
    {
        public CollectionVat(int id) : base(id) { }
    }
}
