using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Commands.Complaint
{
    public class StartComplaintCommand : IRequest<Response>
    {
        public int ComplaintId { get; set; }
        public Guid UserId { get; set; }
        public string Remarks { get; set; }
    }
}
