using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.CarpenterUpdate
{
    public class CarpenterUpdateCommandRequest : IRequest<CarpenterUpdateCommandResponse>
    {
        [JsonIgnore]
        public string? UserIdentifier { get; set; }

        public string? WorkShopName { get; set; }
        public string? AdressDescription { get; set; }
        public int Experience { get; set; }
        
        

    }
}
