using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Queries
{
    public class GetAllEquipmentQuery : IRequest<Response>
    {
        public GetAllEquipmentQuery() { }
    }
}
