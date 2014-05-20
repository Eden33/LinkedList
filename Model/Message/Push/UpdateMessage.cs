using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Model.Message.Push
{
    [Serializable]
    public class UpdateMessage : NotificationMessage
    {
        public UpdateMessage()
        {
            this.msgType = NotificationMessage.MessageType.UpdateMessage;
        }

        private ItemType itemType;

        public ItemType ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }
        private Item item;

        public Item Item
        {
            get { return item; }
            set { item = value; }
        }
    }
}
