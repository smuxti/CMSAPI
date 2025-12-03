using Merchants.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Commands
{
    public class AddRouteCommand : IRequest<Response>
    {
        public string ModuleName { get; set; }
        public string RouteName { get; set; }
        public string RoutePath { get; set; }
    }
}
