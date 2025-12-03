using Merchants.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands
{
    public class RefreshTokenCommand : IRequest<Response>
    {
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
    }
}
