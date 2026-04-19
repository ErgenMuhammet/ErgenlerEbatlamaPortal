using Application.Features.Command.MaterialTransactionCommand.ReduceBackPanel;
using Application.Features.Command.MaterialTransactionCommand.ReduceMdf;
using Application.Interface;
using Domain.Entitiy.Material;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.ReduceGlue
{
    public  class ReduceGlueCommandHandler : IRequestHandler<ReduceGlueCommandRequest, ReduceGlueCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly ISignalR _signalR;
        public ReduceGlueCommandHandler(IAppContext context, ISignalR signalR)
        {
            _context = context;
            _signalR = signalR;
        }
        public async Task<ReduceGlueCommandResponse> Handle(ReduceGlueCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var Glue = await _context.Glue.
                FirstOrDefaultAsync(x => x.OwnerID.ToString() == request.OwnerID && x.Weight == request.Weight && x.Brand == request.Brand);

            if (Glue == null)
            {
                return new ReduceGlueCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili ürün bulunamadı."
                };
            }

            if (Glue.Stock == 0)
            {
                return new ReduceGlueCommandResponse
                {
                    IsSuccess = true,
                    Message = "İlgili kullanıcıya ait tutkal stoğu bulunmamaktadır."
                };
            }

            if (request.Count > Glue.Stock)
            {
                return new ReduceGlueCommandResponse
                {
                    IsSuccess = true,
                    Message = "Stoğunuzdan fazla ürün kullanamazsınız."
                };
            }

            Glue.Stock -= request.Count;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new ReduceGlueCommandResponse
                {
                    IsSuccess = true,
                    Message = $"Stoğunuzdan stok güncellenirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                }; ;
            }

            if (Glue.Stock <= 10)
            {

            }
            await _signalR.SendNotification(request.OwnerID!, $"Yeni tutkal Stoğu : {Glue.Stock}");

            return new ReduceGlueCommandResponse
            {
                IsSuccess = true,
                Message = "Tutkal stoğu güncellendi"
            };
        }
    }
}
