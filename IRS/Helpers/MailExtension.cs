using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace IRS.Helpers
{
    public interface IMailExtension
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task<string> SendEmail2Async(string email, string subject, string message);
        Task SendEmailWithAttactExcelFileAsync(List<string> emails, string subject, string message, string fileName, byte[] filepath);
        Task SendEmailRangeAsync(List<string> emails, string subject, string message);
        Task SendEmailRange(List<string> emails, string subject, string message);
    }
    public class MailExtension : IMailExtension
    {
        #region Feild
        private readonly IConfiguration _configuration;
        private delegate Task SendEmailDelegate(MailMessage m);
        #endregion

        #region Constructor
        public MailExtension(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Helper Method
        public async Task SendEmail(MailMessage m)
        {
            await SendEmail(m, true);
        }
        public async Task SendEmail(MailMessage m, Boolean Async)
        {
            SmtpClient smtp = null;

            smtp = new SmtpClient(_configuration["MailSettings:Server"])
            {
                UseDefaultCredentials = bool.Parse(_configuration["MailSettings:UseDefaultCredentials"]),
                Port = int.Parse(_configuration["MailSettings:Port"]),
                EnableSsl = bool.Parse(_configuration["MailSettings:EnableSsl"]),
                Credentials = new NetworkCredential(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"])
            };
            smtp = new SmtpClient();

            if (Async)
            {
                SendEmailDelegate sd = new SendEmailDelegate(smtp.SendMailAsync);
                AsyncCallback cb = new AsyncCallback(SendEmailResponse);
                sd.BeginInvoke(m, cb, sd);
            }
            else
            {
                await smtp.SendMailAsync(m);
            }

        }
        private void SendEmailResponse(IAsyncResult ar)
        {
            SendEmailDelegate sd = (SendEmailDelegate)(ar.AsyncState);
            try
            {
                sd.EndInvoke(ar);

            }
            catch
            {
            }
        }

        #endregion

        #region Method
        public Task<string> SendEmail2Async(string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient(_configuration["MailSettings:Server"])
            {

                UseDefaultCredentials = bool.Parse(_configuration["MailSettings:UseDefaultCredentials"]),
                Port = int.Parse(_configuration["MailSettings:Port"]),
                EnableSsl = bool.Parse(_configuration["MailSettings:EnableSsl"]),
                Credentials = new NetworkCredential(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"])
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["MailSettings:FromEmail"]),
            };
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.High;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            try
            {
                mailMessage.To.Add(email);
                client.Send(mailMessage);
                return Task.FromResult("");
            }
            catch (Exception ex)
            {
                return Task.FromResult(ex.Message);
            }
        }

        public Task SendEmailRange(List<string> emails, string subject, string message)
        {
            SmtpClient client = new SmtpClient(_configuration["MailSettings:Server"])
            {

                UseDefaultCredentials = bool.Parse(_configuration["MailSettings:UseDefaultCredentials"]),
                Port = int.Parse(_configuration["MailSettings:Port"]),
                EnableSsl = bool.Parse(_configuration["MailSettings:EnableSsl"]),
                Credentials = new NetworkCredential(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"])
            };
            emails.Add(_configuration["MailSettings:TestEmail"].ToString());
            using MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress(_configuration["MailSettings:FromEmail"], _configuration["MailSettings:FromName"]),
                IsBodyHtml = true,
                Body = message,
                Subject = subject,
                Priority = MailPriority.High,
                BodyEncoding = System.Text.Encoding.UTF8
            };
            foreach (var email in emails)
            {
                mailMessage.To.Add(email);
            }
            try
            {
                client.Send(mailMessage);

            }
            catch
            {

            }
            return Task.CompletedTask;

        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient(_configuration["MailSettings:Server"])
            {

                UseDefaultCredentials = bool.Parse(_configuration["MailSettings:UseDefaultCredentials"]),
                Port = int.Parse(_configuration["MailSettings:Port"]),
                EnableSsl = bool.Parse(_configuration["MailSettings:EnableSsl"]),
                Credentials = new NetworkCredential(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"])
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["MailSettings:FromEmail"], _configuration["MailSettings:FromName"]),
            };
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.High;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            try
            {
                mailMessage.To.Add(email);
                client.Send(mailMessage);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Task.CompletedTask;
        }
        public Task SendEmailRangeAsync(List<string> emails, string subject, string message)
        {
            SmtpClient client = new SmtpClient(_configuration["MailSettings:Server"])
            {
                UseDefaultCredentials = bool.Parse(_configuration["MailSettings:UseDefaultCredentials"]),
                Port = int.Parse(_configuration["MailSettings:Port"]),
                EnableSsl = bool.Parse(_configuration["MailSettings:EnableSsl"]),
                Credentials = new NetworkCredential(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"])
            };
            emails.Add(_configuration["MailSettings:TestEmail"].ToString());
            using MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress(_configuration["MailSettings:FromEmail"], _configuration["MailSettings:FromName"]),
                IsBodyHtml = true,
                Body = message,
                Subject = subject,
                Priority = MailPriority.High,
                BodyEncoding = System.Text.Encoding.UTF8
            };
            foreach (var email in emails)
            {
                mailMessage.To.Add(email);
            }

            try
            {
                client.Send(mailMessage);

            }
            catch
            {

            }
            return Task.CompletedTask;
        }

        public Task SendEmailWithAttactExcelFileAsync(List<string> emails, string subject, string message, string fileName, Byte[] file)
        {
            SmtpClient client = new SmtpClient(_configuration["MailSettings:Server"])
            {
                UseDefaultCredentials = bool.Parse(_configuration["MailSettings:UseDefaultCredentials"]),
                Port = int.Parse(_configuration["MailSettings:Port"]),
                EnableSsl = bool.Parse(_configuration["MailSettings:EnableSsl"]),
                Credentials = new NetworkCredential(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"])
            };
            emails.Add(_configuration["MailSettings:TestEmail"].ToString());
            using MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress(_configuration["MailSettings:FromEmail"], _configuration["MailSettings:FromName"]),
                IsBodyHtml = true,
                Body = message,
                Subject = subject,
                Priority = MailPriority.High,
                BodyEncoding = System.Text.Encoding.UTF8
            };
            var name = fileName;

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(file, 0, file.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Attachment attachment = new Attachment(memStream, name, "application/vnd.ms-excel");
            mailMessage.Attachments.Add(attachment);
            foreach (var email in emails)
            {
                mailMessage.To.Add(email);
            }

            try
            {
                client.Send(mailMessage);
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Send email successfully!");
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Send email failed!" + ex.Message);
                throw;
            }
            return Task.CompletedTask;
        }
        #endregion




    }
}