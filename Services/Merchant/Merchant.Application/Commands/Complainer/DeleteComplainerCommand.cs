using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Complainer
{
    public class DeleteComplainerCommand : IRequest<Response>
    {
        public int ID { get; set; }

    }
}
