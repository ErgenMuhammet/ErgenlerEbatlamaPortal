using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.ConfirmEmail
{
    public class ConfirmEmailCommandRequest : IRequest<ConfirmEmailCommandResponse>
    {
        public string? UserId { get; set; }
        public string? Token { get; set; }
        
    }
}
