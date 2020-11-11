using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Class
{
    public class ErrorResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public ErrorResponse(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }
    }
}
