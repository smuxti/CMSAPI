using Merchants.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands
{
    public class GetAllUserByMerchantIdCommand : IRequest<IReadOnlyList<User>>
    {
        public int Id { get; set; }
    }
}
