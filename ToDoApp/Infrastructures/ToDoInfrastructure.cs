using System;
using Microsoft.AspNetCore.Http;
using ToDoApp.Models.Domain;
using ToDoApp.Models.Dto.Requests;
using ToDoApp.Models.Dto.Responses;

namespace ToDoApp.Infrastructures
{
    public static class ToDoInfrastructure
    {
        public static ToDo GetToDo(this CreateToDoRequest toDoRequest, HttpContext context)
            => new ToDo { Title = toDoRequest.Title, Details = toDoRequest.Details, DateCreated = DateTime.UtcNow, Status = Status.NotDone, UserId = context.GetUserId() };

            public static ToDo GetToDo(this UpdateToDoRequest toDoRequest)
            => new ToDo { Id = toDoRequest.Id, DateCompleted = toDoRequest.DateCompleted, Status = toDoRequest.Status };

            public static ToDoResponse GetToDoResponse(this ToDo toDo)
                => new ToDoResponse{
                    Id = toDo.Id,
                    Title = toDo.Title,
                    Details = toDo.Details,
                    Status = toDo.Status,
                    DateCreated = toDo.DateCreated,
                    DateCompleted = toDo.DateCompleted
                };
    }
}