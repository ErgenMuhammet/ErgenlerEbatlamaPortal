using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.ValidatePasswordToken
{
    public class ValidatePasswordTokenCommandHandler : IRequestHandler<ValidatePasswordTokenCommandRequest, ValidatePasswordTokenCommandResponse>
    {
        private readonly UserManager<AppUser>? _userManager;

        public ValidatePasswordTokenCommandHandler(UserManager<AppUser>? userManager)
        {
            _userManager = userManager;
        }

        public async Task<ValidatePasswordTokenCommandResponse> Handle(ValidatePasswordTokenCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.userId);

            if (user == null)
            {
                return new ValidatePasswordTokenCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisi okunamadı. Token geçersiz veya süresi dolmuş. Daha sonra tekrar deneyiniz."
                };
            }
                var decodedtokenbytes = WebEncoders.Base64UrlDecode(request.passwordToken);
                var decodedtoken = Encoding.UTF8.GetString(decodedtokenbytes);

                var result = await _userManager.ResetPasswordAsync(user, decodedtoken, request.NewPassword);

                if (result.Succeeded)
                {
                    return new ValidatePasswordTokenCommandResponse
                    {
                        IsSucces = true,
                        Message = "Şifreniz başarı ile güncellenmiştir. Yeni şifreniz ile giriş yapabilirsiniz."
                    };
                }

                return new ValidatePasswordTokenCommandResponse
                {
                    IsSucces = false,
                    Message = "Şifre değiştirilirken bir hata ile karşılaşıldı daha sonra tekrar deneyiniz."
                };
        }
    }
}

