using System;
using ToDoApp.Models.Domain;

namespace ToDoApp.Models.Dto.Requests
{
    public class UpdateToDoRequest
    {
        public int Id { get; set; }

        public Status Status { get; set; }

        public DateTime? DateCompleted { get; set; }
    }
}