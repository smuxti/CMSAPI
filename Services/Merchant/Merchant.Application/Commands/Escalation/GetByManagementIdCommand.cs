using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Commands.Escalation
{
    public class GetByManagementIdCommand : IRequest<Response>
    {
        public int managementId { get; set; }
        public GetByManagementIdCommand(int managementId)
        {
            this.managementId = managementId;
        }
    }
}
