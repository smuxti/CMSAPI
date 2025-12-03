using MediatR;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetAllTenantsQuery : IRequest<IReadOnlyList<Tenant>>
    {
        public GetAllTenantsQuery()
        {
                
        }
    }
}
