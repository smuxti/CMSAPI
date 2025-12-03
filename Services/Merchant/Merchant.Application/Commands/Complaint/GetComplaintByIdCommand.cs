using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Commands.Complaint
{
    public class GetComplaintByIdCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public GetComplaintByIdCommand(int Id)
        {
            this.Id = Id;
        }
    }
}
