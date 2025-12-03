using Authentication.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Commands
{
    public class GetAllUserByMerchantIdCommand : IRequest<IReadOnlyList<User>>
    {
        public Guid Id { get; set; }
    }
}
