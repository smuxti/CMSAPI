using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands
{
    public class UpdateUserTypeCommand : IRequest<Response>
    {
        public string TypeName { get; set; }
        public int TypeCode { get; set; }
        public Guid Id { get; set; }
    }
}
