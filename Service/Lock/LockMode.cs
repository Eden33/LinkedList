using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lock
{
    public enum LockMode
    {
        Locked,
        WriteLock,
        ReadLock
    }
}
