using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public interface IToDoRepository
    {
        IQueryable<ToDo> ToDos {get;}

        ToDo AddToDo(ToDo toDo);

        ToDo this[int id] { get; }

        ToDo UpdateToDo(ToDo toDo);

    }
}