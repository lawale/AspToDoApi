using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Errors.Validation;

namespace ToDoApp.Infrastructures
{
    public static class IdentityErrorInfrastructure
    {
        public static ValidationError GetPasswordError(this IdentityError identityError)
            => new ValidationError(identityError.Code, identityError.Description);
    }
}
