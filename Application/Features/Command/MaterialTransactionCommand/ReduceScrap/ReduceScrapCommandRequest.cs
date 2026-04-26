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


        [JsonIgnore]
        public string? ScrapsId { get; set; }

        public int Count { get; set; }
    }
}
