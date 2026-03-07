using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.UpdateScrap
{
    public class UpdateScrapCommandRequest : IRequest<UpdateScrapCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        [JsonIgnore]
        public string? ScrapId { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }
        public string? Color { get; set; }
        public string? Brand { get; set; }
        public float Weight { get; set; }
        public int Stock { get; set; }
        public float Thickness { get; set; }
        public string? MaterialType { get; set; }

    }
}
