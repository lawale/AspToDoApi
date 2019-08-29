using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title of To Do Activity is required")]
        [MaxLength(25)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Details of To Do Activiy is required")]
        public string Details { get; set; }

        public Status Status { get; set; }

        public string UserId {get; set;}

        [ForeignKey(nameof(UserId))]
        public AppUser User {get; set;}

        public DateTime DateCreated { get; set; }

        public DateTime? DateCompleted { get; set; }

    }
}
