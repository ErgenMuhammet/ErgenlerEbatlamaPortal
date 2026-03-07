using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.AssemblerUpdate
{
    public class AssemblerUpdateCommandRequest : IRequest<AssemblerUpdateCommandResponse>
    {

        public string? UserId { get; set; }
        public int Experience { get; set; }

        [JsonIgnore]
        public string? UserIdentifier { get; set; }
    }
}
