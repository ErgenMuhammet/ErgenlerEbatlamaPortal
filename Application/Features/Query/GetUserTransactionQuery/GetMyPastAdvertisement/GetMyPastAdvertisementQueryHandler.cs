using Application.DTOs;
using Application.Features.Query.GetAllAdvertisementQuery;
using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.GetMyPastAdvertisement
{
    public class GetMyPastAdvertisementQueryHandler : IRequestHandler<GetMyPastAdvertisementQueryRequest, GetMyPastAdvertisementQueryResponse>
    {
        private readonly IAppContext _context;

        public GetMyPastAdvertisementQueryHandler(IAppContext context)
        {
            _context = context;
        }
        public async Task<GetMyPastAdvertisementQueryResponse> Handle(GetMyPastAdvertisementQueryRequest request, CancellationToken cancellationToken)
        {
            List<AdvertisementDto> advs = new List<AdvertisementDto>();

            try
            {
                advs = await _context.Advertisements.Where(x => x.OwnerId.ToString() == request.OwnerId).
                    Select(z => new AdvertisementDto
                    {
                        Id = z.AdvertisementId.ToString(),
                        OwnerId = z.OwnerId,
                        AdvertisementAddress = z.AdvertisementAddress,
                        AdvertisementDate = z.AdvertisementDate,
                        ImgUrl = z.ImgUrl,
                        Latitude = z.Latitude,
                        Longitude = z.Longitude,
                        Title = z.Title,
                        IsActive = z.IsActive
                    }).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return new GetMyPastAdvertisementQueryResponse
                {
                    Advs = null,
                    Message = $" Geçmiş ilanlar listelenirken bir hata oluştu. Hata : {ex.Message}.",
                    IsSucces = false
                };
            }

            if (!advs.Any())
            {
                return new GetMyPastAdvertisementQueryResponse
                {
                    Advs = advs,
                    IsSucces = true,
                    Message = "Görüntülenecek ilan bulunmamaktadır."
                };
            }

            return new GetMyPastAdvertisementQueryResponse
            {
                IsSucces = true,
                Advs = advs,
                Message = "Geçmiş ilanlar başarıyla getirildi."
            };
        }
    }
}
