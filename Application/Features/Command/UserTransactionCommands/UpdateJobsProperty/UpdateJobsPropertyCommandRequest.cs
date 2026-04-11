using Domain.Entitiy;
using Domain.GlobalEnum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransactionCommands.UpdateJobsProperty
{
    public class UpdateJobsPropertyCommandRequest : IRequest<UpdateJobsPropertyCommandResponse>
    {
        [JsonIgnore]
        public string UserId { get; set; }

        public string? WorkShopName { get; set; }
        public string? AdressDescription { get; set; }
        public int Experience { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
