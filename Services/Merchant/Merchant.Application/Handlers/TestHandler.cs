using EmailManager.MailService;
using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers
{
    public class TestHandler : IRequestHandler<TestQuery,Response>
    {
        private readonly Mail _emailService;

        public TestHandler(Mail EmailService)
        {
            _emailService = EmailService;
        }

        public async Task<Response> Handle(TestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Response response = new Response();
                var a =await _emailService.SendEmailAsync(request.Email,string.Empty,string.Empty, "Complaint Generated", "Complaint Generated Successfully");
                response.isSuccess= true;
                response.ResponseDescription = "True";
                response.ResponseCode = 1;
                response.Data = a;


                return response;
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.isSuccess = true;
                response.ResponseDescription = ex.Message;
                response.ResponseCode = 0;
                response.Data = null;
                throw;
            }
        }
    }
}
