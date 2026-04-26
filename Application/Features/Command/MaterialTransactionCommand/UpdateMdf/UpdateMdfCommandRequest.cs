using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.UpdateMdf
{
    public class UpdateMdfCommandRequest : IRequest<UpdateMdfCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        [JsonIgnore]
        public string? MdfId{ get; set; }

        public float Thickness { get; set; }
        public string? Color { get; set; }
        public string? Brand { get; set; }
        public int Stock { get; set; }
        public int Weight { get; set; }
     
    }
}
