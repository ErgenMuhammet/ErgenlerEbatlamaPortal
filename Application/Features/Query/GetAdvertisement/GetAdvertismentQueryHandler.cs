using Application.DTOs;
using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetAdvertisement
{
    public class GetAdvertismentQueryHandler : IRequestHandler<GetAdvertisementQueryRequest, GetAdvertisementQueryResponse>
    {
        private readonly IAppContext _context;

        public GetAdvertismentQueryHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<GetAdvertisementQueryResponse> Handle(GetAdvertisementQueryRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var Advs = await _context.Advertisements.FirstOrDefaultAsync(x => x.AdvertisementId.ToString() == request.AdvertismentId);

            if (Advs == null)
            {
                return new GetAdvertisementQueryResponse
                {
                    Advs = null,
                    IsSucces = false,
                    Message = "İlgili ilan bulunamadı"
                };
            }

            var DTO = new AdvertisementDto
            {
                AdvertisementAddress = Advs.AdvertisementAddress,
                AdvertisementDate = Advs.AdvertisementDate,
                ImgUrl = Advs.ImgUrl,
                Latitude = Advs.Latitude,
                Longitude = Advs.Longitude,
                Title = Advs.Title,
                OwnerId = Advs.OwnerId,
            };

            return new GetAdvertisementQueryResponse
            {
                Advs = DTO,
                IsSucces = true,
                Message = "İlan bilgileri başarıyla getirildi"
            };
        }
    }
}
