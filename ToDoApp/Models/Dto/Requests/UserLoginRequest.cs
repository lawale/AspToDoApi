using System.ComponentModel.DataAnnotations;
namespace ToDoApp.Models.Dto.Requests
{
    public class UserLoginRequest
    {
        [EmailAddress]
        public string Email {get; set;}

        public string Password {get; set;}
    }
}