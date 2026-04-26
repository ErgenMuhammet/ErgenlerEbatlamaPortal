using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.UpdateUserDefaultProperty
{
    public class UpdateUserDefaultPropertyCommandRequest : IRequest<UpdateUserDefaultPropertyCommandResponse>
    {
        [JsonIgnore]
        public string Id { get; set; }

        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }        
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
