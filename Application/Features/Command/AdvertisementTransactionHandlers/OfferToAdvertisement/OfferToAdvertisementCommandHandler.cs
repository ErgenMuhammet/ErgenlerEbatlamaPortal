using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.AdvertisementTransactionHandlers.OfferToAdvertisement
{
    public class OfferToAdvertisementCommandHandler : IRequestHandler<OfferToAdvertisementCommandRequest, OfferToAdvertisementCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly ISignalR _signalR;

        public OfferToAdvertisementCommandHandler(IAppContext context , ISignalR signalR)
        {
            _context = context;
            _signalR = signalR;
        }

        public async Task<OfferToAdvertisementCommandResponse> Handle(OfferToAdvertisementCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            


            var advs = await _context.Advertisements.FirstOrDefaultAsync(x => x.AdvertisementId.ToString() == request.AdvertisementId);

            if (advs == null)
            {
                return new OfferToAdvertisementCommandResponse
                {
                    IsSucces = false,
                    Message = "İlgili ilan bulunamadı."
                };
            }

            advs.Bidder = request.OwnerId;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return new OfferToAdvertisementCommandResponse
                {
                    IsSucces = false,
                    Message = $"İlgili ilan teklif verilirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                }; 
            }

            await _signalR.SendNotification(advs.OwnerId, $"{advs.Title} isimli ilanınıza bir teklif ");

            return new OfferToAdvertisementCommandResponse
            {
                IsSucces = true,
                Message = "Teklif başarıyla verildi",
                
            };
        }
    }
}
