using Domain.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.Login
{
    public class LoginQueryResponse
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
        public string? UserId { get; set; }
        public Category Job { get; set; }
    }
}
