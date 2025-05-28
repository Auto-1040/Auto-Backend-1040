using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Net.Mail;
using Auto1040.Core.Services;
using Auto1040.Core.Shared;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Auto1040.Service
{

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(EmailRequest request)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_configuration["GOOGLE_USER_NAME"], _configuration["GOOGLE_USER_EMAIL"]));
            emailMessage.To.Add(new MailboxAddress(request.To, request.To));
            emailMessage.Subject = request.Subject;

            var bodyBuilder = new BodyBuilder { TextBody = request.Body };
            emailMessage.Body = bodyBuilder.ToMessageBody();



            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_configuration["SMTP_SERVER"], int.Parse(_configuration["PORT"]), SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["GOOGLE_USER_EMAIL"], _configuration["PASSWORD"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }


    }
}