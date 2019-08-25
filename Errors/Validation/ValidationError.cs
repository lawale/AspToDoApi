using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Errors.Validation
{
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = Field != string.Empty ? field: null;
            Message = message;
        }
    }
}
