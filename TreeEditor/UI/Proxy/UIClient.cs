using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeEditor.UI.Proxy
{
    public class UIClient : UIItem
    {
        private Client client;

        public UIClient(Client c)
        {
            this.client = c;
        }

        public Client Client
        {
            get
            {
                return this.client;
            }
        }
        public override int Id
        {
            get
            {
                return this.client.Id;
            }
        }
    }
}
