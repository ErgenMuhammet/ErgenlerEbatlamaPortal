using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.UpdateMdf
{
    public class UpdateMdfCommandHandler : IRequestHandler<UpdateMdfCommandRequest, UpdateMdfCommandResponse>
    {
        private readonly IAppContext _context;

        public UpdateMdfCommandHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<UpdateMdfCommandResponse> Handle(UpdateMdfCommandRequest request, CancellationToken cancellationToken)
        {
            var mdf = await _context.Mdf.FirstOrDefaultAsync(x => x.Id.ToString() == request.MdfId.ToString());

            if (mdf == null)
            {
                return new UpdateMdfCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili ürün bilgilerine ulaşılamadı."
                };
            }

            if (mdf.OwnerID != request.OwnerId)
            {
                return new UpdateMdfCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili ürün mevcut kullanıcıya ait değildir"
                };
            }

            var IsContain = await _context.Mdf.FirstOrDefaultAsync
                (
                    x => x.Id.ToString() != request.MdfId &&
                    x.Brand == request.Brand &&
                    x.Color == request.Color &&
                    x.OwnerID == request.OwnerId &&
                    x.Thickness == request.Thickness 
                );
            try
            {
                if (IsContain == null)
                {
                    mdf.Brand = request.Brand;
                    mdf.Color = request.Color;
                    mdf.Stock = request.Stock;
                    mdf.Thickness = request.Thickness;
                    mdf.Weight = request.Weight;

                    await _context.SaveChangesAsync(cancellationToken);
                    return new UpdateMdfCommandResponse
                    {
                        IsSuccess = true,
                        Message = "İlgili ürün güncellendi"
                    };
                }
                else
                {
                    
                    IsContain.Stock += request.Stock;

                    _context.Mdf.Remove(mdf);
                    await _context.SaveChangesAsync(cancellationToken);
                    return new UpdateMdfCommandResponse
                    {
                        IsSuccess = true,
                        Message = "İlgili ürün mevcut olduğundan stok bilgisi güncellendi"
                    };
                }
            }
            catch (Exception ex)
            {

                return new UpdateMdfCommandResponse
                {
                    IsSuccess = false,
                    Message = $"İlgili ürün güncellenirken bir hata ile karşılaşıldı.Hata : {ex.Message}"
                };
            }
            
        }
    }
}
