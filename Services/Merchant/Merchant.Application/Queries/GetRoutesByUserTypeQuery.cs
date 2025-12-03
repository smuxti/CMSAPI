using Merchants.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Queries
{
    public class GetRoutesByUserTypeQuery : IRequest<Response>
    {
        public int TypeId { get; set; }

        public GetRoutesByUserTypeQuery(int TypeId)
        {
                this.TypeId = TypeId;
        }
    }
}
