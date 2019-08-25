using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Errors.Validation;
using ToDoApp.Infrastructures;
using ToDoApp.Models;
using ToDoApp.Models.Dto;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private UserManager<AppUser> userManager;
        public AuthController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<object> Register([FromBody]UserModel userModel)
        {
            AppUser user = userModel.GetAppUser();
            IdentityResult result = await userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
                return user.GetUserModel();
            else
            {
                var value = result.Errors.Select(code => code.GetPasswordError());
                var resultError = new ValidationResultModel("Regisration Error", value);
                return Json(resultError);
            }
        }

        [HttpPost("login")]
        public async Task<object> SignIn([FromBody] UserModel userModel)
        {
            AppUser user = userModel.GetAppUser();
            await Task.Delay(2);
            return null;
        }

        [HttpPost("forgot-password/{email}")]
        public async Task ForgotPassword(string email)
        {
            var appUser = await userManager.FindByEmailAsync(email);
            var token = await userManager.GeneratePasswordResetTokenAsync(appUser);
        }

        [HttpPost("reset-password")]
        public async Task<JsonResult> ResetPassword([FromHeader] string token, [FromBody] UserAuthModel userAuthModel)
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