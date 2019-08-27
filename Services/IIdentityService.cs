using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models.Dto;

namespace ToDoApp.Services
{
    public interface IIdentityService
    {
        Task<UserAuthenticationResult> RegisterAsync(UserRegistrationModel user);
    }
}
