using MediatR;
using System.Text.Json.Serialization;


namespace Application.Features.Command.OrderTransactionCommands.DeleteOrder
{
    public class DeleteOrderCommandRequest : IRequest<DeleteOrderCommandResponse>
    {
        [JsonIgnore]
        public string? OrderId { get; set; }

        [JsonIgnore]
        public string? OwnerId { get; set; }
    }

}
