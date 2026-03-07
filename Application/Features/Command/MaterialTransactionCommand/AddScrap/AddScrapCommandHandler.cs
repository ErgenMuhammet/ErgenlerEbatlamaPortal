using Application.Interface;
using Domain.Entitiy;
using Domain.Entitiy.Material;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.AddScrap
{
    public class AddScrapCommandHandler : IRequestHandler<AddScrapCommandRequest, AddScrapCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppContext _context;

        public AddScrapCommandHandler(UserManager<AppUser> userManager, IAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<AddScrapCommandResponse> Handle(AddScrapCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.OwnerId);

            if (user == null)
            {
                return new AddScrapCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı"
                };
            }

            var Scrap = new Scraps
            {
                Brand = request.Brand,
                Color = request.Color,
                Height = request.Height,
                OwnerID = request.OwnerId,
                Thickness = request.Thickness,
                Weight = request.Weight,
                Width = request.Width,       
                MaterialType = request.MaterialType,
            };

            try
            {
                await _context.Scraps.AddAsync(Scrap);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return new AddScrapCommandResponse
                {
                    IsSucces = false,
                    Message = $"Fire ürün eklenirken bir hata ile karşılaşıldı.Hata : {ex.Message}",
                };               
            }

            return new AddScrapCommandResponse
            {
                IsSucces = true,
                Message = "Fire ürün başarıyla eklendi",
            };
        }
    }
}
