
using System.Reflection.Emit;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ToDoApp.Controllers;
using ToDoApp.Errors.Validation;
using ToDoApp.Models.Domain;
using ToDoApp.Services;
using ToDoApp.Settings;
using ToDoApp.Models.DataContext;

namespace ToDoApp
{
    public class StartupProduction : Startup
    {
        public StartupProduction(IConfiguration configuration)
        :base(configuration)
        {
            Console.WriteLine("Startup Prod");
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }
    }


    public class StartupDevelopment : Startup
    {
        public StartupDevelopment(IConfiguration configuration)
        :base(configuration)
        {
            Console.WriteLine("Startup dev");
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }
    }

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public virtual void ConfigureServices(IServiceCollection services)
        {
            
            var jwtSettings = new JwtSettings();
            Configuration.Bind(nameof(jwtSettings), jwtSettings);
            
            services.AddEntityFrameworkNpgsql()
            .AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseNpgsql(
                        Configuration["Data:ToDoApp:ConnectionString"]);
                    })
                    .BuildServiceProvider();

            services.AddTransient<IToDoRepository, ToDoRepository>();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidatorActionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton(jwtSettings);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddIdentity<AppUser, IdentityRole>(option => option.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }

        
    }
}