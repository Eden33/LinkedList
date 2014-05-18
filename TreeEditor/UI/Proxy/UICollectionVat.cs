using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace TreeEditor.UI.Proxy
{
    public class UICollectionVat : UIObject
    {
        public UICollectionVat(CollectionVat vat)
        {
            Id = vat.Id;
            Name = "Vat " + Id;
        }
    }
}
