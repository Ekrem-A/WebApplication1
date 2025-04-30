using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    // Infrastructure/Services/EmailService.cs
    public interface IEmailService
    {
        Task SendActivationEmailAsync(string toEmail, string activationLink);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendActivationEmailAsync(string toEmail, string activationLink)
        {
            var smtpClient = new SmtpClient(_config["Smtp:Host"])
            {
                Port = int.Parse(_config["Smtp:Port"]!),
                Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"]),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_config["Smtp:Username"]!),
                Subject = "TurApp Hesap Aktivasyonu",
                Body = $"Lütfen hesabınızı aktifleştirmek için bu linke tıklayın:\n{activationLink}",
                IsBodyHtml = false
            };

            mail.To.Add(toEmail);
            await smtpClient.SendMailAsync(mail);
        }
    }

}
