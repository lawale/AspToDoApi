using System.ComponentModel.DataAnnotations;
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
using ToDoApp.Models.Domain;
using ToDoApp.Services;
using ToDoApp.Models.Dto.Requests;
using ToDoApp.Infrastructures;
using ToDoApp.Errors.Validation;

namespace ToDoApp.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ToDoController : Controller
    {
        readonly IToDoRepository repository;
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
        public async Task<IActionResult> Post([FromBody] CreateToDoRequest toDoRequest) {
            var toDo = toDoRequest.GetToDo(HttpContext);
            toDo = await repository.AddToDoAsync(toDo);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var request = HttpContext.Request.Path;
            var location = $"{baseUrl}{request}/{toDo.Id}";
            return Created(location,toDo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateToDoRequest toDoRequest)
        {
            var isOwner = await repository.UserOwnsPostAsync(id, HttpContext.GetUserId());
            if(!isOwner)
            {
                var error = new ValidationError("To Do", "You do not own the ToDo acticity you're trying to update");
                var validation = new ValidationResultModel(new[] {error});
                return Unauthorized(validation);
            }
            var toDo = toDoRequest.GetToDo();
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