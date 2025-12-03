using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class TestQuery : IRequest<Response>
    {
        public string Email { get; set; }
        public TestQuery(string Email)
        {
            this.Email = Email;
        }
    }
}
