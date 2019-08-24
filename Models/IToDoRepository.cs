using System;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
    public interface IToDoRepository
    {
        IQueryable<ToDo> ToDos {get;}

        ToDo AddToDo(ToDo toDo);

        ToDo this[int id] { get; }

        ToDo UpdateToDo(ToDo toDo);

    }
}