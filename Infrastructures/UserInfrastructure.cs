using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Models.Dto.Requests;
using ToDoApp.Models.Dto;
namespace ToDoApp.Infrastructures
{
    public static class UserInfrastructure
    {
        public static UserBaseModel GetUserModel(this AppUser user)
            => new UserBaseModel { Email = user.Email, Name = user.UserName, PhoneNumber = user.PhoneNumber };

        public static AppUser GetAppUser(this UserModel userModel)
            => new AppUser { Email = userModel.Email };

        public static AppUser GetAppUser(this UserRegistrationRequest userAuthModel)
            => new AppUser { Email = userAuthModel.Email, UserName = userAuthModel.Email };
    }
}
