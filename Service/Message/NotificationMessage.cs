﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Message
{
    abstract class NotificationMessage
    {
        protected MessageType msgType;

        public MessageType MsgType
        {
            get { return msgType; }
        }

        public enum MessageType
        {
            LockMessage
        }
    }
}
