using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.AdvertisementTransactionHandlers.ConfirmAdvertisementOffer
{
    public class ConfirmAdvertisementOfferHandler : IRequestHandler<ConfirmAdvertisementOfferRequest, ConfirmAdvertisementOfferResponse>
    {
        private readonly IAppContext _context;

        public ConfirmAdvertisementOfferHandler(IAppContext context)
        {            
            _context = context;
        }
        public async Task<ConfirmAdvertisementOfferResponse> Handle(ConfirmAdvertisementOfferRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var advs = await _context.Advertisements.FirstOrDefaultAsync(x => x.AdvertisementId.ToString() == request.AdvertisementId);

            if (advs == null)
            {
                return new ConfirmAdvertisementOfferResponse
                {
                    IsSuccess = false,
                    Message = "İlgili ilan bulunamadı."
                };
            }

            if (advs.OwnerId != request.OwnerId)
            {
                return new ConfirmAdvertisementOfferResponse
                {
                    IsSuccess = false,
                    Message = "İlan sahibi olmadan ilanı yayından kaldıramazsınız."
                };
            }

            try
            {
                advs.OwnerConfirmForOffer = true;
                advs.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
                return new ConfirmAdvertisementOfferResponse
                {
                    Message = "İlan teklifi kabul edildi.İlan yayından kaldırılmıştır.",
                    IsSuccess = true,
                };

            }
            catch (Exception ex)
            {

                return new ConfirmAdvertisementOfferResponse
                {
                    IsSuccess = true,
                    Message = $"Teklif onayı verirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                };
            }
        }
    }
}
