using Domain.Entitiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ITokenService
    {
       public Task<string> CreateToken(AppUser user);
       public string ValidateTokenAndGetUserId(string Token);
       
        
    }
}
