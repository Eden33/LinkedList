using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Service.Message
{
    class LockMessage : NotificationMessage
    {
        #region members

        private LockBatch lockBatch;
        private string loginName;

        #endregion

        public LockMessage(String loginName, LockBatch batch)
        {
            msgType = NotificationMessage.MessageType.LockMessage;
            lockBatch = batch;
            this.loginName = loginName;
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

        #endregion
    }
}
