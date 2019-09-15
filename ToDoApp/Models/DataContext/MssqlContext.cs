// using System.Runtime.CompilerServices;
// using Microsoft.Extensions.Configuration;
// using Microsoft.EntityFrameworkCore;

// namespace ToDoApp.Models.DataContext
// {
//     public class MssqlContext : ApplicationDbContext
//     {
//         IConfiguration configuration;
//         // public MssqlContext(DbContextOptions<MssqlContext> options, IConfiguration configuration)
//         // :base(options as DbContextOptions<ApplicationDbContext>)
//         // {
//         //     this.configuration = configuration;
//         // }

//         public MssqlContext(IConfiguration configuration)
//         {
//             this.configuration = configuration;
//         }

//         protected override void OnConfiguring(DbContextOptionsBuilder options)
//             => options.UseSqlServer(configuration["Data:ToDoApp:ConnectionString"]);   
//     }
// }