using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ToDoApp.Models.Domain;

namespace ToDoApp.Services
{
    public interface IToDoRepository
    {
        Task<IEnumerable<ToDo>> GetToDosAsync();

        Task<ToDo> AddToDoAsync(ToDo toDo);

        ToDo GetToDoById(int id);

        Task<bool> UpdateToDoAsync(ToDo toDo);
        
        Task<bool> UserOwnsPostAsync(int id, string userId);
    }
}