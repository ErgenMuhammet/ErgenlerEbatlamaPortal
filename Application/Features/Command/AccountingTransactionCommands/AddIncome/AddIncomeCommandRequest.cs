using Domain.Entitiy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.AccountingTransaction.AddIncome
{
    public class AddIncomeCommandRequest : IRequest<AddIncomeCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }             
        public float Amount { get; set; }
        public string? Description { get; set; }
    }
}
