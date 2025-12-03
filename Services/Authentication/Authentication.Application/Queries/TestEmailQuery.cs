using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Queries
{
    public class TestEmailQuery : IRequest<string>
    {
        public string Test { get; set; }
        public TestEmailQuery(string Test)
        {
            this.Test = Test;
        }
    }
}
