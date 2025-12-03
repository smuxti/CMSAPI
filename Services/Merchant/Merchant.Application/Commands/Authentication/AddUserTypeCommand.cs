using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchants.Application.Responses;

namespace Merchants.Application.Commands
{
    public class AddUserTypeCommand : IRequest<Response>
    {
        public string TypeName { get; set; }
        public int TypeCode { get; set; }
    }
}
