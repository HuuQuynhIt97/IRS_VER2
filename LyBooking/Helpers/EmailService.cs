using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IRS.Helpers
{
    public interface IEmailService
    {
        Task<string> SendTextAsync(string to, string subject, string message);
        Task<string> SendAsync(string to, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        #region Feild
        private readonly IConfiguration _configuration;
        private delegate Task SendEmailDelegate(MailMessage m);
        #endregion

        #region Constructor
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        public async Task<string> SendAsync(string to, string subject, string message)
        {
            try
            {


                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["MailSettings:FromEmail"]));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = message };
                // send email
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_configuration["MailSettings:Server"], _configuration["MailSettings:Port"].ToInt(), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                return string.Empty;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> SendTextAsync(string to, string subject, string message)
        {
            try
            {


                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["MailSettings:FromEmail"]));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Plain) { Text = message };
                // send email
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_configuration["MailSettings:Server"], _configuration["MailSettings:Port"].ToInt(), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                return string.Empty;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
