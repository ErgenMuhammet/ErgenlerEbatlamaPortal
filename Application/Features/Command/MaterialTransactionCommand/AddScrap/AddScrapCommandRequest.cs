using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.AddScrap
{
    public class AddScrapCommandRequest : IRequest<AddScrapCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }
        public float Thickness { get; set; }
        public string? Color { get; set; }
        public float Weight { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }       
        public string? Brand { get; set; }
        public string? MaterialType { get; set; }

    }
}
