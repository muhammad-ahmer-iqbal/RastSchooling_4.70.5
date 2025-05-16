using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Utilities
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
                error = this.message;
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


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool success { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string error { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object model { get; set; }

    }
}
