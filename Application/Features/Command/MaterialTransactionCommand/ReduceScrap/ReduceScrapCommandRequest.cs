using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.ReduceScrap
{
    public class ReduceScrapCommandRequest : IRequest<ReduceScrapCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        public string Color { get; set; }
        public float Thickness { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public int Count { get; set; }
    }
}
