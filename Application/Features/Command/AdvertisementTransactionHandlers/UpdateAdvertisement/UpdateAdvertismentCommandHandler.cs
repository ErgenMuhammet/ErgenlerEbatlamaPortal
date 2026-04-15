using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.AdvertisementTransactionHandlers.UpdateAdvertisement
{
    public class UpdateAdvertismentCommandHandler : IRequestHandler<UpdateAdvertisementCommandRequest, UpdateAdvertisementCommandResponse>
    {
        private readonly IAppContext _context;
        public UpdateAdvertismentCommandHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<UpdateAdvertisementCommandResponse> Handle(UpdateAdvertisementCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null )
            {
                throw new ArgumentNullException(nameof(request));
            }

            var Advs = await _context.Advertisements.FirstOrDefaultAsync(x=> x.AdvertisementId.ToString() == request.AdvertisementId);

            if (Advs == null)
            {
                return new UpdateAdvertisementCommandResponse
                {
                    IsSucces = false,
                    Message = "İlgili ilan bulunamadı."
                };
            }
            var IsOwnerIsValid = await _context.AppUsers.AnyAsync(x => x.Id.ToString() == request.OwnerId) &&
                request.OwnerId == Advs.OwnerId.ToString();

            if (!IsOwnerIsValid)
            {
                return new UpdateAdvertisementCommandResponse
                {
                    IsSucces = false,
                    Message = "İlgili ilan mevcut kullanıcıya ait değildir."
                };
            }

            Advs.AdvertisementAddress = request.AdvertisementAddress;
            Advs.ImgUrl = request.ImgUrl;
            Advs.Latitude = request.Latitude;
            Advs.Longitude = request.Longitude;
            Advs.Title = request.Title;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new UpdateAdvertisementCommandResponse
                {
                    IsSucces = false,
                    Message = $"İlgili ilan güncellenirken bir hata ile karşılaşıldı. Hata : {ex.Message}."
                };
            }

            return new UpdateAdvertisementCommandResponse
            {
                IsSucces = true,
                Message = "İlan başarıyla güncellendi."
            };

         }
    }
}
