using Authentication.Application.Responses;
using Authentication.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Queries
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
