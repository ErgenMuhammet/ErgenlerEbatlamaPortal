using Application.Features.Command.MaterialTransactionCommand.AddScrap;
using Application.Interface;
using Domain.Entitiy;
using Domain.Entitiy.Material;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;


namespace Application.Features.Command.MaterialTransactionCommand.AddPvcBand
{
    public class AddPvcBandCommandHandler : IRequestHandler<AddPvcBandCommandRequest, AddPvcBandCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppContext _context;

        public AddPvcBandCommandHandler(UserManager<AppUser> userManager, IAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }

       public async Task<AddPvcBandCommandResponse> Handle(AddPvcBandCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.OwnerId);

            var UserAccounting = await _context.ProfitLossSituation
                .FirstOrDefaultAsync(x => x.OwnerId == request.OwnerId &&
                                        x.Date.Value.Year == DateTime.UtcNow.Year &&
                                        x.Date.Value.Month == DateTime.UtcNow.Month, cancellationToken);
            if (UserAccounting == null)
            {
                return new AddPvcBandCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı muhasebe bilgilerine ulaşılamadı"
                };
            }

            if (user == null)
            {
                return new AddPvcBandCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı"
                };
            }

           
            var material = await _context.
                PvcBand.
                FirstOrDefaultAsync(x => x.Brand == request.Brand &&
                x.OwnerID == request.OwnerId &&
                x.Color == request.Color &&
                x.Thickness == request.Thickness);
            try
            {

                if (request.IsPurchase == true)
                {
                    var Purchase = new Expense
                    {
                        Amount = request.Stock * request.UnitPrice ?? 0f,
                        Description = request.SaleDescription,
                        ExpenseDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day),
                        OwnerId = request.OwnerId,
                    };

                    UserAccounting.TotalLoss += Purchase.Amount;
                    UserAccounting.LastSituation = UserAccounting.GetLastSituation();

                    await _context.Expense.AddAsync(Purchase, cancellationToken);
                }

                if (material == null)
                {
                    var PvcBand = new PvcBand
                    {
                        Brand = request.Brand,
                        Color = request.Color,
                        Thickness = request.Thickness,
                        Stock = request.Stock,
                        OwnerID = request.OwnerId
                    };

                    await _context.PvcBand.AddAsync(PvcBand);
                    await _context.SaveChangesAsync(cancellationToken);

                    return new AddPvcBandCommandResponse
                    {
                        IsSucces = true,
                        Message = "Pvc band başarıyla eklendi",
                    };
                }
                else
                {
                    material.Stock += request.Stock;
                    await _context.SaveChangesAsync(cancellationToken);

                    return new AddPvcBandCommandResponse
                    {
                        IsSucces = true,
                        Message = "Pvc band stoğu başarıyla güncellendi",
                    };

                }

            }
            catch (Exception ex)
            {
                return new AddPvcBandCommandResponse
                {
                    IsSucces = false,
                    Message = $"Pvc band eklenirken bir hata ile karşılaşıldı.Hata : {ex.Message}",
                };
            }

            
        }
    }
}
