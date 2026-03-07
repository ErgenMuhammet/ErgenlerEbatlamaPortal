using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.Login
{
    public class LoginQueryRequest  : IRequest<LoginQueryResponse>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
}
