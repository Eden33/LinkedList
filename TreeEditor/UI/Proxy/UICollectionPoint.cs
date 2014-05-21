using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using TreeEditor.Resource;

namespace TreeEditor.UI.Proxy
{
    public class UICollectionPoint : UIItem
    {
        private CollectionPoint collectionPoint;

        public UICollectionPoint(CollectionPoint p)
        {
            this.collectionPoint = p;
        }

        public String Description
        {
            get
            {
                return this.collectionPoint.Description;
            }
        }

        public List<UIClient> Clients
        {
            get
            {
                List<UIClient> clients = new List<UIClient>();
                foreach(Client c in collectionPoint.Clients)
                {
                    clients.Add(ResourceManager.Instance.GetSingleItem<UIClient>(c.Id));
                }
                return clients;
            }
        }

        public override int Id
        {
            get 
            {
                return this.collectionPoint.Id;
            }
        }
    }
}
