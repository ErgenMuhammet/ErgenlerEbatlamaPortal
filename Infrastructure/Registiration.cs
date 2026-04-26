using Application.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Resend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public static class Registiration
    {
        public static void AddInfrastructure (this IServiceCollection services, IConfiguration configuration)
        {          
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileReader, FileReaderService>();
            services.AddScoped<ISignalR, SignalRService>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,            // Tokenı dağıtan sunucunun (Issuer) doğrulanıp doğrulanmayacağını belirler.
                    ValidateAudience = true,          // Tokenın hangi hedef kitle/uygulama (Audience) için üretildiğinin kontrol edilmesini sağlar.
                    ValidateLifetime = true,          // Tokenın süresinin (Expiration) dolup dolmadığını kontrol eder; süresi geçmiş tokenları reddeder.
                    ValidateIssuerSigningKey = true,  // Tokenın imzalanırken kullanılan anahtarın (Security Key), sunucudaki anahtarla eşleşip eşleşmediğine bakar.

                    ValidIssuer = configuration["Jwt:Issuer"],     // Sistemin kabul edeceği geçerli yayıncı adını (Örn: "marangoz-api.com") konfigürasyondan okur.
                    ValidAudience = configuration["Jwt:Audience"], // Sistemin kabul edeceği geçerli hedef kitleyi (Örn: "marangoz-app") konfigürasyondan okur.

                    // Tokenı imzalamak ve imzayı çözmek için kullanılacak olan gizli anahtarı (Secret Key) belirler.
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty)),

                    // Token süresi bittiğinde, sistemin tanıdığı ek tolerans süresidir. 
                    // TimeSpan.Zero diyerek, süre biter bitmez (saniyesinde) tokenın geçersiz sayılmasını sağlıyoruz.
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddOptions();
            services.AddHttpClient<ResendClient>();
            services.Configure<ResendClientOptions>(o =>
            {
                o.ApiToken = configuration["ResendEmail:ApiKey"];
            });
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IResend, ResendClient>(); // 

            services.AddAuthorization();

        }
    }
}
