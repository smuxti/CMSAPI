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
    public class GetUserByEmailQuery : IRequest<User>
    {
        public string email { get; set; }
        public GetUserByEmailQuery(string email)
        {
            this.email = email;
        }
    }
}
