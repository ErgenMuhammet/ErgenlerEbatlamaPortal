using Application.Interface;
using Domain.Entitiy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MiddleWare
{
    public class RefreshTokenMw
    {
        private readonly RequestDelegate _next;
        private readonly UserManager<AppUser> _userManager;
        public RefreshTokenMw(RequestDelegate next, UserManager<AppUser> userManager)
        {
            _next = next;
            _userManager = userManager; 
        }

        public async Task Invoke(HttpContext context, ITokenService tokenService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var userId = tokenService.ValidateTokenAndGetUserId(token);

                if (userId != null)
                { 
                    var user = await _userManager.FindByIdAsync(userId);

                    if (user != null) 
                    {

                        var newToken = tokenService.CreateToken(user);

                        context.Response.Headers.Add("New-Token", newToken.ToString());
                        context.Response.Headers.Add("Access-Control-Expose-Headers", "New-Token");
                    }
                }
            }

            await _next(context);
        }
    }
}
