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
using ToDoApp.Models.Dto;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> SignInManager;
        private IIdentityService IdentityService;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IIdentityService identityService)
        {
            IdentityService = identityService;
            this.userManager = userManager;
            this.SignInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegistrationModel userModel)
        {
            var response = await IdentityService.RegisterAsync(userModel);
            return StatusCode(StatusCodes.Status201Created,response);
        }

        [HttpPost("login")]
        public async Task<object> SignIn([FromBody] UserRegistrationModel userAuthModel)
        {
            AppUser user = userAuthModel.GetAppUser();
            var result = await SignInManager.CheckPasswordSignInAsync(user, userAuthModel.Password, false);
            if (result.Succeeded)
                return user.GetUserModel();

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        [HttpPost("forgot-password/{email}")]
        public async Task ForgotPassword(string email)
        {
            var appUser = await userManager.FindByEmailAsync(email);
            var token = await userManager.GeneratePasswordResetTokenAsync(appUser);
        }

        [HttpPost("reset-password")]
        public async Task<JsonResult> ResetPassword([FromHeader] string token, [FromBody] UserRegistrationModel userAuthModel)
        {
            var appUser = await userManager.FindByEmailAsync(userAuthModel.Email);
            var result = await userManager.ResetPasswordAsync(appUser, token, userAuthModel.Password);
            if(result.Succeeded)
            {
                return null;
            }
            else
            {
                var e = result.Errors.First();
                
                return Json(null);
            }
        }
    }
}