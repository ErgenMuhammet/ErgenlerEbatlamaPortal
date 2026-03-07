using Application.Features.Command.MaterialTransactionCommand.UpdateMdf;
using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.UpdateGlue
{
    public class UpdateGlueCommandHandler : IRequestHandler<UpdateGlueCommandRequest, UpdateGlueCommandResponse>
    {
        private readonly IAppContext _context;

        public UpdateGlueCommandHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<UpdateGlueCommandResponse> Handle(UpdateGlueCommandRequest request, CancellationToken cancellationToken)
        {
            var Glue = await _context.Glue.FirstOrDefaultAsync(x => x.Id.ToString() == request.GlueId);

            if (Glue == null)
            {
                return new UpdateGlueCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili ürün bilgilerine ulaşılamadı."
                };
            }

            if (Glue.OwnerID != request.OwnerId)
            {
                return new UpdateGlueCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili ürün mevcut kullanıcıya ait değildir"
                };
            }

            var IsContain = await _context.Glue.FirstOrDefaultAsync
                (
                    x => x.Brand == request.Brand &&
                    x.Weight == request.Weight &&                   
                    x.OwnerID == request.OwnerId               
                );
            try
            {
                if (IsContain == null)
                {
                    Glue.Brand = request.Brand;
                    Glue.Weight = request.Weight;
                    
                    await _context.SaveChangesAsync(cancellationToken);

                    return new UpdateGlueCommandResponse
                    {
                        IsSuccess = true,
                        Message = "İlgili ürün güncellendi"
                    };
                }
                else
                {

                    IsContain.Stock += request.Stock;
                    _context.Glue.Remove(Glue);
                    await _context.SaveChangesAsync(cancellationToken);
                    return new UpdateGlueCommandResponse
                    {
                        IsSuccess = true,
                        Message = "İlgili ürün mevcut olduğundan stok bilgisi güncellendi"
                    };
                }
            }
            catch (Exception ex)
            {

                return new UpdateGlueCommandResponse
                {
                    IsSuccess = false,
                    Message = $"İlgili ürün güncellenirken bir hata ile karşılaşıldı.Hata : {ex.Message}"
                };
            }
        }
    }
}
