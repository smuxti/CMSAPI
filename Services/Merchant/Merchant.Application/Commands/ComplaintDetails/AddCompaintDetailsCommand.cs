using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.ComplaintDetails
{
    public class AddCompaintDetailsCommand : IRequest<Response>
    {
        //public int ID { get; set; }

        //public int ComplaintID { get; set; }
        public string AssignedTo {     get; set; }
        public string EscalationTime { get; set; }
        public string CurrentStatus { get; set; }
        public string Description { get; set; }
    }
}
