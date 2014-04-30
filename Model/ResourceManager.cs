using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public class ResourceManager
    {
        private static Dictionary<int, CollectionVat> vats = new Dictionary<int, CollectionVat>();
        private static Dictionary<int, CollectionPoint> points = new Dictionary<int, CollectionPoint>();

        public static CollectionVat getCollectionVat(int id)
        {
            CollectionVat flyweight = null;
            if (!vats.TryGetValue(id, out flyweight))
            {
                flyweight = new CollectionVat(id);
                vats.Add(id, flyweight);
            }
            return flyweight;
        }

        public static CollectionPoint getCollectionPoint(int id)
        {
            CollectionPoint flyweight = null;
            if (!points.TryGetValue(id, out flyweight))
            {
                flyweight = new CollectionPoint(id);
                points.Add(id, flyweight);
            }
            return flyweight;
        }
    }
}
