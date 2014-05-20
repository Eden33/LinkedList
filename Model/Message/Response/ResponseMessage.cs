using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Message.Response
{
    [Serializable]
    public class ResponseMessage
    {
        public ResponseMessage(bool success, string errorDesc = "")
        {
            this.success = success;
            this.errorDesc = errorDesc;
        }

        private bool success;

        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        private string errorDesc;

        public string ErrorDesc
        {
            get { return errorDesc; }
            set { errorDesc = value; }
        }
    }
}
