using Application.Interface;
using Domain.Entitiy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

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

          
        }
        

    }
}
