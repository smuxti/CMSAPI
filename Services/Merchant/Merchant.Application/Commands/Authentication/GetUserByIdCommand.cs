using Merchants.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands
{
    public class GetUserByIdCommand : IRequest<User>
    {
        public Guid Id { get; set; }
      
    }
}
