using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeEditor.UI.Proxy
{
    public class UILockInfo
    {
        private string lockOwner;
        private bool locked;

        public UILockInfo(string lockOwner, bool locked)
        {
            this.lockOwner = lockOwner;
            this.locked = locked;
        }

        public string LockOwner
        {
            get { return lockOwner; }
        }
        public bool Locked
        {
            get { return locked; }
        }
    }
}
