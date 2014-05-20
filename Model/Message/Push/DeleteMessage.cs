using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Message.Push
{
    [Serializable]
    public class DeleteMessage : NotificationMessage
    {
        public DeleteMessage()
        {
            this.msgType = NotificationMessage.MessageType.DeleteMessage;
        }

    }
}
