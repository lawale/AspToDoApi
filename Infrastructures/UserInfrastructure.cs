using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models.Domain;
using ToDoApp.Models.Dto.Requests;
using ToDoApp.Models.Dto;
namespace ToDoApp.Infrastructures
{
    public static class UserInfrastructure
    {
        public static AppUser GetAppUser(this UserRegistrationRequest userAuthModel)
            => new AppUser { Email = userAuthModel.Email, UserName = userAuthModel.Email };

        public static AppUser GetAppUser(this UserLoginRequest userAuthModel)
            => new AppUser { Email = userAuthModel.Email };

        public static AppUser TryGetAppUser(this string email)
            => new AppUser { Email = email, UserName = email };
    }
}
