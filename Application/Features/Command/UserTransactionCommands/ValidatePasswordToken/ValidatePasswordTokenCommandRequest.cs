using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.ValidatePasswordToken
{
    public class ValidatePasswordTokenCommandRequest : IRequest<ValidatePasswordTokenCommandResponse>
    {
        [JsonIgnore]
        public string? userId{ get; set; }

        [JsonIgnore]
        public string? passwordToken { get; set; }
        public string? NewPassword { get; set; }
    }
}
