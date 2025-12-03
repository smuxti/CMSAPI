using Authentication.Application.Responses;
using Authentication.Core.Entities;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Queries
{
    public class GetUserByUsernameQuery : IRequest<UserResponseWithSecurity>
    {
        public string Username { get; set; }
        public GetUserByUsernameQuery(string Username)
        {
            this.Username = Username;
        }
    }
}
