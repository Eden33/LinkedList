using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using TreeEditor.Resource;

namespace TreeEditor.UI.Proxy
{
    public class UICollectionPoint : UIObject
    {
        private IList<UICollectionVat> vats = new List<UICollectionVat>();

        public UICollectionPoint(CollectionPoint p)
        {
            Id = p.Id;
            Name = "CP " + Id;

            foreach (CollectionVat v in p.Vats)
            {
                vats.Add(ResourceManager.Instance.getCollectionVat(v.Id));
            }
        }

        public IList<UICollectionVat> Vats
        {
            get { return vats; }
        }

    }
}
