using Merchants.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Queries
{
    public class DeleteUserTypeQuery : IRequest<Response>
    {
        public Guid id { get; set; }
        public DeleteUserTypeQuery(Guid id)
        {
                this.id = id;
        }
    }
}
