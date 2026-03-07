using Domain.Entitiy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.OrderTransactionCommands.AddOrder
{
    public class AddOrderCommandRequest : IRequest<AddOrderCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }
        public string? OrderName { get; set; }
        public int CountOfMdf { get; set; }
        public int CountOfBackPanel { get; set; }
        public float MetreOfPvcBand { get; set; }
        public float CostOfOrder { get; set; }
       
        
        
    }
}
