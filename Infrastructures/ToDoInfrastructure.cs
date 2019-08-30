using Microsoft.AspNetCore.Http;
using ToDoApp.Models.Domain;
using ToDoApp.Models.Dto.Requests;

namespace ToDoApp.Infrastructures
{
    public static class ToDoInfrastructure
    {
        public static ToDo GetToDo(this CreateToDoRequest toDoRequest, HttpContext context)
            => new ToDo { Title = toDoRequest.Title, Details = toDoRequest.Details, Status = Status.NotDone, UserId = context.GetUserId() };

            public static ToDo GetToDo(this UpdateToDoRequest toDoRequest)
            => new ToDo { Title = toDoRequest.Title, Details = toDoRequest.Details, Status = toDoRequest.Status };
    }
}