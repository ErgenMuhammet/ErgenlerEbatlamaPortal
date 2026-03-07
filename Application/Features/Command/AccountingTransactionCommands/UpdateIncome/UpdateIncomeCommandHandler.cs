using Application.Features.Command.AccountingTransactionCommands.UpdateExpense;
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

namespace Application.Features.Command.AccountingTransactionCommands.UpdateIncome
{
    public class UpdateIncomeCommandHandler : IRequestHandler<UpdateIncomeCommandRequest, UpdateIncomeCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;
        public UpdateIncomeCommandHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<UpdateIncomeCommandResponse> Handle(UpdateIncomeCommandRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new UpdateIncomeCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı"
                };
            }

            var income = await _context.Incomes.FirstOrDefaultAsync(x => x.Id.ToString() == request.IncomeId);

            if (income == null)
            {
                return new UpdateIncomeCommandResponse
                {
                    IsSucces = false,
                    Message = "Gelir bilgisine ulaşılamadı."
                };
            }

            if (income.OwnerId != request.OwnerId)
            {
                return new UpdateIncomeCommandResponse
                {
                    IsSucces = false,
                    Message = "İlgili gelir mevcut kullanıcıya ait değildir."
                };
            }

            var UserAccounting = await _context.ProfitLossSituation
                .FirstOrDefaultAsync(x => x.OwnerId == request.OwnerId &&
                                        x.Date.Value.Year == income.IncomeDate.Value.Year &&
                                              x.Date.Value.Month == income.IncomeDate.Value.Month, cancellationToken);

            try
            {
                UserAccounting.TotalLoss = (UserAccounting.TotalProfit - income.Amount) + request.Amount;
                UserAccounting.LastSituation = UserAccounting.GetLastSituation();

                income.Description = request.Description;
                income.Amount = request.Amount;

                await _context.SaveChangesAsync(cancellationToken);
                return new UpdateIncomeCommandResponse
                {
                    IsSucces = true,
                    Message = "Gelir bilgisi başarıyla güncellendi."
                };
            }
            catch (Exception ex)
            {

                return new UpdateIncomeCommandResponse
                {
                    IsSucces = true,
                    Message = $"Gelir bilgisi güncellenirken bir hata ile karşılaşıldı.Hata : {ex.Message}"
                };
            }
        }
    }
}
