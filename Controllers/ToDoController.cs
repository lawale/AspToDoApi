using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    public class ToDoController : Controller
    {
        public ToDoController()
        {
        }

        [HttpGet]
        public IEnumerable<ToDo> Get() => null;

        [HttpGet("{id}")]
        public ToDo Get(int id) => null;

        [HttpPost]
        public ToDo Post([FromBody] ToDo toDo) => null;

        [HttpPut]
        public ToDo Put([FromBody] ToDo toDo) => null;

        [HttpDelete("{id}")]
        public void Delete(int id) { }

        [HttpPatch("{id}")]
        public StatusCodeResult Patch(int id, [FromBody] JsonPatchDocument<ToDo> jsonPatch) => null;
    }
}