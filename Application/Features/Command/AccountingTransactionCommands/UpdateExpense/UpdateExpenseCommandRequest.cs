using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.AccountingTransactionCommands.UpdateExpense
{
    public class UpdateExpenseCommandRequest : IRequest<UpdateExpenseCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        [JsonIgnore]
        public string? ExpenseId { get; set; }
        public float Amount { get; set; }
        public string? Description { get; set; }
    }
}
