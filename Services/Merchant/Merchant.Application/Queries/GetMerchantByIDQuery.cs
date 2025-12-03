using Merchants.Application.Responses;
using Merchants.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetMerchantByIDQuery : IRequest<Response>
    {
        public int Id { get; set; }
        public GetMerchantByIDQuery(int id)
        {
            this.Id = id;
        }

    }
}
