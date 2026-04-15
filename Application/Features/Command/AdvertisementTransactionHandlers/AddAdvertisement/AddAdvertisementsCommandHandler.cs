using Application.Interface;
using Domain.Entitiy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.AdvertisementTransactionHandlers.AddAdvertisement
{
    internal class AddAdvertisementsCommandHandler : IRequestHandler<AddAdvertisementsCommandRequest, AddAdvertisementsCommandResponse>
    {
        private readonly IAppContext _context;

        public AddAdvertisementsCommandHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<AddAdvertisementsCommandResponse> Handle(AddAdvertisementsCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request)); // request boş diyoruz 
            }

            var Advs = new Advertisements
            {
                AdvertisementAddress = request.AdvertisementAddress,
                ImgUrl = request.ImgUrl,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                OwnerId = request.OwnerId,
                Title = request.Title,
            };

            try
            {
                await _context.Advertisements.AddAsync(Advs,cancellationToken);
            }
            catch (Exception ex)
            {

                return new AddAdvertisementsCommandResponse
                {
                    IsSucces = false,
                    Message = $"İlan eklenirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                };
            }
            return new AddAdvertisementsCommandResponse
            {
                IsSucces = true,
                Message = "İlan başarıyla eklendi."
            };

        }
    }
}
