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

namespace Application.Features.Command.AccountingTransaction.AddIncome
{
    public class AddIncomeCommandHandler : IRequestHandler<AddIncomeCommandRequest, AddIncomeCommandResponse>
    {
        private readonly IAppContext _appContext;
        private readonly UserManager<AppUser> _userManager;

        public AddIncomeCommandHandler(IAppContext appContext, UserManager<AppUser> userManager)
        {
            _appContext = appContext;
            _userManager = userManager;          
        }

        public async Task<AddIncomeCommandResponse> Handle(AddIncomeCommandRequest request, CancellationToken cancellationToken)
        {
            var currentMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            var UserAccounting = await _appContext.ProfitLossSituation
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
                await _appContext.AddAsync(UserAccounting, cancellationToken);
            }
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);
            if (Owner == null)
            {
                return new AddIncomeCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı"
                };
            }

            var Income = new Income
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Description = request.Description,
                IncomeDate = DateTime.Now.Date,
                OwnerId = Owner.Id,
            };
           
            try
            {
                await _appContext.Incomes.AddAsync(Income, cancellationToken);

                UserAccounting.TotalProfit += request.Amount;
                UserAccounting.LastSituation = UserAccounting.GetLastSituation();

                var result = await _appContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new AddIncomeCommandResponse
                {
                    IsSucces = false,
                    Message = $"Gelir bilgisi eklenirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                }; 
            }

            return new AddIncomeCommandResponse
            {
                IsSucces = true,
                Message = "Gelir bilgisi başarıyla eklendi"
            };
        }
    }
}
