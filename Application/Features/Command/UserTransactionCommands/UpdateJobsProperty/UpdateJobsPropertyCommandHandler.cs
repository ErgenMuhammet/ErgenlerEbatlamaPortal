using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransactionCommands.UpdateJobsProperty
{
    public class UpdateJobsPropertyCommandHandler : IRequestHandler<UpdateJobsPropertyCommandRequest, UpdateJobsPropertyCommandResponse>
    {
        
        private readonly IAppContext _appContext;
        public UpdateJobsPropertyCommandHandler(IAppContext appContext)
        {          
            _appContext = appContext;
        }
        public async Task<UpdateJobsPropertyCommandResponse> Handle(UpdateJobsPropertyCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _appContext.AppUsers.Include(u => u.Jobs).FirstOrDefaultAsync(x => x.Id.ToString() == request.UserId);

            if (user == null)
            {
                return new UpdateJobsPropertyCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili kullanıcı bulunamadı."
                };
            }

            user.Jobs.AdressDescription = request.AdressDescription;
            user.Jobs.WorkShopName = request.WorkShopName;
            user.Jobs.PhoneNumber = request.PhoneNumber;
            user.Jobs.MastersName = user.FullName;
            user.Jobs.UserId = request.UserId;
            user.IsUpdated = true;
            try
            {
                
                await _appContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new UpdateJobsPropertyCommandResponse
                {
                    IsSuccess = false,
                    Message = $"Bilgiler kaydedilirken bir hata ile karşılaşıldı. Hata : {ex.Message}",
                };
            }

            return new UpdateJobsPropertyCommandResponse
            {
                IsSuccess = true,
                Message = "Bilgiler başarıyla kaydedildi."
            };
            
        }
    }
}
