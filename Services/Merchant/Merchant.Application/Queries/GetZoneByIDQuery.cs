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
    public class GetZoneByIDQuery : IRequest<Response>
    {
        public int ID { get; set; }
        public GetZoneByIDQuery(int id)
        {
            ID = id;
        }
    }
}
