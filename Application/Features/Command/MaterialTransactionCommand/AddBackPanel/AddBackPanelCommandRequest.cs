using Domain.Entitiy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.AddBackPanel
{
    public class AddBackPanelCommandRequest : IRequest<AddBackPanelCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        public string? Brand { get; set; }
        public int Stock { get; set; }
            
        public float Thickness { get; set; }
        public string? Color { get; set; }
       

    }
}
