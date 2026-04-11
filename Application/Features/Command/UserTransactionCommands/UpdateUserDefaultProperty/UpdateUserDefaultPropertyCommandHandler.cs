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

namespace Application.Features.Command.UpdateUserDefaultProperty
{
    public class UpdateUserDefaultPropertyCommandHandler : IRequestHandler<UpdateUserDefaultPropertyCommandRequest, UpdateUserDefaultPropertyCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppContext _context;

        public UpdateUserDefaultPropertyCommandHandler(UserManager<AppUser> userManager, IAppContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UpdateUserDefaultPropertyCommandResponse> Handle(UpdateUserDefaultPropertyCommandRequest request, CancellationToken cancellationToken)
        {
            var User = await _userManager.FindByIdAsync(request.Id);

            if (User == null)
            {
                return new UpdateUserDefaultPropertyCommandResponse
                {
                    IsSuccess = false,
                    Message = "Mevcut kullanıcı bilgisine ulaşılamadı."
                };
            }

            User.FullName = request.FullName;
            User.BirthDate = request.BirthDate;
            User.PhoneNumber = request.PhoneNumber;
            User.City = request.City;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new UpdateUserDefaultPropertyCommandResponse
                {
                    IsSuccess = false,
                    Message = $"Kullanıcı bilgileri güncellenirken bir hata ile karşılaşıldı Hata : {ex.Message}"
                };
            }

            return new UpdateUserDefaultPropertyCommandResponse
            {
                IsSuccess = true,
                Message = "Kullanıcı bilgileri başarıyla güncellendi."
            };
        }
    }
}
