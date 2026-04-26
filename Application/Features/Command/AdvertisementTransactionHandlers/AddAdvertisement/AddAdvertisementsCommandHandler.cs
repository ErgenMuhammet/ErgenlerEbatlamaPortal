using Application.Interface;
using Domain.Entitiy;
using Domain.GlobalEnum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<AppUser> _userManager;

        public AddAdvertisementsCommandHandler(IAppContext context , UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AddAdvertisementsCommandResponse> Handle(AddAdvertisementsCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request)); // request boş diyoruz 
            }

            var Owner = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.ToString() == request.OwnerId);

            if (Owner.UserCategory == Category.Carpenter)
            { 
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
                    await _context.Advertisements.AddAsync(Advs);
                    await _context.SaveChangesAsync(cancellationToken);
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

            else
            {
                return new AddAdvertisementsCommandResponse
                {
                    IsSucces = false,
                    Message = "İlan eklemeye yetkiniz bulunmamaktadır."
                };
            }
        }
    }
}
