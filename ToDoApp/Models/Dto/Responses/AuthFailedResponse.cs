using ToDoApp.Errors.Validation;

namespace ToDoApp.Models.Dto.Responses
{
    public class AuthFailedResponse
    {
        public string Status { get; } = "failed";

        public ValidationError[] Errors { get; set;}
    }
}