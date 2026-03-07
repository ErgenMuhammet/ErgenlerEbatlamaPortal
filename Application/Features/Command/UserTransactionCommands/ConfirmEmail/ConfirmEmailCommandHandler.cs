using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommandRequest, ConfirmEmailCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        public ConfirmEmailCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ConfirmEmailCommandResponse> Handle(ConfirmEmailCommandRequest request, CancellationToken cancellationToken)
        {
            var user =await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new ConfirmEmailCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bulunamadı.",
                    Errors = null
                };

            }

            var decodedtokenbytes = WebEncoders.Base64UrlDecode(request.Token);
            var decodedtoken = Encoding.UTF8.GetString(decodedtokenbytes);

            var result = await _userManager.ConfirmEmailAsync(user,decodedtoken);

            if (result.Succeeded)
            {
                return new ConfirmEmailCommandResponse
                {
                    Errors = null,
                    Message = "Hesap doğrulaması başarılı.Tekrar giriş yaparak işleminize devam edebilirsiniz.",
                    IsSucces = true
                };
            }
            return new ConfirmEmailCommandResponse
            {
                Message = "Hesap doğrulaması başarısız.Token geçersiz veya süresi dolmuş",
                IsSucces = true
            };
        }
    }
}
