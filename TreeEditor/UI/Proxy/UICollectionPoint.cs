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

        public CollectionPoint CollectionPoint
        {
            get
            {
                return this.collectionPoint;
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
