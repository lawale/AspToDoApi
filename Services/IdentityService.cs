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
using ToDoApp.Models.Domain;
using ToDoApp.Models.Dto;
using ToDoApp.Models.Dto.Requests;
using ToDoApp.Settings;
using ToDoApp.Models.Dto.Responses;
using ToDoApp.Models.DataContext;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> userManager;

        private readonly JwtSettings jwtSettings;

        private readonly AppIdentityDbContext context;

        private readonly TokenValidationParameters tokenValidationParameters;

        public IdentityService(UserManager<AppUser> userManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, AppIdentityDbContext context)
        {
            this.userManager = userManager;
            this.jwtSettings = jwtSettings;
            this.tokenValidationParameters = tokenValidationParameters;
            this.context = context;
        }

        private async Task<UserAuthenticationResult> GenerateUserAuthenticationResult(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
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
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreatedOn = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();
            return new UserAuthenticationResult
            {
                Token = tokenHandler.WriteToken(token),
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<UserAuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return new UserAuthenticationResult
                {
                    Success = false,
                    Errors = new[] { new ValidationError("User", "This user does not exist")}
                };
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, password);
            if(!isPasswordValid)
                return new UserAuthenticationResult
                {
                    Success = false,
                    Errors = new[] { new ValidationError("Login", "Invalid Email/Password")}
                };
            return await GenerateUserAuthenticationResult(user);
        }

        /// <summary>
        /// Method used to Login a User
        /// </summary>
        /// <param name="userModel">Body of the incoming Request</param>
        /// <returns>Response indicating errors if failed and token if success</returns>
        public async Task<UserAuthenticationResult> RegisterAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user != null)
            {
                return new UserAuthenticationResult
                {
                    Success = false,
                    Errors = new[] { new ValidationError("Email", $"The email address '{email}' already exists")}
                };
            }
            var newUser = email.TryGetAppUser();
            var createdUser = await userManager.CreateAsync(newUser, password);
            if(!createdUser.Succeeded)
            {
                return new UserAuthenticationResult
                {
                    Success = false,
                    Errors = createdUser.Errors.Select(error => error.GetPasswordError()).ToArray()
                };
            }
            return await GenerateUserAuthenticationResult(newUser);
        }

        public async Task<UserAuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);
            if(validatedToken == null)
                return new UserAuthenticationResult { Errors = new[] { new ValidationError("token", "Invalid token") } };
            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

            if(expiryDateTimeUtc > DateTime.UtcNow)
                return new UserAuthenticationResult { Errors = new[] { new ValidationError("token", "Token hasn't expired") } };

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
            if(storedRefreshToken == null)
                return new UserAuthenticationResult { Errors = new[] { new ValidationError("refresh token", "This refresh token doesn't exist.") } };
            
            if(DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return new UserAuthenticationResult { Errors = new[] { new ValidationError("refresh token", "This refresh token hasn't expired.") } };
            if(storedRefreshToken.Invalidated)
                return new UserAuthenticationResult { Errors = new[] { new ValidationError("refresh token", "This refresh token has been invalidated.") } };
            if(storedRefreshToken.Used)
                return new UserAuthenticationResult { Errors = new[] { new ValidationError("refresh token", "This refresh token has been used.") } };
            if(storedRefreshToken.JwtId != jti)
                return new UserAuthenticationResult { Errors = new[] { new ValidationError("refresh token", "This refresh token  been invalidated.") } };
            storedRefreshToken.Used = true;
            context.RefreshTokens.Update(storedRefreshToken);
            await context.SaveChangesAsync();

            var user = await userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return await GenerateUserAuthenticationResult(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,out var validatedToken);
                if(!IsJwtWithValidSecurityAlgo(validatedToken))
                    return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgo(SecurityToken validatedToken)
            => validatedToken is JwtSecurityToken jwtSecurityToken 
            && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
    }
}
