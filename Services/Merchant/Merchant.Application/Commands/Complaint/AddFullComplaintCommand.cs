using MediatR;
using Merchants.Application.Commands.Complainer;
using Merchants.Application.Commands.ComplaintDetails;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Complaint
{
    public class AddFullComplaintCommand : IRequest<Response>
    {
        public List<AddCompaintCommand> AddComplaint { get; set; }
        public AddComplainerCommand AddComplainer { get; set; }
    }
}
