using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ToDoApp.Models.Domain;


namespace ToDoApp.Models.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {   }
            
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<ToDo> ToDos { get; set; }
    }
}
