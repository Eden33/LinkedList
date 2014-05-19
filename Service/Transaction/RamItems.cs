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
        //TODO: fix me, decide wisely
        internal Dictionary<int, object> vats = new Dictionary<int, object>();
        internal Dictionary<int, object> points = new Dictionary<int, object>();
    }
}
