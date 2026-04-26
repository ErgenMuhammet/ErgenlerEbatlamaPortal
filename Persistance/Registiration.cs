using Application.Interface;
using Domain.Entitiy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class Registiration
    {
        public static void AddPersistenceService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("database")));

            services.AddIdentityCore<AppUser>(options => {
               
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>() 
                .AddDefaultTokenProviders(); 

            services.AddScoped<IAppContext>(x => x.GetRequiredService<IdentityContext>());

            var modelBuilder = new ModelBuilder();


        }
        

    }
}
