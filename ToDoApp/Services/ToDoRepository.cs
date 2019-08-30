using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models.Domain;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Models.DataContext;

namespace ToDoApp.Services
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationDbContext context;

        public ToDoRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ToDo> ToDos => context.ToDos;

        public async Task<IEnumerable<ToDo>> GetToDosAsync() => await ToDos.ToListAsync();

        public ToDo this[int id] => ToDos.SingleOrDefault(t => t.Id == id);


        public async Task<ToDo> AddToDoAsync(ToDo toDo)
        {
            await context.ToDos.AddAsync(toDo);
            await context.SaveChangesAsync();
            return toDo;
        }

        public async Task<ToDo> UpdateToDoAsync(int id,ToDo toDo)
        {
            var _toDo = GetToDoById(id);
            if(_toDo == null)
                return null;
            Console.WriteLine(_toDo);
            _toDo.Status = toDo.Status;
            _toDo.DateCompleted = toDo.DateCompleted;
            await context.SaveChangesAsync();
            return _toDo;
        }


        public ToDo GetToDoById(int id) => this[id];

        public async Task<bool> UserOwnsPostAsync(int id, string userId)
        {
            var toDo = await context.ToDos.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
            if(toDo == null)
                return false;
            if(toDo.UserId != userId)
                return false;
            return true;
        }
    }
}
