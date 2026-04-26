using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.AdvertisementTransactionHandlers.DeleteAdvertisement
{
    public class DeleteAdvertisementCommandHandler : IRequestHandler<DeleteAdvertismentCommandRequest, DeleteAdvertisementCommandResponse>
    {
        private readonly IAppContext _context;

        public DeleteAdvertisementCommandHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<DeleteAdvertisementCommandResponse> Handle(DeleteAdvertismentCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var Advs = await _context.Advertisements.
                FirstOrDefaultAsync(x => x.OwnerId.ToString() == request.OwnerId && x.AdvertisementId.ToString() == request.AdvertisementId);

            

            if (Advs == null)
            {
                return new DeleteAdvertisementCommandResponse
                {
                    IsSucces = false,
                    Message = "İlgili ilan bulunamadı"
                };
            }
                
            if (Advs.OwnerId != request.OwnerId)
            {
                return new DeleteAdvertisementCommandResponse
                {
                    IsSucces = false,
                    Message = "Sahibi olmadığınız ilanı silemezsiniz."
                };
            }

            try
            {
                 _context.Advertisements.Remove(Advs);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new DeleteAdvertisementCommandResponse
                {
                    IsSucces = false,
                    Message = $"İlan silinirken bir hata ile karşılaşıldı. Hata : {ex.Message}",
                };
            }

            return new DeleteAdvertisementCommandResponse
            {
                IsSucces = true,
                Message = $"İlan başarıyla silindi.",
            };
        }
    }
}
