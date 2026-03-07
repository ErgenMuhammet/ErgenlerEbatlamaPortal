using Domain.Entitiy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.AccountingTransaction.AddExpense
{
    public class AddExpenseCommandRequest : IRequest <AddExpenseCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; } 

        public DateTime? ExpenseDate { get; set; }             
        public float Amount { get; set; }
        public string? Description { get; set; }
    }
}
