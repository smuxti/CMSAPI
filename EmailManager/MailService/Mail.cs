using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Net.Mail;
using System.Net;
using MimeKit;
using MailKit.Net.Smtp;
using RabbitMQ.Client;
using System.Text.Json;

namespace EmailManager.MailService
{
    public class Mail
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public Mail(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
        }

        //public async Task SendEmailAsync(string toEmail, string subject, string body)
        //{
        //    try
        //    {
        //        using (var client = new SmtpClient(_smtpServer, _smtpPort))
        //        {
        //            client.UseDefaultCredentials = false;
        //            client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
        //            client.EnableSsl = true;

        //            var mailMessage = new MailMessage
        //            {
        //                From = new MailAddress(_smtpUser),
        //                Subject = subject,
        //                Body = body,
        //                //IsBodyHtml = true,
        //            };
        //            mailMessage.To.Add(toEmail);

        //            await client.SendMailAsync(mailMessage);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        public async Task<string> SendEmailAsync(string toEmail,string ccEmail,string bccEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpUser, _smtpUser));
            message.To.Add(new MailboxAddress("", toEmail));
            if(! string.IsNullOrEmpty(ccEmail))
            {
                message.Bcc.Add(new MailboxAddress("", ccEmail));
            }
            if (!string.IsNullOrEmpty(bccEmail))
            {
                message.Bcc.Add(new MailboxAddress("", bccEmail));
            }
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.None);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);

                await client.SendAsync(message);
                return "true";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return ex.Message.ToString();
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
        public async Task PublishEmailToQueueAsync(Email email)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            email.isHtml = true;

            await Task.Run(() =>
            {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "ComplaintEmail",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var message = JsonSerializer.Serialize(email);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;


                channel.BasicPublish(exchange: "",
                                     routingKey: "ComplaintEmail",
                                     basicProperties: properties,
                                     body: body);
            });
        }


        public class Email
        {
            public string? to { get; set; }
            public string? cc { get; set; }
            public string? bcc { get; set; }
            public string? subject { get; set; }
            public string? body { get; set; }
            public bool? isHtml { get; set; }
            public Guid? guid { get; set; }
        }
    }
}
