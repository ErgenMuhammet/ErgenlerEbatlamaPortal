using Domain.Entitiy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.AddGlue
{
    public class AddGlueCommandRequest : IRequest<AddGlueCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        public float Weight { get; set; }
        public string? Brand { get; set; }
        public int Stock { get; set; }

        public float Cost { get; set; }
        public float Price { get; set; }
    }
}
