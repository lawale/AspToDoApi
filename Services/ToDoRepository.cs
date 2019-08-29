using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class ToDoRepository : IToDoRepository
    {
        private ApplicationDbContext context;

        public ToDoRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ToDo> ToDos => context.ToDos;

        public ToDo this[int id] => ToDos.SingleOrDefault(t => t.Id == id);


        public ToDo AddToDo(ToDo toDo)
        {
            context.Add(toDo);
            context.SaveChanges();
            return toDo;
        }

        public ToDo UpdateToDo(ToDo toDo)
        {
            context.Update(toDo);
            context.SaveChanges();
            return toDo;
        }
    }
}
