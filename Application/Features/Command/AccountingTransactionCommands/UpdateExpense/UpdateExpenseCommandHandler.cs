using Application.Features.Command.AccountingTransaction.PayTheInvoice;
using Application.Features.Query.AccountingTransactionQuery.GetAllExpense;
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

namespace Application.Features.Command.AccountingTransactionCommands.UpdateExpense
{
    public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommandRequest, UpdateExpenseCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;
        public UpdateExpenseCommandHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<UpdateExpenseCommandResponse> Handle(UpdateExpenseCommandRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);


            if (Owner == null)
            {
                return new UpdateExpenseCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı"
                };
            }

            var expense = await _context.Expense.FirstOrDefaultAsync(x => x.Id.ToString() == request.ExpenseId);

            if (expense == null)
            {
                return new UpdateExpenseCommandResponse
                {
                    IsSucces = false,
                    Message = "Gider bilgisine ulaşılamadı."
                };
            }

            if (expense.OwnerId != request.OwnerId)
            {
                return new UpdateExpenseCommandResponse
                {
                    IsSucces = false,
                    Message = "İlgili harcama mevcut kullanıcıya ait değildir."
                };
            }

            var UserAccounting = await _context.ProfitLossSituation
                .FirstOrDefaultAsync(x => x.OwnerId == request.OwnerId &&
                                        x.Date.Value.Year == expense.ExpenseDate.Value.Year &&
                                              x.Date.Value.Month == expense.ExpenseDate.Value.Month, cancellationToken);

            try
            {
                UserAccounting.TotalLoss = (UserAccounting.TotalLoss - expense.Amount) + request.Amount;
                UserAccounting.LastSituation = UserAccounting.GetLastSituation();

                expense.Description = request.Description;
                expense.Amount = request.Amount;
                
                await _context.SaveChangesAsync(cancellationToken);
                return new UpdateExpenseCommandResponse
                {
                    IsSucces = true,
                    Message = "Gider bilgisi başarıyla güncellendi."
                };
            }
            catch (Exception ex)
            {

                return new UpdateExpenseCommandResponse
                {
                    IsSucces = true,
                    Message = $"Gider bilgisi güncellenirken bir hata ile karşılaşıldı.Hata : {ex.Message}"
                };
            }
        }
    }
}
