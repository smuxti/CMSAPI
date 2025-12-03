using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.ComplaintDetails
{
    public class DeleteCompaintDetailsCommand : IRequest<Response>
    {
        public int ID { get; set; }
   
    }
}
