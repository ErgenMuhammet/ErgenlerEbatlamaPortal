using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.ReduceBackPanel
{
    public class ReduceBackPanelCommandRequest : IRequest<ReduceBackPanelCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        public string Color { get; set; }
        public float Thickness { get; set; }
        public string Brand { get; set; }

        public int Count { get; set; } 
    }
}
