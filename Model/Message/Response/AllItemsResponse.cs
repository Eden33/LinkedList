using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Model.Message.Response
{
    [Serializable]
    public class AllItemsResponse : ResponseMessage
    {
        public AllItemsResponse() : base() { }

        public AllItemsResponse(bool success, string errorDesc = "") : base(success, errorDesc) { }

        private List<Item> items;

        public List<Item> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
