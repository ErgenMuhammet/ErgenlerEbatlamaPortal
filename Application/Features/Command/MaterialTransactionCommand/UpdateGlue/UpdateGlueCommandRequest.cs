using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.UpdateGlue
{
    public class UpdateGlueCommandRequest : IRequest<UpdateGlueCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        [JsonIgnore]
        public string? GlueId{ get; set; }
        public string? Brand { get; set; }
        public float Weight { get; set; }
        public int Stock { get; set; }
    }
}
