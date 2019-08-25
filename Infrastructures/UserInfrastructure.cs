﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Models.Dto;

namespace ToDoApp.Infrastructures
{
    public static class UserInfrastructure
    {
        public static UserBaseModel GetUserModel(this AppUser user)
            => new UserBaseModel { Email = user.Email, Name = user.UserName, PhoneNumber = user.PhoneNumber };

        public static AppUser GetAppUser(this UserModel userModel)
            => new AppUser { Email = userModel.Email, UserName = userModel.Name };

        public static AppUser GetAppUser(this UserAuthModel userAuthModel)
            => new AppUser { Email = userAuthModel.Email };
    }
}
