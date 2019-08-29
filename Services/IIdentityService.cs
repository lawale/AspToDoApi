using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models.Dto;
using ToDoApp.Models.Dto.Requests;
using ToDoApp.Models.Dto.Responses;

namespace ToDoApp.Services
{
    public interface IIdentityService
    {
        Task<UserAuthenticationResult> RegisterAsync(UserRegistrationRequest user);

        Task<UserAuthenticationResult> LoginAsync(UserLoginRequest user);
    }
}
