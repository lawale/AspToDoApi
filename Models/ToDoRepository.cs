using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Models
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

        public async Task<ToDo> AddToDo(ToDo toDo)
        {
            context.Add(toDo);
            await context.SaveChangesAsync();
            return toDo;
        }

        public async Task<ToDo> UpdateToDo(ToDo toDo)
        {
            context.Update(toDo);
            await context.SaveChangesAsync();
            return toDo;
        }
    }
}
