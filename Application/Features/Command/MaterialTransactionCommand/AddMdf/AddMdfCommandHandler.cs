using Application.Features.Command.MaterialTransactionCommand.AddScrap;
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

namespace Application.Features.Command.MaterialTransactionCommand.AddMdf
{
    public class AddMdfCommandHandler : IRequestHandler<AddMdfCommandRequest, AddMdfCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppContext _context;

        public AddMdfCommandHandler(UserManager<AppUser> userManager, IAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<AddMdfCommandResponse> Handle(AddMdfCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.OwnerId);

            if (user == null)
            {
                return new AddMdfCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı"
                };
            }
             var material = await _context.
                Mdf.
                FirstOrDefaultAsync(x => x.OwnerID == request.OwnerId &&
                x.Brand == request.Brand &&
                x.Thickness == request.Thickness &&
                x.Color == request.Color);          

            try
            {
                if (material != null)
                {
                    material.Stock += request.Stock;

                    await _context.SaveChangesAsync(cancellationToken);

                    return new AddMdfCommandResponse
                    {
                        IsSucces = true,
                        Message = "Mdf stoğu başarıyla güncellendi",
                    };

                }
                else
                {
                    var mdf = new Mdf
                    {
                        Brand = request.Brand,
                        Color = request.Color,
                        Stock = request.Stock,
                        Thickness = request.Thickness,
                        Weight = request.Weight,
                        OwnerID = request.OwnerId,
                    };
                    await _context.Mdf.AddAsync(mdf);
                    await _context.SaveChangesAsync(cancellationToken);

                    return new AddMdfCommandResponse
                    {
                        IsSucces = true,
                        Message = "Mdf başarıyla eklendi",
                    };
                }
                
            }
            catch (Exception ex)
            {
                return new AddMdfCommandResponse
                {
                    IsSucces = false,
                    Message = $"Mdf eklenirken bir hata ile karşılaşıldı.Hata : {ex.Message}",
                };
            }

            
        }
    }
}
