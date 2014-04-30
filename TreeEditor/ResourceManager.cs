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
        private static Dictionary<int, UICollectionPoint> points = new Dictionary<int, UICollectionPoint>();
        private static ResourceService.ResourceServiceClient client = new ResourceService.ResourceServiceClient("NetTcpBinding_IResourceService");

        public static UICollectionVat getCollectionVat(int id)
        {
            UICollectionVat flyweight = null;
            if (!vats.TryGetValue(id, out flyweight))
            {
                flyweight = new UICollectionVat(client.GetCollectionVat(id));
                vats.Add(id, flyweight);
            }
            return flyweight;
        }

        public static UICollectionPoint getCollectionPoint(int id)
        {
            UICollectionPoint flyweight = null;
            if (!points.TryGetValue(id, out flyweight))
            {
                flyweight = new UICollectionPoint(client.GetCollectionPoint(id));
                points.Add(id, flyweight);
            }
            return flyweight;
        }
    }
}
