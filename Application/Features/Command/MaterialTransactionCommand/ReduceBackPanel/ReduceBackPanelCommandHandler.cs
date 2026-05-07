using Application.Features.Command.MaterialTransactionCommand.AddBackPanel;
using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Command.MaterialTransactionCommand.ReduceBackPanel
{
    public class ReduceBackPanelCommandHandler : IRequestHandler<ReduceBackPanelCommandRequest, ReduceBackPanelCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly ISignalR _signalR;
        public ReduceBackPanelCommandHandler(IAppContext context, ISignalR signalR)
        {
            _context = context;
            _signalR = signalR;
        }

        public async Task<ReduceBackPanelCommandResponse> Handle(ReduceBackPanelCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var Backpanel = await _context.BackPanel.
                FirstOrDefaultAsync(x => x.OwnerID.ToString() == request.OwnerId && x.Id.ToString() == request.BackPanelId);

            var UserAccounting = await _context.ProfitLossSituation
           .FirstOrDefaultAsync(x => x.OwnerId == request.OwnerId &&
                                 x.Date.Value.Year == DateTime.UtcNow.Year &&
                                 x.Date.Value.Month == DateTime.UtcNow.Month, cancellationToken);

            if (UserAccounting == null)
            {
                return new ReduceBackPanelCommandResponse
                {
                    IsSuccess = false,
                    Message = "Kullanıcı muhasebe bilgilerine ulaşılamadı"
                };
            }


            if (Backpanel == null)
            {
                return new ReduceBackPanelCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili ürün bulunamadı."
                };
            }

            if (Backpanel.Stock == 0)
            {
                return new ReduceBackPanelCommandResponse
                {
                    IsSuccess = true,
                    Message = "İlgili kullanıcıya ait arkalık stoğu bulunmamaktadır."
                };
            }

            if (request.Count > Backpanel.Stock)
            {
                return new ReduceBackPanelCommandResponse
                {
                    IsSuccess = true,
                    Message = "Stoğunuzdan fazla ürün kullanamazsınız."
                };
            }

            Backpanel.Stock -= request.Count;

            if (request.IsSale == true)
            {
                var Sale = new Income
                {
                    Amount = request.Count * request.UnitPrice ?? 0f,
                    Description = request.SaleDescription,
                    IncomeDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day),
                    OwnerId = request.OwnerId,
                };
                await _context.Incomes.AddAsync(Sale,cancellationToken);
                UserAccounting.TotalProfit += Sale.Amount;
                UserAccounting.LastSituation =  UserAccounting.GetLastSituation();

                await _context.Incomes.AddAsync(Sale, cancellationToken);
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new ReduceBackPanelCommandResponse
                {
                    IsSuccess = true,
                    Message = $"Stoğunuzdan stok güncellenirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                }; ;
            }
            if (Backpanel.Stock <= 10)
            {
                await _signalR.SendNotification(request.OwnerId!, $"Arkalık stoğu azaldı. Yeni Arkalık Stoğu : {Backpanel.Stock}");

            }
            await _signalR.SendNotification(request.OwnerId!, $"Yeni Arkalık Stoğu : {Backpanel.Stock}");

            return new ReduceBackPanelCommandResponse
            {
                IsSuccess = true,
                Message = "Arkalık stoğu güncellendi"
            };

        }
        
    }
}
