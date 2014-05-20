using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Message.Response
{
    [Serializable]
    public class DeleteResponse : ResponseMessage
    {
        public DeleteResponse(bool success, String errorDesc = "") : base(success, errorDesc) { }

    }
}
