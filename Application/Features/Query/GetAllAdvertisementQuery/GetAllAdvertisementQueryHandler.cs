using Application.DTOs;
using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetAllAdvertisementQuery
{
    public class GetAllAdvertisementQueryHandler : IRequestHandler<GetAllAdvertisementQueryRequest, GetAllAdvertisementQueryResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GetAllAdvertisementQueryHandler(IAppContext context , UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<GetAllAdvertisementQueryResponse> Handle(GetAllAdvertisementQueryRequest request, CancellationToken cancellationToken)
        {
            var User = await _userManager.FindByIdAsync(request.UserId);
            var advs = new List<AdvertisementDto>();

            try
            {
                advs = await _context.Advertisements.Where(z => (z.TargetCategory & User.UserCategory) != 0 && z.IsActive == true).
                Select(x => new AdvertisementDto
                {
                    Id = x.AdvertisementId.ToString(),
                    AdvertisementAddress = x.AdvertisementAddress,
                    AdvertisementDate = x.AdvertisementDate,
                    ImgUrl = x.ImgUrl,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    OwnerId = x.OwnerId,
                    Title = x.Title,
                }).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return new GetAllAdvertisementQueryResponse
                {
                    Advs = null,
                    Message = $"İlanlar listelenirken bir hata oluştu. Hata : {ex.Message}.",
                    IsSucces = false
                };   
            }
             
            if (!advs.Any())
            {
                return new GetAllAdvertisementQueryResponse
                {
                    Advs = advs,
                    IsSucces = true,
                    Message = "Görüntülenecek ilan bulunmamaktadır."
                };
            }

            return new GetAllAdvertisementQueryResponse
            {
                IsSucces = true,
                Advs = advs,
                Message = "İlanlar başarıyla getirildi."
            };

        }
    }
}
