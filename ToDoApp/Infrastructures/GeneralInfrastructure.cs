using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ToDoApp.Infrastructures
{
    public static class GeneralInfrastructure
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if(httpContext.User == null)
                return string.Empty;
            return httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}