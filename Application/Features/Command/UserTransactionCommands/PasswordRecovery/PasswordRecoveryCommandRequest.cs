using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.PasswordRecovery
{
    public class PasswordRecoveryCommandRequest : IRequest<PasswordRecoveryCommandResponse>
    {
        public string? Email { get; set; }
       

    }
}
