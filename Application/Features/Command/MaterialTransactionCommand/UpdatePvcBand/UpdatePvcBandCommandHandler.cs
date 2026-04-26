using Application.Features.Command.MaterialTransactionCommand.UpdateMdf;
using Application.Interface;
using Domain.Entitiy.Material;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.UpdatePvcBand
{
    public class UpdatePvcBandCommandHandler : IRequestHandler<UpdatePvcBandCommandRequest, UpdatePvcBandCommandResponse>
    {
        private readonly IAppContext _context;

        public UpdatePvcBandCommandHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<UpdatePvcBandCommandResponse> Handle(UpdatePvcBandCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.AnyAsync(x => x.Id.ToString() == request.OwnerId);

            if (!user)
            {
                return new UpdatePvcBandCommandResponse
                {
                    IsSuccess = false,
                    Message = "Mevcut kullanıcı bilgilerine ulaşılamadı"
                };
            }
            var Pvcband = await _context.PvcBand.FirstOrDefaultAsync(x => x.Id.ToString() == request.PvcBandId);

            if (Pvcband.OwnerID.ToString() != request.OwnerId)
            {
                return new UpdatePvcBandCommandResponse
                {
                    IsSuccess = false,
                    Message = "Mevcüt ürün ilgili kişiye ait değildir."
                };
            }

            var IsContain = await _context.PvcBand.FirstOrDefaultAsync
                (
                    x => x.Id.ToString() != request.PvcBandId &&
                    x.Brand == request.Brand && 
                    x.OwnerID == request.OwnerId &&
                    x.Thickness == request.Thickness && 
                    x.Color == request.Color

                );
            try
            {
                if (IsContain == null)
                {
                    Pvcband.Brand = request.Brand;
                    Pvcband.Color = request.Color;
                    Pvcband.Stock = request.Stock;
                    Pvcband.Thickness = request.Thickness;

                    await _context.SaveChangesAsync(cancellationToken);

                    return new UpdatePvcBandCommandResponse
                    {
                        IsSuccess = true,
                        Message = "İlgili ürün güncellendi"
                    };
                }
                else
                {

                    IsContain.Stock += request.Stock;
                    _context.PvcBand.Remove(Pvcband);

                    await _context.SaveChangesAsync(cancellationToken);

                    return new UpdatePvcBandCommandResponse
                    {
                        IsSuccess = true,
                        Message = "İlgili ürün mevcut olduğundan stok bilgisi güncellendi"
                    };
                }
            }
            catch (Exception ex)
            {

                return new UpdatePvcBandCommandResponse
                {
                    IsSuccess = false,
                    Message = $"İlgili ürün güncellenirken bir hata ile karşılaşıldı.Hata : {ex.Message}"
                };
            }
        }
    }
}
