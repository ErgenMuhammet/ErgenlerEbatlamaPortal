using Application.DTOs;
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

namespace Application.Features.Query.GetTheBiddersAdvertisements
{
    public class GetTheBiddersAdvertisementHandler : IRequestHandler<GetTheBiddersAdvertisementRequest, GetTheBiddersAdvertisementResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GetTheBiddersAdvertisementHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<GetTheBiddersAdvertisementResponse> Handle(GetTheBiddersAdvertisementRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
          
            var advs = await _context.Advertisements.
                Where(x => x.OwnerId == request.OwnerId && x.Bidder == request.BiddersId).
                Select(x => new AdvertisementDto
            {
                Id = x.AdvertisementId.ToString(),
                AdvertisementAddress = x.AdvertisementAddress,
                AdvertisementDate = x.AdvertisementDate,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                OwnerName = _context.AppUsers.Where(a => a.Id.ToString() == x.OwnerId).Select(b => b.FullName).FirstOrDefault(),
                Title = x.Title,
                OwnerId = x.OwnerId,
            }).ToListAsync();

            if (advs.Count == 0 || advs is null)
            {
                return new GetTheBiddersAdvertisementResponse
                {
                    IsSuccess = false,
                    Message = "İlgili kişinin teklif verdiği herhangi bir ilan bulunmamaktadır.",
                    BiddersAdvertisements = null
                };
            }

            return new GetTheBiddersAdvertisementResponse
            {
                IsSuccess = true,
                BiddersAdvertisements = advs,
                Message = "İlgili ilanlar başarıyla getirildi."
            };

        }
    }
}
