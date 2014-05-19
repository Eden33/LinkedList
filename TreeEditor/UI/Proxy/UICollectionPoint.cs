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

        public IList<Client> Clients
        {
            get 
            {
                IList<Client> clients = null;
                if (collectionPoint != null && collectionPoint.Clients != null)
                {
                    foreach (Client c in collectionPoint.Clients)
                    {
                        //TODO: get the clients from ResourceManager
                    }
                }
                return clients;
            }
        }
    }
}
