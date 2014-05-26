using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lock
{
    class LockData
    {
        public string login;
        public LockMode mode;

        public LockData(string login, LockMode mode)
        {
            this.login = login;
            this.mode = mode;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            LockData d = obj as LockData;
            if ((System.Object)d == null)
            {
                return false;
            }

            return (login.Equals(d.login)) && (mode.Equals(d.mode));
        }
        public bool Equals(LockData d)
        {
            if ((object)d == null)
            {
                return false;
            }
            return (login.Equals(d.login)) && (mode.Equals(d.mode));
        }
    }
}
