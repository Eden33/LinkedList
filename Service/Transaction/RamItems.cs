using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Service.Transaction
{
    class RamItems
    {
        internal Dictionary<int, CollectionVat> vats = new Dictionary<int, CollectionVat>();
        internal Dictionary<int, CollectionPoint> points = new Dictionary<int, CollectionPoint>();
    }
}
