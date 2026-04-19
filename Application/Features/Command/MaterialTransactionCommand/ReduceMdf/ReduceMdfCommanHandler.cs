using Application.Features.Command.MaterialTransactionCommand.ReduceBackPanel;
using Application.Features.Command.MaterialTransactionCommand.ReducePvcBand;
using Application.Interface;
using Domain.Entitiy.Material;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.ReduceMdf
{
    public class ReduceMdfCommanHandler : IRequestHandler<ReduceMdfCommandRequest, ReduceMdfCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly ISignalR _signalR;
        public ReduceMdfCommanHandler(IAppContext context, ISignalR signalR)
        {
            _context = context;
            _signalR = signalR;
        }
        public async Task<ReduceMdfCommandResponse> Handle(ReduceMdfCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var Mdf = await _context.Mdf.
                FirstOrDefaultAsync(x => x.OwnerID.ToString() == request.OwnerID && x.Color == request.Color && x.Thickness == request.Thickness && x.Brand == request.Brand && x.Weight == request.Weight) ;

            if (Mdf == null)
            {
                return new ReduceMdfCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili ürün bulunamadı."
                };
            }

            if (Mdf.Stock == 0)
            {
                return new ReduceMdfCommandResponse
                {
                    IsSuccess = true,
                    Message = "İlgili kullanıcıya ait Mdf stoğu bulunmamaktadır."
                };
            }

            if (request.Count > Mdf.Stock)
            {
                return new ReduceMdfCommandResponse
                {
                    IsSuccess = true,
                    Message = "Stoğunuzdan fazla ürün kullanamazsınız."
                };
            }

            Mdf.Stock -= request.Count;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new ReduceMdfCommandResponse
                {
                    IsSuccess = true,
                    Message = $"Stoğunuzdan stok güncellenirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                }; ;
            }

            if (Mdf.Stock <= 10)
            {
                await _signalR.SendNotification(request.OwnerID!, $" Mdf Stoğu Azaldı.Yeni Stok : {Mdf.Stock}");
            }

            await _signalR.SendNotification(request.OwnerID!, $"Yeni Mdf Stoğu : {Mdf.Stock}");

            return new ReduceMdfCommandResponse
            {
                IsSuccess = true,
                Message = "Mdf stoğu güncellendi"
            };
        }
    }
}
