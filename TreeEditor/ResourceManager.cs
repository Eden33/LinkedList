using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                flyweight.PropertyChanged += UIObject_PropertyChanged;
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
                flyweight.PropertyChanged += UIObject_PropertyChanged;
                points.Add(id, flyweight);
            }
            return flyweight;
        }

        static private void UIObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Deleted"))
            {
                if(sender is UICollectionVat)
                {
                    UICollectionVat removed = sender as UICollectionVat;
                    if (vats.Remove(removed.Id))
                    {
                        Console.WriteLine("Vat with id: " + removed.Id + " removed from ResourceManager.");
                    }
                }
                else if(sender is UICollectionPoint)
                {
                    UICollectionPoint removed = sender as UICollectionPoint;
                    if(points.Remove(removed.Id))
                    {
                        Console.WriteLine("CollectionPoint with id: " + removed.Id + " removed from ResourceManager.");
                    }
                }
            }
        }
    }
}
