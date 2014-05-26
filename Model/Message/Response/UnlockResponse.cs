using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Message.Response
{
    [Serializable]
    public class UnlockResponse : ResponseMessage
    {
        public UnlockResponse() : base() { }

        public UnlockResponse(bool success, string errorDesc = "") : base(success, errorDesc) { }
    }
}
