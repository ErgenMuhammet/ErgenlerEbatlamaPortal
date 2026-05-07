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

namespace Application.Features.Command.AdvertisementTransactionHandlers.OfferToAdvertisement
{
    public class OfferToAdvertisementCommandHandler : IRequestHandler<OfferToAdvertisementCommandRequest, OfferToAdvertisementCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly ISignalR _signalR;
        private readonly UserManager<AppUser> _userManager;

        public OfferToAdvertisementCommandHandler(IAppContext context , ISignalR signalR, UserManager<AppUser> userManager)
        {
            _context = context;
            _signalR = signalR;
            _userManager = userManager;
        }

        public async Task<OfferToAdvertisementCommandResponse> Handle(OfferToAdvertisementCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = await _userManager.FindByIdAsync(request.OwnerId);

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

            await _signalR.SendNotification(advs.OwnerId, $"{advs.Title} isimli ilanınıza bir teklif var");
            await _signalR.SendMessage(advs.OwnerId, "İlan ile ilgili bilgi almak istiyorum.");
            var Message = new ChatMessage
            {
                Content = "İlan ile ilgili bilgi almak istiyorum.",
                ReceiverId = advs.OwnerId,
                SendAt = DateTime.Now,
                SenderId = request.OwnerId,
            };

            await _context.Messages.AddAsync(Message);

            var notification = new Notification
            {
                Content = $"{advs.Title} isimli ilanınıza {user.FullName} isimli kişiden teklif var mesaj kutunuzu kontrol ediniz.",
                OwnerId = advs.OwnerId,
            };

            await _context.notifications.AddAsync(notification);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Mesaj ve bildirimler veri tabanına kaydedilirken bir hata ile karşılaşıldı. Hata : {ex.Message}");
            }

            return new OfferToAdvertisementCommandResponse
            {
                IsSucces = true,
                Message = "Teklif başarıyla verildi",
                
            };
        }
    }
}
