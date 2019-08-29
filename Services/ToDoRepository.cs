﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> UpdateToDoAsync(ToDo toDo)
        {
            context.ToDos.Update(toDo);
            var update = await context.SaveChangesAsync();
            return update > 0;
        }


        public ToDo GetToDoById(int id) => this[id];

    }
}
