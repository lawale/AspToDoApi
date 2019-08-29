using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using ToDoApp.Errors.Validation;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Infrastructures;
using ToDoApp.Models;
using ToDoApp.Models.Dto;
using ToDoApp.Models.Dto.Requests;
using ToDoApp.Settings;
using ToDoApp.Models.Dto.Responses;

namespace ToDoApp.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> userManager;

        private readonly JwtSettings jwtSettings;

        public IdentityService(UserManager<AppUser> userManager, JwtSettings jwtSettings)
        {
            this.userManager = userManager;
            this.jwtSettings = jwtSettings;
        }

        private string GenerateUserToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<UserAuthenticationResult> LoginAsync(UserLoginRequest userModel)
        {
            var user = await userManager.FindByEmailAsync(userModel.Email);
            if(user == null)
            {
                return new UserAuthenticationResult
                {
                    Success = false,
                    Errors = new[] { new ValidationError("User", "This user does not exist")}
                };
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user,userModel.Password);
            if(!isPasswordValid)
                return new UserAuthenticationResult
                {
                    Success = false,
                    Errors = new[] { new ValidationError("Login", "Invalid Email/Password")}
                };
            return new UserAuthenticationResult
            {
                Token = GenerateUserToken(user),
                Success = true
            };
        }

        /// <summary>
        /// Method used to Login a User
        /// </summary>
        /// <param name="userModel">Body of the incoming Request</param>
        /// <returns>Response indicating errors if failed and token if success</returns>
        public async Task<UserAuthenticationResult> RegisterAsync(UserRegistrationRequest userModel)
        {
            var user = await userManager.FindByEmailAsync(userModel.Email);
            if(user != null)
            {
                return new UserAuthenticationResult
                {
                    Success = false,
                    Errors = new[] { new ValidationError("Email", $"The email address '{userModel.Email}' already exists")}
                };
            }
            var newUser = userModel.GetAppUser();
            var createdUser = await userManager.CreateAsync(newUser,userModel.Password);
            if(!createdUser.Succeeded)
            {
                return new UserAuthenticationResult
                {
                    Success = false,
                    Errors = createdUser.Errors.Select(error => error.GetPasswordError()).ToArray()
                };
            }
            return new UserAuthenticationResult
            {
                Token = GenerateUserToken(newUser),
                Success = true
            };
        }
    }
}
