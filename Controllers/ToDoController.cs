using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    public class ToDoController : Controller
    {
        IToDoRepository repository;
        public ToDoController(IToDoRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<ToDo> Get() => repository.ToDos;

        [HttpGet("{id}")]
        public ToDo Get(int id) => repository[id];

        [HttpPost]
        public ToDo Post([FromBody] ToDo toDo) {
            var input = new ToDo
            {
                Title = toDo.Title,
                Details = toDo.Details,
                DateCreated = DateTime.Now,
                Status = Status.NotDone
            };
            return repository.AddToDo(input);
        }

        [HttpPut]
        public ToDo Put([FromBody] ToDo toDo) => repository.UpdateToDo(toDo);

        [HttpPatch("{id}")]
        public StatusCodeResult Patch(int id, [FromBody] JsonPatchDocument<ToDo> jsonPatch)
        {
            var toDo = Get(id);
            if (toDo != null)
            {
                jsonPatch.ApplyTo(toDo);
                return Ok();
            }
            else
                return NotFound();
        }
    }
}