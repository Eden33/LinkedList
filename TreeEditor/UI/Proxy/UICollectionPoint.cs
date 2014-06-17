using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Client.Resource;

namespace Client.UI.Proxy
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

        public String Address
        {
            get
            {
                return this.collectionPoint.Address;
            }
        }

        public List<UICustomer> Customers
        {
            get
            {
                List<UICustomer> customers = new List<UICustomer>();
                foreach(Customer c in collectionPoint.Customers)
                {
                    customers.Add(ResourceManager.Instance.GetSingleItem<UICustomer>(c.Id));
                }
                return customers;
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
