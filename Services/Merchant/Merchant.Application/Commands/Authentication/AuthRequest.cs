using Merchants.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands
{
    public class AuthRequest: IRequest<Response>
    {
        public string? username { get; set; }
        public string password { get; set; }

    }
}
