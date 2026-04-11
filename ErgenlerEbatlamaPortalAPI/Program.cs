using Application;
using Application.Interface;
using Application.Registiration;
using Domain.Entitiy;
using Infrastructure;
using Infrastructure.MiddleWare;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Portal API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        
        Name = "Authorization",
        In = ParameterLocation.Header,

        
        Type = SecuritySchemeType.Http, 
        Scheme = "bearer",             
        BearerFormat = "JWT",          

        Description = "Sadece Token deðerini yapýþtýrýn. (Baþýna Bearer eklemenize gerek yoktur)"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope()) 
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<IAppContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var serviceProvider = services.GetRequiredService<IServiceProvider>();

        var AppUserSeedData = new Persistence.Context.AppUserSeedData(userManager, roleManager);
        await AppUserSeedData.InitializeAsync(serviceProvider);
      

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Seed Data yüklenirken bir hata oluþtu!");
    }
}

app.UseCors("AllowFrontend"); 

app.UseAuthentication(); //Bu kim kontrol et
app.UseAuthorization(); //Girebilir mi

app.UseMiddleware<UserIsUpdatedMW>();

app.MapControllers();

app.Run();
