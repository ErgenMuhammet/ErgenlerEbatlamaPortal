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

namespace Application.Features.Command.AccountingTransaction.PayTheInvoice
{
    public class PayTheInvoiceCommandHandler : IRequestHandler<PayTheInvoiceCommanRequest, PayTheInvoiceCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;
        public PayTheInvoiceCommandHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<PayTheInvoiceCommandResponse> Handle(PayTheInvoiceCommanRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);
            if (Owner == null)
            {
                return new PayTheInvoiceCommandResponse
                {
                    IsSuccess = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı"
                };
            }
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

            var Invoice = await _context.Invoice.Where(x => x.Id.ToString() == request.InvoiceId).FirstOrDefaultAsync();

            if (Invoice == null) 
            {
                return new PayTheInvoiceCommandResponse
                {
                    IsSuccess = false,
                    Message = "Fatura bilgisine ulaşılamadı"
                };
            }

            if (Invoice.OwnerId != request.OwnerId)
            {
                return new PayTheInvoiceCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili fatura mevcut kullanıcıya ait değidlir"
                };
            }

            try
            {
                Invoice.BeenPaid = true;

                UserAccounting.TotalLoss += Invoice.Cost;
                UserAccounting.LastSituation = UserAccounting.GetLastSituation();

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return new PayTheInvoiceCommandResponse
                {
                    IsSuccess = false,
                    Message = $"İşlem yapılırken bir hata ile karşılaşıldı.Hata : {ex.Message}"
                };
            }

            return new PayTheInvoiceCommandResponse
            {
                IsSuccess = true,
                Message = "İşlem başarıyla tamamlandı."
            };

        }
    }
}
