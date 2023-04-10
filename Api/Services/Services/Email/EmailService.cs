using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace App.Service.Services.Email
{
    public class EmailService : IEmailSender
    {
        private readonly EmailConfiguration configuration;

        public EmailService(EmailConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string senderEmail = configuration.From;
            string password = configuration.Password;

            MailMessage message = new()
            {
                Subject = subject,
                Body = htmlMessage,
                From = new(senderEmail),
                Sender = new(senderEmail)
            };
            message.To.Add(email);

            SmtpClient smtpClient = new(configuration.SmtpServer, configuration.Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, password),
            };

            try
            {
                await smtpClient.SendMailAsync(message); 
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                throw new SmtpException(ex.Message);
            }
        }
    }
}
