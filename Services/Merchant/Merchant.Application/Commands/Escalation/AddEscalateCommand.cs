using MediatR;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Escalation
{
    public class AddEscalateCommand : IRequest<Response>
    {
    }
}
