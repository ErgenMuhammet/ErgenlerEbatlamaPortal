using Application.Features.Command.MaterialTransactionCommand.AddScrap;
using Application.Interface;
using Domain.Entitiy;
using Domain.Entitiy.Material;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


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
                if (material == null)
                {
                    var PvcBand = new PvcBand
                    {
                        Brand = request.Brand,
                        Color = request.Color,
                        Thickness = request.Thickness,
                        Length = request.Length,
                        Stock = request.Stock,
                        Price = request.Price,
                        Cost = request.Cost,
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
                    material.Cost = request.Cost;
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
