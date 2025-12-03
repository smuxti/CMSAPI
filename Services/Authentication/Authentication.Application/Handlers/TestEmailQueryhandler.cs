using Authentication.Application.Queries;
using EmailManager.MailService;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Authentication.Application.Handlers
{
    public class TestEmailQueryhandler : IRequestHandler<TestEmailQuery, string>
    {
        private readonly Mail _mail;
        private readonly ILogger<TestEmailQueryhandler> _logger;

        public TestEmailQueryhandler(Mail mail, ILogger<TestEmailQueryhandler> logger)
        {
            _mail = mail;
            _logger = logger;
        }

        public async Task<string> Handle(TestEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Mail.Email test = new Mail.Email();
                test.subject = "Welcome";
                test.cc = "";
                test.body = "This is the Test Email Send By RabitMq";
                test.to = request.Test;
                await _mail.PublishEmailToQueueAsync(test);

                return $"Email Send To Rabbit mq to Email :: {test.to}";
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);

                return $"Email Send To Rabbit mq Failed Because {ex.Message}";
                throw;
            }
        }
    }
}
