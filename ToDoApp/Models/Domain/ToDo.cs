using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Models.Domain
{
    public class ToDo
    {
        public int Id { get; set; }

        [MaxLength(25)]
        public string Title { get; set; }

        public string Details { get; set; }

        public Status Status { get; set; }

        public string UserId {get; set;}

        [ForeignKey(nameof(UserId))]
        public AppUser User {get; set;}

        public DateTime DateCreated { get; set; }

        public DateTime? DateCompleted { get; set; }

    }
}
