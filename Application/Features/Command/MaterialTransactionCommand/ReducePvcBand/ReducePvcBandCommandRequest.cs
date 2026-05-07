using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.ReducePvcBand
{
    public class ReducePvcBandCommandRequest : IRequest <ReducePvcBandCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        [JsonIgnore]
        public string? PvcBandId { get; set; }

        public bool? IsSale { get; set; } = false;
        public string? SaleDescription { get; set; }

        public float? UnitPrice { get; set; }


        public int Count { get; set; }
    }
}
