using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchants.Application.Responses;

namespace Merchants.Application.Commands
{
    public class UpdateRouteCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string RouteName { get; set; }
        public string RoutePath { get; set; }
    }
}
