using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.ReduceGlue
{
    public class ReduceGlueCommandRequest : IRequest<ReduceGlueCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerID { get; set; }
        public int Weight { get; set; }
        public string Brand { get; set; }

        public int Count { get; set; }
    }
}
