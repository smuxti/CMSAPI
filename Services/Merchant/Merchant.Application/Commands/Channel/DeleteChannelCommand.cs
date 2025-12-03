using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Channel
{
    public class DeleteChannelCommand: IRequest<Response>
    {
        public int Id { get; set; }

    }
}
