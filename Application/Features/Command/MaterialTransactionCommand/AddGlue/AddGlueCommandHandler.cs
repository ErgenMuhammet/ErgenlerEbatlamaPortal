using Application.Features.Command.MaterialTransactionCommand.AddMdf;
using Application.Interface;
using Domain.Entitiy;
using Domain.Entitiy.Material;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.AddGlue
{
    public class AddGlueCommandHandler : IRequestHandler<AddGlueCommandRequest, AddGlueCommandResponse>
    {
        private readonly IAppContext _context;

        public AddGlueCommandHandler( IAppContext context)
        {
            _context = context;
        }
        public async Task<AddGlueCommandResponse> Handle(AddGlueCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.AnyAsync(x => x.Id == request.OwnerId);

            if (!user)
            {
                return new AddGlueCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı"
                };
            }
            var material = await _context.Glue.FirstOrDefaultAsync(x => x.Brand == request.Brand && x.OwnerID == request.OwnerId);
            try
            {
                if (material != null)
                {
                    material.Stock += request.Stock;
                    await _context.SaveChangesAsync(cancellationToken);

                    return new AddGlueCommandResponse
                    {
                        IsSucces = true,
                        Message = "Tutkal stoğu başarıyla güncellendi"
                    };
                }
                else 
                {
                    var Glue = new Glue
                    {   OwnerID = request.OwnerId,
                        Brand = request.Brand,                        
                        Stock = request.Stock,
                        Weight = request.Weight,
                    };
                    await _context.Glue.AddAsync(Glue);
                    await _context.SaveChangesAsync(cancellationToken);

                    return new AddGlueCommandResponse
                    {
                        IsSucces = true,
                        Message = "Ürün başarıyla eklendi"
                    };
                }
            }          
            catch (Exception ex)
            {
                return new AddGlueCommandResponse
                {
                    IsSucces = false,
                    Message = $"Tutkal eklenirken bir hata ile karşılaşıldı.Hata : {ex.Message}",
                };
            }

           
        }
    }
}
