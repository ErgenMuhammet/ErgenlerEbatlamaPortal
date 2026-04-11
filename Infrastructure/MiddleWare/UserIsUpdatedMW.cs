using Domain.Entitiy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.MiddleWare
{
    public class UserIsUpdatedMW
    {
        private readonly RequestDelegate _next;

        public UserIsUpdatedMW( RequestDelegate next)
        {
            
            _next = next;
        }

        public async Task Invoke(HttpContext _context , UserManager<AppUser> _userManager)
        {
            
            if (_context.User.Identity.IsAuthenticated == true)
            {
                bool UserUpdateInformation = bool.TryParse(_context.User.FindFirstValue("IsUpdated"), out bool IsUpdated);
                
                if (!UserUpdateInformation)
                {
                    _context.Response.Redirect($"/portal/{_context.User.FindFirstValue("Category")}");

                    return;
                }
            }

            await _next(_context);
        }
    }
}
