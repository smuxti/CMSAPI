using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Commands.Complaint
{
    public class ForwardComplaintCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int HierarchyId { get; set; }
    }
}
