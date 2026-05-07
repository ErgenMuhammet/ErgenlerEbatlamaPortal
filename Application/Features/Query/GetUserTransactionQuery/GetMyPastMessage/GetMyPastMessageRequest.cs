using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.GetMyPastMessage
{
    public class GetMyPastMessageRequest : IRequest<GetMyPastMessageResponse>
    {
        [JsonIgnore]
        public string OwnerId { get; set; }

        [JsonIgnore]    
        public string TargetId { get; set; }
    }
}
