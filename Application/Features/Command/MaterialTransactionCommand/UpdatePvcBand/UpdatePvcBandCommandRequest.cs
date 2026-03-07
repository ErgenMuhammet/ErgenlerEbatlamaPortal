using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.UpdatePvcBand
{
    public class UpdatePvcBandCommandRequest : IRequest<UpdatePvcBandCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        [JsonIgnore]
        public string? PvcBandId{ get; set; }

        public string? Brand { get; set; }
        public float Thickness { get; set; }
        public string? Color { get; set; }
        public int Stock { get; set; }
    }
}
