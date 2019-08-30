using System;
using ToDoApp.Models.Domain;

namespace ToDoApp.Models.Dto.Requests
{
    public class UpdateToDoRequest
    {
        public string Title { get; set; }

        public string Details { get; set; }

        public Status Status { get; set; }

        public DateTime? DateCompleted { get; set; }
    }
}