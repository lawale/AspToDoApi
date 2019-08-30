using ToDoApp.Errors.Validation;
using Newtonsoft.Json;

namespace ToDoApp.Models.Dto.Responses
{
    public class UserAuthenticationResult
    {
        public bool Success {get;set;}

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ValidationError[] Errors {get;set;}

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}