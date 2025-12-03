using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Merchants
{
    public class SendActivationEmailCommand : IRequest<Response>
    {
        public string Email { get; set; }
        public string BaseUrl { get; set; }
        public SendActivationEmailCommand(string Email, string BaseUrl) { this.Email = Email; this.BaseUrl = BaseUrl; }
    }
}
