using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeEditor.Resource
{
    public class ResourceManager
    {
        private static Dictionary<int, UICollectionVat> vats = new Dictionary<int, UICollectionVat>();

        public static UICollectionVat getCollectionVat(int id) 
        {
            UICollectionVat flyweight = null;
            if(!vats.TryGetValue(id, out flyweight))
            {
                flyweight = new UICollectionVat(id);
                vats.Add(id, flyweight);
            }
            return flyweight;
        }
    }
}
