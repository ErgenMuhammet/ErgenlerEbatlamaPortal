using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.ReduceMdf
{
    public class ReduceMdfCommandRequest : IRequest<ReduceMdfCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerID { get; set; }

        [JsonIgnore]
        public string? MdfId { get; set; }

        public int Count { get; set; }
    }
}
