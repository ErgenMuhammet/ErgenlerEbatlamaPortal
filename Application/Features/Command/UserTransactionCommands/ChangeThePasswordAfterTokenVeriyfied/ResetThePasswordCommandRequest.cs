using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransactionCommands.ChangeThePasswordAfterTokenVeriyfied
{
    public class ResetThePasswordCommandRequest : IRequest <ResetThePasswordCommandResponse>
    {
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        public string Token { get; set; }

        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
