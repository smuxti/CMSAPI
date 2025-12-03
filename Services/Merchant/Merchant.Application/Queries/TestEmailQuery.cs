using MediatR;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class TestEmailQuery : IRequest<User>
    {
        public string Test { get;set; }
        public TestEmailQuery(string test) 
        { 
            this.Test = test;
        }
    }
}
