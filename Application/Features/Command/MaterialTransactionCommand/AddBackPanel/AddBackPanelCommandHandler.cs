using Application.Features.Command.MaterialTransactionCommand.AddGlue;
using Application.Features.Command.MaterialTransactionCommand.AddPvcBand;
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

namespace Application.Features.Command.MaterialTransactionCommand.AddBackPanel
{
    public class AddBackPanelCommandHandler : IRequestHandler<AddBackPanelCommandRequest, AddBackPanelCommandResponse>
    {
        private readonly IAppContext _context;

        public AddBackPanelCommandHandler(IAppContext context)
        {
            _context = context;
        }
        public async Task<AddBackPanelCommandResponse> Handle(AddBackPanelCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.AnyAsync(x => x.Id == request.OwnerId, cancellationToken);

            if (!user)
            {
                return new AddBackPanelCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı"
                };
            }            

            var Material = await _context.
                BackPanel.
                FirstOrDefaultAsync(m => m.OwnerID == request.OwnerId &&
                m.Brand == request.Brand &&
                m.Color == request.Color && m.Thickness == request.Thickness);
            try
            {
                if (Material == null)
                {
                    var BackPanel = new BackPanel
                    {
                        Brand = request.Brand,
                        Color = request.Color,
                        Cost = request.Cost,
                        OwnerID = request.OwnerId,
                        Price = request.Price,
                        Profit = request.Profit,
                        Stock = request.Stock,
                        Thickness = request.Thickness,
                    };

                    await _context.BackPanel.AddAsync(BackPanel);
                    await _context.SaveChangesAsync(cancellationToken);

                    return new AddBackPanelCommandResponse
                    {
                        IsSucces = true,
                        Message = "Arkalık stoğu kaydedildi"
                    };
                }
                else
                {


                    Material.Stock += request.Stock;
                    Material.Cost = request.Cost;
                    await _context.SaveChangesAsync(cancellationToken);

                    return new AddBackPanelCommandResponse
                    {
                        IsSucces = true,
                        Message = "Arkalık stoğu başarıyla güncellendi",
                    };

                }
            }
            catch (Exception ex)
            {
                return new AddBackPanelCommandResponse
                {
                    IsSucces = false,
                    Message = $"Arkalık eklenirken bir hata ile karşılaşıldı.Hata : {ex.Message}",
                };
            }
        }                         
        
    }
}
