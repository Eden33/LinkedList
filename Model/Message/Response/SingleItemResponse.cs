using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Model.Message.Response
{
    [Serializable]
    public class SingleItemResponse : ResponseMessage
    {
        public SingleItemResponse() : base() { }

        public SingleItemResponse(bool success, string errorDesc = "") : base(success, errorDesc) { }

        private Item item;

        public Item Item
        {
            get { return item; }
            set { item = value; }
        }
    }
}
