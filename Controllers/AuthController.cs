using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Errors.Validation;
using ToDoApp.Infrastructures;
using ToDoApp.Models;
using ToDoApp.Models.Dto.Requests;
using ToDoApp.Models.Dto.Responses;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IIdentityService IdentityService;
        public AuthController(IIdentityService identityService)
        {
            IdentityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegistrationRequest userModel)
        {
            var response = await IdentityService.RegisterAsync(userModel);
            if(!response.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = response.Errors
                });
            }
            return StatusCode(StatusCodes.Status201Created,response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn([FromBody] UserRegistrationRequest userAuthModel)
        {
            // AppUser user = userAuthModel.GetAppUser();
            // var result = await SignInManager.CheckPasswordSignInAsync(user, userAuthModel.Password, false);
            // if (result.Succeeded)
            //     return Ok(user.GetUserModel());

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        [HttpPost("forgot-password/{email}")]
        public async Task ForgotPassword(string email)
        {
            // var appUser = await userManager.FindByEmailAsync(email);
            // var token = await userManager.GeneratePasswordResetTokenAsync(appUser);
        }

        [HttpPost("reset-password")]
        public async Task<JsonResult> ResetPassword([FromHeader] string token, [FromBody] UserRegistrationRequest userAuthModel)
        {
            // var appUser = await userManager.FindByEmailAsync(userAuthModel.Email);
            // var result = await userManager.ResetPasswordAsync(appUser, token, userAuthModel.Password);
            // if(result.Succeeded)
            // {
            //     return null;
            // }
            // else
            // {
            //     var e = result.Errors.First();
                
            //     return Json(null);
            // }
            return Json(null);
        }
    }
}