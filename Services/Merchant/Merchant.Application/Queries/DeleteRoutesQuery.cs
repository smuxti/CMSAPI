using Merchants.Application.Responses;
using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Queries
{
    public class DeleteRoutesQuery : IRequest<Response>
    {
        public int Id { get; set; }
        public DeleteRoutesQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
