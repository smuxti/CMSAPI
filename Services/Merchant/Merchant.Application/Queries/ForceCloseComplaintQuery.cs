using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Queries
{
    public class ForceCloseComplaintQuery : IRequest<Response>
    {
        public int Id { get; set; }
        public string? Remarks { get; set; }
        public ForceCloseComplaintQuery() { }
    }
}
