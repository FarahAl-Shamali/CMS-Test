using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
      
            private readonly IConfiguration _configuration;

            public EmailService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task Send(string to, string subject, string body)
            {
                var host = _configuration["EmailSettings:Host"];
                var port = int.Parse(_configuration["EmailSettings:Port"] ?? "587");
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderName = _configuration["EmailSettings:SenderName"];
                var password = _configuration["EmailSettings:Password"];

                var message = new MailMessage();
                message.From = new MailAddress(senderEmail!, senderName);
                message.Subject = subject;
                message.Body = body;
                message.To.Add(new MailAddress(to));
                message.IsBodyHtml = false;

                try
                {
                    using var emailClient = new SmtpClient
                    {
                        Host = host,
                        Port = port,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail, password),
                        Timeout = 3000
                    };

                    await emailClient.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
}
