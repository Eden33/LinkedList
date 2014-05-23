using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Model.Lock;

namespace Model.Message.Push
{
    [Serializable]
    public class LockMessage : NotificationMessage
    {
        #region members

        private LockBatch lockBatch;
        private string loginName;
        private bool isLocked;

        #endregion

        public LockMessage(String loginName, bool isLocked, LockBatch batch)
        {
            msgType = NotificationMessage.MessageType.LockMessage;
            lockBatch = batch;
            this.loginName = loginName;
            this.isLocked = isLocked;
        }

        #region properties

        public LockBatch LockBatch
        {
            get { return lockBatch; }
        }

        public string LoginName
        {
            get { return loginName; }
        }

        public bool IsLocked
        {
            get { return isLocked;  }
        }

        #endregion
    }
}
