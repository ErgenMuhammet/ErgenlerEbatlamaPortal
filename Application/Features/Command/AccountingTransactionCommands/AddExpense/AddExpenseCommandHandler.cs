using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.AccountingTransaction.AddExpense
{
    public class AddExpenseCommandHandler : IRequestHandler<AddExpenseCommandRequest, AddExpenseCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AddExpenseCommandHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AddExpenseCommandResponse> Handle(AddExpenseCommandRequest request, CancellationToken cancellationToken)
        {
            var currentMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            var UserAccounting = await _context.ProfitLossSituation
                .FirstOrDefaultAsync(x => x.OwnerId == request.OwnerId &&
                                        x.Date.Value.Year == currentMonth.Year &&
                                              x.Date.Value.Month == currentMonth.Month, cancellationToken);
            if (UserAccounting == null)
            {
                UserAccounting = new ProfitLossSituation
                {
                    Id = Guid.NewGuid(),
                    OwnerId = request.OwnerId,
                    LastSituation = 0,
                    Date = currentMonth,
                    TotalLoss = 0,
                    TotalProfit = 0,
                };
                await _context.AddAsync(UserAccounting, cancellationToken);
            }

            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

                

                if (Owner == null)
                {
                    return new AddExpenseCommandResponse
                    {
                        IsSucces = false,
                        Message = "Kullanıcı bilgisine ulaşılamadı"
                    };
                }

                var expense = new Expense
                {
                    Id = Guid.NewGuid(),
                    Owner = Owner,
                    Amount = request.Amount,
                    Description = request.Description,
                    ExpenseDate = request.ExpenseDate,
                    OwnerId = request.OwnerId,
                    
                };

                await _context.AddAsync(expense, cancellationToken);

                UserAccounting.TotalLoss += expense.Amount;
                UserAccounting.LastSituation = UserAccounting.GetLastSituation();

                var result = await _context.SaveChangesAsync(cancellationToken);

                if (result >= 2)
                {
                    return new AddExpenseCommandResponse
                    {
                        IsSucces = true,
                        Message = "Harcama bilgisi başarıyla kaydedildi."
                    };
                }

                return new AddExpenseCommandResponse
                {
                    IsSucces = false,
                    Message = "Harcama bilgisi eklenirken bir sorun oluştu."
                };
            
        }
    }
}

