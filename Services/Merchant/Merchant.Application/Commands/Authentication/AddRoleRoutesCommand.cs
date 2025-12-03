using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchants.Application.Responses;

namespace Merchants.Application.Commands
{
    public class AddRoleRoutesCommand : IRequest<Response>
    {
        public int UserTypeCode { get; set; }
        public int RoutePathId { get; set; }
        public int RouteId { get; set; }
    }
}
