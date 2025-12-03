using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Escalation
{
    public class DeleteEscalationCommand:IRequest<Response>
    {
        public int MatrixID { get; set; }

    }
}
