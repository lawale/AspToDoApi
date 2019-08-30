using System.ComponentModel.DataAnnotations;
using ToDoApp.Models.Domain;

namespace ToDoApp.Models.Dto.Requests
{
    public class CreateToDoRequest
    {

        [Required(ErrorMessage = "Title of To Do Activity is required")]
        [MaxLength(25)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Details of To Do Activiy is required")]
        public string Details { get; set; }

    }
}