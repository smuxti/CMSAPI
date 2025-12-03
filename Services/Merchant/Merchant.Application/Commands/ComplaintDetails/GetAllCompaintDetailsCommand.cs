using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.ComplaintDetails
{
    public class GetAllCompaintDetailsCommand : IRequest<Response>
    {
        public int ComplaintID { get; set; }
        public string AssignedTo { get; set; }
        public DateTime EscalationTime { get; set; }
        public string CurrentStatus { get; set; }
        public string Description { get; set; }
    }
}
