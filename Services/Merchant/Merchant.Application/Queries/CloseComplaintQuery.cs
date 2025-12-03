using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class CloseComplaintQuery : IRequest<Response>
    {
        public int Id { get; set; }
        public string? Otp { get; set; }
        public string? Remarks { get; set; }
        public CloseComplaintQuery()
        {
                
        }
    }
}
