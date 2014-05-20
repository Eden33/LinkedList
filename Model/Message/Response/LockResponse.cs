using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Message.Response
{
    [Serializable]
    public class LockResponse : ResponseMessage 
    {
        public LockResponse(bool success, string errorDesc = "") : base(success, errorDesc) { }
    }

}
