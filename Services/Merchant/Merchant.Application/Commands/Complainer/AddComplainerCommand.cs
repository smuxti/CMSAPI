using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Complainer
{
    public class AddComplainerCommand : IRequest<Response>
    {
        //public int ID { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string? Email { get; set; }
    }
}
