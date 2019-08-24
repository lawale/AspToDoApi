using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(25)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Details of To Do Activiy is required")]
        public string Details { get; set; }

        public Status Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateCompleted { get; set; }

        public ToDo()
        {
            Console.WriteLine(value: "Jo Sise na");
        }
    }
}
