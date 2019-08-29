using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ToDoController : Controller
    {
        IToDoRepository repository;
        public ToDoController(IToDoRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>  Ok(await repository.GetToDosAsync());

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var toDo = repository.GetToDoById(id);
            if(toDo == null)
                return NotFound();
            else
            return Ok(toDo);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ToDo toDo) {
            var input = new ToDo
            {
                Title = toDo.Title,
                Details = toDo.Details,
                DateCreated = DateTime.Now,
                Status = Status.NotDone
            };
            //var _toDo = repository.AddToDo(input);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var request = HttpContext.Request.Path;
            var location = $"{baseUrl}{request}/{input.Id}";
            return Created(location,input);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ToDo toDo)
        {
            var updated = await repository.UpdateToDoAsync(toDo);
            if(updated)
                return Ok(toDo);
            return NotFound();
        }

        // [HttpPatch("{id}")]
        // public StatusCodeResult Patch(int id, [FromBody] JsonPatchDocument<ToDo> jsonPatch)
        // {
        //     var toDo = repository[id];
        //     if (toDo != null)
        //     {
        //         jsonPatch.ApplyTo(toDo);
        //         return Ok();
        //     }
        //     else
        //         return NotFound();
        // }
    }
}