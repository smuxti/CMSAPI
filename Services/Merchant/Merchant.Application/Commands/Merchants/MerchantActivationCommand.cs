using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Merchants
{
    public class MerchantActivationCommand : IRequest<Response>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public MerchantActivationCommand(Guid Id, string Email)
        {
            this.Id = Id;
            this.Email = Email;
        }
    }
}
