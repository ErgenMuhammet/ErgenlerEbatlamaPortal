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
builder.Services.AddSignalR();

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

app.UseRouting();

app.UseCors("AllowFrontend");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); //Bu kim kontrol et
app.UseAuthorization(); //Girebilir mi

app.UseMiddleware<UserIsUpdatedMW>();
app.MapHub<HubService>("/portal"); //parametre olarak hub sýnýfýný veriyoruz Chat endpointine gelindiðinde çalýþacak
app.MapControllers();

app.Run();
