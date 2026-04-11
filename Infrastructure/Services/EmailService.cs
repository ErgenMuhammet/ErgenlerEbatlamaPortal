using Application.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;


        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string toUser, string Subject, string htmlMessage)
        {
            var apikey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apikey);

            var from = new EmailAddress(_configuration["SendGrid:FromEmail"], _configuration["SendGrid:FromName"]);

            var to = new EmailAddress(toUser);

            var plainTextContent = System.Text.RegularExpressions.Regex.Replace(htmlMessage, "<[^>]*>", "");

            var msg = MailHelper.CreateSingleEmail(from, to, Subject, plainTextContent, htmlMessage);

            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Body.ReadAsStringAsync();
                throw new Exception($"SendGrid Hatası: {error}");
            }
        }

    }
}
