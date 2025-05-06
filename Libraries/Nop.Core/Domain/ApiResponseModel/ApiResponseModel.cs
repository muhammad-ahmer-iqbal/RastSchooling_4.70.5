using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.ApiResponseModel
{
    public class ApiResponseModel
    {

        public ApiResponseModel(bool success)
        {
            this.success = success;
        }

        public ApiResponseModel(bool success, string message) : this(success)
        {
            this.message = message;
            if (!success)
            {
                this.error = this.message;
            }
        }

        public ApiResponseModel(bool success, object model) : this(success)
        {
            this.model = model;
        }

        public ApiResponseModel(bool success, string message, object model) : this(success, message)
        {
            this.model = model;
        }

        public bool success { get; set; }
        public string message { get; set; }
        public string error { get; set; }
        public object model { get; set; }

    }
}
