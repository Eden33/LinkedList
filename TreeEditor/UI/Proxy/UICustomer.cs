using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeEditor.UI.Proxy
{
    public class UICustomer : UIItem
    {
        private Customer customer;

        public UICustomer(Customer c)
        {
            this.customer = c;
        }

        public string FirstName
        {
            get
            {
                return this.customer.FirstName;
            }
        }

        public string LastName
        {
            get
            {
                return this.customer.LastName;
            }
        }

        public string Address
        {
            get
            {
                return this.customer.Address;
            }
        }

        public override int Id
        {
            get
            {
                return this.customer.Id;
            }
        }
    }
}
