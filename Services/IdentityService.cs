using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Infrastructures;
using ToDoApp.Models;
using ToDoApp.Models.Dto;
using ToDoApp.Settings;

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
        public async Task<UserAuthenticationResult> RegisterAsync(UserRegistrationModel user)
        {
            Console.WriteLine("just entered registerasync");
            var newUser = user.GetAppUser();
            Console.WriteLine($"NewUser: {newUser}");
            var result = await userManager.CreateAsync(newUser, user.Password);
            Console.WriteLine(result);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new UserAuthenticationResult
            {
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
