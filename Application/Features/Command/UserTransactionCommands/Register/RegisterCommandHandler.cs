using Application.Interface;
using Domain.Entitiy;
using Domain.GlobalEnum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IAppContext _context;
        public RegisterCommandHandler(UserManager<AppUser> userManager, IEmailService emailService, IAppContext context)
        {
            _userManager = userManager;
            _emailService = emailService;
            _context = context;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("İstek sınıfı boş parametreler dönderdi");
            }

            if (request.Password != request.PasswordConfirm)
            {
                return new RegisterCommandResponse
                {
                    Message = "Şifreler uyuşmuyor tekrar deneyiniz.",
                    IsSucces = false
                };
            }

            var user = new AppUser
            {
                UserName = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                BirthDate = request.BirthDate,
                City = request.City,

                UserCategory = request.UserCategory,
            };
           
            var createUserResult = await _userManager.CreateAsync(user, request.Password);

        
            if (!createUserResult.Succeeded)
            {
                
                var errors = string.Join(" - ", createUserResult.Errors.Select(e => e.Description)); // hata mesajlarını birleştirip sadece okunabilir olankları - ile ayırarak alıyoruz

                return new RegisterCommandResponse
                {
                    Message = "Kullanıcı oluşturulurken kurallara takıldı: " + errors,
                    IsSucces = false
                };
            }

            
            var UserAccounting = new ProfitLossSituation
            {
                Date = DateTime.UtcNow,
                OwnerId = user.Id,
                TotalLoss = 0,
                TotalProfit = 0,
                LastSituation = 0,
            };

            var BaseJobs = new BaseJobs
            {
                AdressDescription = "",
                Category = request.UserCategory,
                Experience = 0,
                MastersName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                UserId = user.Id,
                WorkShopName = ""
            };

            await _context.AddAsync(BaseJobs, cancellationToken);
            await _context.AddAsync(UserAccounting, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result <= 0)
            {
                return new RegisterCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı muhasebe hesabı açılırken bir hata ile karşılaşıldı"
                };
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var EncodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            string baseUrl = "http://localhost:5293";

            var confirmationLink = $"{baseUrl}/Portal/ConfirmEmail?userId={user.Id}&token={EncodedToken}";

            string htmlBody = $@"
            <div style=""font-family: sans-serif; background-color: #f4f4f7; padding: 20px;"">
                <div style=""max-width: 500px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 8px;"">
                    <h2>Hesabını Onayla</h2>
                    <p>Merhaba, kaydını tamamlamak için aşağıdaki butona tıkla:</p>
                    <a href=""{confirmationLink}"" style=""background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;"">
                        E-postayı Doğrula
                    </a>
                </div>
            </div>";

            try
            {
                await _emailService.SendEmail(user.Email, "Hesap Doğrulama Linki", htmlBody);
            }
            catch (Exception ex)
            {
                return new RegisterCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı doğrulama linki gönderilirken bir hata oluştu : " + ex.Message,
                };
            }

            return new RegisterCommandResponse
            {
                IsSucces = true,
                Message = "Lütfen Email hesabınıza gelen doğrulama linkine tıklayınız."
            };

        }
    }
}
