using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetComplainerByIDQuery : IRequest<Response>
    {
        public int ID { get; set; }
        public GetComplainerByIDQuery(int iD)
        {
            ID = iD;
        }
    }
}
