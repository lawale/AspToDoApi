using System;
using ToDoApp.Models.Domain;

namespace ToDoApp.Models.Dto.Responses
{
    public class ToDoResponse
    {
        public int Id { get; set; }
        
        public string Title { get; set; }

        public string Details { get; set; }

        public Status Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateCompleted { get; set; }
    }
}