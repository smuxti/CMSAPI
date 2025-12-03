using Merchants.Application.Responses;
using Merchants.Core.Entities;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merhchants.Application.Queries
{
    public class GetUserByUsernameQuery : IRequest<Response>
    {
        public string Username { get; set; }
        public GetUserByUsernameQuery(string Username)
        {
            this.Username = Username;
        }
    }
}
